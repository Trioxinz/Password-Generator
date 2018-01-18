﻿using System;
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

        private void textBoxSymbols_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '!' || e.KeyChar == '"' || e.KeyChar == '\\'||
                e.KeyChar == ';' || e.KeyChar == '#' || e.KeyChar == '$' ||
                e.KeyChar == '%' || e.KeyChar == '(' || e.KeyChar == '\''||
                e.KeyChar == ')' || e.KeyChar == '&' || e.KeyChar == '*' ||
                e.KeyChar == '+' || e.KeyChar == ',' || e.KeyChar == '-' ||
                e.KeyChar == '.' || e.KeyChar == '/' || e.KeyChar == ':' ||
                e.KeyChar == '<' || e.KeyChar == '>' || e.KeyChar == '=' ||
                e.KeyChar == '?' || e.KeyChar == '[' || e.KeyChar == ']' ||
                e.KeyChar == '@' || e.KeyChar == '^' || e.KeyChar == '_' ||
                e.KeyChar == '{' || e.KeyChar == '}' || e.KeyChar == '|' ||
                e.KeyChar == '`' || e.KeyChar == '~' || e.KeyChar == '\b')
            {
                if(!(textBoxSymbols.Text.Contains(e.KeyChar.ToString())))
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
                if(textBoxSymbols.TextLength > 0)
                {
                    for(int i = 0; i < textBoxSymbols.TextLength; i++)
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
                array.Remove("`");
                array.Remove("~");
                array.Remove(",");
                array.Remove(";");
                array.Remove(":");
                array.Remove(".");
                array.Remove("<");
                array.Remove(">");
            }
            if(checkBoxNotAllowDuplicate.Checked && array.Count < length)
            {
                length = array.Count;
                comboBox1.SelectedIndex = (length - 4); 
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
                else
                {
                    password = password + array[j].ToString();
                }
            }
            passwordBox.Text = password;
        }
    }
}
