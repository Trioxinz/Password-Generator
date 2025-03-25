using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Password_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Constants
        static Random rnd = new Random();
        static string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        static string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string numbers = "1234567890";
        static string symbols = "!" + "\"" + "\\" + "#$%'(&)*+,-./:;<=>?[@]^_`{|}~";
        public static double score = 0.0;

        // Constants For Titlebar Color change
        private const int WM_SETTINGCHANGE = 0x1A;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_CAPTION_COLOR = 34;
        private const int DWMWA_BORDER_COLOR = 35;
        private const uint KEY_QUERY_VALUE = 0x0001;
        private static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(unchecked((int)0x80000001));
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_FRAMECHANGED = 0x0020;

        // P/Invoke declarations
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegOpenKeyEx(IntPtr hKey, string subKey, int ulOptions, uint samDesired, out IntPtr hkResult);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved, int lpType, ref int lpData, ref int lpcbData);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegCloseKey(IntPtr hKey);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// Initializes the main window and sets up the theme listener.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += (s, e) => initializeThemeListener();
        }

        /// <summary>
        /// Sets up a Windows message hook to listen for system theme changes.
        /// </summary>
        private void initializeThemeListener()
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(hwnd);
            if (hwndSource != null)
            {
                hwndSource.AddHook(WndProc);
            }

            applyThemeAwareTitleBar();
        }

        /// <summary>
        /// Processes Windows messages and updates the title bar when the system theme changes.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message identifier.</param>
        /// <param name="wParam">Additional message information.</param>
        /// <param name="lParam">Additional message information.</param>
        /// <param name="handled">Indicates whether the message was handled.</param>
        /// <returns>An IntPtr representing the result.</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SETTINGCHANGE)
            {
                string param = Marshal.PtrToStringAuto(lParam);
                if (param == "ImmersiveColorSet")
                {
                    applyThemeAwareTitleBar();
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Applies the appropriate title bar color and theme settings based on the Windows version and system theme.
        /// </summary>
        private void applyThemeAwareTitleBar()
        {
            WindowsVersion windowsVersion = getWindowsVersion();
            if (windowsVersion >= WindowsVersion.Windows10Pre20H1)
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                int darkMode = systemThemeIsDark() ? 1 : 0;
                if (windowsVersion == WindowsVersion.Windows10Pre20H1)
                {
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref darkMode, Marshal.SizeOf(darkMode));
                }
                else
                {
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, Marshal.SizeOf(darkMode));
                }

                if (windowsVersion >= WindowsVersion.Windows11)
                {
                    int color = systemThemeIsDark() ? unchecked((int)0xFF000000) : unchecked((int)0xFFFFFFFF); // Black for dark, White for light
                    DwmSetWindowAttribute(handle, DWMWA_CAPTION_COLOR, ref color, Marshal.SizeOf(color));
                    DwmSetWindowAttribute(handle, DWMWA_BORDER_COLOR, ref color, Marshal.SizeOf(color));
                }

                // Force window redraw
                SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }
        }

        /// <summary>
        /// Enum representing different versions of Windows.
        /// </summary>
        public enum WindowsVersion
        {
            Unsupported,
            Windows7,
            Windows8,
            Windows10Pre20H1,
            Windows10After20H1,
            Windows11
        }

        /// <summary>
        /// Determines the current version of Windows.
        /// </summary>
        /// <returns>A WindowsVersion enum value representing the detected Windows version.</returns>
        private WindowsVersion getWindowsVersion()
        {
            Version version = Environment.OSVersion.Version;

            if (version.Major == 5 && version.Minor == 1)
            {
                //Windows XP
                return WindowsVersion.Unsupported;
            }
            else if (version.Major == 6 && version.Minor == 0)
            {
                //Windows Vista
                return WindowsVersion.Unsupported;
            }
            else if (version.Major == 6 && version.Minor == 1)
            {
                //Windows 7
                return WindowsVersion.Windows7;
            }
            else if (version.Major == 6 && (version.Minor == 2 || version.Minor == 3))
            {
                //Windows 8   Minor = 2
                //Windows 8.1 Minor = 3
                return WindowsVersion.Windows8;
            }
            else if (version.Major == 10 && (version.Build < 18985 && version.Build >= 17763))
            {
                //Windows 10 Pre 20H1
                return WindowsVersion.Windows10Pre20H1;
            }
            else if (version.Major == 10 && (version.Build < 22000 && version.Build >= 18985))
            {
                //Windows 10 Pre 20H1
                return WindowsVersion.Windows10After20H1;
            }
            else if (version.Major == 10 && version.Build >= 22000)
            {
                //Windows 10 Pre 20H1
                return WindowsVersion.Windows11;
            }
            else
            {
                //Version Not Found
                return WindowsVersion.Unsupported;
            }
        }

        /// <summary>
        /// Checks if the system is currently using Dark Mode.
        /// </summary>
        /// <returns>True if the system theme is dark; otherwise, false.</returns>

        private bool systemThemeIsDark()
        {
            int isDarkMode = 0;
            int size = Marshal.SizeOf(typeof(int));
            var result = RegOpenKeyEx(HKEY_CURRENT_USER, @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", 0, KEY_QUERY_VALUE, out IntPtr hKey);

            if (result == 0)
            {
                RegQueryValueEx(hKey, "AppsUseLightTheme", 0, 0, ref isDarkMode, ref size);
                RegCloseKey(hKey);
            }

            return isDarkMode == 0; // 0 means Dark Mode, 1 means Light Mode
        }

        /// <summary>
        /// Initializes default checkbox states when the window loads.
        /// </summary>
        private void mainWindow_Loaded(object sender, EventArgs e)
        {
            selectedLengthComboBox.SelectedIndex = 12;
            checkBoxIncludeLowerCase.IsChecked = true;
            checkBoxIncludeUpperCase.IsChecked = true;
            checkBoxIncludeNumbers.IsChecked = true;
            checkBoxIncludeSymbols.IsChecked = true;
            checkBoxExcludeSimilar.IsChecked = true;
            checkBoxExcludeAmbiguous.IsChecked = true;
            checkBoxNotAllowDuplicate.IsChecked = true;
            checkBoxNotAllowGroupRepeat.IsChecked = true;
            checkBoxBeginWithLetter.IsChecked = true;
        }

        /// <summary>
        /// Counts the number of boolean values that are true.
        /// </summary>
        private int boolCount(params bool[] booleans)
        {
            return booleans.Count(b => b);
        }

        /// <summary>
        /// Ensures that Not Allow Repeat and Not Allow Duplicate are not enabled together.
        /// </summary>
        private void checkBoxNotAllowRepeat_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxNotAllowRepeat.IsChecked)
            {
                if ((bool)checkBoxNotAllowDuplicate.IsChecked)
                {
                    checkBoxNotAllowDuplicate.IsChecked = false;
                }
                checkBoxNotAllowDuplicate.IsEnabled = false;
            }
            else
            {
                checkBoxNotAllowDuplicate.IsEnabled = true;
            }
        }

        /// <summary>
        /// Ensures mutual exclusivity between Not Allow Duplicate and Not Allow Repeat.
        /// </summary>
        private void checkBoxNotAllowDuplicate_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxNotAllowDuplicate.IsChecked)
            {
                if ((bool)checkBoxNotAllowRepeat.IsChecked)
                {
                    checkBoxNotAllowRepeat.IsChecked = false;
                }
                checkBoxNotAllowRepeat.IsEnabled = false;
            }
            else if (!(bool)checkBoxNotAllowDuplicate.IsChecked && !(bool)checkBoxNotAllowGroupRepeat.IsChecked)
            {
                checkBoxNotAllowRepeat.IsEnabled = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event for checkBoxNotAllowGroupRepeat.
        /// Ensures that Not Allow Group Repeat and Not Allow Repeat are not enabled together,
        /// based on user selection.
        /// </summary>
        private void checkBoxNotAllowGroupRepeat_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxNotAllowGroupRepeat.IsChecked)
            {
                if ((bool)checkBoxNotAllowRepeat.IsChecked)
                {
                    checkBoxNotAllowRepeat.IsChecked = false;
                }
                checkBoxNotAllowRepeat.IsEnabled = false;
            }
            else if (!(bool)checkBoxNotAllowDuplicate.IsChecked && !(bool)checkBoxNotAllowGroupRepeat.IsChecked)
            {
                checkBoxNotAllowRepeat.IsEnabled = true;
            }
        }

        /// <summary>
        /// Validates and applies settings when the Include Lowercase checkbox is checked.
        /// </summary>
        private void checkBoxIncludeLowerCase_Check(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxIncludeLowerCase.IsChecked)
            {
                checkBoxBeginWithLetter.IsEnabled = true;
            }
            else
            {
                beginWithLetterDisable();
            }
            notAllowGroupRepeatDisableCheck();
        }

        /// <summary>
        /// Validates and applies settings when the Include Uppercase checkbox is checked.
        /// </summary>
        private void checkBoxIncludeUpperCase_Check(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxIncludeUpperCase.IsChecked)
            {
                checkBoxBeginWithLetter.IsEnabled = true;
            }
            else
            {
                beginWithLetterDisable();
            }
            notAllowGroupRepeatDisableCheck();
        }

        /// <summary>
        /// Validates and applies settings when the Include Symbols checkbox is checked.
        /// </summary>
        private void checkBoxIncludeSymbols_Check(object sender, RoutedEventArgs e)
        {
            notAllowGroupRepeatDisableCheck();
        }

        /// <summary>
        /// Validates and applies settings when the Include Numbers checkbox is checked.
        /// </summary>
        private void checkBoxIncludeNumbers_Check(object sender, RoutedEventArgs e)
        {
            notAllowGroupRepeatDisableCheck();
        }


        /// <summary>
        /// Disables related settings when Not Allow Group Repeat is disabled.
        /// </summary>
        private void notAllowGroupRepeatDisableCheck()
        {
            if (boolCount((bool)checkBoxIncludeLowerCase.IsChecked, (bool)checkBoxIncludeUpperCase.IsChecked, (bool)checkBoxIncludeNumbers.IsChecked, (bool)checkBoxIncludeSymbols.IsChecked) > 1)
            {
                checkBoxNotAllowGroupRepeat.IsEnabled = true;
            }
            else
            {
                if ((bool)checkBoxNotAllowGroupRepeat.IsChecked)
                {
                    checkBoxNotAllowGroupRepeat.IsChecked = false;
                }
                checkBoxNotAllowGroupRepeat.IsEnabled = false;
            }
        }

        /// <summary>
        /// Disables Begin With Letter option if neither lowercase nor uppercase letters are included.
        /// </summary>
        private void beginWithLetterDisable()
        {
            if ((bool)!checkBoxIncludeLowerCase.IsChecked && (bool)!checkBoxIncludeUpperCase.IsChecked)
            {
                if ((bool)checkBoxBeginWithLetter.IsChecked)
                {
                    checkBoxBeginWithLetter.IsChecked = false;
                }
                checkBoxBeginWithLetter.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles preview text input events for the Symbols text box.
        /// Ensures only valid symbol characters are accepted.
        /// </summary>
        private void textBoxSymbols_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char key = Convert.ToChar(e.Text);
            if (
                key == '!' || key == '"' || key == '\\'||
                key == ';' || key == '#' || key == '$' ||
                key == '%' || key == '(' || key == '\''||
                key == ')' || key == '&' || key == '*' ||
                key == '+' || key == ',' || key == '-' ||
                key == '.' || key == '/' || key == ':' ||
                key == '<' || key == '>' || key == '=' ||
                key == '?' || key == '[' || key == ']' ||
                key == '@' || key == '^' || key == '_' ||
                key == '{' || key == '}' || key == '|' ||
                key == '`' || key == '~' || key == '\b'
               )
            {
                if (!(textBoxSymbols.Text.Contains(e.Text)))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles preview key down events.
        /// Implements custom key-handling logic.
        /// This will block space presses
        /// </summary>
        private void onPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Handles preview key down events for a RichTextBox control.
        /// Implements custom key-handling logic specific to the RichTextBox.
        /// Blocks all key presses unless (ctrl+A) or (ctrl+C) is pressed.
        /// This will allow the user to easily copy the generated password from the application.
        /// </summary>
        private void onPreviewKeyDownRTB(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = false;
            }
            else if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Updates the password strength indicator based on computed score.
        /// </summary>
        private void updateProgressBar()
        {
            scoreCheck();
            if (score > scoreBar.Maximum)
            {
                score = scoreBar.Maximum;
            }
            if (score < 20)
            {
                labelPasswordStrength.Content = "Very Weak";
                scoreBar.Foreground = Brushes.DarkRed;
            }
            else if (score >= 20 && score < 40)
            {
                labelPasswordStrength.Content = "Weak";
                scoreBar.Foreground = Brushes.Red;
            }
            else if (score >= 40 && score < 60)
            {
                labelPasswordStrength.Content = "Good";
                scoreBar.Foreground = Brushes.Orange;
            }
            else if (score >= 60 && score < 80)
            {
                labelPasswordStrength.Content = "Very Good";
                scoreBar.Foreground = Brushes.Yellow;
            }
            else if (score >= 80 && score < 90)
            {
                labelPasswordStrength.Content = "Strong";
                scoreBar.Foreground = Brushes.YellowGreen;
            }
            else if (score <= 90 && score < 101)
            {
                labelPasswordStrength.Content = "Very Strong";
                scoreBar.Foreground = Brushes.GreenYellow;
            }
            else if (score <= 101)
            {
                labelPasswordStrength.Content = "Crazy Strong";
                scoreBar.Foreground = Brushes.Green;
            }
            // Wait 100 milliseconds.
            Thread.Sleep(100);
            scoreBar.Value = (int)score;
        }

        /// <summary>
        /// Evaluates and assigns a score based on specific criteria.
        /// </summary>
        private void scoreCheck()
        {
            ArrayList array = passwordBreakDown();
            score = 0;
            int temp = 0;
            int length = int.Parse(selectedLengthComboBox.SelectedItem.ToString());
            //Additions
            int numberOfCharactersScore = (length * 1);
            int numberOfLowerrcaseLettersScore = ((length - (int)array[0]) * 2);
            int numberOfUppercaseLettersScore = ((length - (int)array[1]) * 2);
            int numberOfNumbersScore = ((int)array[2] * 2);
            int numberOfSymbolsScore = ((int)array[3] * 4);
            int numberOfMiddleNumbersOrSymbols = ((int)array[4] * 2);
            for (int i = 0; i < 4; i++)
            {
                if ((int)array[i] > 1)
                {
                    temp++;
                }
            }
            int numberOfRequirementTypes = (temp * 2);

            score = (numberOfCharactersScore + numberOfLowerrcaseLettersScore + numberOfUppercaseLettersScore + numberOfNumbersScore + numberOfSymbolsScore + numberOfMiddleNumbersOrSymbols);
            //Deductions
            if (((bool)checkBoxIncludeLowerCase.IsChecked || (bool)checkBoxIncludeUpperCase.IsChecked) && ((bool)!checkBoxIncludeSymbols.IsChecked && (bool)!checkBoxIncludeNumbers.IsChecked))
            {
                score -= ((int)array[0] + (int)array[1]);
            }
            if ((bool)checkBoxIncludeNumbers.IsChecked && ((bool)!checkBoxIncludeUpperCase.IsChecked && (bool)!checkBoxIncludeLowerCase.IsChecked && ((bool)!checkBoxIncludeSymbols.IsChecked || ((bool)checkBoxIncludeSymbols.IsChecked) && (textBoxSymbols.Text.Length == 0))))
            {
                score -= ((int)array[4] * 2);
                score -= ((int)array[2]);
                score = (score / 2);
            }
        }

        /// <summary>
        /// Analyzes the breakdown of character types in the generated password.
        /// </summary>
        private ArrayList passwordBreakDown()
        {
            ArrayList array = new ArrayList();
            string temp = new TextRange(passwordBox.Document.ContentStart, passwordBox.Document.ContentEnd).Text;
            char[] sort = new char[temp.Length];
            int lowerCase = 0;
            int upperCase = 0;
            int numbers = 0;
            int symbols = 0;
            int middleNumbersOrSymbols = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                sort[i] = temp.Substring(i, 1)[0];
                if (char.IsLower(sort[i]))
                {
                    lowerCase++;
                }
                else if (char.IsUpper(sort[i]))
                {
                    upperCase++;
                }
                else if (char.IsNumber(sort[i]))
                {
                    numbers++;
                    if (i != 0 && i++ != temp.Length)
                    {
                        middleNumbersOrSymbols++;
                    }
                }
                else
                {
                    symbols++;
                    if (i != 0 && i++ != temp.Length)
                    {
                        middleNumbersOrSymbols++;
                    }
                }
            }
            array.Clear();
            array.Add(lowerCase);
            array.Add(upperCase);
            array.Add(numbers);
            array.Add(symbols);
            array.Add(middleNumbersOrSymbols);
            return array;
        }

        /// <summary>
        /// Checks if the same character group is being used.
        /// </summary>
        private bool sameGroupCheck(string newChar, string lastChar)
        {
            if (lowerCase.Contains(lastChar) && lowerCase.Contains(newChar))
            {
                return true;
            }
            else if (upperCase.Contains(lastChar) && upperCase.Contains(newChar))
            {
                return true;
            }
            else if (numbers.Contains(lastChar) && numbers.Contains(newChar))
            {
                return true;
            }
            else if (symbols.Contains(lastChar) && symbols.Contains(newChar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Generates a password based on selected criteria.
        /// </summary>
        private void passGenButton_Click(object sender, RoutedEventArgs e)
        {
            if (((bool)!checkBoxIncludeLowerCase.IsChecked && (bool)!checkBoxIncludeUpperCase.IsChecked && (bool)!checkBoxIncludeSymbols.IsChecked && (bool)!checkBoxIncludeNumbers.IsChecked) || ((bool)checkBoxIncludeSymbols.IsChecked && textBoxSymbols.Text.Length == 0 && (bool)!checkBoxIncludeLowerCase.IsChecked && (bool)!checkBoxIncludeUpperCase.IsChecked && (bool)!checkBoxIncludeNumbers.IsChecked))
            {
                MessageBox.Show("You need to select a type of character to include.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string password = "";
            ArrayList array = new ArrayList();
            string[] backup = new string[0];
            int length = int.Parse(selectedLengthComboBox.SelectedItem.ToString());
            if ((bool)checkBoxIncludeLowerCase.IsChecked)
            {
                array.Add("a");
                array.Add("b");
                array.Add("c");
                array.Add("d");
                array.Add("e");
                array.Add("f");
                array.Add("g");
                array.Add("h");
                array.Add("i");
                array.Add("j");
                array.Add("k");
                array.Add("l");
                array.Add("m");
                array.Add("n");
                array.Add("o");
                array.Add("p");
                array.Add("q");
                array.Add("r");
                array.Add("s");
                array.Add("t");
                array.Add("u");
                array.Add("v");
                array.Add("w");
                array.Add("x");
                array.Add("y");
                array.Add("z");
            }
            if ((bool)checkBoxIncludeUpperCase.IsChecked)
            {
                array.Add("A");
                array.Add("B");
                array.Add("C");
                array.Add("D");
                array.Add("E");
                array.Add("F");
                array.Add("G");
                array.Add("H");
                array.Add("I");
                array.Add("J");
                array.Add("K");
                array.Add("L");
                array.Add("M");
                array.Add("N");
                array.Add("O");
                array.Add("P");
                array.Add("Q");
                array.Add("R");
                array.Add("S");
                array.Add("T");
                array.Add("U");
                array.Add("V");
                array.Add("W");
                array.Add("X");
                array.Add("Y");
                array.Add("Z");
            }
            if ((bool)checkBoxIncludeNumbers.IsChecked)
            {
                array.Add("1");
                array.Add("2");
                array.Add("3");
                array.Add("4");
                array.Add("5");
                array.Add("6");
                array.Add("7");
                array.Add("8");
                array.Add("9");
                array.Add("0");
            }
            if ((bool)checkBoxIncludeSymbols.IsChecked)
            {
                if (textBoxSymbols.Text.Length > 0)
                {
                    for (int i = 0; i < textBoxSymbols.Text.Length; i++)
                    {
                        array.Add(textBoxSymbols.Text.Substring(i, 1));
                    }
                }
            }
            if ((bool)checkBoxExcludeSimilar.IsChecked)
            {
                array.Remove("i");
                array.Remove("1");
                array.Remove("l");
                array.Remove("L");
                array.Remove("|");
                array.Remove("o");
                array.Remove("O");
                array.Remove("0");
            }
            if ((bool)checkBoxExcludeAmbiguous.IsChecked)
            {
                array.Remove("{");
                array.Remove("}");
                array.Remove("[");
                array.Remove("]");
                array.Remove("(");
                array.Remove(")");
                array.Remove("\\");
                array.Remove("/");
                array.Remove("'");
                array.Remove("\"");
                array.Remove("`");
                array.Remove("~");
                array.Remove(",");
                array.Remove(";");
                array.Remove(":");
                array.Remove(".");
                array.Remove("<");
                array.Remove(">");
            }
            if ((bool)checkBoxNotAllowDuplicate.IsChecked)
            {
                backup = new string[array.Count];
                if (array.Count < length)
                {
                    length = array.Count;
                    selectedLengthComboBox.SelectedIndex = (length - 4);
                }
                if((bool)checkBoxNotAllowGroupRepeat.IsChecked)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        backup[i] = (array[i].ToString());
                    }
                }
            }
            int j;
            bool adding;
            int loopedTimes = 0;
            for (int i = 0; i < length;)
            {
                if(loopedTimes == 25 && (bool)checkBoxNotAllowGroupRepeat.IsChecked)
                {
                    password = "";
                    i = 0;
                    array.Clear();
                    for (int x = 0; x < length; x++)
                    {
                        array.Add(backup[x]);
                    }
                }
                j = rnd.Next(array.Count);
                adding = false;
                if (((bool)checkBoxNotAllowRepeat.IsChecked) && (password.Length > 0))
                {
                    if (!password.EndsWith(array[j].ToString()))
                    {
                        adding = true;
                    }
                }
                else if (((bool)checkBoxNotAllowDuplicate.IsChecked) && (password.Length > 0) && !(bool)checkBoxNotAllowGroupRepeat.IsChecked)
                {
                    if (!password.Contains(array[j].ToString()))
                    {
                        adding = true;
                    }
                }
                else if (((bool)checkBoxNotAllowDuplicate.IsChecked) && (password.Length > 0) && (bool)checkBoxNotAllowGroupRepeat.IsChecked)
                {
                    if (!password.Contains(array[j].ToString()) && !sameGroupCheck(array[j].ToString(), password.Substring(i - 1)))
                    {
                        adding = true;
                    }
                }
                else if((bool)checkBoxNotAllowGroupRepeat.IsChecked && (password.Length > 0) && !(bool)checkBoxNotAllowDuplicate.IsChecked)
                {
                    if (!sameGroupCheck(array[j].ToString(), password.Substring(i - 1)))
                    {
                        adding = true;
                    }
                }
                else if (((bool)checkBoxBeginWithLetter.IsChecked) && (password.Length == 0))
                {
                    if (Char.IsLetter(((array[j].ToString()).ToCharArray()[0])))
                    {
                        adding = true;
                    }
                }
                else
                {
                    adding = true;
                }
                if (adding)
                {
                    loopedTimes = 0;
                    i++;
                    password = password + array[j].ToString();
                    if ((bool)checkBoxNotAllowDuplicate.IsChecked)
                    {
                        array.RemoveAt(j);
                    }
                }
                else
                {
                    loopedTimes++;
                }
            }
            passwordBox.Document.Blocks.Clear();
            passwordBox.Document.Blocks.Add(new Paragraph(new Run(password)));
            updateProgressBar();
        }
    }
} 