namespace ConanExplorer.Windows
{
    partial class HardcodedTextWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardcodedTextWindow));
            this.listBox_HardcodedTexts = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_DefNewStr = new System.Windows.Forms.Button();
            this.button_RemoveSel = new System.Windows.Forms.Button();
            this.button_LoadBlob = new System.Windows.Forms.Button();
            this.textBox_Translation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_NewString = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_CurrentString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_OriginalString = new System.Windows.Forms.TextBox();
            this.button_Reset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_HardcodedTexts
            // 
            this.listBox_HardcodedTexts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_HardcodedTexts.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_HardcodedTexts.FormattingEnabled = true;
            this.listBox_HardcodedTexts.Location = new System.Drawing.Point(0, 0);
            this.listBox_HardcodedTexts.Name = "listBox_HardcodedTexts";
            this.listBox_HardcodedTexts.Size = new System.Drawing.Size(390, 237);
            this.listBox_HardcodedTexts.TabIndex = 0;
            this.listBox_HardcodedTexts.SelectedIndexChanged += new System.EventHandler(this.listBox_HardcodedTexts_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox_HardcodedTexts);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button_Reset);
            this.splitContainer1.Panel2.Controls.Add(this.button_DefNewStr);
            this.splitContainer1.Panel2.Controls.Add(this.button_RemoveSel);
            this.splitContainer1.Panel2.Controls.Add(this.button_LoadBlob);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_Translation);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_NewString);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_CurrentString);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_OriginalString);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel2MinSize = 315;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(710, 237);
            this.splitContainer1.SplitterDistance = 390;
            this.splitContainer1.TabIndex = 2;
            // 
            // button_DefNewStr
            // 
            this.button_DefNewStr.Location = new System.Drawing.Point(148, 176);
            this.button_DefNewStr.Name = "button_DefNewStr";
            this.button_DefNewStr.Size = new System.Drawing.Size(110, 23);
            this.button_DefNewStr.TabIndex = 10;
            this.button_DefNewStr.Text = "Default New String";
            this.button_DefNewStr.UseVisualStyleBackColor = true;
            this.button_DefNewStr.Click += new System.EventHandler(this.button_DefNewStr_Click);
            // 
            // button_RemoveSel
            // 
            this.button_RemoveSel.Enabled = false;
            this.button_RemoveSel.Location = new System.Drawing.Point(26, 176);
            this.button_RemoveSel.Name = "button_RemoveSel";
            this.button_RemoveSel.Size = new System.Drawing.Size(116, 23);
            this.button_RemoveSel.TabIndex = 9;
            this.button_RemoveSel.Text = "Remove selected";
            this.button_RemoveSel.UseVisualStyleBackColor = true;
            this.button_RemoveSel.Click += new System.EventHandler(this.button_RemoveSel_Click);
            // 
            // button_LoadBlob
            // 
            this.button_LoadBlob.Location = new System.Drawing.Point(26, 205);
            this.button_LoadBlob.Name = "button_LoadBlob";
            this.button_LoadBlob.Size = new System.Drawing.Size(262, 23);
            this.button_LoadBlob.TabIndex = 8;
            this.button_LoadBlob.Text = "Parse Binary";
            this.button_LoadBlob.UseVisualStyleBackColor = true;
            this.button_LoadBlob.Click += new System.EventHandler(this.button_LoadBlob_Click);
            // 
            // textBox_Translation
            // 
            this.textBox_Translation.Location = new System.Drawing.Point(25, 70);
            this.textBox_Translation.Name = "textBox_Translation";
            this.textBox_Translation.ReadOnly = true;
            this.textBox_Translation.Size = new System.Drawing.Size(263, 20);
            this.textBox_Translation.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Translation:";
            // 
            // textBox_NewString
            // 
            this.textBox_NewString.Location = new System.Drawing.Point(25, 109);
            this.textBox_NewString.Name = "textBox_NewString";
            this.textBox_NewString.Size = new System.Drawing.Size(263, 20);
            this.textBox_NewString.TabIndex = 5;
            this.textBox_NewString.TextChanged += new System.EventHandler(this.textBox_NewString_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "New String:";
            // 
            // textBox_CurrentString
            // 
            this.textBox_CurrentString.Location = new System.Drawing.Point(25, 150);
            this.textBox_CurrentString.Name = "textBox_CurrentString";
            this.textBox_CurrentString.ReadOnly = true;
            this.textBox_CurrentString.Size = new System.Drawing.Size(263, 20);
            this.textBox_CurrentString.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current String:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original String:";
            // 
            // textBox_OriginalString
            // 
            this.textBox_OriginalString.Location = new System.Drawing.Point(25, 31);
            this.textBox_OriginalString.Name = "textBox_OriginalString";
            this.textBox_OriginalString.ReadOnly = true;
            this.textBox_OriginalString.Size = new System.Drawing.Size(263, 20);
            this.textBox_OriginalString.TabIndex = 0;
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(256, 176);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(32, 23);
            this.button_Reset.TabIndex = 11;
            this.button_Reset.Text = "(R)";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // HardcodedTextWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 261);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(520, 300);
            this.Name = "HardcodedTextWindow";
            this.Text = "Hard-coded Text Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_HardcodedTexts;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox_NewString;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_CurrentString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_OriginalString;
        private System.Windows.Forms.TextBox textBox_Translation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_RemoveSel;
        private System.Windows.Forms.Button button_LoadBlob;
        private System.Windows.Forms.Button button_DefNewStr;
        private System.Windows.Forms.Button button_Reset;
    }
}