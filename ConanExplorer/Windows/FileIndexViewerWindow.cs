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
            FileDictionary fileDirectoryOriginal = ApplicationState.Instance.ProjectFile.OriginalImage.FileDictionary;
            dataGridView1.DataSource = fileDirectoryOriginal.Files;

            FileDictionary fileDirectoryModified = ApplicationState.Instance.ProjectFile.ModifiedImage.FileDictionary;
            dataGridView2.DataSource = fileDirectoryModified.Files;
        }

        private void button_ClearP3_Click(object sender, EventArgs e)
        {
            foreach (FileDictionaryFile File in ApplicationState.Instance.ProjectFile.ModifiedImage.FileDictionary.Files)
            {
                File.Param3 = 0;
            }
            
        }
    }
}
