using System;
using System.IO;
using System.Text.Json;
using TamilDesktopWidget.Models;

namespace TamilDesktopWidget.Services
{
    public static class WidgetSettingsService
    {
        private static readonly string FilePath =
    Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "TamilDesktopWidget",
        "widgetsettings.json");

        public static WidgetSettings Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    WidgetSettings settings = new WidgetSettings();
                    Save(settings);          // Create the file automatically
                    return settings;
                }

                string json = File.ReadAllText(FilePath);

                return JsonSerializer.Deserialize<WidgetSettings>(json)
                       ?? new WidgetSettings();
            }
            catch
            {
                return new WidgetSettings();
            }
        }

        public static void Save(WidgetSettings settings)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

            string json = JsonSerializer.Serialize(settings,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(FilePath, json);
        }
    }
}