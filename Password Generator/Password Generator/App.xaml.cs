using Microsoft.Win32;
using System;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

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
        private enum AppTheme
        {
            Light,
            Dark,
            HighContrastBlack,
            HighContrastWhite,
            HighContrast1,
            HighContrast2
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
        /// Imports the GetSysColor function from user32.dll to retrieve system color settings.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern int GetSysColor(int nIndex);

        /// <summary>
        /// Retrieves the specified system color and converts it to a WPF Color object.
        /// </summary>
        /// <param name="colorIndex">The system color index to retrieve.</param>
        /// <returns>A Color object representing the system color.</returns>

        private static Color GetSystemColor(int colorIndex)
        {
            int color = GetSysColor(colorIndex);
            return Color.FromRgb((byte)(color & 0xFF), (byte)((color >> 8) & 0xFF), (byte)((color >> 16) & 0xFF));
        }

        /// <summary>
        /// Monitors the Windows theme setting and updates the application theme accordingly.
        /// </summary>
        public void WatchTheme()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            AppTheme appTheme = GetCurrentAppTheme();

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
                    ApplyTheme(GetCurrentAppTheme());
                };

                // Start listening for events
                watcher.Start();
            }
            catch (Exception)
            {
                //TODO
                // Exception handling (likely failure on older Windows versions like Windows 7)
            }


            SystemParameters.StaticPropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(SystemParameters.HighContrast))
                {
                    ApplyTheme(GetCurrentAppTheme());
                }
            };

            // Set the initial theme in the application resources
            ApplyTheme(appTheme);
        }

        /// <summary>
        /// Applies the specified application theme by updating the resource dictionary.
        /// </summary>
        /// <param name="appTheme">The theme to be applied.</param>
        private void ApplyTheme(AppTheme appTheme)
        {
            // Apply new theme to application resources
            this.Resources.MergedDictionaries[0].Source = new Uri($"/Themes/{appTheme}.xaml", UriKind.Relative);
        }

        /// <summary>
        /// Determines the current application theme based on system settings.
        /// </summary>
        /// <returns>The detected AppTheme enum value.</returns>
        private AppTheme GetCurrentAppTheme()
        {
            // Check if High Contrast mode is enabled
            if (SystemParameters.HighContrast)
            {
                // Try detecting High Contrast theme via registry-based detection
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Accessibility\HighContrast"))
                {
                    if (key != null)
                    {
                        object themeName = key.GetValue("High Contrast Scheme");

                        if (themeName != null)
                        {
                            string theme = themeName.ToString().ToLowerInvariant();
                            if (theme.Contains("black"))
                            {
                                return AppTheme.HighContrastBlack;
                            }
                            else if (theme.Contains("white"))
                            {
                                return AppTheme.HighContrastWhite; 
                            }
                            else if (theme.Contains("#1"))
                            {
                                return AppTheme.HighContrast1;
                            }
                            else if (theme.Contains("#2"))
                            {
                                return AppTheme.HighContrast2;
                            }
                        }
                    }
                }

                // Fallback: Detect High Contrast theme based on system colors
                Color background = GetSystemColor(5); // COLOR_WINDOW
                Color textColor = GetSystemColor(8);  // COLOR_WINDOWTEXT

                // Compare with known high contrast themes
                if (background == Colors.Black && textColor == Colors.White)
                {
                    return AppTheme.HighContrastBlack;
                }
                else if (background == Colors.White && textColor == Colors.Black)
                {
                    return AppTheme.HighContrastWhite;
                }
                else if (background == Color.FromRgb(0, 0, 0) && textColor == Color.FromRgb(255, 255, 0))// Example color for High Contrast #1
                {
                    return AppTheme.HighContrast1;
                }
                else if (background == Color.FromRgb(0, 0, 0) && textColor == Color.FromRgb(0, 255, 0))// Example color for High Contrast #2
                {
                    return AppTheme.HighContrast2;
                }
                else
                {
                    return AppTheme.HighContrastBlack; // Default fallback
                }
            }
            else
            { 
                // Detect standard Light/Dark mode from Windows settings
                WindowsTheme newWindowsTheme = GetWindowsTheme();

                // Return the corresponding AppTheme
                if (newWindowsTheme == WindowsTheme.Dark)
                {
                    return AppTheme.Dark;
                }
                else if (newWindowsTheme == WindowsTheme.Light)
                {
                    return AppTheme.Light;
                }
                else
                {
                    return AppTheme.Light; // Default to Light mode
                }
            }
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
