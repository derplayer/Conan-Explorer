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
            if (ApplicationState.Instance.ProjectFile == null)
            {
                MessageBox.Show("There is no project maaaan.");
                Close();
                return;
            }
            FileDictionary fileDirectory = ApplicationState.Instance.ProjectFile.ModifiedImage.FileDictionary;
            if (fileDirectory == null)
            {
                MessageBox.Show("There are no file indices maaaan.");
                Close();
                return;
            }
            dataGridView1.DataSource = fileDirectory.Files;
        }
    }
}
