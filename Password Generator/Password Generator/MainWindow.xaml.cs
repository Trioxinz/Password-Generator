using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Password_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random rnd = new Random();
        static string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        static string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string numbers = "1234567890";
        static string symbols = "!" + "\"" + "\\" + "#$%'(&)*+,-./:;<=>?[@]^_`{|}~";
        public static double score = 0.0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, EventArgs e)
        {
            selectedLengthComboBox.SelectedIndex = 0;
            checkBoxIncludeLowerCase.IsChecked = true;
        }

        private int boolCount(params bool[] booleans)
        {
            return booleans.Count(b => b);
        }

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

        private void checkBoxIncludeSymbols_Check(object sender, RoutedEventArgs e)
        {
            notAllowGroupRepeatDisableCheck();
        }

        private void checkBoxIncludeNumbers_Check(object sender, RoutedEventArgs e)
        {
            notAllowGroupRepeatDisableCheck();
        }

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

        private void onPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }
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