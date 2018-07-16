namespace ConanExplorer.Windows
{
    partial class TIMEncodingWindow
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_MagicPinkTransparent = new System.Windows.Forms.CheckBox();
            this.checkBox_BlackTransparent = new System.Windows.Forms.CheckBox();
            this.checkBox_SetSemiTransparencyBit = new System.Windows.Forms.CheckBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.checkBox_UseOriginalTransparencyBits = new System.Windows.Forms.CheckBox();
            this.checkBox_UseOriginalColor = new System.Windows.Forms.CheckBox();
            this.checkBox_UseOriginalCLUT = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_UseOriginalCLUT);
            this.groupBox1.Controls.Add(this.checkBox_UseOriginalColor);
            this.groupBox1.Controls.Add(this.checkBox_UseOriginalTransparencyBits);
            this.groupBox1.Controls.Add(this.checkBox_MagicPinkTransparent);
            this.groupBox1.Controls.Add(this.checkBox_BlackTransparent);
            this.groupBox1.Controls.Add(this.checkBox_SetSemiTransparencyBit);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // checkBox_MagicPinkTransparent
            // 
            this.checkBox_MagicPinkTransparent.AutoSize = true;
            this.checkBox_MagicPinkTransparent.Location = new System.Drawing.Point(22, 74);
            this.checkBox_MagicPinkTransparent.Name = "checkBox_MagicPinkTransparent";
            this.checkBox_MagicPinkTransparent.Size = new System.Drawing.Size(139, 17);
            this.checkBox_MagicPinkTransparent.TabIndex = 4;
            this.checkBox_MagicPinkTransparent.Text = "Magic Pink Transparent";
            this.checkBox_MagicPinkTransparent.UseVisualStyleBackColor = true;
            // 
            // checkBox_BlackTransparent
            // 
            this.checkBox_BlackTransparent.AutoSize = true;
            this.checkBox_BlackTransparent.Location = new System.Drawing.Point(22, 51);
            this.checkBox_BlackTransparent.Name = "checkBox_BlackTransparent";
            this.checkBox_BlackTransparent.Size = new System.Drawing.Size(113, 17);
            this.checkBox_BlackTransparent.TabIndex = 3;
            this.checkBox_BlackTransparent.Text = "Black Transparent";
            this.checkBox_BlackTransparent.UseVisualStyleBackColor = true;
            // 
            // checkBox_SetSemiTransparencyBit
            // 
            this.checkBox_SetSemiTransparencyBit.AutoSize = true;
            this.checkBox_SetSemiTransparencyBit.Location = new System.Drawing.Point(22, 28);
            this.checkBox_SetSemiTransparencyBit.Name = "checkBox_SetSemiTransparencyBit";
            this.checkBox_SetSemiTransparencyBit.Size = new System.Drawing.Size(151, 17);
            this.checkBox_SetSemiTransparencyBit.TabIndex = 2;
            this.checkBox_SetSemiTransparencyBit.Text = "Set Semi Transparency Bit";
            this.checkBox_SetSemiTransparencyBit.UseVisualStyleBackColor = true;
            // 
            // button_Apply
            // 
            this.button_Apply.Location = new System.Drawing.Point(12, 190);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(260, 23);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // checkBox_UseOriginalTransparencyBits
            // 
            this.checkBox_UseOriginalTransparencyBits.AutoSize = true;
            this.checkBox_UseOriginalTransparencyBits.Location = new System.Drawing.Point(22, 97);
            this.checkBox_UseOriginalTransparencyBits.Name = "checkBox_UseOriginalTransparencyBits";
            this.checkBox_UseOriginalTransparencyBits.Size = new System.Drawing.Size(171, 17);
            this.checkBox_UseOriginalTransparencyBits.TabIndex = 5;
            this.checkBox_UseOriginalTransparencyBits.Text = "Use Original Transparency Bits";
            this.checkBox_UseOriginalTransparencyBits.UseVisualStyleBackColor = true;
            // 
            // checkBox_UseOriginalColor
            // 
            this.checkBox_UseOriginalColor.AutoSize = true;
            this.checkBox_UseOriginalColor.Location = new System.Drawing.Point(22, 120);
            this.checkBox_UseOriginalColor.Name = "checkBox_UseOriginalColor";
            this.checkBox_UseOriginalColor.Size = new System.Drawing.Size(110, 17);
            this.checkBox_UseOriginalColor.TabIndex = 6;
            this.checkBox_UseOriginalColor.Text = "Use Original Color";
            this.checkBox_UseOriginalColor.UseVisualStyleBackColor = true;
            // 
            // checkBox_UseOriginalCLUT
            // 
            this.checkBox_UseOriginalCLUT.AutoSize = true;
            this.checkBox_UseOriginalCLUT.Location = new System.Drawing.Point(22, 143);
            this.checkBox_UseOriginalCLUT.Name = "checkBox_UseOriginalCLUT";
            this.checkBox_UseOriginalCLUT.Size = new System.Drawing.Size(114, 17);
            this.checkBox_UseOriginalCLUT.TabIndex = 7;
            this.checkBox_UseOriginalCLUT.Text = "Use Original CLUT";
            this.checkBox_UseOriginalCLUT.UseVisualStyleBackColor = true;
            // 
            // TIMEncodingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 225);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TIMEncodingWindow";
            this.Text = "TIM Encoding Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_MagicPinkTransparent;
        private System.Windows.Forms.CheckBox checkBox_BlackTransparent;
        private System.Windows.Forms.CheckBox checkBox_SetSemiTransparencyBit;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.CheckBox checkBox_UseOriginalTransparencyBits;
        private System.Windows.Forms.CheckBox checkBox_UseOriginalColor;
        private System.Windows.Forms.CheckBox checkBox_UseOriginalCLUT;
    }
}