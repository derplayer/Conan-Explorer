using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan.Headers;

namespace ConanExplorer.Controls
{
    public partial class CLUTControl : UserControl
    {
        private TIMHeader _header;

        public TIMHeader Header
        {
            get { return _header; }
            set
            {
                _header = value;
                UpdateCLUT();
            }
        }

        public CLUTControl()
        {
            InitializeComponent();
        }

        public CLUTControl(TIMHeader header)
        {
            InitializeComponent();
            _header = header;

            UpdateCLUT();
        }

        public void UpdateCLUT()
        {
            if (_header == null) return;
            foreach (CLUTEntry entry in _header.ClutEntries)
            {
                flowLayoutPanel_Palette.Controls.Add(new CLUTButton(entry));
            }
        }
    }
}
