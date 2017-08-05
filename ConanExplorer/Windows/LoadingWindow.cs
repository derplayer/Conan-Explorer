using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan;

namespace ConanExplorer.Windows
{
    public partial class LoadingWindow : Form
    {
        private TaskProgress _progress;

        public LoadingWindow(TaskProgress progress)
        {
            InitializeComponent();
            _progress = progress;
        }

        private void timer_Update_Tick(object sender, EventArgs e)
        {
            progressBar_Progress.Value = _progress.Progress;
        }
    }
}
