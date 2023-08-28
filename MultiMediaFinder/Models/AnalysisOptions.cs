using MultiMediaFinder.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MultiMediaFinder.Models;

public class AnalysisOptions
{
    public IEnumerable<string> Directories { get; set; } = null!;

    public bool Recursive { get; set; } = true;

    public bool ExtractArchives { get; set; }

    public bool PerformDeepAnalysis { get; set; }

    public static AnalysisOptions Create(AnalysisConfigDialogViewModel analysisConfigDialogViewModel)
        => new()
        {
            Directories = analysisConfigDialogViewModel.Directories.ToArray(),
            Recursive = analysisConfigDialogViewModel.Recursive,
            ExtractArchives = analysisConfigDialogViewModel.ExtractArchives,
            PerformDeepAnalysis = analysisConfigDialogViewModel.PerformDeepAnalysis
        };
}
