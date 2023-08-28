using MultiMediaFinder.ViewModels;
using System.Windows;

namespace MultiMediaFinder.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }

    private void QuitButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
