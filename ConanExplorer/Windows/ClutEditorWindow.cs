using ConanExplorer.Conan.Headers;
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
    public partial class ClutEditorWindow : Form
    {
        public TIMHeader Header
        {
            get
            {
                return clutControl1.Header;
            }
        }

        public ClutEditorWindow(TIMHeader header)
        {
            InitializeComponent();
            clutControl1.Header = header.DeepClone();
            DialogResult = DialogResult.Cancel;
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
