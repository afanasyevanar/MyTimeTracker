using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MyTimeTracker.Models;

namespace MyTimeTracker.Services;

public static class SettingsService
{
    private static readonly string SettingsFilePath = "settings.json";
    
    public static void SaveSettings(ObservableCollection<TrackedApp> trackedApps)
    {
        try
        {
            var config = string.Join(',', trackedApps.Select(a => $"{a.AppName} {a.WorkApplication}"));
            File.WriteAllText(SettingsFilePath, config);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении настроек: {ex.Message}");
        }
    }

    public static Dictionary<string, bool> LoadSettings()
    {
        var result = new Dictionary<string, bool>();
        try
        {
            if (!File.Exists(SettingsFilePath))
                return result;
            
            var config = File.ReadAllText(SettingsFilePath);
            var elements = config.Split(',');

            foreach (var element in elements)
            {
                var s = element.Split(' ');
                result.Add(s[0], bool.Parse(s[1]));
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке настроек: {ex.Message}");
            return [];
        }
    }
}

