using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MultiMediaFinder.ViewModels;

[ObservableObject]
public partial class AnalysisConfigDialogViewModel
{
    [ObservableProperty]
    private bool _recursive = true;

    [ObservableProperty]
    private bool _extractArchives;

    [ObservableProperty]
    private bool _performDeepAnalysis;

    public ObservableCollection<string> Directories = new();
}
