using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ConanExplorer.Conan.Headers;

namespace ConanExplorer.Conan.Filetypes
{
    [XmlInclude(typeof(PKNFile)), XmlInclude(typeof(LZBFile)), XmlInclude(typeof(PBFile)), XmlInclude(typeof(TIMFile)), XmlInclude(typeof(BGFile)), XmlInclude(typeof(FONTFile)), XmlInclude(typeof(SEQFile)), XmlInclude(typeof(XAFile)), XmlInclude(typeof(VHFile)), XmlInclude(typeof(VBFile)), XmlInclude(typeof(STRFile))]
    public class BaseFile
    {
        public string FileName { get; set; }

        public string RelativePath { get; set; }

        [XmlIgnore]
        public string FilePath
        {
            get
            {
                if (RelativePath[1] == ':') return RelativePath;
                return ApplicationState.Instance.ProjectPath + RelativePath;
            }
            set
            {
                if (!String.IsNullOrEmpty(ApplicationState.Instance.ProjectPath))
                {
                    RelativePath = value.Replace(ApplicationState.Instance.ProjectPath, "");
                    return;
                }
                RelativePath = value;
            }
        }

        public byte[] Checksum { get; set; }

        public BaseFile() { }

        public BaseFile(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(FilePath);
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    Checksum = md5.ComputeHash(stream);
                }
            }
        }

        public bool Exists()
        {
            return File.Exists(FilePath);
        }

        public bool Check()
        {
            using (MD5 md5 = MD5.Create())
            using (FileStream stream = new FileStream(FilePath, FileMode.Open))
            {
                return md5.ComputeHash(stream) == Checksum;
            }
        }       

        public bool Delete()
        {
            File.Delete(FilePath);
            return true;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
