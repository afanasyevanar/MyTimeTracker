using System;
using MyTimeTracker.ViewModels;
using ReactiveUI;

namespace MyTimeTracker.Models;

public class TrackedApp : ViewModelBase
{
    private TimeSpan _activeTime;
    private bool _workApplication;

    public TrackedApp(string appName)
    {
        AppName = appName;
        ActiveTime = TimeSpan.Zero;
        WorkApplication = false;
    }

    public string AppName { get; }

    public TimeSpan ActiveTime
    {
        get => _activeTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _activeTime, value);
            this.RaisePropertyChanged(nameof(FormattedTime));
        }
    }

    public string FormattedTime => $"{(int)ActiveTime.TotalHours:D2}:{ActiveTime.Minutes:D2}:{ActiveTime.Seconds:D2}";

    public bool WorkApplication
    {
        get => _workApplication;
        set => this.RaiseAndSetIfChanged(ref _workApplication, value);
    }
}