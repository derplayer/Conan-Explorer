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

namespace ConanExplorer.Windows
{
    public partial class ScriptEditorWindow : Form
    {
        private PKNFile _scriptPKN;
        private Encoding _shiftJis = Encoding.GetEncoding("shift_jis");
        private ScriptDocument _lastScriptFile;
        private List<IScriptElement> _elements = new List<IScriptElement>();
        private System.Windows.Forms.Timer _timerApply = new System.Windows.Forms.Timer();
        private Bitmap _window;
        private bool _changingSelection = false;
        private int lastFoundItem = 0;
        private bool _IsSearch = false;

        private Color[] _fontColors = new Color[]
            {
                Color.FromArgb(0, 0, 0, 0),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 0, 0, 248),
                Color.FromArgb(255, 248, 0, 0),
                Color.FromArgb(255, 0, 248, 248),
                Color.FromArgb(255, 255, 255, 0),
                Color.FromArgb(255, 0, 248, 0),
                Color.FromArgb(255, 0, 0, 80),
                Color.FromArgb(255, 96, 0, 0),
                Color.FromArgb(255, 144, 144, 144),
                Color.FromArgb(255, 0, 0, 160),
                Color.FromArgb(255, 192, 168, 0),
                Color.FromArgb(255, 0, 176, 192),
                Color.FromArgb(255, 192, 192, 0),
                Color.FromArgb(255, 0, 160, 0),
                Color.FromArgb(255, 192, 0, 0)
            };

        public ScriptFile ScriptFile;

        public ScriptEditorWindow()
        {
            InitializeComponent();
            richTextBox_Script.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_Script.AutoWordSelection = false;
            richTextBox_ScriptModelView.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_ScriptModelView.AutoWordSelection = false;

            _timerApply.Interval = 200;
            _timerApply.Tick += _timerApply_Tick;

            _window = (Bitmap)Resources.ResourceManager.GetObject("WINDOW");

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
            richTextBox_Script.Text = ((ScriptDocument)listBox_ScriptFiles.SelectedItem).TextBuffer;
            if (listBox_ScriptModelList.SelectedIndex == -1) return;
            richTextBox_ScriptModelView.Text = ((ScriptMessage)listBox_ScriptModelList.SelectedItem).Content;

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
            //(1 unit = 16px)
            //speech 13x6 
            //selection 15x?

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
                    DrawDialogueWindow(graphic, 13, 6, messageCount);
                    DrawDialogueText(graphic, message);
                }
                pictureBox_MessagePreview.Image = bitmap;

                //Text offset 15x7

                //144;0 - 160;80 sel blue
                //160;0 - 176;80 speech
                //176;0 - 192;80 sel red
                //192;0 - 208;80 sel green
                //208;0 - 224;80 sel yellow

                //0 - 16 middle
                //16 - 32 top/bottom
                //32 - 48 left/right
                //48 - 64 corner edgy (bottom left/top right)
                //64 - 80 corner diagonal (bottom right/top left)     

                //_window
            }
            else if (comboBox_PreviewType.SelectedIndex == 1)
            {
                if (!message.IsSelectionWindow) return;
                Bitmap bitmap = new Bitmap(15 * 16, 11 * 16);
                using (Graphics graphic = Graphics.FromImage(bitmap))
                {
                    DrawSelectionWindow(graphic, 15, 11, comboBox_PreviewColor.SelectedIndex);
                    DrawSelectionText(graphic, message);
                };
                pictureBox_MessagePreview.Image = bitmap;
            }
            
        }

        private void DrawSelectionText(Graphics graphics, ScriptMessage message)
        {
            Color fontColor = Color.FromArgb(255, 255, 255);
            Font font = ScriptFile.Font;
            int left = 15;
            int top = 7;
            string[] lines = ScriptParser.TextToLines(message.Content);

            bool open = false;
            foreach (string line in lines)
            {
                if (line.StartsWith("%SEL:"))
                {
                    open = true;
                    continue;
                }

                if (line.StartsWith("%END:"))
                { 
                    open = false;
                    continue;
                }

                if (open)
                {
                    string[] splitted = line.Split(',');
                    string displayText = splitted[2];

                    for (int i = 0; i < displayText.Length; i++)
                    {
                        char character = displayText[i];
                        
                        if (left >= 223 || top >= 159) continue;
                        if (ScriptFile.IsValidChar(character))
                        {
                            i++;
                            if (i == displayText.Length)
                            {
                                FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                                Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                                graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                                left += 16;
                            }
                            else if (ScriptFile.IsValidChar(displayText[i]))
                            {
                                FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character, displayText[i]), font);
                                Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                                graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                                left += 16;
                            }
                            else
                            {
                                FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                                Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                                graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                                left += 16;
                            }
                        }
                    }
                }

                top += 16;
                left = 15;
            }
        }

        private void DrawDialogueText(Graphics graphics, ScriptMessage message)
        {
            Color fontColor = Color.FromArgb(255, 255, 255);
            int line = 0;
            int left = 15;
            int top = 7;
            int windowCount = 1;
            int matchIndex = 0;
            MatchCollection matches = message.Matches;
            for (int i = 0; i < message.Content.Length; i++)
            {
                if (message.Content[i] == '\n' || message.Content[i] == '\r') continue;
                if (matchIndex < matches.Count)
                {
                    if (i == matches[matchIndex].Index + 1)
                    {
                        Match match = matches[matchIndex];
                        if (match.Groups[1].Value == "COL")
                        {
                            int colorIndex = 0;
                            int.TryParse(match.Groups[2].Value, out colorIndex);
                            if (colorIndex > 15)
                            {
                                fontColor = _fontColors[0];
                            }
                            else
                            {
                                fontColor = _fontColors[colorIndex];
                            }
                        }
                        else if (match.Groups[1].Value == "LF")
                        {
                            line++;
                            top += 16;
                            left = 15;
                        }
                        else if (match.Groups[1].Value == "CLR")
                        {
                            left = 15;
                            top = windowCount * 96 + 7;
                            windowCount++;
                        }
                        matchIndex++;
                        i += match.Length - 2;
                        continue;
                    }
                }

                char character = message.Content[i];
                Font font = ScriptFile.Font;
                if (left >= 191) continue;
                if (ScriptFile.IsValidChar(character))
                {
                    i++;
                    if (i == message.Content.Length)
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                        Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                        graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                        left += 16;
                    }
                    else if (ScriptFile.IsValidChar(message.Content[i]))
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character, message.Content[i]), font);
                        Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                        graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                        left += 16;
                    }
                    else
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                        Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                        graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                        left += 16;
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        private void DrawSelectionWindow(Graphics graphics, int width, int height, int color = 0)
        {
            int[] offsets = new int[] { 144, 176, 192, 208 };
            int colorOffset = offsets[color];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle rectangle = new Rectangle(j * 16, i * 16, 16, 16);
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            //corner top left
                            graphics.DrawImage(_window, rectangle, colorOffset, 64, 16, 16, GraphicsUnit.Pixel);
                        }
                        else if (j == width - 1)
                        {
                            //corner top right
                            graphics.DrawImage(_window, rectangle, colorOffset, 48, 16, 16, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            //top
                            graphics.DrawImage(_window, rectangle, colorOffset, 16, 16, 16, GraphicsUnit.Pixel);
                        }
                    }
                    else if (i == height - 1)
                    {
                        if (j == 0)
                        {
                            //corner bottom left
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            graphics.DrawImage(_window, rectangle, 240 - colorOffset, 192, 16, 16, GraphicsUnit.Pixel);
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else if (j == width - 1)
                        {
                            //corner bottom right
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            graphics.DrawImage(_window, rectangle, 240 - colorOffset, 176, 16, 16, GraphicsUnit.Pixel);
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else
                        {
                            //bottom
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            graphics.DrawImage(_window, rectangle, colorOffset, 224, 16, 16, GraphicsUnit.Pixel);
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            //left
                            graphics.DrawImage(_window, rectangle, colorOffset, 32, 16, 16, GraphicsUnit.Pixel);
                        }
                        else if (j == width - 1)
                        {
                            //right
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            graphics.DrawImage(_window, rectangle, 240 - colorOffset, 32, 16, 16, GraphicsUnit.Pixel);
                            _window.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else
                        {
                            //middle
                            graphics.DrawImage(_window, rectangle, colorOffset, 0, 16, 16, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
        }

        private void DrawDialogueWindow(Graphics graphics, int width, int height, int count = 1)
        {
            int height_offset = 0;
            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Rectangle rectangle = new Rectangle(j * 16, i * 16 + height_offset, 16, 16);
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                //corner left
                                graphics.DrawImage(_window, rectangle, 160, 64, 16, 16, GraphicsUnit.Pixel);
                            }
                            else if (j == width - 1)
                            {
                                //corner right
                                graphics.DrawImage(_window, rectangle, 160, 48, 16, 16, GraphicsUnit.Pixel);
                            }
                            else
                            {
                                //top
                                graphics.DrawImage(_window, rectangle, 160, 16, 16, 16, GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == height - 1)
                        {
                            if (j == 0)
                            {
                                //corner left
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(_window, rectangle, 160, 176, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                            else if (j == width - 1)
                            {
                                //corner right
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(_window, rectangle, 160, 192, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                            else
                            {
                                //bottom
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(_window, rectangle, 160, 224, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                //left
                                graphics.DrawImage(_window, rectangle, 160, 32, 16, 16, GraphicsUnit.Pixel);
                            }
                            else if (j == width - 1)
                            {
                                //right
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                graphics.DrawImage(_window, rectangle, 80, 32, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            }
                            else
                            {
                                //middle
                                graphics.DrawImage(_window, rectangle, 160, 0, 16, 16, GraphicsUnit.Pixel);
                            }
                        }
                    }
                }
                height_offset += 96;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="selection">True for drawing selection window</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        /// <param name="color">0 = blue, 1 = red, 2 = green, 3 = yellow</param>
        private void DrawWindow(Graphics graphics, bool selection, int width, int height, int color = -1)
        {
            if (selection)
            {
                //TODO Selection Window Preview
            }
            else
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Rectangle rectangle = new Rectangle(j * 16, i * 16, 16, 16);
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                //corner left
                                graphics.DrawImage(_window, rectangle, 160, 64, 16, 16, GraphicsUnit.Pixel);
                            }
                            else if (j == width - 1)
                            {
                                //corner right
                                graphics.DrawImage(_window, rectangle, 160, 48, 16, 16, GraphicsUnit.Pixel);
                            }
                            else
                            {
                                //top
                                graphics.DrawImage(_window, rectangle, 160, 16, 16, 16, GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == height - 1)
                        {
                            if (j == 0)
                            {
                                //corner left
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(_window, rectangle, 160, 176, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                            else if (j == width - 1)
                            {
                                //corner right
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(_window, rectangle, 160, 192, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                            else
                            {
                                //bottom
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(_window, rectangle, 160, 224, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                //left
                                graphics.DrawImage(_window, rectangle, 160, 32, 16, 16, GraphicsUnit.Pixel);
                            }
                            else if (j == width - 1)
                            {
                                //right
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                graphics.DrawImage(_window, rectangle, 80, 32, 16, 16, GraphicsUnit.Pixel);
                                _window.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            }
                            else
                            {
                                //middle
                                graphics.DrawImage(_window, rectangle, 160, 0, 16, 16, GraphicsUnit.Pixel);
                            }
                        }
                    }
                }
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

            richTextBox_Script.Text = "";
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

        public void UpdateSelection()
        {
            if (_changingSelection) return;
            IScriptElement element = (IScriptElement)listBox_ScriptModelList.SelectedItem;
            if (element == null) return;
            if (element.GetType() == typeof(ScriptMessage))
            {
                ScriptMessage message = (ScriptMessage)element;
                message.Content = richTextBox_ScriptModelView.Text.Replace("\n", "\r\n");
                UpdatePreview(message);
            }

            ScriptDocument script = (ScriptDocument)listBox_ScriptFiles.SelectedItem;
            ScriptParser.Parse(script, _elements);

            richTextBox_Script.Select(0, 0);
            richTextBox_Script.ScrollToCaret();
            richTextBox_Script.Text = script.TextBuffer;
        }

        private void _timerApply_Tick(object sender, EventArgs e)
        {
            _timerApply.Enabled = false;
            UpdateSelection();
        }

        private void listBox_ScriptFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lastScriptFile != null) _lastScriptFile.TextBuffer = richTextBox_Script.Text.Replace("\n", "\r\n");

            ScriptDocument file = ScriptFile.Scripts[listBox_ScriptFiles.SelectedIndex];
            richTextBox_Script.Select(0, 0);
            richTextBox_Script.ScrollToCaret();
            richTextBox_Script.Text = file.TextBuffer;
            _lastScriptFile = file;

            _elements.Clear();
            listBox_ScriptModelList.Items.Clear();
            foreach (IScriptElement element in ScriptParser.Parse(file))
            {
                _elements.Add(element);
                if (element.GetType() != typeof(ScriptMessage)) continue;
                listBox_ScriptModelList.Items.Add(element);
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
            if (_lastScriptFile != null) _lastScriptFile.TextBuffer = richTextBox_Script.Text.Replace("\n", "\r\n");

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
            file.TextBuffer = richTextBox_Script.Text.Replace("\n", "\r\n");
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

        private void richTextBox_Script_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel_Row.Text = "Row: " + (richTextBox_Script.GetLineFromCharIndex(richTextBox_Script.SelectionStart) + 1);
        }

        private void toolStripMenuItem_Format_Click(object sender, EventArgs e)
        {
            Format();
        }

        private void toolStripMenuItem_DeFormat_Click(object sender, EventArgs e)
        {
            DeFormat();
        }

        private void listBox_ScriptModelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changingSelection = true;
            _timerApply.Enabled = false;
            ScriptMessage message = (ScriptMessage)listBox_ScriptModelList.SelectedItem;
            if (message == null) return;
            richTextBox_ScriptModelView.Text = message.Content;
            _changingSelection = false;
            UpdateSelection();
        }

        private void generateScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (IScriptElement element in listBox_ScriptModelList.Items)
            {
                stringBuilder.Append(element.Text);
            }
            DebugTextWindow window = new DebugTextWindow(stringBuilder.ToString());
            window.Show();
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            IScriptElement element = (IScriptElement)listBox_ScriptModelList.SelectedItem;
            if (element.GetType() == typeof(ScriptMessage))
            {
                ScriptMessage message = (ScriptMessage)element;
                message.Content = richTextBox_ScriptModelView.Text.Replace("\n", "\r\n");
            }

            ScriptDocument script = (ScriptDocument)listBox_ScriptFiles.SelectedItem;
            ScriptParser.Parse(script, _elements);

            richTextBox_Script.Select(0, 0);
            richTextBox_Script.ScrollToCaret();
            richTextBox_Script.Text = script.TextBuffer;
        }

        private void richTextBox_ScriptModelView_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem_GenerateFont_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_PreviewColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void richTextBox_Script_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox_Script_KeyDown(object sender, KeyEventArgs e)
        {
            if (_IsSearch && e.Control && e.KeyCode == Keys.F)
            {
                this.searchPanel.Visible = false;
                _IsSearch = false;
                return;
            }

            if (_IsSearch == false && e.Control && e.KeyCode == Keys.F)
            {
                this.searchPanel.Visible = true;
                _IsSearch = true;
                return;
            }

        }

        private void Button_Search_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds options = RichTextBoxFinds.None;

            int from = richTextBox_Script.SelectionStart;
            int to = richTextBox_Script.TextLength - 1;

            int start = 0;
            start = richTextBox_Script.Find(TextBox_Search.Text, from, to, options);

            if(lastFoundItem == start)
            {
                from = lastFoundItem + 1;
                richTextBox_Script.Find(TextBox_Search.Text, from, to, options);
            }

            if (start > 0)
            {
                richTextBox_Script.SelectionStart = start;
                richTextBox_Script.SelectionLength = TextBox_Search.TextLength;
                richTextBox_Script.ScrollToCaret();
                richTextBox_Script.Refresh();
                richTextBox_Script.Focus();

                lastFoundItem = start; //update last item index pos
            }
            else
            {
                MessageBox.Show("No match found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void richTextBox_ScriptModelView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.W) //New Window Hotkey
            {
                richTextBox_ScriptModelView.SelectedText += "Test";
                return;
            }
        }
    }
}
