namespace ConanExplorer.Windows
{
    partial class FontSettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontSettingsWindow));
            this.label_AllowedSymbols = new System.Windows.Forms.Label();
            this.textBox_AllowedSymbols = new System.Windows.Forms.TextBox();
            this.comboBox_Font = new System.Windows.Forms.ComboBox();
            this.label_Font = new System.Windows.Forms.Label();
            this.pictureBox_Preview = new System.Windows.Forms.PictureBox();
            this.label_Preview = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_FontDialog = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label_AllowedSplittedSymbols = new System.Windows.Forms.Label();
            this.textBox_AllowedSplittedSymbols = new System.Windows.Forms.TextBox();
            this.numericUpDown_FontSize = new System.Windows.Forms.NumericUpDown();
            this.label_PreviewText = new System.Windows.Forms.Label();
            this.textBox_PreviewText = new System.Windows.Forms.TextBox();
            this.comboBox_Presets = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label_AllowedSymbols
            // 
            this.label_AllowedSymbols.AutoSize = true;
            this.label_AllowedSymbols.Location = new System.Drawing.Point(3, 6);
            this.label_AllowedSymbols.Name = "label_AllowedSymbols";
            this.label_AllowedSymbols.Size = new System.Drawing.Size(119, 13);
            this.label_AllowedSymbols.TabIndex = 0;
            this.label_AllowedSymbols.Text = "Allowed Symbols: (WIP)";
            // 
            // textBox_AllowedSymbols
            // 
            this.textBox_AllowedSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_AllowedSymbols.Enabled = false;
            this.textBox_AllowedSymbols.Location = new System.Drawing.Point(3, 22);
            this.textBox_AllowedSymbols.Multiline = true;
            this.textBox_AllowedSymbols.Name = "textBox_AllowedSymbols";
            this.textBox_AllowedSymbols.Size = new System.Drawing.Size(274, 146);
            this.textBox_AllowedSymbols.TabIndex = 1;
            // 
            // comboBox_Font
            // 
            this.comboBox_Font.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Font.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Font.FormattingEnabled = true;
            this.comboBox_Font.Location = new System.Drawing.Point(3, 16);
            this.comboBox_Font.Name = "comboBox_Font";
            this.comboBox_Font.Size = new System.Drawing.Size(183, 21);
            this.comboBox_Font.TabIndex = 2;
            this.comboBox_Font.SelectedIndexChanged += new System.EventHandler(this.comboBox_Font_SelectedIndexChanged);
            // 
            // label_Font
            // 
            this.label_Font.AutoSize = true;
            this.label_Font.Location = new System.Drawing.Point(3, 0);
            this.label_Font.Name = "label_Font";
            this.label_Font.Size = new System.Drawing.Size(31, 13);
            this.label_Font.TabIndex = 3;
            this.label_Font.Text = "Font:";
            // 
            // pictureBox_Preview
            // 
            this.pictureBox_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Preview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_Preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Preview.Location = new System.Drawing.Point(3, 79);
            this.pictureBox_Preview.Name = "pictureBox_Preview";
            this.pictureBox_Preview.Size = new System.Drawing.Size(339, 310);
            this.pictureBox_Preview.TabIndex = 4;
            this.pictureBox_Preview.TabStop = false;
            // 
            // label_Preview
            // 
            this.label_Preview.AutoSize = true;
            this.label_Preview.Location = new System.Drawing.Point(3, 62);
            this.label_Preview.Name = "label_Preview";
            this.label_Preview.Size = new System.Drawing.Size(48, 13);
            this.label_Preview.TabIndex = 5;
            this.label_Preview.Text = "Preview:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 43);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_FontDialog);
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.numericUpDown_FontSize);
            this.splitContainer1.Panel1.Controls.Add(this.label_Font);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox_Font);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label_PreviewText);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_PreviewText);
            this.splitContainer1.Panel2.Controls.Add(this.label_Preview);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox_Preview);
            this.splitContainer1.Size = new System.Drawing.Size(635, 395);
            this.splitContainer1.SplitterDistance = 286;
            this.splitContainer1.TabIndex = 7;
            // 
            // button_FontDialog
            // 
            this.button_FontDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_FontDialog.Location = new System.Drawing.Point(242, 15);
            this.button_FontDialog.Name = "button_FontDialog";
            this.button_FontDialog.Size = new System.Drawing.Size(38, 23);
            this.button_FontDialog.TabIndex = 8;
            this.button_FontDialog.Text = "...";
            this.button_FontDialog.UseVisualStyleBackColor = true;
            this.button_FontDialog.Click += new System.EventHandler(this.button_FontDialog_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 45);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBox_AllowedSymbols);
            this.splitContainer2.Panel1.Controls.Add(this.label_AllowedSymbols);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label_AllowedSplittedSymbols);
            this.splitContainer2.Panel2.Controls.Add(this.textBox_AllowedSplittedSymbols);
            this.splitContainer2.Size = new System.Drawing.Size(280, 347);
            this.splitContainer2.SplitterDistance = 171;
            this.splitContainer2.TabIndex = 7;
            // 
            // label_AllowedSplittedSymbols
            // 
            this.label_AllowedSplittedSymbols.AutoSize = true;
            this.label_AllowedSplittedSymbols.Location = new System.Drawing.Point(3, 6);
            this.label_AllowedSplittedSymbols.Name = "label_AllowedSplittedSymbols";
            this.label_AllowedSplittedSymbols.Size = new System.Drawing.Size(127, 13);
            this.label_AllowedSplittedSymbols.TabIndex = 6;
            this.label_AllowedSplittedSymbols.Text = "Allowed Splitted Symbols:";
            // 
            // textBox_AllowedSplittedSymbols
            // 
            this.textBox_AllowedSplittedSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_AllowedSplittedSymbols.Location = new System.Drawing.Point(3, 22);
            this.textBox_AllowedSplittedSymbols.Multiline = true;
            this.textBox_AllowedSplittedSymbols.Name = "textBox_AllowedSplittedSymbols";
            this.textBox_AllowedSplittedSymbols.Size = new System.Drawing.Size(274, 147);
            this.textBox_AllowedSplittedSymbols.TabIndex = 5;
            this.textBox_AllowedSplittedSymbols.TextChanged += new System.EventHandler(this.textBox_AllowedSplittedSymbols_TextChanged);
            // 
            // numericUpDown_FontSize
            // 
            this.numericUpDown_FontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_FontSize.Location = new System.Drawing.Point(192, 17);
            this.numericUpDown_FontSize.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_FontSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_FontSize.Name = "numericUpDown_FontSize";
            this.numericUpDown_FontSize.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_FontSize.TabIndex = 4;
            this.numericUpDown_FontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDown_FontSize.ValueChanged += new System.EventHandler(this.numericUpDown_FontSize_ValueChanged);
            // 
            // label_PreviewText
            // 
            this.label_PreviewText.AutoSize = true;
            this.label_PreviewText.Location = new System.Drawing.Point(3, 0);
            this.label_PreviewText.Name = "label_PreviewText";
            this.label_PreviewText.Size = new System.Drawing.Size(72, 13);
            this.label_PreviewText.TabIndex = 8;
            this.label_PreviewText.Text = "Preview Text:";
            // 
            // textBox_PreviewText
            // 
            this.textBox_PreviewText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PreviewText.Location = new System.Drawing.Point(6, 16);
            this.textBox_PreviewText.Multiline = true;
            this.textBox_PreviewText.Name = "textBox_PreviewText";
            this.textBox_PreviewText.Size = new System.Drawing.Size(336, 43);
            this.textBox_PreviewText.TabIndex = 7;
            this.textBox_PreviewText.TextChanged += new System.EventHandler(this.textBox_PreviewText_TextChanged);
            // 
            // comboBox_Presets
            // 
            this.comboBox_Presets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Presets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Presets.FormattingEnabled = true;
            this.comboBox_Presets.Items.AddRange(new object[] {
            "ASCII",
            "Latin1"});
            this.comboBox_Presets.Location = new System.Drawing.Point(61, 12);
            this.comboBox_Presets.Name = "comboBox_Presets";
            this.comboBox_Presets.Size = new System.Drawing.Size(586, 21);
            this.comboBox_Presets.TabIndex = 8;
            this.comboBox_Presets.SelectedIndexChanged += new System.EventHandler(this.comboBox_Presets_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Preset:";
            // 
            // FontSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_Presets);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "FontSettingsWindow";
            this.Text = "Font Settings";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Preview)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FontSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_AllowedSymbols;
        private System.Windows.Forms.TextBox textBox_AllowedSymbols;
        private System.Windows.Forms.ComboBox comboBox_Font;
        private System.Windows.Forms.Label label_Font;
        private System.Windows.Forms.PictureBox pictureBox_Preview;
        private System.Windows.Forms.Label label_Preview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown numericUpDown_FontSize;
        private System.Windows.Forms.Label label_AllowedSplittedSymbols;
        private System.Windows.Forms.TextBox textBox_AllowedSplittedSymbols;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label_PreviewText;
        private System.Windows.Forms.TextBox textBox_PreviewText;
        private System.Windows.Forms.Button button_FontDialog;
        private System.Windows.Forms.ComboBox comboBox_Presets;
        private System.Windows.Forms.Label label1;
    }
}