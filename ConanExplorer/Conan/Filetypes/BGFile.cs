using ConanExplorer.Conan.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Filetypes
{
    /// <summary>
    /// BG file class
    /// </summary>
    public class BGFile : BaseFile
    {
        /// <summary>
        /// Files of the BG file.
        /// </summary>
        public List<BaseFile> Files { get; set; } = new List<BaseFile>();

        /// <summary>
        /// Header of the BG file.
        /// </summary>
        public BGHeader Header { get; set; }


        /// <summary>
        /// Constructor for serialization.
        /// </summary>
        public BGFile() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public BGFile(string filePath) : base(filePath)
        {
            Header = new BGHeader();
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                Header.Load(reader.ReadBytes(64));
            }
            Unpack();
        }

        /// <summary>
        /// Packs all the files back inside the BG file.
        /// </summary>
        public void Pack()
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.Create)))
            {
                List<int> indices = new List<int>();
                int lastOffset = 0;
                foreach (BaseFile file in Files)
                {
                    FileInfo info = new FileInfo(file.FilePath);
                    int length = (int)info.Length;
                    indices.Add(length + lastOffset);
                    lastOffset += length;
                }
                Header.RealIndices = indices;
                writer.Write(Header.GetBytes());

                foreach (BaseFile file in Files)
                {
                    using (BinaryReader reader = new BinaryReader(new FileStream(file.FilePath, FileMode.Open)))
                    {
                        writer.Write(reader.ReadBytes((int)reader.BaseStream.Length));
                    }
                }
            }
        }

        /// <summary>
        /// Unpacks all the files inside the BG file into a folder.
        /// </summary>
        public void Unpack()
        {
            Files.Clear();
            List<int> indices = Header.RealIndices;
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                int fileCount = Header.FileCount;
                int lastOffset = 64;
                reader.BaseStream.Seek(64, SeekOrigin.Begin);
                for (int i = 0; i < fileCount; i++)
                {
                    int endOffset = indices[i] + 64;
                    int length = endOffset - lastOffset;
                    byte[] buffer = reader.ReadBytes(length);
                    string directory = Path.GetDirectoryName(FilePath) + "\\" + Path.GetFileNameWithoutExtension(FilePath);
                    Directory.CreateDirectory(directory);
                    string fileName = directory + "\\" + i + "." + HeaderList.GetExtensionFromBuffer(buffer);
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                    {
                        writer.Write(buffer);
                    }
                    Files.Add(HeaderList.GetTypeFromFile(fileName));
                    lastOffset = indices[i] + 64;
                }
            }
        }
    }
}
