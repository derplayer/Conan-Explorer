namespace ConanExplorer.Windows
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompressSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.packSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unpackSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runModifiedImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopModifiedImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.buildModifiedImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertModifiedImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertToOriginalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileIndexViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lZBDeCompressorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new ConanExplorer.Controls.TriStateTreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.compareControl = new ConanExplorer.Controls.CompareControl();
            this.dynamicControl = new ConanExplorer.Controls.DynamicControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.projectToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressSelectedToolStripMenuItem,
            this.decompressSelectedToolStripMenuItem,
            this.toolStripMenuItem5,
            this.packSelectedToolStripMenuItem,
            this.unpackSelectedToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // compressSelectedToolStripMenuItem
            // 
            this.compressSelectedToolStripMenuItem.Name = "compressSelectedToolStripMenuItem";
            this.compressSelectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.compressSelectedToolStripMenuItem.Text = "Compress Selected";
            // 
            // decompressSelectedToolStripMenuItem
            // 
            this.decompressSelectedToolStripMenuItem.Name = "decompressSelectedToolStripMenuItem";
            this.decompressSelectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.decompressSelectedToolStripMenuItem.Text = "Decompress Selected";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(183, 6);
            // 
            // packSelectedToolStripMenuItem
            // 
            this.packSelectedToolStripMenuItem.Name = "packSelectedToolStripMenuItem";
            this.packSelectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.packSelectedToolStripMenuItem.Text = "Pack Selected";
            // 
            // unpackSelectedToolStripMenuItem
            // 
            this.unpackSelectedToolStripMenuItem.Name = "unpackSelectedToolStripMenuItem";
            this.unpackSelectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.unpackSelectedToolStripMenuItem.Text = "Unpack Selected";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runModifiedImageToolStripMenuItem,
            this.stopModifiedImageToolStripMenuItem,
            this.toolStripMenuItem3,
            this.buildModifiedImageToolStripMenuItem,
            this.revertModifiedImageToolStripMenuItem,
            this.revertToOriginalToolStripMenuItem,
            this.toolStripMenuItem2,
            this.deleteProjectToolStripMenuItem});
            this.projectToolStripMenuItem.Enabled = false;
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // runModifiedImageToolStripMenuItem
            // 
            this.runModifiedImageToolStripMenuItem.Name = "runModifiedImageToolStripMenuItem";
            this.runModifiedImageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.runModifiedImageToolStripMenuItem.Text = "Run Modified Image";
            this.runModifiedImageToolStripMenuItem.Click += new System.EventHandler(this.runModifiedImageToolStripMenuItem_Click);
            // 
            // stopModifiedImageToolStripMenuItem
            // 
            this.stopModifiedImageToolStripMenuItem.Enabled = false;
            this.stopModifiedImageToolStripMenuItem.Name = "stopModifiedImageToolStripMenuItem";
            this.stopModifiedImageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.stopModifiedImageToolStripMenuItem.Text = "Stop Modified Image";
            this.stopModifiedImageToolStripMenuItem.Click += new System.EventHandler(this.stopModifiedImageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(185, 6);
            // 
            // buildModifiedImageToolStripMenuItem
            // 
            this.buildModifiedImageToolStripMenuItem.Name = "buildModifiedImageToolStripMenuItem";
            this.buildModifiedImageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.buildModifiedImageToolStripMenuItem.Text = "Build Modified Image";
            this.buildModifiedImageToolStripMenuItem.Click += new System.EventHandler(this.buildModifiedImageToolStripMenuItem_Click);
            // 
            // revertModifiedImageToolStripMenuItem
            // 
            this.revertModifiedImageToolStripMenuItem.Name = "revertModifiedImageToolStripMenuItem";
            this.revertModifiedImageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.revertModifiedImageToolStripMenuItem.Text = "Revert To Last Build";
            this.revertModifiedImageToolStripMenuItem.Click += new System.EventHandler(this.revertModifiedImageToolStripMenuItem_Click);
            // 
            // revertToOriginalToolStripMenuItem
            // 
            this.revertToOriginalToolStripMenuItem.Name = "revertToOriginalToolStripMenuItem";
            this.revertToOriginalToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.revertToOriginalToolStripMenuItem.Text = "Revert To Original";
            this.revertToOriginalToolStripMenuItem.Click += new System.EventHandler(this.revertToOriginalToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 6);
            // 
            // deleteProjectToolStripMenuItem
            // 
            this.deleteProjectToolStripMenuItem.Name = "deleteProjectToolStripMenuItem";
            this.deleteProjectToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.deleteProjectToolStripMenuItem.Text = "Delete Project";
            this.deleteProjectToolStripMenuItem.ToolTipText = "This will revert the whole project process and give back your original image";
            this.deleteProjectToolStripMenuItem.Click += new System.EventHandler(this.deleteProjectToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileIndexViewerToolStripMenuItem,
            this.fontEditorToolStripMenuItem,
            this.scriptEditorToolStripMenuItem,
            this.lZBDeCompressorToolStripMenuItem,
            this.toolStripMenuItem4,
            this.settingsToolStripMenuItem,
            this.debugToolStripMenuItem1});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.debugToolStripMenuItem.Text = "Tools";
            // 
            // fileIndexViewerToolStripMenuItem
            // 
            this.fileIndexViewerToolStripMenuItem.Name = "fileIndexViewerToolStripMenuItem";
            this.fileIndexViewerToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.fileIndexViewerToolStripMenuItem.Text = "File Index Viewer...";
            this.fileIndexViewerToolStripMenuItem.Click += new System.EventHandler(this.fileIndexViewerToolStripMenuItem_Click);
            // 
            // fontEditorToolStripMenuItem
            // 
            this.fontEditorToolStripMenuItem.Name = "fontEditorToolStripMenuItem";
            this.fontEditorToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.fontEditorToolStripMenuItem.Text = "Font Editor... [DEPRECATED]";
            this.fontEditorToolStripMenuItem.Click += new System.EventHandler(this.fontEditorToolStripMenuItem_Click);
            // 
            // scriptEditorToolStripMenuItem
            // 
            this.scriptEditorToolStripMenuItem.Name = "scriptEditorToolStripMenuItem";
            this.scriptEditorToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.scriptEditorToolStripMenuItem.Text = "Script Editor...";
            this.scriptEditorToolStripMenuItem.Click += new System.EventHandler(this.scriptEditorToolStripMenuItem_Click);
            // 
            // lZBDeCompressorToolStripMenuItem
            // 
            this.lZBDeCompressorToolStripMenuItem.Name = "lZBDeCompressorToolStripMenuItem";
            this.lZBDeCompressorToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.lZBDeCompressorToolStripMenuItem.Text = "LZB De/Compressor...";
            this.lZBDeCompressorToolStripMenuItem.Click += new System.EventHandler(this.lZBDeCompressorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(219, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem1
            // 
            this.debugToolStripMenuItem1.Name = "debugToolStripMenuItem1";
            this.debugToolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
            this.debugToolStripMenuItem1.Text = "_Debug";
            this.debugToolStripMenuItem1.Click += new System.EventHandler(this.debugToolStripMenuItem1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2MinSize = 310;
            this.splitContainer1.Size = new System.Drawing.Size(784, 512);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(302, 512);
            this.treeView1.TabIndex = 0;
            this.treeView1.TriStateStyleProperty = ConanExplorer.Controls.TriStateTreeView.TriStateStyles.Standard;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dynamicControl);
            this.splitContainer2.Size = new System.Drawing.Size(475, 512);
            this.splitContainer2.SplitterDistance = 125;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.compareControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 123);
            this.panel1.TabIndex = 0;
            // 
            // compareControl
            // 
            this.compareControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compareControl.Location = new System.Drawing.Point(0, 0);
            this.compareControl.Name = "compareControl";
            this.compareControl.Size = new System.Drawing.Size(473, 123);
            this.compareControl.TabIndex = 0;
            // 
            // dynamicControl
            // 
            this.dynamicControl.BackColor = System.Drawing.SystemColors.Control;
            this.dynamicControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicControl.EnabledBG = true;
            this.dynamicControl.EnabledLZB = true;
            this.dynamicControl.EnabledPB = true;
            this.dynamicControl.EnabledTIM = true;
            this.dynamicControl.Location = new System.Drawing.Point(0, 0);
            this.dynamicControl.MinimumSize = new System.Drawing.Size(300, 300);
            this.dynamicControl.Name = "dynamicControl";
            this.dynamicControl.Size = new System.Drawing.Size(473, 381);
            this.dynamicControl.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "MainWindow";
            this.Text = "Conan Explorer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private ConanExplorer.Controls.TriStateTreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildModifiedImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem revertModifiedImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runModifiedImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem stopModifiedImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertToOriginalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lZBDeCompressorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileIndexViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private Controls.DynamicControl dynamicControl;
        private Controls.CompareControl compareControl;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decompressSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem packSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unpackSelectedToolStripMenuItem;
    }
}

