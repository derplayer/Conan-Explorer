using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan.Filetypes;

namespace ConanExplorer.Windows
{
    public partial class ScriptViewerWindow : Form
    {
        private Encoding _shiftJist = Encoding.GetEncoding("shift_jis");
        public ScriptViewerWindow(BaseFile file)
        {
            InitializeComponent();
            richTextBox_Script.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            richTextBox_Script.AutoWordSelection = false;

            using (BinaryReader reader = new BinaryReader(new FileStream(file.FilePath, FileMode.Open)))
            {
                byte[] charArray = new byte[reader.BaseStream.Length];
                reader.Read(charArray, 0, charArray.Length);
                richTextBox_Script.Text = _shiftJist.GetString(charArray);
            }
        }
    }
}
