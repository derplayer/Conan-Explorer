namespace ConanExplorer.Windows
{
    partial class LockedCharactersWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LockedCharactersWindow));
            this.label_LoadingFont = new System.Windows.Forms.Label();
            this.listView_FontCharacters = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label_LoadingFont
            // 
            this.label_LoadingFont.AutoSize = true;
            this.label_LoadingFont.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_LoadingFont.Location = new System.Drawing.Point(12, 9);
            this.label_LoadingFont.Name = "label_LoadingFont";
            this.label_LoadingFont.Size = new System.Drawing.Size(78, 13);
            this.label_LoadingFont.TabIndex = 1;
            this.label_LoadingFont.Text = "Loading Font...";
            // 
            // listView_FontCharacters
            // 
            this.listView_FontCharacters.CheckBoxes = true;
            this.listView_FontCharacters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_FontCharacters.Location = new System.Drawing.Point(0, 0);
            this.listView_FontCharacters.Name = "listView_FontCharacters";
            this.listView_FontCharacters.Size = new System.Drawing.Size(284, 261);
            this.listView_FontCharacters.TabIndex = 0;
            this.listView_FontCharacters.UseCompatibleStateImageBehavior = false;
            // 
            // LockedCharactersWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label_LoadingFont);
            this.Controls.Add(this.listView_FontCharacters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LockedCharactersWindow";
            this.Text = "Locked Characters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LockedCharactersWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_LoadingFont;
        private System.Windows.Forms.ListView listView_FontCharacters;
    }
}