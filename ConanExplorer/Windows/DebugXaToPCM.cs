using ConanExplorer.Conan.Filetypes;
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

namespace ConanExplorer.Windows
{
    public partial class DebugXaToPCM : Form
    {
        public DebugXaToPCM()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.XA|*.XA";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.RAW|*.RAW";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var test = new XAFile(textBox1.Text, true);
        }
    }
}
