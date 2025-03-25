using Microsoft.Win32;
using System;
using System.Globalization;
using System.Management;
using System.Security.Principal;
using System.Windows;

namespace Password_Generator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// This class represents the main application entry point and handles theme settings based on Windows system theme changes for Win 10/11.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Enum representing application themes.
        /// </summary>
        public enum AppTheme
        {
            Light,
            Dark,
            HighContrast
        }

        // Registry path to access Windows theme settings
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        // Registry value name that determines if light or dark mode is enabled
        private const string RegistryValueName = "AppsUseLightTheme";

        /// <summary>
        /// Enum representing Windows themes.
        /// </summary>
        private enum WindowsTheme
        {
            Light,
            Dark
        }

        /// <summary>
        /// Monitors the Windows theme setting and updates the application theme accordingly.
        /// </summary>
        public void WatchTheme()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            AppTheme appTheme;

            // WMI query to detect registry value changes related to theme settings
            string query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                currentUser.User.Value,
                RegistryKeyPath.Replace(@"\", @"\\"),
                RegistryValueName);
            try
            {
                // Watch for changes in the Windows theme settings
                var watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += (sender, args) =>
                {
                    WindowsTheme newWindowsTheme = GetWindowsTheme();

                    // Update application theme based on detected changes
                    if (newWindowsTheme == WindowsTheme.Dark)
                    {
                        appTheme = AppTheme.Dark;
                    }
                    else if (newWindowsTheme == WindowsTheme.Light)
                    {
                        appTheme = AppTheme.Light;
                    }
                    else
                    {
                        appTheme = AppTheme.Light;
                    }

                    // Apply new theme to application resources
                    this.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/{appTheme}.xaml", UriKind.Relative);
                };

                // Start listening for events
                watcher.Start();
            }
            catch (Exception)
            {
                //TODO
                // Exception handling (likely failure on older Windows versions like Windows 7)
            }

            // Set initial theme based on the current registry value
            WindowsTheme initialTheme = GetWindowsTheme();
            if (initialTheme == WindowsTheme.Dark)
            {
                appTheme = AppTheme.Dark;
            }
            else if (initialTheme == WindowsTheme.Light)
            {
                appTheme = AppTheme.Light;
            }
            else
            {
                appTheme = AppTheme.Light;
            }

            // Set the initial theme in the application resources
            this.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/{appTheme}.xaml", UriKind.Relative);
        }

        /// <summary>
        /// Retrieves the current Windows theme by checking the registry.
        /// </summary>
        /// <returns>Returns WindowsTheme.Light or WindowsTheme.Dark based on registry settings.</returns>
        private static WindowsTheme GetWindowsTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistryValueName);

                // Default to Light theme if value is missing
                if (registryValueObject == null)
                {
                    return WindowsTheme.Light;
                }

                int registryValue = (int)registryValueObject;
                return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
            }
        }

        /// <summary>
        /// Overrides the startup method to initialize the theme watcher when the application starts.
        /// </summary>
        /// <param name="e">Event arguments related to application startup.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Start monitoring theme changes
            WatchTheme();

            base.OnStartup(e);
        }
    }
}
