using ConanExplorer.Conan;
using GoogleTranslateFreeApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.Windows
{
    public partial class HardcodedTextWindow : Form
    {
        private ScriptFile _scriptFile;
        private Encoding _shiftJist = Encoding.GetEncoding("shift_jis");
        private GoogleTranslator translator = new GoogleTranslator();

        public HardcodedTextWindow(ScriptFile scriptFile)
        {
            InitializeComponent();
            _scriptFile = scriptFile;
            InitializeList();
        }

        private void InitializeList()
        {
            //Parse data from resources
            ImportCSVToSJIS(Properties.Resources.SLPS_016);

            foreach (HardCodedText text in HardCodedText.Texts)
            {
                HardCodedText foundText = _scriptFile.HardCodedTexts.FirstOrDefault(t => t.Offset == text.Offset);
                if (foundText != null)
                {
                    listBox_HardcodedTexts.Items.Add(foundText);
                }
                else
                {
                    _scriptFile.HardCodedTexts.Add(text);
                    listBox_HardcodedTexts.Items.Add(_scriptFile.HardCodedTexts.Last());
                }
            }
            listBox_HardcodedTexts.SelectedIndex = 0;
        }

        private void listBox_HardcodedTexts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_HardcodedTexts.SelectedItem != null)
            {
                HardCodedText text = (HardCodedText)listBox_HardcodedTexts.SelectedItem;
                textBox_OriginalString.Text = text.OriginalString;
                textBox_NewString.Text = text.NewString;
                textBox_NewString.MaxLength = text.Length;

                if(ApplicationState.Instance.ProjectFile == null)
                    textBox_CurrentString.Text = "PROJECT BINARY IS NOT LOADED!";
                else
                    textBox_CurrentString.Text = text.CurrentString;
                textBox_Translation.Text = text.Translation;
            }
        }

        private void textBox_NewString_TextChanged(object sender, EventArgs e)
        {
            HardCodedText text = (HardCodedText)listBox_HardcodedTexts.SelectedItem;
            text.NewString = textBox_NewString.Text;
        }

        private List<HardCodedText> GetSJISFromBlob(byte[] buffer, bool nullterminated = false, int stringlength = 3)
        {
            int buffer_pos = 0;
            int tmp_pos = -1;
            List<HardCodedText> finalList = new List<HardCodedText>();

            //ASCII ranges
            //if (ENABLE_ASCII)
            //    if (0x20 <= buffer[buffer_pos] && buffer[buffer_pos] <= 0x7E)
            //        return true;

            StringBuilder tmpStr = new StringBuilder();

            while (buffer_pos + 1 <= buffer.Length) {

            //SJIS ranges (my eyes)
            if (        // 81
                        (0x81 == buffer[buffer_pos] && (0x40 <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0xAC)) ||

                        //82
                        (0x82 == buffer[buffer_pos] && (0x4F <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0xF1)) ||

                        //83
                        (0x83 == buffer[buffer_pos] && (0x40 <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0xD6)) ||

                        //84
                        (0x84 == buffer[buffer_pos] && (0x40 <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0x91)) ||

                        //88
                        (0x88 == buffer[buffer_pos] && (0x9F <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0xFC)) ||

                        // 89 - 9F and E0 - E9 SJIS
                        (((0x89 <= buffer[buffer_pos] && buffer[buffer_pos] <= 0x9F) || (0xE0 <= buffer[buffer_pos] && buffer[buffer_pos] <= 0xE9)) &&
                            (0x40 <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0xFC)) ||

                        // EA SJIS
                        (0xEA == buffer[buffer_pos] && (0x40 <= buffer[buffer_pos + 1] && buffer[buffer_pos + 1] <= 0xA2)))

                {
                    byte[] tmpSymbolRaw = new byte[2] { buffer[buffer_pos], buffer[buffer_pos + 1] };
                    string tmpSymbol = _shiftJist.GetString(tmpSymbolRaw);
                    tmpStr.Append(tmpSymbol);

                    if (tmpStr.Length == 1)
                        tmp_pos = buffer_pos;

                    buffer_pos += 2;
                    continue;
                }

                if (buffer[buffer_pos] == 0x00 && nullterminated) //TODO: SJIS NULL TERM IS DOUBLE 0x00
                {
                    if (tmpStr.Length >= stringlength)
                    {
                        //tmpStr.Append(0x00);
                        finalList.Add(new HardCodedText
                        {
                            Offset = tmp_pos,
                            OriginalString = tmpStr.ToString(),
                            Length = tmpStr.Length * 2
                        }
                        );
                    }
                }

                tmpStr.Clear();
                tmp_pos = -1;
                buffer_pos++;
            }

            return finalList;
        }

        private void button_RemoveSel_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(listBox_HardcodedTexts);
            selectedItems = listBox_HardcodedTexts.SelectedItems;

            if (listBox_HardcodedTexts.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    listBox_HardcodedTexts.Items.Remove(selectedItems[i]);
            }
        }

        private void button_LoadBlob_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Binary files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    using (BinaryReader reader = new BinaryReader(openFileDialog.OpenFile()))
                    {
                        byte[] newBuffer = reader.ReadBytes((int)reader.BaseStream.Length);
                        var sjisData = GetSJISFromBlob(newBuffer, true);
                        Debug.WriteLine(sjisData.Count + " SJIS-Strings were found!");

                        //Export as csv file
                        ExportSJISToCSV(sjisData, Path.ChangeExtension(filePath, "csv"));
                    }
                }
            }
        }

        public void ExportSJISToCSV(List<HardCodedText> sjisList, string path)
        {
            StreamWriter file = new StreamWriter(path, false, _shiftJist);
            string[] sjisFormat = { "Offset", "OriginalString", "Translation", "Translation_Eng", "OriginalLength" };
            CultureInfo cultInfo = CultureInfo.CreateSpecificCulture("ja-JP");
            Language from = Language.Japanese;
            Language to = Language.German;

            foreach (var collumn in sjisFormat)
            {
                if (collumn != sjisFormat.LastOrDefault())
                    file.Write(collumn + ";");
                else
                    file.WriteLine(collumn + ";");
            }

            int progCnt = 0;
            foreach (HardCodedText sjisItem in sjisList)
            {
                file.Write(sjisItem.Offset + ";");
                file.Write(sjisItem.OriginalString + ";");

                //TranslationResult result = await translator.TranslateLiteAsync(sjisItem.OriginalString, from, to);
                //file.WriteLine(result.MergedTranslation.Replace("\r", " ").Replace("\n", string.Empty) + ";");
                file.Write(";");
                file.Write(";");
                file.WriteLine(sjisItem.Length + ";");

                progCnt++;
                Debug.WriteLine(progCnt + " strings were written to export!");
            }

            file.Close();
        }

        public void ImportCSVToSJIS(string content)
        {
            HardCodedText.Texts = new List<HardCodedText>();
            List<string> lines = content.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            ).ToList();

            lines.RemoveAt(0); //remove header
            foreach (var line in lines)
            {
                try
                {
                    string[] fields = line.Split(';');
                    //Ignore empty lines or other stuff that can happen
                    if(fields.Count() >= 4) { 
                        var hardctxt = new HardCodedText
                        {
                            Offset = Convert.ToInt32(fields[0]),
                            Length = Convert.ToInt32(fields[1]),
                            OriginalString = fields[2],
                            Translation = fields[3], //[4] for english
                        };

                        //String blacklist (breaks game engine)
                        //14368; 14; 孤島の宝物事件; Schatzinsel; ; Speicherstand: Inhalt;
                        //14384; 14; 同級生殺人事件; Mädchenschule; ; Speicherstand: Inhalt;
                        if (hardctxt.Offset == 14368 || hardctxt.Offset == 14384) continue;
                        HardCodedText.Texts.Add(hardctxt);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something is wrong with a Hardcoded String: " + e.ToString());
                }
            }

            //Here are string that are missing in database, because the search skipped everything under 0x4 of length
            HardCodedText.Texts.Add(new HardCodedText(0x000A6E90, 0x4, "はい", "Ja"));
        }

        private void button_DefNewStr_Click(object sender, EventArgs e)
        {
            foreach (HardCodedText item in listBox_HardcodedTexts.Items)
            {
                if (item.NewString == null) item.NewString = item.Translation.Truncate(item.Length);
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            foreach (HardCodedText item in listBox_HardcodedTexts.Items)
            {
                if (item.NewString != null) item.NewString = null;
            }
        }
    }

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

}
