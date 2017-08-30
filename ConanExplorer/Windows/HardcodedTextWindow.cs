using ConanExplorer.Conan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.Windows
{
    public partial class HardcodedTextWindow : Form
    {
        private ScriptFile _scriptFile;

        public HardcodedTextWindow(ScriptFile scriptFile)
        {
            InitializeComponent();
            _scriptFile = scriptFile;
            InitializeList();
        }

        private void InitializeList()
        {
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
            HardCodedText text = (HardCodedText)listBox_HardcodedTexts.SelectedItem;
            textBox_OriginalString.Text = text.OriginalString;
            textBox_NewString.Text = text.NewString;
            textBox_NewString.MaxLength = text.Length;
            textBox_CurrentString.Text = text.CurrentString;
        }

        private void textBox_NewString_TextChanged(object sender, EventArgs e)
        {
            HardCodedText text = (HardCodedText)listBox_HardcodedTexts.SelectedItem;
            text.NewString = textBox_NewString.Text;
        }
    }
}
