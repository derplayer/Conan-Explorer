namespace ConanExplorer.Windows
{
    partial class SelectModeWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectModeWindow));
            this.radioButton_Mode1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_Mode0 = new System.Windows.Forms.RadioButton();
            this.radioButton_Mode2 = new System.Windows.Forms.RadioButton();
            this.button_Compress = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton_Mode1
            // 
            this.radioButton_Mode1.AutoSize = true;
            this.radioButton_Mode1.Enabled = false;
            this.radioButton_Mode1.Location = new System.Drawing.Point(19, 46);
            this.radioButton_Mode1.Name = "radioButton_Mode1";
            this.radioButton_Mode1.Size = new System.Drawing.Size(179, 17);
            this.radioButton_Mode1.TabIndex = 0;
            this.radioButton_Mode1.Text = "Mode 1 - 256 Offset, 257 Length";
            this.radioButton_Mode1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_Mode0);
            this.groupBox1.Controls.Add(this.radioButton_Mode2);
            this.groupBox1.Controls.Add(this.radioButton_Mode1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Compression Mode";
            // 
            // radioButton_Mode0
            // 
            this.radioButton_Mode0.AutoSize = true;
            this.radioButton_Mode0.Enabled = false;
            this.radioButton_Mode0.Location = new System.Drawing.Point(19, 23);
            this.radioButton_Mode0.Name = "radioButton_Mode0";
            this.radioButton_Mode0.Size = new System.Drawing.Size(147, 17);
            this.radioButton_Mode0.TabIndex = 2;
            this.radioButton_Mode0.Text = "Mode 0 - No Compression";
            this.radioButton_Mode0.UseVisualStyleBackColor = true;
            // 
            // radioButton_Mode2
            // 
            this.radioButton_Mode2.AutoSize = true;
            this.radioButton_Mode2.Checked = true;
            this.radioButton_Mode2.Location = new System.Drawing.Point(19, 69);
            this.radioButton_Mode2.Name = "radioButton_Mode2";
            this.radioButton_Mode2.Size = new System.Drawing.Size(179, 17);
            this.radioButton_Mode2.TabIndex = 1;
            this.radioButton_Mode2.TabStop = true;
            this.radioButton_Mode2.Text = "Mode 2 - 4096 Offset, 17 Length";
            this.radioButton_Mode2.UseVisualStyleBackColor = true;
            // 
            // button_Compress
            // 
            this.button_Compress.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Compress.Location = new System.Drawing.Point(12, 118);
            this.button_Compress.Name = "button_Compress";
            this.button_Compress.Size = new System.Drawing.Size(208, 23);
            this.button_Compress.TabIndex = 2;
            this.button_Compress.Text = "Compress";
            this.button_Compress.UseVisualStyleBackColor = true;
            this.button_Compress.Click += new System.EventHandler(this.button_Compress_Click);
            // 
            // SelectModeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 150);
            this.Controls.Add(this.button_Compress);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectModeWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_Mode1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_Mode0;
        private System.Windows.Forms.RadioButton radioButton_Mode2;
        private System.Windows.Forms.Button button_Compress;
    }
}