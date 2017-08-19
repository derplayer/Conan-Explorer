using ConanExplorer.Controls;

namespace ConanExplorer.Windows
{
    partial class ScriptEditorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptEditorWindow));
            this.listBox_ScriptFiles = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBox_ScriptMessages = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.helpSearchLAbel = new System.Windows.Forms.Label();
            this.TextBox_Search = new System.Windows.Forms.TextBox();
            this.Button_Search = new System.Windows.Forms.Button();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.panel_Preview = new System.Windows.Forms.Panel();
            this.pictureBox_MessagePreview = new System.Windows.Forms.PictureBox();
            this.comboBox_PreviewColor = new System.Windows.Forms.ComboBox();
            this.comboBox_PreviewType = new System.Windows.Forms.ComboBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.progressBar_Progress = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.decompressAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_FontSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_GenerateFont = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Format = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_DeFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.viewScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Row = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.switchRawEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox_ScriptFile = new ConanExplorer.Controls.FixedRichTextBox();
            this.richTextBox_ScriptMessage = new ConanExplorer.Controls.FixedRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.panel_Preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MessagePreview)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_ScriptFiles
            // 
            this.listBox_ScriptFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_ScriptFiles.FormattingEnabled = true;
            this.listBox_ScriptFiles.Location = new System.Drawing.Point(0, 0);
            this.listBox_ScriptFiles.Name = "listBox_ScriptFiles";
            this.listBox_ScriptFiles.Size = new System.Drawing.Size(170, 269);
            this.listBox_ScriptFiles.TabIndex = 2;
            this.listBox_ScriptFiles.SelectedIndexChanged += new System.EventHandler(this.listBox_ScriptFiles_SelectedIndexChanged);
            this.listBox_ScriptFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_ScriptFiles_MouseDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 36);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(950, 585);
            this.splitContainer1.SplitterDistance = 170;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBox_ScriptFiles);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBox_ScriptMessages);
            this.splitContainer2.Size = new System.Drawing.Size(170, 585);
            this.splitContainer2.SplitterDistance = 269;
            this.splitContainer2.TabIndex = 3;
            // 
            // listBox_ScriptMessages
            // 
            this.listBox_ScriptMessages.DisplayMember = "DisplayName";
            this.listBox_ScriptMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_ScriptMessages.FormattingEnabled = true;
            this.listBox_ScriptMessages.Location = new System.Drawing.Point(0, 0);
            this.listBox_ScriptMessages.Name = "listBox_ScriptMessages";
            this.listBox_ScriptMessages.Size = new System.Drawing.Size(170, 312);
            this.listBox_ScriptMessages.TabIndex = 0;
            this.listBox_ScriptMessages.SelectedIndexChanged += new System.EventHandler(this.listBox_ScriptMessages_SelectedIndexChanged);
            this.listBox_ScriptMessages.DoubleClick += new System.EventHandler(this.listBox_ScriptMessages_DoubleClick);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.searchPanel);
            this.splitContainer3.Panel1.Controls.Add(this.richTextBox_ScriptFile);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(776, 585);
            this.splitContainer3.SplitterDistance = 397;
            this.splitContainer3.TabIndex = 1;
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.searchPanel.Controls.Add(this.helpSearchLAbel);
            this.searchPanel.Controls.Add(this.TextBox_Search);
            this.searchPanel.Controls.Add(this.Button_Search);
            this.searchPanel.Location = new System.Drawing.Point(2, 534);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(377, 49);
            this.searchPanel.TabIndex = 3;
            this.searchPanel.Visible = false;
            // 
            // helpSearchLAbel
            // 
            this.helpSearchLAbel.AutoSize = true;
            this.helpSearchLAbel.Location = new System.Drawing.Point(2, 30);
            this.helpSearchLAbel.Name = "helpSearchLAbel";
            this.helpSearchLAbel.Size = new System.Drawing.Size(278, 13);
            this.helpSearchLAbel.TabIndex = 3;
            this.helpSearchLAbel.Text = "Protip: To close the search formular press \"Strg+F\" again!";
            // 
            // TextBox_Search
            // 
            this.TextBox_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Search.Location = new System.Drawing.Point(5, 5);
            this.TextBox_Search.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox_Search.Name = "TextBox_Search";
            this.TextBox_Search.Size = new System.Drawing.Size(216, 20);
            this.TextBox_Search.TabIndex = 1;
            this.TextBox_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_Search_KeyDown);
            // 
            // Button_Search
            // 
            this.Button_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Search.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_Search.Location = new System.Drawing.Point(228, 4);
            this.Button_Search.Name = "Button_Search";
            this.Button_Search.Size = new System.Drawing.Size(146, 22);
            this.Button_Search.TabIndex = 2;
            this.Button_Search.Text = "Search";
            this.Button_Search.UseVisualStyleBackColor = true;
            this.Button_Search.Click += new System.EventHandler(this.Button_Search_Click);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panel_Preview);
            this.splitContainer4.Panel1.Controls.Add(this.comboBox_PreviewColor);
            this.splitContainer4.Panel1.Controls.Add(this.comboBox_PreviewType);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.richTextBox_ScriptMessage);
            this.splitContainer4.Panel2.Controls.Add(this.button_Apply);
            this.splitContainer4.Size = new System.Drawing.Size(375, 585);
            this.splitContainer4.SplitterDistance = 256;
            this.splitContainer4.TabIndex = 1;
            // 
            // panel_Preview
            // 
            this.panel_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Preview.AutoScroll = true;
            this.panel_Preview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_Preview.Controls.Add(this.pictureBox_MessagePreview);
            this.panel_Preview.Location = new System.Drawing.Point(3, 30);
            this.panel_Preview.Name = "panel_Preview";
            this.panel_Preview.Size = new System.Drawing.Size(369, 223);
            this.panel_Preview.TabIndex = 3;
            // 
            // pictureBox_MessagePreview
            // 
            this.pictureBox_MessagePreview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_MessagePreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_MessagePreview.Name = "pictureBox_MessagePreview";
            this.pictureBox_MessagePreview.Size = new System.Drawing.Size(280, 95);
            this.pictureBox_MessagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_MessagePreview.TabIndex = 0;
            this.pictureBox_MessagePreview.TabStop = false;
            // 
            // comboBox_PreviewColor
            // 
            this.comboBox_PreviewColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_PreviewColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PreviewColor.FormattingEnabled = true;
            this.comboBox_PreviewColor.Items.AddRange(new object[] {
            "Blue",
            "Red",
            "Green",
            "Yellow"});
            this.comboBox_PreviewColor.Location = new System.Drawing.Point(274, 3);
            this.comboBox_PreviewColor.Name = "comboBox_PreviewColor";
            this.comboBox_PreviewColor.Size = new System.Drawing.Size(98, 21);
            this.comboBox_PreviewColor.TabIndex = 2;
            this.comboBox_PreviewColor.SelectedIndexChanged += new System.EventHandler(this.comboBox_PreviewColor_SelectedIndexChanged);
            // 
            // comboBox_PreviewType
            // 
            this.comboBox_PreviewType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_PreviewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PreviewType.Enabled = false;
            this.comboBox_PreviewType.FormattingEnabled = true;
            this.comboBox_PreviewType.Items.AddRange(new object[] {
            "Speech Window",
            "Selection Window"});
            this.comboBox_PreviewType.Location = new System.Drawing.Point(3, 3);
            this.comboBox_PreviewType.Name = "comboBox_PreviewType";
            this.comboBox_PreviewType.Size = new System.Drawing.Size(265, 21);
            this.comboBox_PreviewType.TabIndex = 1;
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(3, 299);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(369, 23);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Visible = false;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // progressBar_Progress
            // 
            this.progressBar_Progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar_Progress.Location = new System.Drawing.Point(12, 627);
            this.progressBar_Progress.Name = "progressBar_Progress";
            this.progressBar_Progress.Size = new System.Drawing.Size(950, 14);
            this.progressBar_Progress.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(974, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.decompressAllToolStripMenuItem,
            this.compressAllToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // decompressAllToolStripMenuItem
            // 
            this.decompressAllToolStripMenuItem.Name = "decompressAllToolStripMenuItem";
            this.decompressAllToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.decompressAllToolStripMenuItem.Text = "Decompress All";
            this.decompressAllToolStripMenuItem.Click += new System.EventHandler(this.decompressAllToolStripMenuItem_Click);
            // 
            // compressAllToolStripMenuItem
            // 
            this.compressAllToolStripMenuItem.Name = "compressAllToolStripMenuItem";
            this.compressAllToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.compressAllToolStripMenuItem.Text = "Compress All";
            this.compressAllToolStripMenuItem.Click += new System.EventHandler(this.compressAllToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_FontSettings,
            this.toolStripMenuItem_GenerateFont,
            this.toolStripMenuItem_Format,
            this.toolStripMenuItem_DeFormat,
            this.toolStripMenuItem3,
            this.viewScriptToolStripMenuItem,
            this.switchRawEditorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolStripMenuItem_FontSettings
            // 
            this.toolStripMenuItem_FontSettings.Enabled = false;
            this.toolStripMenuItem_FontSettings.Name = "toolStripMenuItem_FontSettings";
            this.toolStripMenuItem_FontSettings.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItem_FontSettings.Text = "Font Settings...";
            this.toolStripMenuItem_FontSettings.Click += new System.EventHandler(this.toolStripMenuItem_FontSettings_Click);
            // 
            // toolStripMenuItem_GenerateFont
            // 
            this.toolStripMenuItem_GenerateFont.Name = "toolStripMenuItem_GenerateFont";
            this.toolStripMenuItem_GenerateFont.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItem_GenerateFont.Text = "Generate Font...";
            this.toolStripMenuItem_GenerateFont.Visible = false;
            this.toolStripMenuItem_GenerateFont.Click += new System.EventHandler(this.toolStripMenuItem_GenerateFont_Click);
            // 
            // toolStripMenuItem_Format
            // 
            this.toolStripMenuItem_Format.Enabled = false;
            this.toolStripMenuItem_Format.Name = "toolStripMenuItem_Format";
            this.toolStripMenuItem_Format.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItem_Format.Text = "Format";
            this.toolStripMenuItem_Format.Click += new System.EventHandler(this.toolStripMenuItem_Format_Click);
            // 
            // toolStripMenuItem_DeFormat
            // 
            this.toolStripMenuItem_DeFormat.Enabled = false;
            this.toolStripMenuItem_DeFormat.Name = "toolStripMenuItem_DeFormat";
            this.toolStripMenuItem_DeFormat.Size = new System.Drawing.Size(220, 22);
            this.toolStripMenuItem_DeFormat.Text = "De-Format";
            this.toolStripMenuItem_DeFormat.Click += new System.EventHandler(this.toolStripMenuItem_DeFormat_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(217, 6);
            // 
            // viewScriptToolStripMenuItem
            // 
            this.viewScriptToolStripMenuItem.Name = "viewScriptToolStripMenuItem";
            this.viewScriptToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.viewScriptToolStripMenuItem.Text = "View Script...";
            this.viewScriptToolStripMenuItem.Click += new System.EventHandler(this.viewScriptToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateScriptToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.debugToolStripMenuItem.Text = "_Debug";
            this.debugToolStripMenuItem.Visible = false;
            // 
            // generateScriptToolStripMenuItem
            // 
            this.generateScriptToolStripMenuItem.Name = "generateScriptToolStripMenuItem";
            this.generateScriptToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.generateScriptToolStripMenuItem.Text = "Generate Script...";
            this.generateScriptToolStripMenuItem.Click += new System.EventHandler(this.generateScriptToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Row});
            this.statusStrip1.Location = new System.Drawing.Point(0, 648);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(974, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Row
            // 
            this.toolStripStatusLabel_Row.Name = "toolStripStatusLabel_Row";
            this.toolStripStatusLabel_Row.Size = new System.Drawing.Size(0, 17);
            // 
            // switchRawEditorToolStripMenuItem
            // 
            this.switchRawEditorToolStripMenuItem.Name = "switchRawEditorToolStripMenuItem";
            this.switchRawEditorToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.switchRawEditorToolStripMenuItem.Text = "Switch to Global Edit [TEST]";
            this.switchRawEditorToolStripMenuItem.Click += new System.EventHandler(this.switchRawEditorToolStripMenuItem_Click);
            // 
            // richTextBox_ScriptFile
            // 
            this.richTextBox_ScriptFile.DetectUrls = false;
            this.richTextBox_ScriptFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_ScriptFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_ScriptFile.HideSelection = false;
            this.richTextBox_ScriptFile.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_ScriptFile.Name = "richTextBox_ScriptFile";
            this.richTextBox_ScriptFile.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox_ScriptFile.Size = new System.Drawing.Size(397, 585);
            this.richTextBox_ScriptFile.TabIndex = 0;
            this.richTextBox_ScriptFile.Text = "";
            this.richTextBox_ScriptFile.WordWrap = false;
            this.richTextBox_ScriptFile.SelectionChanged += new System.EventHandler(this.richTextBox_ScriptFile_SelectionChanged);
            this.richTextBox_ScriptFile.TextChanged += new System.EventHandler(this.richTextBox_ScriptFile_TextChanged);
            this.richTextBox_ScriptFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox_ScriptFile_KeyDown);
            // 
            // richTextBox_ScriptMessage
            // 
            this.richTextBox_ScriptMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_ScriptMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_ScriptMessage.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_ScriptMessage.Name = "richTextBox_ScriptMessage";
            this.richTextBox_ScriptMessage.Size = new System.Drawing.Size(369, 319);
            this.richTextBox_ScriptMessage.TabIndex = 0;
            this.richTextBox_ScriptMessage.Text = "";
            this.richTextBox_ScriptMessage.TextChanged += new System.EventHandler(this.richTextBox_ScriptMessage_TextChanged);
            this.richTextBox_ScriptMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox_ScriptMessage_KeyDown);
            // 
            // ScriptEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 670);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.progressBar_Progress);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(500, 450);
            this.Name = "ScriptEditorWindow";
            this.Text = "Script Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.panel_Preview.ResumeLayout(false);
            this.panel_Preview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MessagePreview)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FixedRichTextBox richTextBox_ScriptFile;
        private System.Windows.Forms.ListBox listBox_ScriptFiles;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar progressBar_Progress;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem decompressAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_FontSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Format;
        private System.Windows.Forms.ToolStripMenuItem viewScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Row;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_DeFormat;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListBox listBox_ScriptMessages;
        private FixedRichTextBox richTextBox_ScriptMessage;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateScriptToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.PictureBox pictureBox_MessagePreview;
        private System.Windows.Forms.ComboBox comboBox_PreviewColor;
        private System.Windows.Forms.ComboBox comboBox_PreviewType;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_GenerateFont;
        private System.Windows.Forms.Panel panel_Preview;
        private System.Windows.Forms.Button Button_Search;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Panel searchPanel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label helpSearchLAbel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem switchRawEditorToolStripMenuItem;
    }
}
