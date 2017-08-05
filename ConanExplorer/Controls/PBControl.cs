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
    public partial class PBControl : UserControl
    {
        public PBControl()
        {
            InitializeComponent();
        }

        public void Update(PBFile pbFile)
        {
            textBox_FileCount.Text = pbFile.Files.Count.ToString();
        }
    }
}
