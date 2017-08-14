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
    /// <summary>
    /// Basic file class that all filetypes inherit from.
    /// </summary>
    [XmlInclude(typeof(PKNFile)), XmlInclude(typeof(LZBFile)), XmlInclude(typeof(PBFile)), XmlInclude(typeof(TIMFile)), XmlInclude(typeof(BGFile)), XmlInclude(typeof(FONTFile)), XmlInclude(typeof(SEQFile)), XmlInclude(typeof(XAFile)), XmlInclude(typeof(VHFile)), XmlInclude(typeof(VBFile)), XmlInclude(typeof(STRFile))]
    public class BaseFile
    {
        /// <summary>
        /// Name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Path of the file relative to the project path.
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Path of the file.
        /// </summary>
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

        /// <summary>
        /// Checksum of the file (MD5).
        /// </summary>
        public byte[] Checksum { get; set; }

        /// <summary>
        /// Constructor for serialization.
        /// </summary>
        public BaseFile() { }

        /// <summary>
        /// Base Constructor.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
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

        /// <summary>
        /// Checks if the file exists.
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return File.Exists(FilePath);
        }

        /// <summary>
        /// Checks if the file has the same checksum.
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            using (MD5 md5 = MD5.Create())
            using (FileStream stream = new FileStream(FilePath, FileMode.Open))
            {
                return md5.ComputeHash(stream) == Checksum;
            }
        }       

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <returns></returns>
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
