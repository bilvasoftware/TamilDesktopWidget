using System.IO;
using System.Text.Json;

namespace TamilDesktopWidget.Services
{
    public class WindowSettings
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public static class SettingsService
    {
        private static readonly string Folder =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TamilDesktopWidget");

        private static readonly string FilePath =
    Path.Combine(Folder, "windowsettings.json");

        public static void Save(WindowSettings settings)
        {
            Directory.CreateDirectory(Folder);

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FilePath, json);
        }

        public static WindowSettings Load()
        {
            if (!File.Exists(FilePath))
            {
                return new WindowSettings();
            }

            var json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<WindowSettings>(json)
                   ?? new WindowSettings();
        }
    }
}