using Avalonia.Controls;
using MyTimeTracker.Services;
using MyTimeTracker.ViewModels;

namespace MyTimeTracker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        // Сохраняем настройки перед скрытием
        if (DataContext is MainWindowViewModel viewModel)
        {
            SettingsService.SaveSettings(viewModel.TrackedApps);
        }
        
        e.Cancel = true;
        Hide();
    }
}