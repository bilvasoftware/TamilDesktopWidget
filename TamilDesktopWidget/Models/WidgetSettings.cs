using System.Windows.Media;

namespace TamilDesktopWidget.Models
{
    public class WidgetSettings
    {
        public double Opacity { get; set; } = 0.87;

        public string BackgroundMode { get; set; } = "Solid";

        public string BackgroundColor { get; set; } = "#DD1F1F1F";

        public string? BackgroundImage { get; set; }

        public string EnglishTextColor { get; set; } = "#FFFFFF";

        

        public string TamilTextColor { get; set; } = "#FFD08C";

        public double CornerRadius { get; set; } = 20;


        // New

        public string FontFamily { get; set; } = "Segoe UI";

        public double FontSize { get; set; } = 60;

        public bool AlwaysOnTop { get; set; } = true;

        public bool StartWithWindows { get; set; } = true;
    }
}