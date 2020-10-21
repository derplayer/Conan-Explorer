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
            this.button_SearchUp = new System.Windows.Forms.Button();
            this.checkBox_SearchGlobal = new System.Windows.Forms.CheckBox();
            this.textBox_Search = new System.Windows.Forms.TextBox();
            this.button_SearchDown = new System.Windows.Forms.Button();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.panel_Preview = new System.Windows.Forms.Panel();
            this.pictureBox_MessagePreviewAlt = new System.Windows.Forms.PictureBox();
            this.pictureBox_MessagePreview = new System.Windows.Forms.PictureBox();
            this.comboBox_PreviewColor = new System.Windows.Forms.ComboBox();
            this.comboBox_PreviewType = new System.Windows.Forms.ComboBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.progressBar_Progress = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_SaveNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_MultiCompress = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_MultiCompressDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_CompressAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_DecompressAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_FontSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_HardcodedText = new System.Windows.Forms.ToolStripMenuItem();
            this.resetHardcodedTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LockedCharacters = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_Format = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_DeFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_ViewScript = new System.Windows.Forms.ToolStripMenuItem();
            this.searchCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translateToEnglishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Debug = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_GenerateScript = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Row = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MessagePreviewAlt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MessagePreview)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_ScriptFiles
            // 
            this.listBox_ScriptFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_ScriptFiles.FormattingEnabled = true;
            this.listBox_ScriptFiles.IntegralHeight = false;
            this.listBox_ScriptFiles.Location = new System.Drawing.Point(0, 0);
            this.listBox_ScriptFiles.Name = "listBox_ScriptFiles";
            this.listBox_ScriptFiles.Size = new System.Drawing.Size(188, 269);
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
            this.splitContainer1.Size = new System.Drawing.Size(1054, 585);
            this.splitContainer1.SplitterDistance = 188;
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
            this.splitContainer2.Size = new System.Drawing.Size(188, 585);
            this.splitContainer2.SplitterDistance = 269;
            this.splitContainer2.TabIndex = 3;
            // 
            // listBox_ScriptMessages
            // 
            this.listBox_ScriptMessages.DisplayMember = "DisplayName";
            this.listBox_ScriptMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_ScriptMessages.FormattingEnabled = true;
            this.listBox_ScriptMessages.IntegralHeight = false;
            this.listBox_ScriptMessages.Location = new System.Drawing.Point(0, 0);
            this.listBox_ScriptMessages.Name = "listBox_ScriptMessages";
            this.listBox_ScriptMessages.Size = new System.Drawing.Size(188, 312);
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
            this.splitContainer3.Size = new System.Drawing.Size(862, 585);
            this.splitContainer3.SplitterDistance = 440;
            this.splitContainer3.TabIndex = 1;
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.searchPanel.Controls.Add(this.button_SearchUp);
            this.searchPanel.Controls.Add(this.checkBox_SearchGlobal);
            this.searchPanel.Controls.Add(this.textBox_Search);
            this.searchPanel.Controls.Add(this.button_SearchDown);
            this.searchPanel.Location = new System.Drawing.Point(2, 523);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(420, 60);
            this.searchPanel.TabIndex = 3;
            this.searchPanel.Visible = false;
            // 
            // button_SearchUp
            // 
            this.button_SearchUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SearchUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_SearchUp.Location = new System.Drawing.Point(334, 33);
            this.button_SearchUp.Name = "button_SearchUp";
            this.button_SearchUp.Size = new System.Drawing.Size(82, 22);
            this.button_SearchUp.TabIndex = 5;
            this.button_SearchUp.Text = "Search Up";
            this.button_SearchUp.UseVisualStyleBackColor = true;
            this.button_SearchUp.Click += new System.EventHandler(this.button_SearchUp_Click);
            // 
            // checkBox_SearchGlobal
            // 
            this.checkBox_SearchGlobal.AutoSize = true;
            this.checkBox_SearchGlobal.Location = new System.Drawing.Point(6, 35);
            this.checkBox_SearchGlobal.Name = "checkBox_SearchGlobal";
            this.checkBox_SearchGlobal.Size = new System.Drawing.Size(122, 17);
            this.checkBox_SearchGlobal.TabIndex = 4;
            this.checkBox_SearchGlobal.Text = "Search all script files";
            this.checkBox_SearchGlobal.UseVisualStyleBackColor = true;
            // 
            // textBox_Search
            // 
            this.textBox_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Search.Location = new System.Drawing.Point(5, 6);
            this.textBox_Search.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Search.Name = "textBox_Search";
            this.textBox_Search.Size = new System.Drawing.Size(322, 20);
            this.textBox_Search.TabIndex = 1;
            this.textBox_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Search_KeyDown);
            // 
            // button_SearchDown
            // 
            this.button_SearchDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SearchDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_SearchDown.Location = new System.Drawing.Point(334, 5);
            this.button_SearchDown.Name = "button_SearchDown";
            this.button_SearchDown.Size = new System.Drawing.Size(82, 22);
            this.button_SearchDown.TabIndex = 2;
            this.button_SearchDown.Text = "Search Down";
            this.button_SearchDown.UseVisualStyleBackColor = true;
            this.button_SearchDown.Click += new System.EventHandler(this.button_SearchDown_Click);
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
            this.splitContainer4.Size = new System.Drawing.Size(418, 585);
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
            this.panel_Preview.Controls.Add(this.pictureBox_MessagePreviewAlt);
            this.panel_Preview.Controls.Add(this.pictureBox_MessagePreview);
            this.panel_Preview.Location = new System.Drawing.Point(3, 30);
            this.panel_Preview.Name = "panel_Preview";
            this.panel_Preview.Size = new System.Drawing.Size(412, 223);
            this.panel_Preview.TabIndex = 3;
            // 
            // pictureBox_MessagePreviewAlt
            // 
            this.pictureBox_MessagePreviewAlt.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_MessagePreviewAlt.Location = new System.Drawing.Point(206, 0);
            this.pictureBox_MessagePreviewAlt.Name = "pictureBox_MessagePreviewAlt";
            this.pictureBox_MessagePreviewAlt.Size = new System.Drawing.Size(200, 95);
            this.pictureBox_MessagePreviewAlt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_MessagePreviewAlt.TabIndex = 1;
            this.pictureBox_MessagePreviewAlt.TabStop = false;
            // 
            // pictureBox_MessagePreview
            // 
            this.pictureBox_MessagePreview.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_MessagePreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_MessagePreview.Name = "pictureBox_MessagePreview";
            this.pictureBox_MessagePreview.Size = new System.Drawing.Size(200, 95);
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
            this.comboBox_PreviewColor.Location = new System.Drawing.Point(317, 3);
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
            this.comboBox_PreviewType.Size = new System.Drawing.Size(308, 21);
            this.comboBox_PreviewType.TabIndex = 1;
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(3, 299);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(412, 23);
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
            this.progressBar_Progress.Size = new System.Drawing.Size(1054, 14);
            this.progressBar_Progress.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_File,
            this.toolStripMenuItem_Tools,
            this.toolStripMenuItem_Debug});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1078, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip";
            // 
            // toolStripMenuItem_File
            // 
            this.toolStripMenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Open,
            this.toolStripMenuItem_SaveNormal,
            this.toolStripMenuItem_Save,
            this.toolStripMenuItem1,
            this.toolStripMenuItem_MultiCompress,
            this.toolStripMenuItem_MultiCompressDebug,
            this.toolStripMenuItem_CompressAll,
            this.toolStripMenuItem_DecompressAll,
            this.toolStripMenuItem4,
            this.toolStripMenuItem_Clear});
            this.toolStripMenuItem_File.Name = "toolStripMenuItem_File";
            this.toolStripMenuItem_File.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem_File.Text = "File";
            // 
            // toolStripMenuItem_Open
            // 
            this.toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
            this.toolStripMenuItem_Open.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_Open.Text = "Open...";
            this.toolStripMenuItem_Open.Click += new System.EventHandler(this.toolStripMenuItem_Open_Click);
            // 
            // toolStripMenuItem_SaveNormal
            // 
            this.toolStripMenuItem_SaveNormal.Name = "toolStripMenuItem_SaveNormal";
            this.toolStripMenuItem_SaveNormal.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_SaveNormal.Text = "Save";
            this.toolStripMenuItem_SaveNormal.Click += new System.EventHandler(this.toolStripMenuItem_SaveNormal_Click);
            // 
            // toolStripMenuItem_Save
            // 
            this.toolStripMenuItem_Save.Name = "toolStripMenuItem_Save";
            this.toolStripMenuItem_Save.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_Save.Text = "Save as...";
            this.toolStripMenuItem_Save.Click += new System.EventHandler(this.toolStripMenuItem_Save_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(287, 6);
            // 
            // toolStripMenuItem_MultiCompress
            // 
            this.toolStripMenuItem_MultiCompress.Name = "toolStripMenuItem_MultiCompress";
            this.toolStripMenuItem_MultiCompress.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_MultiCompress.Text = "Save -> Format -> Compress All";
            this.toolStripMenuItem_MultiCompress.Click += new System.EventHandler(this.toolStripMenuItem_MultiCompress_Click);
            // 
            // toolStripMenuItem_MultiCompressDebug
            // 
            this.toolStripMenuItem_MultiCompressDebug.Name = "toolStripMenuItem_MultiCompressDebug";
            this.toolStripMenuItem_MultiCompressDebug.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_MultiCompressDebug.Text = "Save (Debug) -> Format -> Compress All";
            this.toolStripMenuItem_MultiCompressDebug.Click += new System.EventHandler(this.toolStripMenuItem_MultiCompressDebug_Click);
            // 
            // toolStripMenuItem_CompressAll
            // 
            this.toolStripMenuItem_CompressAll.Name = "toolStripMenuItem_CompressAll";
            this.toolStripMenuItem_CompressAll.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_CompressAll.Text = "Compress All";
            this.toolStripMenuItem_CompressAll.Click += new System.EventHandler(this.toolStripMenuItem_CompressAll_Click);
            // 
            // toolStripMenuItem_DecompressAll
            // 
            this.toolStripMenuItem_DecompressAll.Name = "toolStripMenuItem_DecompressAll";
            this.toolStripMenuItem_DecompressAll.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_DecompressAll.Text = "Decompress All";
            this.toolStripMenuItem_DecompressAll.Click += new System.EventHandler(this.toolStripMenuItem_DecompressAll_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(287, 6);
            // 
            // toolStripMenuItem_Clear
            // 
            this.toolStripMenuItem_Clear.Name = "toolStripMenuItem_Clear";
            this.toolStripMenuItem_Clear.Size = new System.Drawing.Size(290, 22);
            this.toolStripMenuItem_Clear.Text = "Clear";
            this.toolStripMenuItem_Clear.Click += new System.EventHandler(this.toolStripMenuItem_Clear_Click);
            // 
            // toolStripMenuItem_Tools
            // 
            this.toolStripMenuItem_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_FontSettings,
            this.toolStripMenuItem_HardcodedText,
            this.resetHardcodedTextToolStripMenuItem,
            this.toolStripMenuItem_LockedCharacters,
            this.toolStripMenuItem2,
            this.toolStripMenuItem_Format,
            this.toolStripMenuItem_DeFormat,
            this.toolStripMenuItem3,
            this.toolStripMenuItem_ViewScript,
            this.searchCommandToolStripMenuItem,
            this.translateToEnglishToolStripMenuItem});
            this.toolStripMenuItem_Tools.Name = "toolStripMenuItem_Tools";
            this.toolStripMenuItem_Tools.Size = new System.Drawing.Size(46, 20);
            this.toolStripMenuItem_Tools.Text = "Tools";
            // 
            // toolStripMenuItem_FontSettings
            // 
            this.toolStripMenuItem_FontSettings.Enabled = false;
            this.toolStripMenuItem_FontSettings.Name = "toolStripMenuItem_FontSettings";
            this.toolStripMenuItem_FontSettings.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem_FontSettings.Text = "Font Settings...";
            this.toolStripMenuItem_FontSettings.Click += new System.EventHandler(this.toolStripMenuItem_FontSettings_Click);
            // 
            // toolStripMenuItem_HardcodedText
            // 
            this.toolStripMenuItem_HardcodedText.Enabled = false;
            this.toolStripMenuItem_HardcodedText.Name = "toolStripMenuItem_HardcodedText";
            this.toolStripMenuItem_HardcodedText.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem_HardcodedText.Text = "Hardcoded Text...";
            this.toolStripMenuItem_HardcodedText.Click += new System.EventHandler(this.toolStripMenuItem_HardcodedText_Click);
            // 
            // resetHardcodedTextToolStripMenuItem
            // 
            this.resetHardcodedTextToolStripMenuItem.Name = "resetHardcodedTextToolStripMenuItem";
            this.resetHardcodedTextToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.resetHardcodedTextToolStripMenuItem.Text = "Reset Hardcoded Text...";
            this.resetHardcodedTextToolStripMenuItem.Click += new System.EventHandler(this.resetHardcodedTextToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_LockedCharacters
            // 
            this.toolStripMenuItem_LockedCharacters.Enabled = false;
            this.toolStripMenuItem_LockedCharacters.Name = "toolStripMenuItem_LockedCharacters";
            this.toolStripMenuItem_LockedCharacters.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem_LockedCharacters.Text = "Locked Characters...";
            this.toolStripMenuItem_LockedCharacters.Click += new System.EventHandler(this.toolStripMenuItem_LockedCharacters_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripMenuItem_Format
            // 
            this.toolStripMenuItem_Format.Enabled = false;
            this.toolStripMenuItem_Format.Name = "toolStripMenuItem_Format";
            this.toolStripMenuItem_Format.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem_Format.Text = "Format";
            this.toolStripMenuItem_Format.Click += new System.EventHandler(this.toolStripMenuItem_Format_Click);
            // 
            // toolStripMenuItem_DeFormat
            // 
            this.toolStripMenuItem_DeFormat.Enabled = false;
            this.toolStripMenuItem_DeFormat.Name = "toolStripMenuItem_DeFormat";
            this.toolStripMenuItem_DeFormat.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem_DeFormat.Text = "De-Format";
            this.toolStripMenuItem_DeFormat.Click += new System.EventHandler(this.toolStripMenuItem_DeFormat_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripMenuItem_ViewScript
            // 
            this.toolStripMenuItem_ViewScript.Name = "toolStripMenuItem_ViewScript";
            this.toolStripMenuItem_ViewScript.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem_ViewScript.Text = "View Script...";
            this.toolStripMenuItem_ViewScript.Click += new System.EventHandler(this.toolStripMenuItem_ViewScript_Click);
            // 
            // searchCommandToolStripMenuItem
            // 
            this.searchCommandToolStripMenuItem.Name = "searchCommandToolStripMenuItem";
            this.searchCommandToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.searchCommandToolStripMenuItem.Text = "Search Command...";
            this.searchCommandToolStripMenuItem.Click += new System.EventHandler(this.searchCommandToolStripMenuItem_Click);
            // 
            // translateToEnglishToolStripMenuItem
            // 
            this.translateToEnglishToolStripMenuItem.Name = "translateToEnglishToolStripMenuItem";
            this.translateToEnglishToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.translateToEnglishToolStripMenuItem.Text = "Translate to English";
            this.translateToEnglishToolStripMenuItem.Click += new System.EventHandler(this.translateToEnglishToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_Debug
            // 
            this.toolStripMenuItem_Debug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_GenerateScript});
            this.toolStripMenuItem_Debug.Name = "toolStripMenuItem_Debug";
            this.toolStripMenuItem_Debug.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem_Debug.Text = "_Debug";
            this.toolStripMenuItem_Debug.Visible = false;
            // 
            // toolStripMenuItem_GenerateScript
            // 
            this.toolStripMenuItem_GenerateScript.Name = "toolStripMenuItem_GenerateScript";
            this.toolStripMenuItem_GenerateScript.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItem_GenerateScript.Text = "Generate Script...";
            this.toolStripMenuItem_GenerateScript.Click += new System.EventHandler(this.toolStripMenuItem_GenerateScript_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Row});
            this.statusStrip1.Location = new System.Drawing.Point(0, 648);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1078, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Row
            // 
            this.toolStripStatusLabel_Row.Name = "toolStripStatusLabel_Row";
            this.toolStripStatusLabel_Row.Size = new System.Drawing.Size(0, 17);
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
            this.richTextBox_ScriptFile.Size = new System.Drawing.Size(440, 585);
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
            this.richTextBox_ScriptMessage.Size = new System.Drawing.Size(412, 319);
            this.richTextBox_ScriptMessage.TabIndex = 0;
            this.richTextBox_ScriptMessage.Text = "";
            this.richTextBox_ScriptMessage.TextChanged += new System.EventHandler(this.richTextBox_ScriptMessage_TextChanged);
            this.richTextBox_ScriptMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox_ScriptMessage_KeyDown);
            // 
            // ScriptEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 670);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MessagePreviewAlt)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_DecompressAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_CompressAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Tools;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_FontSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Format;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_ViewScript;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Row;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_DeFormat;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListBox listBox_ScriptMessages;
        private FixedRichTextBox richTextBox_ScriptMessage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Debug;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_GenerateScript;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.PictureBox pictureBox_MessagePreview;
        private System.Windows.Forms.ComboBox comboBox_PreviewColor;
        private System.Windows.Forms.ComboBox comboBox_PreviewType;
        private System.Windows.Forms.Panel panel_Preview;
        private System.Windows.Forms.Button button_SearchDown;
        private System.Windows.Forms.TextBox textBox_Search;
        private System.Windows.Forms.Panel searchPanel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox checkBox_SearchGlobal;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_HardcodedText;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_LockedCharacters;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Clear;
        private System.Windows.Forms.Button button_SearchUp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_MultiCompressDebug;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_SaveNormal;
        private System.Windows.Forms.ToolStripMenuItem searchCommandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem translateToEnglishToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox_MessagePreviewAlt;
        private System.Windows.Forms.ToolStripMenuItem resetHardcodedTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_MultiCompress;
    }
}
