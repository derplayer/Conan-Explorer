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
    /// PB file class
    /// </summary>
    public class PBFile : BaseFile
    {
        /// <summary>
        /// Files of the PB file.
        /// </summary>
        public List<PBFileEntry> Files { get; set; } = new List<PBFileEntry>();

        public PBFile() { }
        public PBFile(string filePath) : base(filePath)
        {
            Unpack();
        }

        /// <summary>
        /// Packs all the files back inside the PB file.
        /// </summary>
        public void Pack()
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.Create)))
            {
                writer.Write(HeaderList.PACK.Signature, 0, 8);
                foreach (PBFileEntry file in Files)
                {
                    FileInfo info = new FileInfo(file.File.FilePath);
                    file.Header.Length = (int)info.Length + 17;
                    writer.Write(file.Header.Data, 0, 17);
                    using (BinaryReader reader = new BinaryReader(new FileStream(file.File.FilePath, FileMode.Open)))
                    {
                        writer.Write(reader.ReadBytes(file.Header.Length - 17), 0, file.Header.Length - 17);
                    }
                }
                writer.Write(0xFFFFFFFF); //write end bytes
                int rest = (int)writer.BaseStream.Length % 2048;
                if (rest != 0)
                {
                    int padding = 2048 - rest;
                    for (int i = 0; i < padding; i++)
                    {
                        writer.Write('\0');
                    }
                }
            }
        }

        /// <summary>
        /// Unpacks all the files inside the PB file into a folder.
        /// </summary>
        public void Unpack()
        {
            Files.Clear();
            int index = 0;
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                reader.BaseStream.Seek(8, SeekOrigin.Begin);
                while (reader.BaseStream.Length != reader.BaseStream.Position)
                {
                    byte[] headerBytes = reader.ReadBytes(17);
                    PBFileHeader header = new PBFileHeader(headerBytes);
                    if (header.Length == 0x00F7FFFF) return;
                    byte[] buffer = reader.ReadBytes(header.Length - 17);
                    string directory = Path.GetDirectoryName(FilePath) + "\\" + Path.GetFileNameWithoutExtension(FilePath);
                    Directory.CreateDirectory(directory);
                    string fileName = directory + "\\" + index + "." + HeaderList.GetExtensionFromBuffer(buffer);
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                    {
                        writer.Write(buffer);
                    }
                    Files.Add(new PBFileEntry(header, HeaderList.GetTypeFromFile(fileName)));
                    index++;
                }
            }
            foreach(PBFileEntry file in Files)
            {
                if (file.File.GetType() == typeof(LZBFile))
                {
                    LZBFile lzb = (LZBFile)file.File;
                    lzb.Decompress();
                }
            }
        }
    }

    
}
