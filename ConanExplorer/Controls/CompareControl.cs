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
using System.IO;
using ConanExplorer.Conan;
using System.Security.Cryptography;

namespace ConanExplorer.Controls
{
    public partial class CompareControl : UserControl
    {
        public CompareControl()
        {
            InitializeComponent();
        }

        public void Update(BaseFile baseFile)
        {
            if (baseFile == null)
            {
                textBox_Original_Size.Text = "";
                textBox_Original_Checksum.Text = "";
                textBox_Modified_Size.Text = "";
                textBox_Modified_Checksum.Text = "";
                return;
            }

            FileInfo fileInfoModified = new FileInfo(baseFile.FilePath);
            string relativePath = baseFile.RelativePath.Replace("modified", "original");
            FileInfo fileInfoOriginal = new FileInfo(ApplicationState.Instance.ProjectPath + relativePath);

            textBox_Original_Size.Text = String.Format("{0} Bytes", fileInfoOriginal.Length);
            textBox_Original_Checksum.Text = String.Format("0x{0}", BitConverter.ToString(GetChecksum(fileInfoOriginal)).Replace("-", string.Empty));
            textBox_Modified_Size.Text = String.Format("{0} Bytes", fileInfoModified.Length);
            textBox_Modified_Checksum.Text = String.Format("0x{0}", BitConverter.ToString(GetChecksum(fileInfoModified)).Replace("-", string.Empty));
        }

        private byte[] GetChecksum(FileInfo fileInfo)
        {
            return CRC.CRC32(File.ReadAllBytes(fileInfo.FullName));
        }

    }
}
