using Microsoft.Win32;
using System.Reflection;

namespace TamilDesktopWidget.Services
{
    public static class StartupService
    {
        private const string AppName = "TamilDesktopWidget";

        public static void EnableStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                true);
           
            if (key != null)
            {
                key.SetValue(AppName, Environment.ProcessPath!);
            }
        }

        public static void DisableStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                true);

            key?.DeleteValue(AppName, false);
        }

        public static bool IsStartupEnabled()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run");

            return key?.GetValue(AppName) != null;
        }
    }
}