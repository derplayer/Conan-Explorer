namespace ConanExplorer.Controls
{
    partial class CLUTControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel_Palette = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanel_Palette
            // 
            this.flowLayoutPanel_Palette.AutoScroll = true;
            this.flowLayoutPanel_Palette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_Palette.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel_Palette.Name = "flowLayoutPanel_Palette";
            this.flowLayoutPanel_Palette.Size = new System.Drawing.Size(275, 199);
            this.flowLayoutPanel_Palette.TabIndex = 0;
            // 
            // CLUTControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel_Palette);
            this.Name = "CLUTControl";
            this.Size = new System.Drawing.Size(275, 199);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Palette;
    }
}
