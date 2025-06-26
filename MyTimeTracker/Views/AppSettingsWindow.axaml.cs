using Avalonia.Controls;
using MyTimeTracker.ViewModels;

namespace MyTimeTracker.Views;

public partial class AppSettingsWindow : Window
{
    public AppSettingsWindow()
    {
        InitializeComponent();
    }

    public AppSettingsWindow(AppSettingsViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.CloseRequested += Close;
    }
}

