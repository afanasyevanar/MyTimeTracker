using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MyTimeTracker.ViewModels;
using MyTimeTracker.Views;

namespace MyTimeTracker;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

        base.OnFrameworkInitializationCompleted();
    }

    private void OnOpenClicked(object? sender, EventArgs eventArgs)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            Dispatcher.UIThread.Post(() =>
            {
                desktop.MainWindow.Show();
                desktop.MainWindow.WindowState = WindowState.Normal;
                desktop.MainWindow.Activate();
            });
    }

    private void OnExitClicked(object sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            Dispatcher.UIThread.Post(() => { desktop.Shutdown(); });
    }
}