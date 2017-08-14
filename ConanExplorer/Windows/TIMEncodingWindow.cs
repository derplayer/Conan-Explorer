using ConanExplorer.Conan.Filetypes;
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
    public partial class TIMEncodingWindow : Form
    {
        public TIMEncodingSettings Settings = new TIMEncodingSettings();

        public TIMEncodingWindow()
        {
            InitializeComponent();
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            Settings.SetSemiTransparencyBit = checkBox_SetSemiTransparencyBit.Checked;
            Settings.BlackTransparent = checkBox_BlackTransparent.Checked;
            Settings.MagicPinkTransparent = checkBox_MagicPinkTransparent.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
