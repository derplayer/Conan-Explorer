namespace ConanExplorer.Windows
{
    partial class FontEditorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontEditorWindow));
            this.button_LoadFont = new System.Windows.Forms.Button();
            this.button_GenerateFont = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox_CharacterMap = new System.Windows.Forms.PictureBox();
            this.textBox_Symbols = new System.Windows.Forms.TextBox();
            this.label_Symbols = new System.Windows.Forms.Label();
            this.button_Filter = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Character = new System.Windows.Forms.ToolStripStatusLabel();
            this.button_SaveFont = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CharacterMap)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_LoadFont
            // 
            this.button_LoadFont.Location = new System.Drawing.Point(12, 12);
            this.button_LoadFont.Name = "button_LoadFont";
            this.button_LoadFont.Size = new System.Drawing.Size(75, 23);
            this.button_LoadFont.TabIndex = 0;
            this.button_LoadFont.Text = "Load Font";
            this.button_LoadFont.UseVisualStyleBackColor = true;
            this.button_LoadFont.Click += new System.EventHandler(this.button_LoadFont_Click);
            // 
            // button_GenerateFont
            // 
            this.button_GenerateFont.Location = new System.Drawing.Point(93, 12);
            this.button_GenerateFont.Name = "button_GenerateFont";
            this.button_GenerateFont.Size = new System.Drawing.Size(122, 23);
            this.button_GenerateFont.TabIndex = 1;
            this.button_GenerateFont.Text = "Generate Font";
            this.button_GenerateFont.UseVisualStyleBackColor = true;
            this.button_GenerateFont.Click += new System.EventHandler(this.button_GenerateFont_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_CharacterMap);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 79);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(660, 260);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // pictureBox_CharacterMap
            // 
            this.pictureBox_CharacterMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_CharacterMap.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_CharacterMap.Name = "pictureBox_CharacterMap";
            this.pictureBox_CharacterMap.Size = new System.Drawing.Size(200, 100);
            this.pictureBox_CharacterMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_CharacterMap.TabIndex = 0;
            this.pictureBox_CharacterMap.TabStop = false;
            this.pictureBox_CharacterMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_CharacterMap_MouseMove);
            // 
            // textBox_Symbols
            // 
            this.textBox_Symbols.Location = new System.Drawing.Point(67, 48);
            this.textBox_Symbols.Name = "textBox_Symbols";
            this.textBox_Symbols.ReadOnly = true;
            this.textBox_Symbols.Size = new System.Drawing.Size(605, 20);
            this.textBox_Symbols.TabIndex = 8;
            this.textBox_Symbols.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜabcdefghijklmnopqrstuvwxyzäöüß ";
            // 
            // label_Symbols
            // 
            this.label_Symbols.AutoSize = true;
            this.label_Symbols.Location = new System.Drawing.Point(12, 51);
            this.label_Symbols.Name = "label_Symbols";
            this.label_Symbols.Size = new System.Drawing.Size(49, 13);
            this.label_Symbols.TabIndex = 9;
            this.label_Symbols.Text = "Symbols:";
            // 
            // button_Filter
            // 
            this.button_Filter.Location = new System.Drawing.Point(221, 12);
            this.button_Filter.Name = "button_Filter";
            this.button_Filter.Size = new System.Drawing.Size(111, 23);
            this.button_Filter.TabIndex = 10;
            this.button_Filter.Text = "Filter Combinations";
            this.button_Filter.UseVisualStyleBackColor = true;
            this.button_Filter.Click += new System.EventHandler(this.button_Filter_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Character});
            this.statusStrip.Location = new System.Drawing.Point(0, 351);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(684, 22);
            this.statusStrip.TabIndex = 11;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Character
            // 
            this.toolStripStatusLabel_Character.Name = "toolStripStatusLabel_Character";
            this.toolStripStatusLabel_Character.Size = new System.Drawing.Size(0, 17);
            // 
            // button_SaveFont
            // 
            this.button_SaveFont.Location = new System.Drawing.Point(338, 12);
            this.button_SaveFont.Name = "button_SaveFont";
            this.button_SaveFont.Size = new System.Drawing.Size(94, 23);
            this.button_SaveFont.TabIndex = 12;
            this.button_SaveFont.Text = "Save Font";
            this.button_SaveFont.UseVisualStyleBackColor = true;
            this.button_SaveFont.Click += new System.EventHandler(this.button_SaveFont_Click);
            // 
            // FontEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 373);
            this.Controls.Add(this.button_SaveFont);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.button_Filter);
            this.Controls.Add(this.label_Symbols);
            this.Controls.Add(this.textBox_Symbols);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button_GenerateFont);
            this.Controls.Add(this.button_LoadFont);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FontEditorWindow";
            this.Text = "Font Editor [DEPRECATED]";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CharacterMap)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_LoadFont;
        private System.Windows.Forms.Button button_GenerateFont;
        private System.Windows.Forms.PictureBox pictureBox_CharacterMap;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox textBox_Symbols;
        private System.Windows.Forms.Label label_Symbols;
        private System.Windows.Forms.Button button_Filter;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Character;
        private System.Windows.Forms.Button button_SaveFont;
    }
}
