using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MultiMediaFinder.Models;
using MultiMediaFinder.Views;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiMediaFinder.ViewModels;

[ObservableObject]
public partial class MainWindowViewModel
{
    private static readonly List<string> _archiveExtensions = new()
    {
        ".zip",
        ".rar",
        ".7z",
        ".bz2",
        ".tar",
        ".gz"
    };

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateWorkspaceClickCommand))]
    [NotifyCanExecuteChangedFor(nameof(CloseWorkspaceCommand))]
    [NotifyCanExecuteChangedFor(nameof(StartAnalysisCommand))]
    private WorkspaceManager? _workspace;

    [ObservableProperty]
    private ObservableCollection<FileDetails> _files = new();

    public bool WorkspaceUnavailable()
        => Workspace is null;

    public bool WorkspaceAvailable()
        => Workspace is not null;

    [RelayCommand(CanExecute = nameof(WorkspaceUnavailable), IncludeCancelCommand = true)]
    public async Task CreateWorkspaceClickAsync(CancellationToken cancellationToken)
    {
        try
        {
            Workspace = new WorkspaceManager();
            await Workspace.InitialiseWorkspace(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Workspace = null;
        }
    }

    [RelayCommand(CanExecute = nameof(WorkspaceAvailable))]
    public async Task CloseWorkspaceAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Workspace!.CloseWorkspace(cancellationToken);
            Workspace = null;
        }
        catch (OperationCanceledException)
        {
            // do nothing
        }
    }

    [RelayCommand(CanExecute = nameof(WorkspaceAvailable))]
    public async Task StartAnalysis(CancellationToken cancellationToken)
    {
        var dialog = new AnalysisConfigDialog();
        if (dialog.ShowDialog() == true)
        {
            Files.Clear();

            foreach (var file in await AnalyseFiles(AnalysisOptions.Create(dialog.Options)))
            {
                Files.Add(file);
            }
        }
    }

    public async Task<IEnumerable<FileDetails>> AnalyseFiles(AnalysisOptions options)
    {
        var searchPaths = LocateSearchDirectories(options, Workspace!.WorkspacePath);

        var files = new List<FileDetails>();
        files.AddRange(await LocateImageFiles(options, searchPaths));
        files.AddRange(await LocateVideoFiles(options, searchPaths));

        return files;
    }

    private static List<string> LocateSearchDirectories(AnalysisOptions options, string workspacePath)
    {
        var searchPaths = new List<string>(options.Directories);
        if (options.Recursive)
        {
            searchPaths.AddRange(
                options.Directories
                    .Where(Directory.Exists)
                    .SelectMany(d => Directory.GetDirectories(d, "*", SearchOption.AllDirectories))
                );
        }

        if (options.ExtractArchives)
        {
            var archivePaths = new List<string>();
            foreach (var archive in searchPaths
                    .SelectMany(d => Directory.GetFiles(d, "*.*", SearchOption.TopDirectoryOnly))
                    .Where(f => _archiveExtensions.Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase))))
            {
                try
                {
                    using var archiveFile = new ArchiveFile(archive);
                    var outputPath = Path.Combine(workspacePath, Guid.NewGuid().ToString("d"));
                    archiveFile.Extract(outputPath, true);
                    archivePaths.Add(outputPath);
                    archivePaths.AddRange(Directory.GetDirectories(outputPath, "*", SearchOption.AllDirectories));
                }
                catch
                {
                    // ignore
                }
            }

            searchPaths = searchPaths
                .Concat(archivePaths)
                .ToList();
        }

        return searchPaths
            .Distinct()
            .Where(Directory.Exists)
            .ToList();
    }

    private static Task<IEnumerable<FileDetails>> LocateImageFiles(AnalysisOptions options, List<string> searchPaths)
    {
        throw new NotImplementedException();
    }

    private static Task<IEnumerable<FileDetails>> LocateVideoFiles(AnalysisOptions options, List<string> searchPaths)
    {
        throw new NotImplementedException();
    }
}
