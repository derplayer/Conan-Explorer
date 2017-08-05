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
    public partial class DebugTextWindow : Form
    {
        public DebugTextWindow(string text)
        {
            InitializeComponent();
            richTextBox_Text.Text = text;
        }
    }
}
