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
    public partial class DebugBmp2Tim : Form
    {

        public DebugBmp2Tim()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.bmp|*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.tim|*.tim";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TIMEncodingSettings settings = new TIMEncodingSettings();
            TIMFile timFile = new TIMFile(textBox2.Text);
            timFile.SetBitmap(new Bitmap(textBox1.Text), settings);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            TIMFile timFile = new TIMFile(textBox2.Text);
            pictureBox1.Image = timFile.GetBitmap();
        }
    }
}
