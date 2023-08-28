using MultiMediaFinder.ViewModels;
using System.Windows;

namespace MultiMediaFinder.Views;

/// <summary>
/// Interaction logic for AnalysisConfigDialog.xaml
/// </summary>
public partial class AnalysisConfigDialog : Window
{
    private readonly AnalysisConfigDialogViewModel ViewModel;

    public AnalysisConfigDialog()
    {
        DataContext = ViewModel = new AnalysisConfigDialogViewModel();
        InitializeComponent();
    }

    public AnalysisConfigDialogViewModel Options => ViewModel;

    private void OkButton_Click(object sender, RoutedEventArgs e)
        => DialogResult = true;

    private void CancelButton_Click(object sender, RoutedEventArgs e)
        => DialogResult = false;

    private void DirectorySelectionAdd_Click(object sender, RoutedEventArgs e)
    {
        var text = AddPathSelector.Text;
        SelectedPaths.Items.Add(text);
        ViewModel.Directories.Add(text);
        AddPathSelector.Text = null;
    }
}
