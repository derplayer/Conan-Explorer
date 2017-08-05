namespace ConanExplorer.Windows
{
    partial class ScriptViewerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptViewerWindow));
            this.richTextBox_Script = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox_Script
            // 
            this.richTextBox_Script.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Script.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.richTextBox_Script.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_Script.Name = "richTextBox_Script";
            this.richTextBox_Script.ReadOnly = true;
            this.richTextBox_Script.Size = new System.Drawing.Size(284, 261);
            this.richTextBox_Script.TabIndex = 0;
            this.richTextBox_Script.Text = "";
            // 
            // ScriptViewerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.richTextBox_Script);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScriptViewerWindow";
            this.Text = "Script Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_Script;
    }
}