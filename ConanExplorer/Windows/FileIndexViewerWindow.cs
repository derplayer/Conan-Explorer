using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan;

namespace ConanExplorer.Windows
{
    public partial class FileIndexViewerWindow : Form
    {
        public FileIndexViewerWindow()
        {
            InitializeComponent();
            FileDictionary fileDirectory = ApplicationState.Instance.ProjectFile.ModifiedImage.FileDictionary;
            dataGridView1.DataSource = fileDirectory.Files;
        }
    }
}
