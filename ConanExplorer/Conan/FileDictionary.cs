using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan
{
    /// <summary>
    /// File dictionary that hold the file indices of the PSX executable
    /// </summary>
    public class FileDictionary
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public List<FileDictionaryFile> Files { get; set; } = new List<FileDictionaryFile>();
        public List<FileDictionaryFolder> Folders { get; set; } = new List<FileDictionaryFolder>();

        public void SortFiles()
        {
            Folders.Clear();
            foreach (FileDictionaryFile entry in Files)
            {
                FileDictionaryFolder folder = Folders.FirstOrDefault(f => f.Name == entry.Folder);
                if (folder == null)
                {
                    folder = new FileDictionaryFolder(entry.Folder);
                    Folders.Add(folder);
                }
                folder.Files.Add(entry);
            }
        }
    }

    /// <summary>
    /// File dictionary file entry
    /// </summary>
    public class FileDictionaryFile
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Folder { get; set; }

        /// <summary>
        /// Offset of the file in 2048 sector count
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Length of the file in 2048 sector count
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// Sector overhead in bytes divided by 4 (without zero padding)
        /// </summary>
        public uint SectorOverhead { get; set; }

        public bool IsInsidePkn { get; set; }


        public FileDictionaryFile() { }

        public FileDictionaryFile(byte[] data)
        {
            FullPath = Encoding.ASCII.GetString(data, 0, 0x18).Replace("\0", "");
            Name = Path.GetFileName(FullPath);
            Extension = Path.GetExtension(FullPath);
            Folder = Path.GetDirectoryName(FullPath)?.Remove(0, 1);
            Offset = BitConverter.ToUInt32(data, 0x18);
            Length = BitConverter.ToUInt32(data, 0x1C);
            SectorOverhead = BitConverter.ToUInt32(data, 0x20);
            IsInsidePkn = SectorOverhead != 0;
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[36];

            byte[] name = Encoding.ASCII.GetBytes(FullPath);
            byte[] offset = BitConverter.GetBytes(Offset);
            byte[] length = BitConverter.GetBytes(Length);
            byte[] param3 = BitConverter.GetBytes(SectorOverhead);

            Array.Copy(name, 0, result, 0, name.Length);
            Array.Copy(offset, 0, result, 24, 4);
            Array.Copy(length, 0, result, 28, 4);
            Array.Copy(param3, 0, result, 32, 4);

            return result;
        }
    }

    public class FileDictionaryFolder
    {
        public string Name { get; set; }
        public List<FileDictionaryFile> Files { get; set; } = new List<FileDictionaryFile>();

        public FileDictionaryFolder() { }

        public FileDictionaryFolder(string name)
        {
            Name = name;
        }
    }
}
