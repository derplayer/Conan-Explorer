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
using System.Drawing.Imaging;
using ConanExplorer.Windows;

namespace ConanExplorer.Controls
{
    public partial class TIMControl : UserControl
    {
        private TIMFile _timFile;

        public TIMControl()
        {
            InitializeComponent();
        }

        public void Update(TIMFile timFile)
        {
            _timFile = timFile;
            pictureBox_Image.Image = timFile.GetBitmap();
            textBox_BPP.Text = timFile.TIMHeader.BPP.ToString();
            textBox_Width.Text = timFile.TIMHeader.ImageWidthPixels + " Pixels";
            textBox_Height.Text = timFile.TIMHeader.ImageHeight + " Pixels";
        }

        private void button_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.bmp|*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TIMEncodingSettings settings = new TIMEncodingSettings();
                TIMEncodingWindow window = new TIMEncodingWindow();
                if (window.ShowDialog() == DialogResult.OK) settings = window.Settings;

                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    Bitmap bitmap = new Bitmap(fileStream);
                    _timFile.SetBitmap(bitmap, settings);
                    bitmap.Dispose();
                }
                pictureBox_Image.Image = _timFile.GetBitmap();
            }
        }

        private void button_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.bmp|*.bmp";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Bitmap bitmap = _timFile.GetBitmap();
                    bitmap.Save(fileStream, ImageFormat.Bmp);
                    bitmap.Dispose();
                }
            }
            
        }
    }
}
