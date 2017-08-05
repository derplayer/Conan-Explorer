using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan.Filetypes;
using System.IO;

namespace ConanExplorer.Controls
{
    public partial class TextControl : UserControl
    {
        private static Encoding _shiftJis = Encoding.GetEncoding("shift_jis");

        public TextControl()
        {
            InitializeComponent();
        }

        public void Update(BaseFile baseFile)
        {
            if (baseFile == null)
            {
                richTextBox.Clear();
                return;
            }

            if (baseFile.Exists())
            {
                using (FileStream fileStream = new FileStream(baseFile.FilePath, FileMode.Open))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    richTextBox.Text = _shiftJis.GetString(buffer);
                }
            }
        }
    }
}
