using System;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace MyTimeTracker.ViewModels;

public class StatisticsViewModel : ViewModelBase
{
    public StatisticsViewModel(ObservableCollection<Models.TrackedApp> trackedApps)
    {
        TrackedApps = trackedApps;
        CalculateStatistics();
    }

    public ObservableCollection<Models.TrackedApp> TrackedApps { get; }

    public TimeSpan TotalWorkTime { get; private set; }
    public TimeSpan TotalNonWorkTime { get; private set; }
    public TimeSpan TotalTime { get; private set; }

    public string FormattedTotalWorkTime => $"{(int)TotalWorkTime.TotalHours:D2}:{TotalWorkTime.Minutes:D2}:{TotalWorkTime.Seconds:D2}";
    public string FormattedTotalNonWorkTime => $"{(int)TotalNonWorkTime.TotalHours:D2}:{TotalNonWorkTime.Minutes:D2}:{TotalNonWorkTime.Seconds:D2}";
    public string FormattedTotalTime => $"{(int)TotalTime.TotalHours:D2}:{TotalTime.Minutes:D2}:{TotalTime.Seconds:D2}";

    public double WorkTimePercentage => TotalTime.TotalSeconds > 0 ? (TotalWorkTime.TotalSeconds / TotalTime.TotalSeconds) * 100 : 0;
    public double NonWorkTimePercentage => TotalTime.TotalSeconds > 0 ? (TotalNonWorkTime.TotalSeconds / TotalTime.TotalSeconds) * 100 : 0;

    private void CalculateStatistics()
    {
        TotalWorkTime = TimeSpan.Zero;
        TotalNonWorkTime = TimeSpan.Zero;

        foreach (var app in TrackedApps)
        {
            if (app.WorkApplication)
            {
                TotalWorkTime += app.ActiveTime;
            }
            else
            {
                TotalNonWorkTime += app.ActiveTime;
            }
        }

        TotalTime = TotalWorkTime + TotalNonWorkTime;

        this.RaisePropertyChanged(nameof(TotalWorkTime));
        this.RaisePropertyChanged(nameof(TotalNonWorkTime));
        this.RaisePropertyChanged(nameof(TotalTime));
        this.RaisePropertyChanged(nameof(FormattedTotalWorkTime));
        this.RaisePropertyChanged(nameof(FormattedTotalNonWorkTime));
        this.RaisePropertyChanged(nameof(FormattedTotalTime));
        this.RaisePropertyChanged(nameof(WorkTimePercentage));
        this.RaisePropertyChanged(nameof(NonWorkTimePercentage));
    }
} 