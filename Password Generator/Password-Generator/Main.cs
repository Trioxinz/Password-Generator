using System;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.ComponentModel;

namespace Password_Generator
{
    public partial class Main : Form
    {
        static Random rnd = new Random();
        public static double score = 0.0;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            checkBoxIncludeLowerCase.Checked = true;
        }

        private void checkBoxNotAllowRepeat_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNotAllowRepeat.Checked)
            {
                checkBoxNotAllowDuplicate.Enabled = false;
            }
            else
            {
                checkBoxNotAllowDuplicate.Enabled = true;
            }
        }

        private void checkBoxNotAllowDuplicate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNotAllowDuplicate.Checked)
            {
                checkBoxNotAllowRepeat.Enabled = false;
            }
            else
            {
                checkBoxNotAllowRepeat.Enabled = true;
            }
        }


        private void checkBoxIncludeLowerCase_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIncludeLowerCase.Checked)
            {
                checkBoxBeginWithLetter.Enabled = true;
            }
            else
            {
                beginWithLetterDisable();
            }
        }

        private void checkBoxIncludeUpperCase_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIncludeUpperCase.Checked)
            {
                checkBoxBeginWithLetter.Enabled = true;
            }
            else
            {
                beginWithLetterDisable();
            }
        }

        private void beginWithLetterDisable()
        {
            if (!checkBoxIncludeLowerCase.Checked && !checkBoxIncludeUpperCase.Checked)
            {
                if (checkBoxBeginWithLetter.Checked)
                {
                    checkBoxBeginWithLetter.Checked = false;
                }
                checkBoxBeginWithLetter.Enabled = false;
            }
        }

        private void textBoxSymbols_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '!' || e.KeyChar == '"' || e.KeyChar == '\\' ||
                e.KeyChar == ';' || e.KeyChar == '#' || e.KeyChar == '$' ||
                e.KeyChar == '%' || e.KeyChar == '(' || e.KeyChar == '\'' ||
                e.KeyChar == ')' || e.KeyChar == '&' || e.KeyChar == '*' ||
                e.KeyChar == '+' || e.KeyChar == ',' || e.KeyChar == '-' ||
                e.KeyChar == '.' || e.KeyChar == '/' || e.KeyChar == ':' ||
                e.KeyChar == '<' || e.KeyChar == '>' || e.KeyChar == '=' ||
                e.KeyChar == '?' || e.KeyChar == '[' || e.KeyChar == ']' ||
                e.KeyChar == '@' || e.KeyChar == '^' || e.KeyChar == '_' ||
                e.KeyChar == '{' || e.KeyChar == '}' || e.KeyChar == '|' ||
                e.KeyChar == '`' || e.KeyChar == '~' || e.KeyChar == '\b')
            {
                if (!(textBoxSymbols.Text.Contains(e.KeyChar.ToString())))
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

        private void updateProgressBar()
        {
            scoreCheck();
            if(score > progressBar1.Maximum)
            {
                score = progressBar1.Maximum;
            }
            if(score < 20)
            {
                labelPasswordStrength.Text = "Very Weak";
                ModifyProgressBarColor.SetState(progressBar1, 2);
            }
            else if (score >= 20 && score < 40)
            {
                labelPasswordStrength.Text = "Weak";
                ModifyProgressBarColor.SetState(progressBar1, 2);
            }
            else if (score >= 40 && score < 60)
            {
                labelPasswordStrength.Text = "Good";
                ModifyProgressBarColor.SetState(progressBar1, 3);
            }
            else if (score >= 60 && score < 80)
            {
                labelPasswordStrength.Text = "Very Good";
                ModifyProgressBarColor.SetState(progressBar1, 3);
            }
            else if (score >= 80 && score < 90)
            {
                labelPasswordStrength.Text = "Strong";
                ModifyProgressBarColor.SetState(progressBar1, 1);
            }
            else if (score <= 90 && score < 101)
            {
                labelPasswordStrength.Text = "Very Strong";
                ModifyProgressBarColor.SetState(progressBar1, 1);
            }
            else if(score <= 101)
            {
                labelPasswordStrength.Text = "Maybe Too Strong";
                ModifyProgressBarColor.SetState(progressBar1, 1);
            }
            // Wait 100 milliseconds.
            Thread.Sleep(100);
            progressBar1.Value = (int)score;
        }

        private void scoreCheck()
        {
            ArrayList array = passwordBreakDown();
            score = 0;
            int temp = 0;
            int length = int.Parse(comboBox1.SelectedItem.ToString());
            //Additions
            int numberOfCharactersScore = (length * 4);
            int numberOfLowerrcaseLettersScore = ((length - (int)array[0]) * 2);
            int numberOfUppercaseLettersScore = ((length - (int)array[1]) * 2);
            int numberOfNumbersScore = ((int)array[2] * 4);
            int numberOfSymbolsScore = ((int)array[3] * 6);
            int numberOfMiddleNumbersOrSymbols = ((int)array[4] * 2);
            for (int i = 0; i < array.Count; i++)
            {
                if ((int)array[i] > 0)
                {
                    temp++;
                }
            }
            int numberOfRequirementTypes = (temp * 2);

            score = (numberOfCharactersScore + numberOfLowerrcaseLettersScore + numberOfUppercaseLettersScore + numberOfNumbersScore + numberOfSymbolsScore + numberOfMiddleNumbersOrSymbols);
            //Deductions
            if ((checkBoxIncludeLowerCase.Checked || checkBoxIncludeUpperCase.Checked) && (!checkBoxIncludeSymbols.Checked && !checkBoxIncludeNumbers.Checked))
            {
                score -= ((int)array[0] + (int)array[1]);
            }
            if (checkBoxIncludeNumbers.Checked && (!checkBoxIncludeSymbols.Checked && !checkBoxIncludeLowerCase.Checked && !checkBoxIncludeUpperCase.Checked))
            {
                score -= ((int)array[2]);
            }
        }

        private ArrayList passwordBreakDown()
        {
            ArrayList array = new ArrayList();
            char[] sort = new char[passwordBox.Text.ToString().Length];
            int lowerCase = 0;
            int upperCase = 0;
            int numbers = 0;
            int symbols = 0;
            int middleNumbersOrSymbols = 0;
            for (int i = 0; i < passwordBox.Text.ToString().Length; i++)
            {
                sort[i] = passwordBox.Text.ToString().Substring(i, 1)[0];
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
                    if(i != 0 && i++ != passwordBox.Text.ToString().Length)
                    {
                        middleNumbersOrSymbols++;
                    }
                }
                else
                {
                    symbols++;
                    if (i != 0 && i++ != passwordBox.Text.ToString().Length)
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

        private void passGenButton_Click(object sender, EventArgs e)
        {
            if ((!checkBoxIncludeLowerCase.Checked && !checkBoxIncludeUpperCase.Checked && !checkBoxIncludeSymbols.Checked && !checkBoxIncludeNumbers.Checked) || (checkBoxIncludeSymbols.Checked && textBoxSymbols.TextLength == 0 && !checkBoxIncludeLowerCase.Checked && !checkBoxIncludeUpperCase.Checked && !checkBoxIncludeNumbers.Checked))
            {
                MessageBox.Show("You need to select a type of character to include.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string password = "";
            int length = int.Parse(comboBox1.SelectedItem.ToString());
            ArrayList array = new ArrayList();
            if (checkBoxIncludeLowerCase.Checked)
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
            if (checkBoxIncludeUpperCase.Checked)
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
            if (checkBoxIncludeNumbers.Checked)
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
            if (checkBoxIncludeSymbols.Checked)
            {
                if (textBoxSymbols.TextLength > 0)
                {
                    for (int i = 0; i < textBoxSymbols.TextLength; i++)
                    {
                        array.Add(textBoxSymbols.Text.Substring(i, 1));
                    }
                }
            }
            if (checkBoxExcludeSimilar.Checked)
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
            if (checkBoxExcludeAmbiguous.Checked)
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
            if (checkBoxNotAllowDuplicate.Checked && array.Count < length)
            {
                length = array.Count;
                comboBox1.SelectedIndex = (length - 4);
            }
            for (int i = 0; i < length; i++)
            {
                int j = rnd.Next(array.Count);
                if ((checkBoxNotAllowRepeat.Checked) && (password.Length > 0))
                {
                    if (password.EndsWith(array[j].ToString()))
                    {
                        i--;
                    }
                    else
                    {
                        password = password + array[j].ToString();
                    }
                }
                else if ((checkBoxNotAllowDuplicate.Checked) && (password.Length > 0))
                {
                    if (password.Contains(array[j].ToString()))
                    {
                        i--;
                    }
                    else
                    {
                        password = password + array[j].ToString();
                    }
                }
                else if ((checkBoxBeginWithLetter.Checked) && (password.Length == 0))
                {
                    if (Char.IsLetter(((array[j].ToString()).ToCharArray()[0])))
                    {
                        password = password + array[j].ToString();
                    }
                    else
                    {
                        i--;
                    }
                }
                else
                {
                    password = password + array[j].ToString();
                }
            }
            passwordBox.Text = password;
            updateProgressBar();
        }
    }

    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }

        /*
         * ModifyProgressBarColor.SetState(progressBarName, #);
         * the # is the SetState, 1 = normal (green); 2 = error (red); 3 = warning (yellow).
         */
    }
}