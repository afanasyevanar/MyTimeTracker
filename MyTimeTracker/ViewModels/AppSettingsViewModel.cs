using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using MyTimeTracker.Models;
using MyTimeTracker.Services;
using ReactiveUI;

namespace MyTimeTracker.ViewModels;

public class AppSettingsViewModel : ViewModelBase
{
    private readonly ObservableCollection<TrackedApp> _sourceTrackedApps;
    
    public AppSettingsViewModel(ObservableCollection<TrackedApp> trackedApps)
    {
        _sourceTrackedApps = trackedApps;

        AppSettings = new ObservableCollection<AppSettingItem>(
            trackedApps.Select(app => new AppSettingItem(app.AppName, app.WorkApplication))
        );
        
        SaveCommand = ReactiveCommand.Create(Save);
        CancelCommand = ReactiveCommand.Create(Cancel);
    }
    
    public ObservableCollection<AppSettingItem> AppSettings { get; }
    
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    
    public event System.Action? CloseRequested;
    
    private void Save()
    {
        foreach (var setting in AppSettings)
        {
            var trackedApp = _sourceTrackedApps.FirstOrDefault(app => app.AppName == setting.AppName);
            if (trackedApp != null)
            {
                trackedApp.WorkApplication = setting.WorkApplication;
            }
        }
        
        SettingsService.SaveSettings(_sourceTrackedApps);
        CloseRequested?.Invoke();
    }
    
    private void Cancel()
    {
        CloseRequested?.Invoke();
    }
}

public class AppSettingItem : ViewModelBase
{
    private bool _workApplication;
    
    public AppSettingItem(string appName, bool workApplication)
    {
        AppName = appName;
        WorkApplication = workApplication;
    }
    
    public string AppName { get; }
    
    public bool WorkApplication
    {
        get => _workApplication;
        set => this.RaiseAndSetIfChanged(ref _workApplication, value);
    }
}

