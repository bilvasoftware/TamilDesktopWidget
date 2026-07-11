using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TamilDesktopWidget.Models;
using TamilDesktopWidget.Services;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;

namespace TamilDesktopWidget
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            settings = WidgetSettingsService.Load();

            SelectCard(SolidCard);

            LoadSettingsToUI();
        
        }
        private readonly MainWindow _mainWindow;
        private WidgetSettings settings;
        private void AppearanceButton_Click(object sender, RoutedEventArgs e)
        {
            AppearancePage.Visibility = Visibility.Visible;
            WidgetPage.Visibility = Visibility.Collapsed;
            AboutPage.Visibility = Visibility.Collapsed;
        }

        private void WidgetButton_Click(object sender, RoutedEventArgs e)
        {
            AppearancePage.Visibility = Visibility.Collapsed;
            WidgetPage.Visibility = Visibility.Visible;
            AboutPage.Visibility = Visibility.Collapsed;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AppearancePage.Visibility = Visibility.Collapsed;
            WidgetPage.Visibility = Visibility.Collapsed;
            AboutPage.Visibility = Visibility.Visible;
        }

        private void SolidCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectCard(SolidCard);

            settings.BackgroundMode = "Solid";
        }
        private void LoadSettingsToUI()
        {
            OpacitySlider.Value = settings.Opacity * 100;

            FontComboBox.Text = settings.FontFamily;

            FontSizeComboBox.Text =
                settings.FontSize.ToString();
            ImagePathText.Text = settings.BackgroundImage;
            CornerRadiusSlider.Value = settings.CornerRadius;

            WindowOpacitySlider.Value = settings.Opacity * 100;
            AlwaysOnTopCheckBox.IsChecked = settings.AlwaysOnTop;

            StartupCheckBox.IsChecked = settings.StartWithWindows;
            
        }

        private void TransparentCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectCard(TransparentCard);

            settings.BackgroundMode = "Transparent";
        }

        private void ImageCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectCard(ImageCard);

            settings.BackgroundMode = "Image";
        }

        private void SelectCard(Border selectedCard)
        {
            // Reset all cards

            SolidCard.BorderBrush = System.Windows.Media.Brushes.LightGray;
            TransparentCard.BorderBrush = System.Windows.Media.Brushes.LightGray;
            ImageCard.BorderBrush = System.Windows.Media.Brushes.LightGray;

            selectedCard.BorderBrush = System.Windows.Media.Brushes.DodgerBlue;

            SolidCard.BorderThickness = new Thickness(2);
            TransparentCard.BorderThickness = new Thickness(2);
            ImageCard.BorderThickness = new Thickness(2);

            // Highlight selected card

            selectedCard.BorderBrush = System.Windows.Media.Brushes.DodgerBlue;
            selectedCard.BorderThickness = new Thickness(3);
        }

        private string PickColor(string currentColor)
        {
            var dialog = new Forms.ColorDialog();

            dialog.FullOpen = true;

            dialog.Color = Drawing.ColorTranslator.FromHtml(currentColor);

            if (dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                return $"#{dialog.Color.R:X2}{dialog.Color.G:X2}{dialog.Color.B:X2}";
            }

            return currentColor;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSettings();

            WidgetSettingsService.Save(settings);

            _mainWindow.RefreshWidget();
        }
        private void UpdateSettings()
        {
            settings.Opacity =
                OpacitySlider.Value / 100;

            settings.FontFamily =
                FontComboBox.Text;

            if (double.TryParse(FontSizeComboBox.Text,
                out double size))
            {
                settings.FontSize = size;
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Font
            if (FontComboBox.SelectedItem is ComboBoxItem fontItem)
            {
                settings.FontFamily = fontItem.Content.ToString()!;
            }


            // Font Size
            if (FontSizeComboBox.SelectedItem is ComboBoxItem sizeItem)
            {
                settings.FontSize =
                    double.Parse(sizeItem.Content.ToString()!);
            }


            // Opacity
            settings.Opacity =
                OpacitySlider.Value / 100;


            WidgetSettingsService.Save(settings);


            _mainWindow.RefreshWidget();


            System.Windows.MessageBox.Show(
                "Settings Saved Successfully.",
                "Tamil Desktop Widget",
                MessageBoxButton.OK,
                MessageBoxImage.Information);


            Close();
        }

        private void BackgroundColorBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settings.BackgroundColor = PickColor(settings.BackgroundColor);

            BackgroundColorBox.Background =
                new SolidColorBrush(
                    (System.Windows.Media.Color)
                    System.Windows.Media.ColorConverter.ConvertFromString(settings.BackgroundColor)!);
        }

        private void EnglishColorBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settings.EnglishTextColor = PickColor(settings.EnglishTextColor);

            EnglishColorBox.Background =
                new SolidColorBrush(
                    (System.Windows.Media.Color)
                    System.Windows.Media.ColorConverter.ConvertFromString(settings.EnglishTextColor)!);
        }

        private void TamilColorBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settings.TamilTextColor = PickColor(settings.TamilTextColor);

            TamilColorBox.Background =
                new SolidColorBrush(
                    (System.Windows.Media.Color)
                    System.Windows.Media.ColorConverter.ConvertFromString(settings.TamilTextColor)!);
        }

        private void OpacitySlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.Opacity = OpacitySlider.Value / 100;

            WidgetSettingsService.Save(settings);

            _mainWindow.RefreshWidget();
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                settings.BackgroundImage = dialog.FileName;
                settings.BackgroundMode = "Image";
                WidgetSettingsService.Save(settings);


                ImagePathText.Text = dialog.FileName;

                SelectCard(ImageCard);
            }
        }

        private void CornerRadiusSlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.CornerRadius = CornerRadiusSlider.Value;

            WidgetSettingsService.Save(settings);

            _mainWindow.RefreshWidget();
        }

        private void WindowOpacitySlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (settings == null)
                return;

            settings.Opacity = WindowOpacitySlider.Value / 100;

            WidgetSettingsService.Save(settings);

            _mainWindow.RefreshWidget();
        }

        private void TopRightButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Left =
                SystemParameters.WorkArea.Right -
                _mainWindow.Width - 30;

            _mainWindow.Top = 30;
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Left =
                (SystemParameters.WorkArea.Width - _mainWindow.Width) / 2;

            _mainWindow.Top =
                (SystemParameters.WorkArea.Height - _mainWindow.Height) / 2;
        }

        private void AlwaysOnTopCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            settings.AlwaysOnTop =
                AlwaysOnTopCheckBox.IsChecked == true;

            _mainWindow.Topmost = settings.AlwaysOnTop;

            WidgetSettingsService.Save(settings);
        }

        private void StartupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            settings.StartWithWindows =
                StartupCheckBox.IsChecked == true;

            if (settings.StartWithWindows)
                StartupService.EnableStartup();
            else
                StartupService.DisableStartup();

            WidgetSettingsService.Save(settings);
        }
        public static void DisableStartup()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                true);

            key?.DeleteValue("TamilDesktopWidget", false);
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "mailto:bilvasoftware@gmail.com",
                UseShellExecute = true
            });
        }

        private void LinkedIn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.linkedin.com/in/bilva-software-aa532a421/",
                UseShellExecute = true
            });
        }

    }
}

    
