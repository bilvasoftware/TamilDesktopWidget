using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TamilDesktopWidget.Services;
using System.Windows.Media;
using TamilDesktopWidget.Models;
using System.IO;
using System.Windows.Media.Imaging;
namespace TamilDesktopWidget

{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private readonly TamilCalendarService tamilCalendarService;
        private WidgetSettings widgetSettings;

        // Locked by default
        private TrayService? trayService;
        private bool _isLocked = false;
        public MainWindow()
        {
            InitializeComponent();
            widgetSettings = WidgetSettingsService.Load();

            ApplyWidgetSettings();

            trayService = new TrayService(this);

            WindowStartupLocation = WindowStartupLocation.Manual;

            // Runs after the window is fully created
            Loaded += MainWindow_Loaded;

            // Save position whenever the window moves
            LocationChanged += MainWindow_LocationChanged;


            // Start with Windows
            StartupService.EnableStartup();

            tamilCalendarService = new TamilCalendarService();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateClock();
        }

        // Set default position (Top Right)
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            const double margin = 30;

            // Position in the top-right corner of the working area
            Left = SystemParameters.WorkArea.Right - ActualWidth - margin;
            Top = SystemParameters.WorkArea.Top + margin;
        }

        private void ApplyWidgetSettings()
        {
            WidgetBorder.Opacity = widgetSettings.Opacity;

            if (widgetSettings.BackgroundMode == "Image"
                && !string.IsNullOrEmpty(widgetSettings.BackgroundImage)
                && File.Exists(widgetSettings.BackgroundImage))
            {
                ImageBrush brush = new ImageBrush();

                brush.ImageSource =
                    new BitmapImage(
                        new Uri(widgetSettings.BackgroundImage));

                brush.Stretch = Stretch.UniformToFill;

                WidgetBorder.Background = brush;
            }
            else if (widgetSettings.BackgroundMode == "Transparent")
            {
                WidgetBorder.Background =
                    new SolidColorBrush(
                        System.Windows.Media.Color.FromArgb(120, 31, 31, 31));
            }
            else
            {
                WidgetBorder.Background =
                    (SolidColorBrush)new BrushConverter()
                    .ConvertFromString(widgetSettings.BackgroundColor)!;
            }



            TimeText.Foreground =
                (SolidColorBrush)new BrushConverter()
                .ConvertFromString(widgetSettings.EnglishTextColor)!;

            EnglishDayText.Foreground = TimeText.Foreground;
            EnglishDateText.Foreground = TimeText.Foreground;


            var tamilBrush =
                (SolidColorBrush)new BrushConverter()
                .ConvertFromString(widgetSettings.TamilTextColor)!;

            TamilDateText.Foreground = tamilBrush;
            TamilDayText.Foreground = tamilBrush;


            WidgetBorder.CornerRadius =
                new CornerRadius(widgetSettings.CornerRadius);


            // Font settings

            var font =
                new System.Windows.Media.FontFamily(widgetSettings.FontFamily);

            TimeText.FontFamily = font;
            EnglishDayText.FontFamily = font;
            EnglishDateText.FontFamily = font;
            TamilDateText.FontFamily = font;
            TamilDayText.FontFamily = font;


            TimeText.FontSize = widgetSettings.FontSize;
            EnglishDayText.FontSize = widgetSettings.FontSize;
            EnglishDateText.FontSize = widgetSettings.FontSize;
            TamilDateText.FontSize = widgetSettings.FontSize;
            TamilDayText.FontSize = widgetSettings.FontSize;
        }

        public void RefreshWidget()
        {
            widgetSettings = WidgetSettingsService.Load();
            ApplyWidgetSettings();
            Topmost = widgetSettings.AlwaysOnTop;

            if (widgetSettings.StartWithWindows)
            {
                StartupService.EnableStartup();
            }
            else
            {
                StartupService.DisableStartup();
            }
        }

        private void MainWindow_LocationChanged(object? sender, EventArgs e)
        {
            SettingsService.Save(new WindowSettings
            {
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height
            });
        }

        private void LockWidget_Click(object sender, RoutedEventArgs e)
        {
            LockWidget();
        }

        private void UnlockWidget_Click(object sender, RoutedEventArgs e)
        {
            UnlockWidget();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow(this);
            window.Owner = this;
            window.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            DateTime now = DateTime.Now;

            // English
            TimeText.Text = now.ToString("hh:mm:ss tt");
            EnglishDayText.Text = now.ToString("dddd");
            EnglishDateText.Text = now.ToString("dd MMMM yyyy");

            // Tamil
            var tamil = tamilCalendarService.GetTamilDate(now);

            TamilDateText.Text = $"{tamil.TamilMonth} {tamil.TamilDay}";
            TamilDayText.Text = tamil.TamilWeekDay;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    

        protected override void OnClosed(EventArgs e)
        {
            trayService?.Dispose();
            base.OnClosed(e);
        }
       
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isLocked)
                return;

            DragMove();
        }

        public void LockWidget()
        {
            _isLocked = true;
            ResizeMode = ResizeMode.NoResize;
        }

        public void UnlockWidget()
        {
            _isLocked = false;
            ResizeMode = ResizeMode.CanResizeWithGrip;
        }


    }
}