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

namespace ConanExplorer.Windows
{
    

    public partial class ScriptEditorWindow : Form
    {
        private PKNFile _scriptPKN;
        private Encoding _shiftJis = Encoding.GetEncoding("shift_jis");
        private ScriptDocument _lastScriptFile;
        private List<IScriptElement> _elements = new List<IScriptElement>();
        private System.Windows.Forms.Timer _timerApply = new System.Windows.Forms.Timer();
        private bool _changingSelection = false;
        private bool _updatingMessage = false;

        public ScriptFile ScriptFile;

        

        public ScriptEditorWindow()
        {
            InitializeComponent();
            richTextBox_ScriptFile.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_ScriptFile.AutoWordSelection = false;
            richTextBox_ScriptMessage.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_ScriptMessage.AutoWordSelection = false;

            _timerApply.Interval = 200;
            _timerApply.Tick += _timerApply_Tick;

            comboBox_PreviewType.SelectedIndex = 0;
            comboBox_PreviewColor.SelectedIndex = 2;
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

            if (MessageBox.Show("Do you want to generate the \"FONT.BIN\"?", "Question!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GenerateFont();
            }

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

        private void CompressScripts()
        {
            if (ApplicationState.Instance.ProjectFile == null)
            {
                MessageBox.Show("Open a project file before compressing!", "Project file not found!");
                return;
            }
            for (int i = 0; i < ScriptFile.Scripts.Count; i++)
            {
                ScriptDocument scriptFile = ScriptFile.Scripts[i];
                scriptFile.WriteToOriginalFile();
                if (scriptFile.BaseFile.GetType() == typeof (LZBFile))
                {
                    LZBFile lzbFile = (LZBFile) scriptFile.BaseFile;
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

        private void DecompressScripts()
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
            if (_changingSelection) return;
            
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
            switchRawEditorToolStripMenuItem.Enabled = false; //everything is broken after edit so... heh
        }

        private bool FindNext()
        {
            int from = richTextBox_ScriptFile.SelectionStart + 1;
            int to = richTextBox_ScriptFile.TextLength - 1;
            int start = richTextBox_ScriptFile.Find(TextBox_Search.Text, from, to, RichTextBoxFinds.None);

            if (start == -1) return false;

            richTextBox_ScriptFile.SelectionStart = start;
            richTextBox_ScriptFile.ScrollToCaret();
            return true;
        }

        private bool FindPrevious()
        {
            int from = 0;
            int to = richTextBox_ScriptFile.SelectionStart - 1;
            int start = richTextBox_ScriptFile.Find(TextBox_Search.Text, from, to, RichTextBoxFinds.Reverse);

            if (start == -1) return false;

            richTextBox_ScriptFile.SelectionStart = start;
            richTextBox_ScriptFile.ScrollToCaret();
            return true;
        }

        private void _timerApply_Tick(object sender, EventArgs e)
        {
            _timerApply.Enabled = false;
            UpdateScriptMessage();
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

        private void decompressAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DecompressScripts();
        }

        private void compressAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(CompressScripts);
            Enabled = false;
            thread.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Conan Explorer Script (*.ces)|*.ces";

            if (openFileDialog.ShowDialog() == DialogResult.OK && File.Exists(openFileDialog.FileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ScriptFile));
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    ScriptFile = (ScriptFile)serializer.Deserialize(reader);
                }
            }
            else
            {
                return;
            }
            
            bool fontFound = false;
            if(ScriptFile.FontName == "Quarlow")
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
                            if(Directory.Exists(tempPathForZip))
                                Directory.Delete(tempPathForZip, true);

                            using (var client = new WebClient())
                            {
                                client.DownloadFile("http://chapter731.net/downloads/fonts/Quarlow.zip", zipTempPath);
                            }

                            System.IO.Compression.ZipFile.ExtractToDirectory(zipTempPath, tempPathForZip);
                            Process.Start("C:\\Windows\\System32\\fontview.exe" , tempPathForZip + "Quarlow.ttf");

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

            toolStripMenuItem_FontSettings.Enabled = true;
            toolStripMenuItem_Format.Enabled = true;
            toolStripMenuItem_DeFormat.Enabled = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ScriptFile.Scripts.Count == 0)
            {
                MessageBox.Show("Decompress all first!");
                return;
            }
            if (_lastScriptFile != null) _lastScriptFile.TextBuffer = richTextBox_ScriptFile.Text.Replace("\n", "\r\n");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Conan Explorer Script (*.ces)|*.ces";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ScriptFile));
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    serializer.Serialize(writer, ScriptFile);
                }
                Text = String.Format("Script Editor - \"{0}\"", saveFileDialog.FileName);
            }
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

        private void viewScriptToolStripMenuItem_Click(object sender, EventArgs e)
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

            //ScriptAnalyseWindow scriptAnalyseWindow = new ScriptAnalyseWindow(ScriptFileCollection);
            //scriptAnalyseWindow.ShowDialog();
        }

        private void richTextBox_ScriptFile_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel_Row.Text = "Row: " + (richTextBox_ScriptFile.GetLineFromCharIndex(richTextBox_ScriptFile.SelectionStart) + 1);
            if (_updatingMessage) return;
            FindNearestMessage();
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
            }
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
            _changingSelection = true;

            _timerApply.Enabled = false;
            ScriptMessage message = (ScriptMessage)listBox_ScriptMessages.SelectedItem;
            if (message == null) return;
            richTextBox_ScriptMessage.Text = message.Content;
            UpdatePreview(message);

            if (listBox_ScriptMessages.SelectedIndex != -1)
            {
                ScriptMessage scriptMessage = (ScriptMessage)listBox_ScriptMessages.SelectedItem;
                richTextBox_ScriptFile.SelectionStart = richTextBox_ScriptFile.GetFirstCharIndexFromLine(scriptMessage.LineIndex);
                richTextBox_ScriptFile.ScrollToCaret();
            }

            _changingSelection = false;
        }

        private void generateScriptToolStripMenuItem_Click(object sender, EventArgs e)
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
            if (_changingSelection) return;
            _timerApply.Enabled = true;
        }

        private void toolStripMenuItem_GenerateFont_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_PreviewColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateScriptMessage();
        }

        private void richTextBox_ScriptFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox_ScriptFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                searchPanel.Visible = !searchPanel.Visible;
                if (searchPanel.Visible) TextBox_Search.Focus();
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

        private void TextBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                searchPanel.Visible = !searchPanel.Visible;
                if (searchPanel.Visible) TextBox_Search.Focus();
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

        private void Button_Search_Click(object sender, EventArgs e)
        {
            if (!FindNext())
            {
                MessageBox.Show("No match found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            richTextBox_ScriptFile.Focus();
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

        private void switchRawEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox_ScriptMessages.Items.Clear();
            _elements.Clear();

            ScriptDocument mergeDocument = new ScriptDocument();
            foreach (var scriptFile in ScriptFile.Scripts)
            {
                mergeDocument.TextBuffer += scriptFile.TextBuffer;
            }

            ScriptDocument file = mergeDocument;
            richTextBox_ScriptFile.Select(0, 0);
            richTextBox_ScriptFile.ScrollToCaret();
            richTextBox_ScriptFile.Text = file.TextBuffer;
            _lastScriptFile = file;
            
            foreach (IScriptElement element in ScriptParser.Parse(file))
            {
                _elements.Add(element);
                if (element.GetType() != typeof(ScriptMessage)) continue;
                listBox_ScriptMessages.Items.Add(element);
            }

            listBox_ScriptMessages.SetSelected(0, true);
        }
    }
}
