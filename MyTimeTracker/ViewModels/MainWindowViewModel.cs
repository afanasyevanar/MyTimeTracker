using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading;
using MyTimeTracker.Models;
using MyTimeTracker.Services;

namespace MyTimeTracker.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;
    public ObservableCollection<TrackedApp> TrackedApps { get; } = new();

    public MainWindowViewModel()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var (processName, _) = ActiveWindowTracker.GetActiveWindowInfo();

        if (string.IsNullOrEmpty(processName) || processName == "Unknown")
        {
            return;
        }

        var trackedApp = TrackedApps.FirstOrDefault(a => a.AppName == processName);

        if (trackedApp == null)
        {
            trackedApp = new TrackedApp(processName);
            TrackedApps.Add(trackedApp);
        }

        trackedApp.ActiveTime += TimeSpan.FromSeconds(1);
        
        Console.WriteLine(trackedApp.AppName + " - " + trackedApp.ActiveTime);
    }
}