using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MultiMediaFinder.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;

namespace MultiMediaFinder.ViewModels;

[ObservableObject]
public partial class MainWindowViewModel
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateWorkspaceClickCommand))]
    [NotifyCanExecuteChangedFor(nameof(CloseWorkspaceCommand))]
    private Workspace? _workspace;

    public bool CanCreateWorkspace()
        => Workspace is null;

    public bool CanCloseWorkspace()
        => Workspace is not null;

    [RelayCommand(CanExecute = nameof(CanCreateWorkspace), IncludeCancelCommand = true)]
    public async Task CreateWorkspaceClickAsync(CancellationToken cancellationToken)
    {
        try
        {
            Workspace = new Workspace();
            await Workspace.InitialiseWorkspace(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Workspace = null;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCreateWorkspace), IncludeCancelCommand = true)]
    public async Task OpenWorkspaceClickAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(1_000, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Workspace = null;
        }
    }

    [RelayCommand(CanExecute = nameof(CanCloseWorkspace))]
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
}
