using ConanExplorer.Conan.Headers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.Controls
{
    class CLUTButton : Button
    {
        public CLUTEntry CLUTEntry;

        public CLUTButton(CLUTEntry clutEntry)
        {
            MouseUp += CLUTButton_Click;
            CLUTEntry = clutEntry;
            Width = 25;
            Height = 25;

            UpdateButton();
        }

        public void UpdateButton()
        {
            BackColor = CLUTEntry.Color;
            if (BackColor.GetBrightness() > 0.5)
            {
                ForeColor = Color.Black;
            }
            else
            {
                ForeColor = Color.White;
            }

            if (CLUTEntry.SemiTransparentBit)
            {
                Text = "T";
            }
            else
            {
                Text = "";
            }
        }

        private void CLUTButton_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CLUTEntry.SemiTransparentBit = !CLUTEntry.SemiTransparentBit;
                UpdateButton();
            }
            else if (e.Button == MouseButtons.Right)
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.Color = CLUTEntry.Color;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    CLUTEntry.Color = colorDialog.Color;
                    UpdateButton();
                }
            }
        }
    }
}
