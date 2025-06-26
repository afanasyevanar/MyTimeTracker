using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Threading;
using MyTimeTracker.Models;
using MyTimeTracker.Services;
using MyTimeTracker.Views;
using ReactiveUI;

namespace MyTimeTracker.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;

    public MainWindowViewModel()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
        
        OpenAppSettingsCommand = ReactiveCommand.Create(OpenAppSettings);
    }

    public ObservableCollection<TrackedApp> TrackedApps { get; } = new();
    
    public ReactiveCommand<Unit, Unit> OpenAppSettingsCommand { get; }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var (processName, _) = ActiveWindowTracker.GetActiveWindowInfo();

        if (string.IsNullOrEmpty(processName) || processName == "Unknown") return;

        var trackedApp = TrackedApps.FirstOrDefault(a => a.AppName == processName);

        if (trackedApp == null)
        {
            trackedApp = new TrackedApp(processName);
            TrackedApps.Add(trackedApp);
        }

        trackedApp.ActiveTime += TimeSpan.FromSeconds(1);
    }
    
    private void OpenAppSettings()
    {
        var settingsViewModel = new AppSettingsViewModel(TrackedApps);
        var settingsWindow = new AppSettingsWindow(settingsViewModel);
        
        // Найти главное окно для установки Owner
        var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
            
        if (mainWindow != null)
        {
            settingsWindow.ShowDialog(mainWindow);
        }
        else
        {
            settingsWindow.Show();
        }
    }
}