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
        public uint Offset { get; set; }
        public uint Length { get; set; }
        public uint Param3
        {
            get; set;
            //get
            //{
            //    if (IsInsidePkn)
            //        return 450;
            //    return 0;
            //}
            //set { }
        }
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
            Param3 = BitConverter.ToUInt32(data, 0x20);
            IsInsidePkn = Param3 != 0;
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[36];

            byte[] name = Encoding.ASCII.GetBytes(FullPath);
            byte[] offset = BitConverter.GetBytes(Offset);
            byte[] length = BitConverter.GetBytes(Length);
            byte[] param3 = BitConverter.GetBytes(Param3);

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
