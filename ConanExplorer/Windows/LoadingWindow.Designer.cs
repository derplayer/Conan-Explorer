namespace ConanExplorer.Windows
{
    partial class LoadingWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingWindow));
            this.progressBar_Progress = new System.Windows.Forms.ProgressBar();
            this.label_DisplayText = new System.Windows.Forms.Label();
            this.timer_Update = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressBar_Progress
            // 
            this.progressBar_Progress.Location = new System.Drawing.Point(12, 26);
            this.progressBar_Progress.Name = "progressBar_Progress";
            this.progressBar_Progress.Size = new System.Drawing.Size(310, 23);
            this.progressBar_Progress.TabIndex = 0;
            // 
            // label_DisplayText
            // 
            this.label_DisplayText.AutoSize = true;
            this.label_DisplayText.Location = new System.Drawing.Point(12, 10);
            this.label_DisplayText.Name = "label_DisplayText";
            this.label_DisplayText.Size = new System.Drawing.Size(95, 13);
            this.label_DisplayText.TabIndex = 1;
            this.label_DisplayText.Text = "Doing something...";
            // 
            // timer_Update
            // 
            this.timer_Update.Enabled = true;
            this.timer_Update.Interval = 200;
            this.timer_Update.Tick += new System.EventHandler(this.timer_Update_Tick);
            // 
            // LoadingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 61);
            this.Controls.Add(this.label_DisplayText);
            this.Controls.Add(this.progressBar_Progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadingWindow";
            this.Text = "Loading...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar_Progress;
        private System.Windows.Forms.Label label_DisplayText;
        private System.Windows.Forms.Timer timer_Update;
    }
}