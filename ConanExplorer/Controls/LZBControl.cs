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

namespace ConanExplorer.Controls
{
    public partial class LZBControl : UserControl
    {
        public LZBControl()
        {
            InitializeComponent();
            dynamicControl.EnabledLZB = false;
            dynamicControl.Initialize();
        }

        public void Update(LZBFile lzbFile)
        {
            textBox_Mode.Text = String.Format("0x{0:X2}", lzbFile.LZSSHeader.Mode);
            textBox_UncompressedSize.Text = String.Format("0x{0:X8}", lzbFile.LZSSHeader.UncompressedSize);
            textBox_Param1.Text = String.Format("0x{0:X4}", lzbFile.LZSSHeader.Param1);
            textBox_Param2.Text = String.Format("0x{0:X2}", lzbFile.LZSSHeader.Param2);

            if (lzbFile.DecompressedFile == null)
            {
                dynamicControl.Update(null);
                return;
            }
            dynamicControl.Update(lzbFile.DecompressedFile);
        }
    }
}
