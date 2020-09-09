using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Serialization;
using ConanExplorer.Conan;
using ConanExplorer.Conan.Filetypes;
using ConanExplorer.Properties;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using ConanExplorer.Conan.Script;
using ConanExplorer.Conan.Script.Elements;
using ConanExplorer.ExtensionMethods;
using System.Drawing.Text;
using System.Net;
using System.Diagnostics;
using GoogleTranslateFreeApi;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ConanExplorer.Windows
{

    public partial class ScriptEditorWindow : Form
    {
        private SearchCommandWindow _searchCommandWindow;
        private PKNFile _scriptPKN;
        private Encoding _shiftJis = Encoding.GetEncoding("shift_jis");
        private ScriptDocument _lastScriptFile;
        private List<IScriptElement> _elements = new List<IScriptElement>();
        private System.Windows.Forms.Timer _timerMessageApply = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer _timerFileApply = new System.Windows.Forms.Timer();
        private bool _changingMessageSelection = false;
        private bool _updatingMessage = false;
        private bool _changingSelection = false;
        private bool _updatingScript = false;
        private string _actualEditorScript;

        public ScriptFile ScriptFile;

        public ScriptEditorWindow()
        {
            InitializeComponent();
            richTextBox_ScriptFile.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_ScriptFile.AutoWordSelection = false;
            richTextBox_ScriptMessage.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_ScriptMessage.AutoWordSelection = false;

            _timerMessageApply.Interval = 1;
            _timerMessageApply.Tick += _timerApply_Tick;

            _timerFileApply.Interval = 2;
            _timerFileApply.Tick += _timerFileApply_Tick;

            comboBox_PreviewType.SelectedIndex = 0;
            comboBox_PreviewColor.SelectedIndex = 2;

            DisableTools();
        }

        private void UpdateScriptFile()
        {
            if (listBox_ScriptFiles.SelectedIndex == -1) return;
            ScriptDocument script = ScriptFile.Scripts[listBox_ScriptFiles.SelectedIndex];
            script.TextBuffer = richTextBox_ScriptFile.Text.Replace("\n", "\r\n");

            _updatingScript = true;
            int messagePos = richTextBox_ScriptMessage.SelectionStart;
            int messageLen = richTextBox_ScriptMessage.SelectionLength;

            Win32.LockWindow(this.Handle);
            int scrollPos = Win32.GetScrollPos(richTextBox_ScriptFile.Handle, 1);
            int pos = richTextBox_ScriptFile.SelectionStart;
            int len = richTextBox_ScriptFile.SelectionLength;
            richTextBox_ScriptFile.Text = script.TextBuffer;
            richTextBox_ScriptFile.SelectionStart = pos;
            richTextBox_ScriptFile.SelectionLength = len;
            Win32.SetScrollPos(richTextBox_ScriptFile.Handle, 1, scrollPos, true);
            Win32.PostMessage(richTextBox_ScriptFile.Handle, 0x115, 4 + 0x10000 * scrollPos, 0);
            Win32.LockWindow(IntPtr.Zero);

            int lastIndex = listBox_ScriptMessages.SelectedIndex;
            _elements.Clear();
            listBox_ScriptMessages.Items.Clear();
            foreach (IScriptElement element in ScriptParser.Parse(script))
            {
                _elements.Add(element);
                if (element.GetType() == typeof(ScriptMessage))
                {
                    listBox_ScriptMessages.Items.Add(element);
                }
            }

            try
            {
                listBox_ScriptMessages.SelectedIndex = lastIndex;
                richTextBox_ScriptMessage.SelectionStart = messagePos;
                richTextBox_ScriptMessage.SelectionLength = messageLen;
            }
            catch (Exception e)
            {

            }
            _updatingScript = false;
        }

        private void GenerateFont()
        {
            PKNFile pknFile = ApplicationState.Instance.ProjectFile.ModifiedImage.PKNFiles.FirstOrDefault(p => p.Name == "GRAPH");
            if (pknFile != null)
            {
                BaseFile baseFile = pknFile.Files.FirstOrDefault(f => f.FileName == "FONT.BIN");
                if (baseFile != null)
                {
                    FONTFile fontFile = (FONTFile)baseFile;
                    fontFile.Characters = ScriptFile.GeneratedFont;
                    fontFile.Save();
                    return;
                }
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "FONT.BIN|FONT.BIN";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FONTFile fontFile = new FONTFile(saveFileDialog.FileName);
                fontFile.Characters = ScriptFile.GeneratedFont;
                fontFile.Save();
            }
        }

        private void Format()
        {
            if (ScriptFile == null) return;

            ScriptFile.GenerateFont();
            ScriptFile.Format();

            //Unnecessary check (generation fast enough)
            //if (MessageBox.Show("Do you want to generate the \"FONT.BIN\"?", "Question!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            GenerateFont();
            //}

            if (listBox_ScriptFiles.SelectedIndex == -1) return;
            richTextBox_ScriptFile.Text = ((ScriptDocument)listBox_ScriptFiles.SelectedItem).TextBuffer;
            if (listBox_ScriptMessages.SelectedIndex == -1) return;
            richTextBox_ScriptMessage.Text = ((ScriptMessage)listBox_ScriptMessages.SelectedItem).Content;

            //if (MessageBox.Show("Do you want to compress the scripts?", "Question!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    CompressScripts();
            //}
        }

        private void DeFormat()
        {
            MessageBox.Show("Too be implemented!");
        }

        private void UpdatePreview(ScriptMessage message)
        {
            if (message.IsSelectionWindow)
            {
                comboBox_PreviewType.SelectedIndex = 1;
                comboBox_PreviewColor.Enabled = true;
            }
            else
            {
                comboBox_PreviewType.SelectedIndex = 0;
                comboBox_PreviewColor.Enabled = false;
            }

            if (comboBox_PreviewType.SelectedIndex == 0)
            {
                if (message.IsSelectionWindow) return;
                int messageCount = message.WindowCount;
                Bitmap bitmap = new Bitmap(13 * 16, 6 * 16 * messageCount);
                using (Graphics graphic = Graphics.FromImage(bitmap))
                {
                    Graphic.DrawDialogueWindow(graphic, 13, 6, messageCount);
                    Graphic.DrawDialogueText(graphic, ScriptFile, message);
                }
                pictureBox_MessagePreview.Image = bitmap;
            }
            else if (comboBox_PreviewType.SelectedIndex == 1)
            {
                if (!message.IsSelectionWindow) return;
                Bitmap bitmap = new Bitmap(15 * 16, 11 * 16);
                using (Graphics graphic = Graphics.FromImage(bitmap))
                {
                    Graphic.DrawSelectionWindow(graphic, 15, 11, comboBox_PreviewColor.SelectedIndex);
                    Graphic.DrawSelectionText(graphic, ScriptFile, message);
                };
                pictureBox_MessagePreview.Image = bitmap;
            }
        }


        public void CompressScripts()
        {
            if (ApplicationState.Instance.ProjectFile == null)
            {
                MessageBox.Show("Open a project file before compressing!", "Project file not found!");
                return;
            }

            for (int i = 0; i < ScriptFile.Scripts.Count; i++)
            {
                ScriptDocument scriptFile = ScriptFile.Scripts[i];

                //Don't touch this file! - Softlocks the engine, and we dont need to modify gamelogic to that degree
                if (scriptFile.Name == "FLAG.TXT") continue;

                scriptFile.WriteToOriginalFile();
                if (scriptFile.BaseFile.GetType() == typeof(LZBFile))
                {
                    LZBFile lzbFile = (LZBFile)scriptFile.BaseFile;
                    lzbFile.Compress(false);
                }
                Invoke((MethodInvoker)delegate
                {
                    progressBar_Progress.Value = (int)((double)i / ScriptFile.Scripts.Count * 100);
                });
            }

            Invoke((MethodInvoker)delegate
            {
                progressBar_Progress.Value = 0;
                Enabled = true;
            });
        }

        public void DecompressScripts()
        {
            if (ApplicationState.Instance.ProjectFile == null)
            {
                MessageBox.Show("Open a project file before decompressing!", "Project file not found!");
                return;
            }
            PSXImage modifiedImage = ApplicationState.Instance.ProjectFile.ModifiedImage;
            _scriptPKN = modifiedImage.PKNFiles.Find(pkn => pkn.Name == "SCRIPT");
            if (_scriptPKN == null) return;
            if (_scriptPKN.Files.Count == 0) //cheap fix for when pkn files dont save files
            {
                _scriptPKN.Unpack();
            }
            Enabled = false;

            ScriptFile = new ScriptFile();

            richTextBox_ScriptFile.Text = "";
            for (int i = 0; i < _scriptPKN.Files.Count; i++)
            {
                BaseFile baseFile = _scriptPKN.Files[i];
                if (baseFile.GetType() == typeof(LZBFile))
                {
                    LZBFile lzbFile = (LZBFile)baseFile;
                    lzbFile.Decompress();
                }
                ScriptDocument scriptFile = new ScriptDocument(baseFile);
                ScriptFile.Scripts.Add(scriptFile);
                progressBar_Progress.Value = (int)((double)i / _scriptPKN.Files.Count * 100);
            }
            listBox_ScriptFiles.Items.Clear();
            foreach (ScriptDocument scriptFile in ScriptFile.Scripts)
            {
                listBox_ScriptFiles.Items.Add(scriptFile);
            }

            progressBar_Progress.Value = 0;
            Enabled = true;
        }


        public void UpdateScriptMessage()
        {
            if (_changingMessageSelection) return;

            IScriptElement element = (IScriptElement)listBox_ScriptMessages.SelectedItem;
            if (element == null) return;
            if (element.GetType() == typeof(ScriptMessage))
            {
                ScriptMessage message = (ScriptMessage)element;
                message.Content = richTextBox_ScriptMessage.Text.Replace("\n", "\r\n");
                UpdatePreview(message);
            }

            ScriptDocument script = (ScriptDocument)listBox_ScriptFiles.SelectedItem;
            ScriptParser.Parse(script, _elements);

            _updatingMessage = true;

            Win32.LockWindow(this.Handle);
            int scrollPos = Win32.GetScrollPos(richTextBox_ScriptFile.Handle, 1);
            int pos = richTextBox_ScriptFile.SelectionStart;
            int len = richTextBox_ScriptFile.SelectionLength;
            richTextBox_ScriptFile.Text = script.TextBuffer;
            richTextBox_ScriptFile.SelectionStart = pos;
            richTextBox_ScriptFile.SelectionLength = len;
            Win32.SetScrollPos(richTextBox_ScriptFile.Handle, 1, scrollPos, true);
            Win32.PostMessage(richTextBox_ScriptFile.Handle, 0x115, 4 + 0x10000 * scrollPos, 0);
            Win32.LockWindow(IntPtr.Zero);

            _updatingMessage = false;
        }

        private bool FindNext()
        {
            if (checkBox_SearchGlobal.Checked)
            {
                int from = richTextBox_ScriptFile.SelectionStart + 1;
                int to = richTextBox_ScriptFile.TextLength - 1;
                int start = richTextBox_ScriptFile.Find(textBox_Search.Text, from, to, RichTextBoxFinds.None);

                if (start == -1)
                {
                    while (start == -1)
                    {
                        if (listBox_ScriptFiles.SelectedIndex == listBox_ScriptFiles.Items.Count - 1)
                        {
                            return false;
                        }
                        listBox_ScriptFiles.SelectedIndex = listBox_ScriptFiles.SelectedIndex + 1;

                        from = richTextBox_ScriptFile.SelectionStart + 1;
                        to = richTextBox_ScriptFile.TextLength - 1;
                        start = richTextBox_ScriptFile.Find(textBox_Search.Text, from, to, RichTextBoxFinds.None);
                    }
                }
                richTextBox_ScriptFile.SelectionStart = start;
                richTextBox_ScriptFile.ScrollToCaret();
                return true;
            }
            else
            {
                int from = richTextBox_ScriptFile.SelectionStart + 1;
                int to = richTextBox_ScriptFile.TextLength - 1;
                int start = richTextBox_ScriptFile.Find(textBox_Search.Text, from, to, RichTextBoxFinds.None);

                if (start == -1) return false;

                richTextBox_ScriptFile.SelectionStart = start;
                richTextBox_ScriptFile.ScrollToCaret();
                return true;
            }
        }

        private bool FindPrevious()
        {
            if (checkBox_SearchGlobal.Checked)
            {
                int from = 0;
                int to = richTextBox_ScriptFile.SelectionStart - 1;
                int start = richTextBox_ScriptFile.Find(textBox_Search.Text, from, to, RichTextBoxFinds.Reverse);

                if (start == -1)
                {
                    while (start == -1)
                    {
                        if (listBox_ScriptFiles.SelectedIndex == 0)
                        {
                            return false;
                        }
                        listBox_ScriptFiles.SelectedIndex = listBox_ScriptFiles.SelectedIndex - 1;

                        to = richTextBox_ScriptFile.SelectionStart - 1;
                        start = richTextBox_ScriptFile.Find(textBox_Search.Text, from, to, RichTextBoxFinds.Reverse);
                    }
                }
                richTextBox_ScriptFile.SelectionStart = start;
                richTextBox_ScriptFile.ScrollToCaret();
                return true;
            }
            else
            {
                int from = 0;
                int to = richTextBox_ScriptFile.SelectionStart - 1;
                int start = richTextBox_ScriptFile.Find(textBox_Search.Text, from, to, RichTextBoxFinds.Reverse);

                if (start == -1) return false;

                richTextBox_ScriptFile.SelectionStart = start;
                richTextBox_ScriptFile.ScrollToCaret();
                return true;
            }
        }

        private void EnableTools(bool skipCheck = false)
        {
            if (ApplicationState.Instance.ProjectFile != null || skipCheck)
            {
                toolStripMenuItem_Save.Enabled = true;
                toolStripMenuItem_SaveNormal.Enabled = true;
                toolStripMenuItem_CompressAll.Enabled = true;
                toolStripMenuItem_MultiCompress.Enabled = true;

                toolStripMenuItem_LockedCharacters.Enabled = true;
                toolStripMenuItem_HardcodedText.Enabled = true;
                toolStripMenuItem_FontSettings.Enabled = true;
                toolStripMenuItem_Format.Enabled = true;
                toolStripMenuItem_DeFormat.Enabled = true;
            }
        }

        private void DisableTools()
        {
            toolStripMenuItem_Save.Enabled = false;
            toolStripMenuItem_SaveNormal.Enabled = false;
            toolStripMenuItem_CompressAll.Enabled = false;
            toolStripMenuItem_MultiCompress.Enabled = false;

            toolStripMenuItem_LockedCharacters.Enabled = false;
            toolStripMenuItem_HardcodedText.Enabled = false;
            toolStripMenuItem_FontSettings.Enabled = false;
            toolStripMenuItem_Format.Enabled = false;
            toolStripMenuItem_DeFormat.Enabled = false;
        }

        private void _timerApply_Tick(object sender, EventArgs e)
        {
            _timerMessageApply.Enabled = false;
            UpdateScriptMessage();
        }

        private void _timerFileApply_Tick(object sender, EventArgs e)
        {
            _timerFileApply.Enabled = false;
            UpdateScriptFile();
        }

        private void listBox_ScriptFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lastScriptFile != null) _lastScriptFile.TextBuffer = richTextBox_ScriptFile.Text.Replace("\n", "\r\n");

            ScriptDocument file = ScriptFile.Scripts[listBox_ScriptFiles.SelectedIndex];
            richTextBox_ScriptFile.Select(0, 0);
            richTextBox_ScriptFile.ScrollToCaret();
            richTextBox_ScriptFile.Text = file.TextBuffer;
            _lastScriptFile = file;

            _elements.Clear();
            listBox_ScriptMessages.Items.Clear();
            foreach (IScriptElement element in ScriptParser.Parse(file))
            {
                _elements.Add(element);
                if (element.GetType() == typeof(ScriptMessage))
                {
                    listBox_ScriptMessages.Items.Add(element);
                }
            }
        }

        private void toolStripMenuItem_DecompressAll_Click(object sender, EventArgs e)
        {
            DecompressScripts();
            EnableTools();
        }

        private void toolStripMenuItem_CompressAll_Click(object sender, EventArgs e)
        {
            CompressAllThread();
        }

        private void CompressAllThread()
        {
            Thread thread = new Thread(CompressScripts);
            Enabled = false;
            thread.Start();
        }

        private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Conan Explorer Script (*.ces)|*.ces";

            if (openFileDialog.ShowDialog() == DialogResult.OK && File.Exists(openFileDialog.FileName))
            {
                _actualEditorScript = openFileDialog.FileName;
                XmlSerializer serializer = new XmlSerializer(typeof(ScriptFile));
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    ScriptFile = (ScriptFile)serializer.Deserialize(reader);

                    //Default hacky fallback for Lockcharacters
                    if(ScriptFile.LockedCharacters == null || ScriptFile.LockedCharacters.Count <= 5)
                    {
                        var data = JsonConvert.DeserializeObject<List<FontCharacter>>(Resources.DefaultLockedCharacters);
                        ScriptFile.LockedCharacters = new List<FontCharacter>();
                        foreach (var character in data)
                        {
                            ScriptFile.LockedCharacters.Add(new FontCharacter(new byte[32], character.Index, ""));
                        }
                    }
                }
            }
            else
            {
                return;
            }

            bool fontFound = false;
            if (ScriptFile.FontName == "Quarlow")
            {
                //Check for Quarlow Default Font in system
                InstalledFontCollection fontsCollection = new InstalledFontCollection();
                foreach (var fontFamiliy in fontsCollection.Families)
                {
                    if (fontFamiliy.Name == "Quarlow")
                        fontFound = true;
                }

                //When font not found on OS
                if (fontFound != true)
                {
                    string messageText = "Quarlow Font was referenced in loaded file, but not found installed on this OS"
                        + Environment.NewLine + "Do you want to install the font from the internet?"
                        + Environment.NewLine + Environment.NewLine
                        + "WARNING: Application will be closed after this!";

                    DialogResult dialogResult = MessageBox.Show(messageText,
                        "Font missing..."
                        , MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            string tempPathForZip = Path.GetTempPath() + "\\TempFontF\\";
                            string zipTempPath = Path.GetTempPath() + "Quarlow.zip";

                            //Delete old try if there
                            if (Directory.Exists(tempPathForZip))
                                Directory.Delete(tempPathForZip, true);

                            using (var client = new WebClient())
                            {
                                client.DownloadFile("http://chapter731.net/downloads/fonts/Quarlow.zip", zipTempPath);
                            }

                            System.IO.Compression.ZipFile.ExtractToDirectory(zipTempPath, tempPathForZip);
                            Process.Start("C:\\Windows\\System32\\fontview.exe", tempPathForZip + "Quarlow.ttf");

                            Application.Exit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Download failed...\n\r" + ex);
                        }

                    }
                }
            }

            Text = String.Format("Script Editor - \"{0}\"", openFileDialog.FileName);

            listBox_ScriptFiles.Items.Clear();
            foreach (ScriptDocument scriptFile in ScriptFile.Scripts)
            {
                listBox_ScriptFiles.Items.Add(scriptFile);
            }

            EnableTools(true);
        }

        private void toolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            SaveData(true);
        }


        private void toolStripMenuItem_SaveNormal_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void SaveData(bool mode = false)
        {
            if (ScriptFile.Scripts.Count == 0)
            {
                MessageBox.Show("Decompress all first!");
                return;
            }
            if (_lastScriptFile != null) _lastScriptFile.TextBuffer = richTextBox_ScriptFile.Text.Replace("\n", "\r\n");

            if (mode)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Conan Explorer Script (*.ces)|*.ces";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _actualEditorScript = saveFileDialog.FileName;
                }
            }

            if (_actualEditorScript == null)
            {
                MessageBox.Show("Open or Save as... the Script Project!");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ScriptFile));
            using (StreamWriter writer = new StreamWriter(_actualEditorScript))
            {
                serializer.Serialize(writer, ScriptFile);
            }
            Text = String.Format("Script Editor - \"{0}\"", _actualEditorScript);

        }

        private void listBox_ScriptFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            listBox_ScriptFiles.SelectedIndex = listBox_ScriptFiles.IndexFromPoint(e.X, e.Y);

            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Compress", OnCompressClick);
            cm.MenuItems.Add("Show Current File State...", OnShowCurrentFileClick);
            cm.Show(listBox_ScriptFiles, e.Location);
        }

        private void OnCompressClick(object sender, EventArgs eventArgs)
        {
            if (ScriptFile.Scripts.Count == 0)
            {
                MessageBox.Show("Decompress all first!");
                return;
            }
            if (listBox_ScriptFiles.SelectedIndex == -1) return;
            Enabled = false;
            ScriptDocument file = ScriptFile.Scripts[listBox_ScriptFiles.SelectedIndex];
            file.TextBuffer = richTextBox_ScriptFile.Text.Replace("\n", "\r\n");
            file.WriteToOriginalFile();
            if (file.BaseFile.GetType() == typeof(LZBFile))
            {
                LZBFile lzbFile = (LZBFile)file.BaseFile;
                lzbFile.Compress(false);
            }
            Enabled = true;
        }

        private void OnShowCurrentFileClick(object sender, EventArgs eventArgs)
        {
            if (listBox_ScriptFiles.SelectedIndex == -1) return;
            Enabled = false;
            ScriptDocument file = ScriptFile.Scripts[listBox_ScriptFiles.SelectedIndex];
            file.WriteToOriginalFile();
            if (file.BaseFile.GetType() == typeof(LZBFile))
            {
                LZBFile lzbFile = (LZBFile)file.BaseFile;
                ScriptViewerWindow scriptViewerWindow = new ScriptViewerWindow(lzbFile.DecompressedFile);
                scriptViewerWindow.Show();
            }
            Enabled = true;
        }

        private void toolStripMenuItem_ViewScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BaseFile baseFile = new BaseFile(openFileDialog.FileName);
                ScriptViewerWindow scriptViewerWindow = new ScriptViewerWindow(baseFile);
                scriptViewerWindow.Show();
            }
        }

        private void toolStripMenuItem_FontSettings_Click(object sender, EventArgs e)
        {
            FontSettingsWindow generateFontWindow = new FontSettingsWindow(ScriptFile);
            generateFontWindow.ShowDialog();
        }

        private void richTextBox_ScriptFile_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel_Row.Text = "Row: " + (richTextBox_ScriptFile.GetLineFromCharIndex(richTextBox_ScriptFile.SelectionStart) + 1);
            if (_updatingMessage) return;
            _changingSelection = true;
            FindNearestMessage();
            _changingSelection = false;
        }

        private void FindNearestMessage()
        {
            int lineIndex = richTextBox_ScriptFile.GetLineFromCharIndex(richTextBox_ScriptFile.SelectionStart);
            int bestIndex = 0;
            int bestDifference = 999999;
            for (int i = 0; i < listBox_ScriptMessages.Items.Count; i++)
            {
                ScriptMessage message = (ScriptMessage)listBox_ScriptMessages.Items[i];
                int difference = Math.Abs(message.LineIndex - lineIndex);
                if (difference < bestDifference)
                {
                    bestIndex = i;
                    bestDifference = difference;
                }
                else if (difference > bestDifference) break;
            }
            if (bestIndex > 0)
            {
                ScriptMessage message = (ScriptMessage)listBox_ScriptMessages.Items[bestIndex - 1];
                int difference = Math.Abs((message.LineIndex + message.ContentLineCount) - lineIndex);
                if (difference < bestDifference)
                {
                    bestIndex--;
                }
            }

            if (listBox_ScriptMessages.Items.Count == 0) return;
            listBox_ScriptMessages.SelectedIndex = bestIndex;
        }

        private void toolStripMenuItem_Format_Click(object sender, EventArgs e)
        {
            Format();
        }

        private void toolStripMenuItem_DeFormat_Click(object sender, EventArgs e)
        {
            DeFormat();
        }

        private void listBox_ScriptMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changingMessageSelection = true;

            _timerMessageApply.Enabled = false;
            ScriptMessage message = (ScriptMessage)listBox_ScriptMessages.SelectedItem;
            if (message == null) return;
            richTextBox_ScriptMessage.Text = message.Content;
            UpdatePreview(message);

            if (listBox_ScriptMessages.SelectedIndex != -1)
            {
                ScriptMessage scriptMessage = (ScriptMessage)listBox_ScriptMessages.SelectedItem;
                if (!(_changingSelection || _updatingScript))
                {
                    if (richTextBox_ScriptFile.Lines.Length > scriptMessage.LineIndex)
                    {
                        richTextBox_ScriptFile.SelectionStart = richTextBox_ScriptFile.GetFirstCharIndexFromLine(scriptMessage.LineIndex);
                        richTextBox_ScriptFile.ScrollToCaret();
                    }
                }
            }

            _changingMessageSelection = false;
        }

        private void toolStripMenuItem_GenerateScript_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (IScriptElement element in listBox_ScriptMessages.Items)
            {
                stringBuilder.Append(element.Text);
            }
            DebugTextWindow window = new DebugTextWindow(stringBuilder.ToString());
            window.Show();
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            IScriptElement element = (IScriptElement)listBox_ScriptMessages.SelectedItem;
            if (element.GetType() == typeof(ScriptMessage))
            {
                ScriptMessage message = (ScriptMessage)element;
                message.Content = richTextBox_ScriptMessage.Text.Replace("\n", "\r\n");
            }

            ScriptDocument script = (ScriptDocument)listBox_ScriptFiles.SelectedItem;
            ScriptParser.Parse(script, _elements);

            richTextBox_ScriptFile.Select(0, 0);
            richTextBox_ScriptFile.ScrollToCaret();
            richTextBox_ScriptFile.Text = script.TextBuffer;
        }

        private void richTextBox_ScriptMessage_TextChanged(object sender, EventArgs e)
        {
            if (_changingMessageSelection) return;
            _timerMessageApply.Enabled = false;
            _timerMessageApply.Enabled = true;
        }

        private void comboBox_PreviewColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateScriptMessage();
        }

        private void richTextBox_ScriptFile_TextChanged(object sender, EventArgs e)
        {
            if (_updatingScript) return;
            _timerFileApply.Enabled = false;
            _timerFileApply.Enabled = true;
        }

        private void richTextBox_ScriptFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                searchPanel.Visible = !searchPanel.Visible;
                if (searchPanel.Visible) textBox_Search.Focus();
                return;
            }
            if (e.Shift && e.KeyCode == Keys.F3)
            {
                FindPrevious();
                return;
            }
            if (e.KeyCode == Keys.F3)
            {
                FindNext();
                return;
            }
        }

        private void textBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                searchPanel.Visible = !searchPanel.Visible;
                if (searchPanel.Visible) textBox_Search.Focus();
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                FindNext();
                return;
            }
            if (e.Shift && e.KeyCode == Keys.F3)
            {
                FindPrevious();
                return;
            }
            if (e.KeyCode == Keys.F3)
            {
                FindNext();
                return;
            }
        }

        private void richTextBox_ScriptMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.W) //New Window Hotkey
            {
                richTextBox_ScriptMessage.SelectedText += "%KW:" + Environment.NewLine + "%CLR:%COL(5):NAME:%COL(1):";
                return;
            }

            if (e.Control && e.KeyCode == Keys.N) //New Window Hotkey
            {
                richTextBox_ScriptMessage.SelectedText += "%LF:";
                return;
            }

            if (e.Control && e.KeyCode == Keys.K) //New Window Hotkey
            {
                richTextBox_ScriptMessage.SelectedText += "%KW:";
                return;
            }
        }

        private void listBox_ScriptMessages_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_ScriptMessages.SelectedIndex == -1) return;
            ScriptMessage scriptMessage = (ScriptMessage)listBox_ScriptMessages.SelectedItem;
            richTextBox_ScriptFile.SelectionStart = richTextBox_ScriptFile.GetFirstCharIndexFromLine(scriptMessage.LineIndex);
            richTextBox_ScriptFile.ScrollToCaret();
        }

        private void toolStripMenuItem_HardcodedText_Click(object sender, EventArgs e)
        {
            try
            {
                HardcodedTextWindow hardcodedTestWindow = new HardcodedTextWindow(ScriptFile);
                hardcodedTestWindow.ShowDialog();
            }
            catch (Exception f)
            {
                return;
            }
        }

        private void toolStripMenuItem_LockedCharacters_Click(object sender, EventArgs e)
        {
            LockedCharactersWindow lockedCharactersWindow = new LockedCharactersWindow(ScriptFile);
            lockedCharactersWindow.ShowDialog();
        }

        private void toolStripMenuItem_Clear_Click(object sender, EventArgs e)
        {
            DisableTools();
            ScriptFile = null;
            listBox_ScriptFiles.Items.Clear();
            listBox_ScriptMessages.Items.Clear();
            richTextBox_ScriptFile.Clear();
            richTextBox_ScriptMessage.Clear();
        }

        private void button_SearchUp_Click(object sender, EventArgs e)
        {
            FindPrevious();
        }

        private void button_SearchDown_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void toolStripMenuItem_MultiCompress_Click(object sender, EventArgs e)
        {
            SaveData();
            Format();
            CompressAllThread();
        }

        private void searchCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_searchCommandWindow == null)
            {
                _searchCommandWindow = new SearchCommandWindow(ScriptFile);
                _searchCommandWindow.FormClosed += _searchCommandWindow_FormClosed;
            }
            _searchCommandWindow.Show();
        }

        private void _searchCommandWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _searchCommandWindow = null;
        }

        private async void translateToEnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var translator = new GoogleTranslator();
            Enabled = false;

            Language from = Language.German;
            //Language to = GoogleTranslator.GetLanguageByName("Japanese");
            Language to = Language.English;
            int tmpCnt = 0;

            foreach (var scriptMsg in listBox_ScriptMessages.Items)
            {
                _timerMessageApply.Enabled = false;
                ScriptMessage message = (ScriptMessage)scriptMsg;
                if (message == null) return;
                //richTextBox_ScriptMessage.Text = message.Content;
                ///UpdatePreview(message);

                List<string> dialogs = new List<string>();
                foreach (var dialog in message.ContentTextArray)
                {
                    if (dialog != "" || dialog != string.Empty)
                    {
                        string cleanInput = dialog.Replace("\r", " ").Replace("\n", string.Empty);
                        //cleanInput = cleanInput.Substring(cleanInput.IndexOf(':') + 1);
                        TranslationResult result = await translator.TranslateLiteAsync(cleanInput, from, to);
                        dialogs.Add(result.MergedTranslation);
                    }
                }

                Regex x = new Regex(@"(\%COL\([0-9]+\)\:([a-zA-Z]+)\%COL\([0-9]+\)\:)([\s\S]*?(?=\%KW\:))");
                Regex y = new Regex(@"(\%COL\([0-9]+\)\:)([\s\S]*?(?=\%KW\:))");
                string tmpContent = message.Content;
                MatchCollection matches = x.Matches(tmpContent);
                int messageRegexGroup = 3;

                //try alterantive regex when no match
                if (matches.Count <= 0) {
                    matches = y.Matches(tmpContent);
                    messageRegexGroup = 2;
                }

                int dialogCharLimitRow = 22;
                int actualRow = 1;

                int matchIterator = 0;
                foreach (Match ItemMatch in matches)
                {
                    var splitDialog = dialogs[matchIterator].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder stringBuilder = new StringBuilder();

                    Console.WriteLine("NameLength: " + ItemMatch.Groups[2].Length);
                    dialogCharLimitRow -= ItemMatch.Groups[2].Length;

                    if (ItemMatch.Groups[2].Length % 2 != 1 && ItemMatch.Groups[2].Length != 0) {
                        stringBuilder.Append(" ");
                        dialogCharLimitRow -= 1;
                    }

                    foreach (var word in splitDialog)
                    {
                        if (word.Length > 22)
                        {
                            MessageBox.Show("Validation failed! The word " + word + " is TOO LONG! Parsing stopped!", "Word is too long for the game engine!");
                            break;
                        }

                        if ((word.Length + 1) >= dialogCharLimitRow)
                        {
                            actualRow += 1;
                            if (actualRow >= 6)
                            {
                                //Optional TODO: Append the correct name!
                                //TODO: clear up this copypaste festival someday
                                stringBuilder.Append("%KW:%CLR:%COL(5):" + ItemMatch.Groups[2].Value + ":%COL(1):");
                                actualRow = 1;
                                dialogCharLimitRow = 22 - ItemMatch.Groups[2].Length;

                                if (ItemMatch.Groups[2].Length % 2 != 1 && ItemMatch.Groups[2].Length != 0)
                                {
                                    stringBuilder.Append(" ");
                                    dialogCharLimitRow -= 1;
                                }

                            }

                            stringBuilder.Append("%LF:"); //NEW LINE
                            stringBuilder.Append(word);
                            stringBuilder.Append(" ");
                            dialogCharLimitRow = 22 - (word.Length + 1);
                        }
                        else
                        {
                            stringBuilder.Append(word); //write out word
                            stringBuilder.Append(" ");
                            dialogCharLimitRow -= (word.Length + 1); // minus empty space
                        }
                    }

                    tmpContent = tmpContent.Replace(ItemMatch.Groups[messageRegexGroup].Value, stringBuilder.ToString());
                    stringBuilder.Clear();
                    actualRow = 1;
                    matchIterator++;

                }

                //(\%COL\([0-9]+\)\:([a-zA-Z]+)\%COL\([0-9]+\)\:)([\s\S]*?(?=\%KW\:))
                //match count is 0..x
                // group2 = Message name (for length parser), group 3: complette text string until kill window command

                //if no match use
                //(\%COL\([0-9]+\)\:)([\s\S]*?(?=\%KW\:))
                //match count is 0..x
                // group2 = complette text string until kill window command
                // regex for messages without name in it

                //if no match then skip message but make at least a console.log

                //message.ContentAlternative = tmpContent;
                message.ContentAlternative = message.Content; //backup old translation for preview purposes
                message.Content = tmpContent;

                //if (tmpCnt >= 16) //debug limit because google api will lock me out when i make too many requests
                //{
                //    Enabled = true;
                //    break;
                //}
                tmpCnt++;

                Invoke((MethodInvoker)delegate
                {
                    progressBar_Progress.Value = (int)((double)tmpCnt / listBox_ScriptMessages.Items.Count * 100);
                });
            }
            MessageBox.Show("Selected chapter was translated!", "API Translation");

            Invoke((MethodInvoker)delegate
            {
                progressBar_Progress.Value = 0;
                Enabled = true;
            });
        }

        private void resetHardcodedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptFile.HardCodedTexts = new List<HardCodedText>();
            MessageBox.Show("Hardcoded Texts were reseted and are now empty in project file!\n" +
                "Please reopen the Hardcoded Text Window to reinitialize the default settings!");
        }
    }
}
