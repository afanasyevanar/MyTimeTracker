using System;
using System.Collections.Generic;
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
    private string _lastActiveApp = string.Empty;
    private int _distractionsCount = 0;
    private readonly Dictionary<string, bool> _settings;

    public MainWindowViewModel()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
        
        OpenAppSettingsCommand = ReactiveCommand.Create(OpenAppSettings);
        OpenStatisticsCommand = ReactiveCommand.Create(OpenStatistics);
        _settings = SettingsService.LoadSettings();
    }

    public ObservableCollection<TrackedApp> TrackedApps { get; } = new();
    public int DistractionsCount => _distractionsCount;
    
    public ReactiveCommand<Unit, Unit> OpenAppSettingsCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenStatisticsCommand { get; }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var (processName, _) = ActiveWindowTracker.GetActiveWindowInfo();

        if (string.IsNullOrEmpty(processName) || processName == "Unknown") return;

        var trackedApp = TrackedApps.FirstOrDefault(a => a.AppName == processName);

        if (trackedApp == null)
        {
            trackedApp = new TrackedApp(processName)
            {
                WorkApplication = _settings.ContainsKey(processName) && _settings[processName]
            };
            TrackedApps.Add(trackedApp);
        }

        // Distraction check
        if (!string.IsNullOrEmpty(_lastActiveApp) && _lastActiveApp != processName)
        {
            var lastApp = TrackedApps.FirstOrDefault(a => a.AppName == _lastActiveApp);
            if (lastApp is { WorkApplication: true } && !trackedApp.WorkApplication)
            {
                _distractionsCount++;
                this.RaisePropertyChanged(nameof(DistractionsCount));
            }
        }

        _lastActiveApp = processName;
        trackedApp.ActiveTime += TimeSpan.FromSeconds(1);
    }
    
    private void OpenAppSettings()
    {
        var settingsViewModel = new AppSettingsViewModel(TrackedApps);
        var settingsWindow = new AppSettingsWindow(settingsViewModel);

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

    private void OpenStatistics()
    {
        var statisticsViewModel = new StatisticsViewModel(TrackedApps, _distractionsCount);
        var statisticsWindow = new StatisticsWindow(statisticsViewModel);
        
        // Найти главное окно для установки Owner
        var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
            
        if (mainWindow != null)
        {
            statisticsWindow.ShowDialog(mainWindow);
        }
        else
        {
            statisticsWindow.Show();
        }
    }
}