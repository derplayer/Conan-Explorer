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
    public partial class SelectModeWindow : Form
    {
        public byte Mode;

        public SelectModeWindow()
        {
            InitializeComponent();
        }

        private void button_Compress_Click(object sender, EventArgs e)
        {
            if (radioButton_Mode0.Checked)
            {
                Mode = 0;
            }
            else if (radioButton_Mode1.Checked)
            {
                Mode = 1;
            }
            else if (radioButton_Mode2.Checked)
            {
                Mode = 2;
            }
        }
    }
}
