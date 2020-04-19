using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan.Headers;
using ConanExplorer.Windows;

namespace ConanExplorer.Conan.Filetypes
{
    /// <summary>
    /// LZB file class
    /// </summary>
    public class LZBFile : BaseFile
    {
        /// <summary>
        /// Decompressed file that gets created when decompressing
        /// </summary>
        public BaseFile DecompressedFile { get; set; }
        /// <summary>
        /// LZSS header file that gets created when decompressing
        /// </summary>
        public BaseFile HeaderFile { get; set; }
        /// <summary>
        /// LZSS header
        /// </summary>
        public LZSSHeader LZSSHeader { get; set; } = new LZSSHeader();

        public LZBFile() { }
        public LZBFile(string filePath) : base(filePath)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                if (reader.BaseStream.Length == 0 || reader.BaseStream.Length < 16) return;
                byte[] header = new byte[16];
                reader.Read(header, 0, 16);
                LZSSHeader.Load(header);
            }
        }

        /// <summary>
        /// Decompresses the LZB file.
        /// </summary>
        /// <returns></returns>
        public bool Decompress()
        {
            byte[] buffer;
            byte mode;

            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                if (reader.BaseStream.Length == 0) return false;
                buffer = new byte[(int)reader.BaseStream.Length - 16];

                byte[] header = new byte[16];
                reader.Read(header, 0, 16);
                if (!LZSSHeader.Load(header))
                {
                    MessageBox.Show("This is not a valid LZSS-bu2 header!");
                    return false;
                }
                reader.Read(buffer, 0, buffer.Length);

                mode = LZSSHeader.Mode;
                switch (mode)
                {
                    default:
                        MessageBox.Show("Not a valid LZSS-bu2 mode number!\nOnly possible modes are 0, 1 and 2.", "Sorry!");
                        return false;
                    case 0:
                        decompress_Mode0(buffer);
                        break;
                    case 1:
                        decompress_Mode(buffer, 1, 256);
                        break;
                    case 2:
                        decompress_Mode(buffer, 2, 4096);
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Compresses the LZB file.
        /// </summary>
        /// <param name="clean">True for cleaning up the remains of the decompression.</param>
        /// <returns></returns>
        public bool Compress(bool clean = true)
        {
            if (DecompressedFile == null || HeaderFile == null) return false;
            if (!DecompressedFile.Exists() || !HeaderFile.Exists()) return false;

            byte[] buffer;

            using (BinaryReader reader = new BinaryReader(new FileStream(HeaderFile.FilePath, FileMode.Open)))
            {
                buffer = new byte[16];
                reader.Read(buffer, 0, 16);
                LZSSHeader.Load(buffer);
            }

            using (BinaryReader reader = new BinaryReader(new FileStream(DecompressedFile.FilePath, FileMode.Open)))
            {
                int bufferLength = (int)reader.BaseStream.Length;
                buffer = new byte[bufferLength];
                reader.Read(buffer, 0, bufferLength);
                LZSSHeader.UncompressedSize = (uint)bufferLength;
            }
            byte mode = LZSSHeader.Mode;

            if (clean)
            {
                DecompressedFile.Delete();
                DecompressedFile = null;
                HeaderFile.Delete();
                HeaderFile = null;
            }

            switch (mode)
            {
                default:
                    MessageBox.Show("Not a valid LZSS-bu2 mode number!\nOnly possible modes are 0, 1 and 2.", "Sorry!");
                    return false;
                case 0:
                    compress_Mode0(buffer);
                    break;
                case 1:
                    compress_Mode(buffer, 1, 256);
                    break;
                case 2:
                    compress_Mode(buffer, 2, 4096);
                    break;
            }
            return true;
        }


        private void decompress_Mode0(byte[] buffer)
        {
            string outputPath = FilePath + ".OUT";
            using (BinaryWriter writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create)))
            {
                writer.Write(buffer, 0, (int)LZSSHeader.UncompressedSize);
            }
            DecompressedFile = HeaderList.GetTypeFromFile(outputPath);

            string headerPath = Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileName(FilePath) + ".HEADER");
            using (BinaryWriter writer = new BinaryWriter(new FileStream(headerPath, FileMode.Create)))
            {
                writer.Write(LZSSHeader.GetBytes());
            }
            HeaderFile = new BaseFile(headerPath);
        }

        private void decompress_Mode(byte[] buffer, byte mode, ushort windowSize)
        {
            CircularBuffer circularBuffer = new CircularBuffer(windowSize);
            string outputPath = FilePath + ".OUT";
            using (BinaryWriter writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create)))
            {
                writer.Write(buffer, 0, 1);
                circularBuffer.AddData(new byte[] { buffer[0] });

                for (int i = 1; i < buffer.Length; i++)
                {
                    bool[] flags = GetFlags(buffer[i]);

                    for (int j = 7; j >= 0; j--)
                    {
                        if (flags[j])
                        {
                            byte[] wordBytes = new byte[2];
                            Array.Copy(buffer, ++i, wordBytes, 0, 2);
                            CompressedWord word = new CompressedWord(wordBytes, mode);
                            byte[] result = circularBuffer.GetData(word.Offset, word.Length);
                            i++;

                            writer.Write(result, 0, result.Length);
                        }
                        else
                        {
                            if (i < buffer.Length - 1)
                            {
                                writer.Write(buffer, ++i, 1);
                                circularBuffer.AddData(new byte[] { buffer[i] });
                            }
                        }
                        if (writer.BaseStream.Length >= LZSSHeader.UncompressedSize) { break; }
                    }
                    if (writer.BaseStream.Length >= LZSSHeader.UncompressedSize) { break; }
                }
            }
            DecompressedFile = HeaderList.GetTypeFromFile(outputPath);

            string headerPath = Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileName(FilePath) + ".HEADER");
            using (BinaryWriter writer = new BinaryWriter(new FileStream(headerPath, FileMode.Create)))
            {
                writer.Write(LZSSHeader.GetBytes());
            }
            HeaderFile = new BaseFile(headerPath);
        }


        private void compress_Mode0(byte[] buffer)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.Create)))
            {
                writer.Write(LZSSHeader.GetBytes(), 0, 16);
                writer.Write(buffer, 0, buffer.Length);
            }
        }

        private void compress_Mode(byte[] buffer, byte mode, ushort windowSize)
        {
            int restBytes = (int)(16 - Math.Log(windowSize, 2));
            int nextBytesSize = (int)Math.Pow(2, restBytes) + 1;

            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.Create)))
            {
                writer.Write(LZSSHeader.GetBytes(), 0, 16);
                writer.Write(buffer[0]);

                byte[] chunk = new byte[17];
                byte chunkLength = 1;
                sbyte flagpos = 7;

                for (int i = 1; i < buffer.Length; i++)
                {
                    if (flagpos == -1)
                    {
                        writer.Write(chunk, 0, chunkLength);
                        flagpos = 7;
                        chunkLength = 1;
                        chunk[0] = 0;
                    }

                    byte[] window;
                    if (i < windowSize)
                    {
                        window = new byte[i];
                    }
                    else
                    {
                        window = new byte[windowSize];
                    }

                    if (i > windowSize)
                    {
                        Array.Copy(buffer, i - windowSize, window, 0, window.Length);
                    }
                    else
                    {
                        Array.Copy(buffer, 0, window, 0, window.Length);
                    }

                    byte[] nextBytes = new byte[nextBytesSize];
                    if (i + nextBytesSize > buffer.Length)
                    {
                        Array.Copy(buffer, i, nextBytes, 0, buffer.Length - i);
                    }
                    else
                    {
                        Array.Copy(buffer, i, nextBytes, 0, nextBytesSize);
                    }

                    BestResult result = FindBest(window, nextBytes);
                    if (result.IsCompressed)
                    {
                        if (result.Length + i > buffer.Length)
                        {
                            result.Length = (byte)(buffer.Length - i);
                        }

                        CompressedWord word = new CompressedWord(result.Offset, result.Length);
                        byte[] wordBytes = word.GetBytes(mode);

                        Array.Copy(wordBytes, 0, chunk, chunkLength, 2);
                        chunkLength += 2;
                        chunk[0] |= (byte)(1 << flagpos);
                        i += result.Length - 1;
                    }
                    else
                    {
                        byte[] compressed = new byte[1];
                        compressed[0] = result.Character;
                        Array.Copy(compressed, 0, chunk, chunkLength, 1);
                        chunkLength += 1;
                    }
                    flagpos--;
                }
                if (flagpos != -1)
                {
                    writer.Write(chunk, 0, chunkLength);
                }
            }
        }



        private BestResult FindBest(byte[] window, byte[] nextBytes)
        {
            BestResult result = new BestResult();
            result.Length = 1;
            result.Character = nextBytes[0];

            for (int i = window.Length - 1; i >= 0; i--)
            {
                //if window matches first next byte
                if (window[i] == nextBytes[0])
                {
                    ushort length = 1;
                    List<byte> expandingWindow = new List<byte>(window);
                    expandingWindow.Add(nextBytes[0]);

                    for (int j = 1; j < nextBytes.Length; j++)
                    {
                        if (expandingWindow[i + j] != nextBytes[j]) break;
                        expandingWindow.Add(nextBytes[j]);
                        length++;
                    }
                    if (length > result.Length)
                    {
                        result.Length = length;
                        result.Offset = (ushort)i;
                    }
                }
            }
            result.IsCompressed = result.Length != 1;
            return result;
        }

        private bool[] GetFlags(byte data)
        {
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                result[i] = (data & (1 << i)) == 1 << i;
            }
            return result;
        }
    }

    /// <summary>
    /// This is a LZSS compressed word consisting of the offset inside the window and the length
    /// </summary>
    class CompressedWord
    {
        public ushort Offset;
        public ushort Length;

        public CompressedWord(ushort offset, ushort length)
        {
            Offset = offset;
            Length = length;
        }

        public CompressedWord(byte[] data, byte mode)
        {
            byte lower = data[0];
            byte higher = data[1];

            if (mode == 2)
            {
                Offset = (ushort)(lower << 4);
                Offset |= (ushort)((higher & 0xF0) >> 4);
                Length = (ushort)((higher & 0x0F) + 2);
            }
            if (mode == 1)
            {
                Offset = lower;
                Length = (ushort)(higher + 2);
            }
        }

        public byte[] GetBytes(byte mode)
        {
            byte[] result = new byte[2];

            if (mode == 2)
            {
                result[0] = (byte)(Offset >> 4);
                result[1] = (byte)(Offset << 4);
                result[1] |= (byte)(Length - 2);
            }
            if (mode == 1)
            {
                result[0] = (byte)Offset;
                result[1] = (byte)(Length - 2);
            }
            return result;
        }
    }

    /// <summary>
    /// Just a little struct to compare other results easier with eachother
    /// </summary>
    struct BestResult
    {
        public bool IsCompressed;
        public byte Character;
        public ushort Length;
        public ushort Offset;
    }
}
