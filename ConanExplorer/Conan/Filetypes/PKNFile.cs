using ConanExplorer.Conan.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ConanExplorer.Conan.Filetypes
{
    /// <summary>
    /// PKN file class
    /// </summary>
    public class PKNFile : BaseFile
    {
        /// <summary>
        /// Name of the PKN file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the folder where the PKN got unpacked to.
        /// </summary>
        [XmlIgnore]
        public string UnpackedFolder
        {
            get { return Path.GetDirectoryName(FilePath) + "\\" + Name + "\\"; }
        }
        /// <summary>
        /// Size of the PKN file.
        /// </summary>
        public UInt32 Size { get; set; }
        /// <summary>
        /// File dictionary folder from the PSX executable.
        /// </summary>
        public FileDictionaryFolder IndexFolder { get; set; }
        /// <summary>
        /// Files of the PKN file.
        /// </summary>
        public List<BaseFile> Files { get; set; } = new List<BaseFile>();
        /// <summary>
        /// File count of the PKN
        /// </summary>
        public int ItemCount => Files.Count;

        public PKNFile() { }

        public PKNFile(string filePath, FileDictionaryFolder folder) : base(filePath)
        {
            Name = Path.GetFileNameWithoutExtension(filePath);
            FileName = Path.GetFileName(filePath);
            FilePath = filePath;
            IndexFolder = folder;
        }

        /// <summary>
        /// Checks all the files that were in the PKN for their checksum
        /// </summary>
        /// <returns></returns>
        public bool CheckPKNFiles()
        {
            return Files.All(file => file.Check());
        }

        /// <summary>
        /// Packs all the files back inside the PKN file.
        /// </summary>
        /// <returns></returns>
        public bool Pack()
        {
            foreach (FileDictionaryFile fileEntry in IndexFolder.Files)
            {
                string filePath = Path.Combine(UnpackedFolder, Path.GetFileName(fileEntry.FullPath));
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("A specific file is missing for packing:\n" + filePath);
                    return false;
                }
            }

            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.Create)))
            {
                foreach (FileDictionaryFile fileEntry in IndexFolder.Files)
                {
                    string filePath = Path.Combine(UnpackedFolder, Path.GetFileName(fileEntry.FullPath));
                    FileInfo fileInfo = new FileInfo(filePath);
                    using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
                    {
                        byte[] buffer = reader.ReadBytes((int)fileInfo.Length);
                        writer.Write(buffer, 0, (int)fileInfo.Length);

                        //TODO: @Phil - WAT, WTF? - Kills the LBA Disc Table
                        //long rest = 2048 - writer.BaseStream.Length % 2048;
                        //for (int i = 0; i < rest; i++)
                        //{
                        //    writer.Write('\0');
                        //}
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Unpacks all the files inside the PKN file into a folder.
        /// </summary>
        /// <returns></returns>
        public bool Unpack()
        {
            byte[] buffer;
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                Size = (uint)reader.BaseStream.Length;
                buffer = new byte[Size];
                reader.Read(buffer, 0, buffer.Length);
            }

            uint startOffset = IndexFolder.Files[0].Offset * 0x800;
            foreach (FileDictionaryFile fileEntry in IndexFolder.Files)
            {
                byte[] file = new byte[fileEntry.Length * 0x800];
                uint offset = fileEntry.Offset * 0x800 - startOffset;
                Array.Copy(buffer, offset, file, 0, fileEntry.Length * 0x800);

                string outputPath = Path.Combine(UnpackedFolder, Path.GetFileName(fileEntry.FullPath));
                Directory.CreateDirectory(UnpackedFolder);
                using (BinaryWriter writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create)))
                {
                    writer.Write(file);
                }
                Files.Add(GetFile(outputPath));
            }
            return true;
        }

        /// <summary>
        /// Deletes the PKN folder from unpacking
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            if (!Directory.Exists(UnpackedFolder)) return false;
            Directory.Delete(UnpackedFolder, true);
            Files.Clear();
            return true;
        }

        public BaseFile GetFile(string filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return null;
            string extension = Path.GetExtension(filePath).ToUpper();
            
            if (extension == ".BIN")
            {
                if (Path.GetFileName(filePath) == "FONT.BIN") return new FONTFile(filePath);
                return new BaseFile(filePath);
            }
            return HeaderList.GetTypeFromFile(filePath);
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
