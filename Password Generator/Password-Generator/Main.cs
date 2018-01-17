using System;
using System.Collections;
using System.Windows.Forms;

namespace Password_Generator
{
    public partial class Main : Form
    {
        static Random rnd = new Random();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            checkBoxIncludeLowerCase.Checked = true;
        }

        private void passGenButton_Click(object sender, EventArgs e)
        {
            if (!checkBoxIncludeLowerCase.Checked && !checkBoxIncludeUpperCase.Checked && !checkBoxIncludeSymbols.Checked && !checkBoxIncludeNumbers.Checked)
            {
                MessageBox.Show("Error", "You need to select a type of character to include.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                array.Add("~");
                array.Add("`");
                array.Add("!");
                array.Add("@");
                array.Add("#");
                array.Add("$");
                array.Add("%");
                array.Add("^");
                array.Add("&");
                array.Add("*");
                array.Add("(");
                array.Add(")");
                array.Add("_");
                array.Add("=");
                array.Add("+");
                array.Add("{");
                array.Add("}");
                array.Add("[");
                array.Add("]");
                array.Add("|");
                array.Add("\\"); //need double \ for the string to take the \ char 
                array.Add("/");
                array.Add("?");
                array.Add("<");
                array.Add(">");
                array.Add(",");
                array.Add(".");
                array.Add(";");
                array.Add(":");
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
                array.Remove("`");
                array.Remove("~");
                array.Remove(",");
                array.Remove(";");
                array.Remove(":");
                array.Remove(".");
                array.Remove("<");
                array.Remove(">");
            }
            for (int i = 0; i < length; i++)
            {
                int j = rnd.Next(array.Count);
                if((checkBoxNotAllowRepeat.Checked) && (password.Length > 0))
                {
                    if(password.EndsWith(array[j].ToString()))
                    {
                        i--;
                    }
                    else
                    {
                        password = password + array[j].ToString();
                    }
                }
                else
                {
                    password = password + array[j].ToString();
                }
            }
            passwordBox.Text = password;
        }
    }
}
