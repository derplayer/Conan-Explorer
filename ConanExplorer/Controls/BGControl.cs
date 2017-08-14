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
using ConanExplorer.ExtensionMethods;

namespace ConanExplorer.Controls
{
    public partial class BGControl : UserControl
    {
        public BGControl()
        {
            InitializeComponent();
        }

        public void Update(BGFile bgFile)
        {
            textBox_FileCount.Text = bgFile.Header.FileCount.ToString();
            textBox_Mask.Text = Conversion.BoolArrayToBinary(bgFile.Header.IndicesMask);
        }
    }
}
