namespace Password_Generator
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.passwordBox = new System.Windows.Forms.RichTextBox();
            this.passGenButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBoxExcludeAmbiguous = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeLowerCase = new System.Windows.Forms.CheckBox();
            this.checkBoxExcludeSimilar = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeSymbols = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeUpperCase = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeNumbers = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(280, 12);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.ReadOnly = true;
            this.passwordBox.Size = new System.Drawing.Size(292, 417);
            this.passwordBox.TabIndex = 0;
            this.passwordBox.Text = "";
            // 
            // passGenButton
            // 
            this.passGenButton.Location = new System.Drawing.Point(81, 193);
            this.passGenButton.Name = "passGenButton";
            this.passGenButton.Size = new System.Drawing.Size(120, 32);
            this.passGenButton.TabIndex = 1;
            this.passGenButton.Text = "Generate Password";
            this.passGenButton.UseVisualStyleBackColor = true;
            this.passGenButton.Click += new System.EventHandler(this.passGenButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.checkBoxExcludeAmbiguous);
            this.panel1.Controls.Add(this.checkBoxIncludeLowerCase);
            this.panel1.Controls.Add(this.checkBoxExcludeSimilar);
            this.panel1.Controls.Add(this.checkBoxIncludeSymbols);
            this.panel1.Controls.Add(this.checkBoxIncludeUpperCase);
            this.panel1.Controls.Add(this.checkBoxIncludeNumbers);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 175);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Length:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "80",
            "81",
            "82",
            "83",
            "84",
            "85",
            "86",
            "87",
            "88",
            "89",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99",
            "100",
            "101",
            "102",
            "103",
            "104",
            "105",
            "106",
            "107",
            "108",
            "109",
            "110",
            "111",
            "112",
            "113",
            "114",
            "115",
            "116",
            "117",
            "118",
            "119",
            "120",
            "121",
            "122",
            "123",
            "124",
            "125",
            "126",
            "127",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "16384"});
            this.comboBox1.Location = new System.Drawing.Point(52, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(94, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // checkBoxExcludeAmbiguous
            // 
            this.checkBoxExcludeAmbiguous.AutoSize = true;
            this.checkBoxExcludeAmbiguous.Location = new System.Drawing.Point(6, 144);
            this.checkBoxExcludeAmbiguous.Name = "checkBoxExcludeAmbiguous";
            this.checkBoxExcludeAmbiguous.Size = new System.Drawing.Size(247, 17);
            this.checkBoxExcludeAmbiguous.TabIndex = 12;
            this.checkBoxExcludeAmbiguous.Text = "Exclude Ambiguous  { } [ ] ( ) / \\ \' \" ` ~ , ; : . < >";
            this.checkBoxExcludeAmbiguous.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeLowerCase
            // 
            this.checkBoxIncludeLowerCase.AutoSize = true;
            this.checkBoxIncludeLowerCase.Location = new System.Drawing.Point(6, 30);
            this.checkBoxIncludeLowerCase.Name = "checkBoxIncludeLowerCase";
            this.checkBoxIncludeLowerCase.Size = new System.Drawing.Size(224, 17);
            this.checkBoxIncludeLowerCase.TabIndex = 7;
            this.checkBoxIncludeLowerCase.Text = "Include Lowercase Characters         abc...";
            this.checkBoxIncludeLowerCase.UseVisualStyleBackColor = true;
            // 
            // checkBoxExcludeSimilar
            // 
            this.checkBoxExcludeSimilar.AutoSize = true;
            this.checkBoxExcludeSimilar.Location = new System.Drawing.Point(6, 121);
            this.checkBoxExcludeSimilar.Name = "checkBoxExcludeSimilar";
            this.checkBoxExcludeSimilar.Size = new System.Drawing.Size(256, 17);
            this.checkBoxExcludeSimilar.TabIndex = 11;
            this.checkBoxExcludeSimilar.Text = "Exclude Similar Characters           i, l, 1, L, o, 0, O";
            this.checkBoxExcludeSimilar.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeSymbols
            // 
            this.checkBoxIncludeSymbols.AutoSize = true;
            this.checkBoxIncludeSymbols.Location = new System.Drawing.Point(6, 98);
            this.checkBoxIncludeSymbols.Name = "checkBoxIncludeSymbols";
            this.checkBoxIncludeSymbols.Size = new System.Drawing.Size(237, 17);
            this.checkBoxIncludeSymbols.TabIndex = 10;
            this.checkBoxIncludeSymbols.Text = "Include Symbols:                              @#!$%^";
            this.checkBoxIncludeSymbols.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeUpperCase
            // 
            this.checkBoxIncludeUpperCase.AutoSize = true;
            this.checkBoxIncludeUpperCase.Location = new System.Drawing.Point(6, 53);
            this.checkBoxIncludeUpperCase.Name = "checkBoxIncludeUpperCase";
            this.checkBoxIncludeUpperCase.Size = new System.Drawing.Size(227, 17);
            this.checkBoxIncludeUpperCase.TabIndex = 8;
            this.checkBoxIncludeUpperCase.Text = "Include Uppercase Characters         ABC...";
            this.checkBoxIncludeUpperCase.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeNumbers
            // 
            this.checkBoxIncludeNumbers.AutoSize = true;
            this.checkBoxIncludeNumbers.Location = new System.Drawing.Point(6, 75);
            this.checkBoxIncludeNumbers.Name = "checkBoxIncludeNumbers";
            this.checkBoxIncludeNumbers.Size = new System.Drawing.Size(226, 17);
            this.checkBoxIncludeNumbers.TabIndex = 9;
            this.checkBoxIncludeNumbers.Text = "Include Numbers                              123... ";
            this.checkBoxIncludeNumbers.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.passGenButton);
            this.Controls.Add(this.passwordBox);
            this.Name = "Main";
            this.Text = "Password Generator";
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox passwordBox;
        private System.Windows.Forms.Button passGenButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxExcludeAmbiguous;
        private System.Windows.Forms.CheckBox checkBoxExcludeSimilar;
        private System.Windows.Forms.CheckBox checkBoxIncludeSymbols;
        private System.Windows.Forms.CheckBox checkBoxIncludeNumbers;
        private System.Windows.Forms.CheckBox checkBoxIncludeUpperCase;
        private System.Windows.Forms.CheckBox checkBoxIncludeLowerCase;
    }
}