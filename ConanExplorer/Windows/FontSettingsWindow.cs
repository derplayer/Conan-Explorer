using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan;
using ConanExplorer.ExtensionMethods;

namespace ConanExplorer.Windows
{
    public partial class FontSettingsWindow : Form
    {
        private ScriptFile _scriptFile;
        private readonly InstalledFontCollection _installedFontCollection = new InstalledFontCollection();

        public FontSettingsWindow(ScriptFile scriptFile)
        {
            InitializeComponent();
            _scriptFile = scriptFile;

            foreach (FontFamily font in _installedFontCollection.Families)
            {
                comboBox_Font.Items.Add(font);
            }
            comboBox_Font.DisplayMember = "Name";

            if (_scriptFile == null) return;
            SelectFont(_scriptFile.FontName);
            numericUpDown_FontSize.Value = _scriptFile.FontSize;
            textBox_AllowedSymbols.Text = new String(_scriptFile.AllowedSymbols.ToArray());
            textBox_AllowedSplittedSymbols.Text = new String(_scriptFile.AllowedSplittedSymbols.ToArray());

            UpdatePreview();
        }

        private void SelectFont(string fontName)
        {
            for (int i = 0; i < _installedFontCollection.Families.Length; i++)
            {
                FontFamily fontFamily = _installedFontCollection.Families[i];
                if (fontFamily.Name == fontName)
                {
                    comboBox_Font.SelectedIndex = i;
                    break;
                }
            }
            numericUpDown_FontSize.Value = _scriptFile.FontSize;
            textBox_AllowedSymbols.Text = new String(_scriptFile.AllowedSymbols.ToArray());
            textBox_AllowedSplittedSymbols.Text = new String(_scriptFile.AllowedSplittedSymbols.ToArray());

            UpdatePreview();
        }

        private void UpdatePreview()
        {
            _scriptFile.FontName = comboBox_Font.Text;
            _scriptFile.FontSize = (int) numericUpDown_FontSize.Value;
            Font font = _scriptFile.Font;

            if (String.IsNullOrWhiteSpace(textBox_PreviewText.Text))
            {
                List<Bitmap> allowedSymbolsBitmap = new List<Bitmap>();
                foreach (string line in textBox_AllowedSymbols.Lines)
                {
                    foreach (char character in line)
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                        allowedSymbolsBitmap.Add(fontCharacter.GetBitmap());
                    }
                }
                foreach (string line in textBox_AllowedSplittedSymbols.Lines)
                {
                    foreach (char character in line)
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character, character), font);
                        allowedSymbolsBitmap.Add(fontCharacter.GetBitmap());
                    }
                }
                pictureBox_Preview.Image = Graphic.CombineBitmaps(allowedSymbolsBitmap, 16);
            }
            else
            {
                _scriptFile = new ScriptFile();
                UpdateAllowedSymbols();
                HashSet<string> dictionary = _scriptFile.CreateDictionary(new string[] { textBox_PreviewText.Text });
                _scriptFile.GenerateFont(dictionary, font);

                //_scriptFile.GeneratedFont
            }
            
        }

        private void UpdateAllowedSymbols()
        {
            List<char> symbols = new List<char>();
            foreach (char symbol in textBox_AllowedSymbols.Text.ToArray())
            {
                if (symbol == '\n' || symbol == '\r' || symbol == ' ') continue;
                if (symbols.Contains(symbol)) continue;
                symbols.Add(symbol);
            }
            _scriptFile.AllowedSymbols = symbols;

            List<char> splittedSymbols = new List<char>();
            foreach (char symbol in textBox_AllowedSplittedSymbols.Text.ToArray())
            {
                if (symbol == '\n' || symbol == '\r' || symbol == ' ') continue;
                if (splittedSymbols.Contains(symbol)) continue;
                splittedSymbols.Add(symbol);
            }
            _scriptFile.AllowedSplittedSymbols = splittedSymbols;
        }


        private void button_UpdatePreview_Click(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void comboBox_Font_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scriptFile.FontName = comboBox_Font.Text;
        }

        private void numericUpDown_FontSize_ValueChanged(object sender, EventArgs e)
        {
            _scriptFile.FontSize = (int)numericUpDown_FontSize.Value;
        }

        private void textBox_AllowedSplittedSymbols_TextChanged(object sender, EventArgs e)
        {
            UpdateAllowedSymbols();
        }

        private void button_FontDialog_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowDialog();

            SelectFont(fontDialog.Font.Name);
            numericUpDown_FontSize.Value = (decimal)fontDialog.Font.Size;
        }
    }
}
