﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ConanExplorer.Conan.Filetypes;
using DiscUtils.Iso9660;
using DiscUtils;

namespace ConanExplorer.Conan
{
    /// <summary>
    /// This class holds information about an PSX image that it also can manipulate
    /// </summary>
    public class PSXImage
    {
        private const string PSXBuildExecutable = "Tools\\psximager\\psxbuild.exe";
        private const string PSXRipExecutable = "Tools\\psximager\\psxrip.exe";
        private const int DictionaryOffset = 0x77F10;
        private const int DictionaryLength = 0x7F68;

        /// <summary>
        /// Ignoring memory optimization will have a high risk of causing files inside the PKN to be corrupt when the size changes.
        /// </summary>
        public bool IgnoreMemoryOptimization = false;
        /// <summary>
        /// Disabling memory optimization will have a high risk in insufficient memory causing memory to be overwritten and corrupted.
        /// </summary>
        public bool DisableMemoryOptimization = false;

        public string ImageName { get; set; }

        public string RelativeImageCuePath { get; set; }
        public string RelativeImageBinPath { get; set; }
        public string RelativeRippedDirectory { get; set; }
        public string RelativeRippedCatPath { get; set; }
        public string RelativeRippedExecutablePath { get; set; }

        [XmlIgnore]
        public string ImageCuePath
        {
            get { return ApplicationState.Instance.ProjectPath + RelativeImageCuePath; }
            set { RelativeImageCuePath = value.Replace(ApplicationState.Instance.ProjectPath, ""); }
        }
        [XmlIgnore]
        public string ImageBinPath
        {
            get { return ApplicationState.Instance.ProjectPath + RelativeImageBinPath; }
            set { RelativeImageBinPath = value.Replace(ApplicationState.Instance.ProjectPath, ""); }
        }
        [XmlIgnore]
        public string RippedDirectory
        {
            get { return ApplicationState.Instance.ProjectPath + RelativeRippedDirectory; }
            set { RelativeRippedDirectory = value.Replace(ApplicationState.Instance.ProjectPath, ""); }
        }
        [XmlIgnore]
        public string RippedCatPath
        {
            get { return ApplicationState.Instance.ProjectPath + RelativeRippedCatPath; }
            set { RelativeRippedCatPath = value.Replace(ApplicationState.Instance.ProjectPath, ""); }
        }
        [XmlIgnore]
        public string RippedExecutablePath
        {
            get { return ApplicationState.Instance.ProjectPath + RelativeRippedExecutablePath; }
            set { RelativeRippedExecutablePath = value.Replace(ApplicationState.Instance.ProjectPath, ""); }
        }

        public FileDictionary FileDictionary { get; set; }
        public List<PKNFile> PKNFiles { get; set; } = new List<PKNFile>();
        [XmlIgnore]
        public FONTFile FONTFile
        {
            get
            {
                PKNFile pknFile = PKNFiles.FirstOrDefault(p => p.Name == "GRAPH");
                return (FONTFile)pknFile.Files.FirstOrDefault(f => f.FileName == "FONT.BIN");
            }
        }

        /// <summary>
        /// Default constructor for serialization use only
        /// </summary>
        public PSXImage() {}

        /// <summary>
        /// Constructor for creating a new PSXImage
        /// </summary>
        /// <param name="imageBinPath">Path of the image ".bin" file</param>
        /// <param name="imageCuePath">Path of the image ".cue" file</param>
        public PSXImage(string imageBinPath, string imageCuePath)
        {
            ImageBinPath = imageBinPath;
            ImageCuePath = imageCuePath;
            ImageName = Path.GetFileNameWithoutExtension(imageBinPath);
        }


        public void InitPKNs()
        {
            PKNFiles.Clear();
            foreach (ConanImageFile pkn in ConanImage.PKNFiles)
            {
                foreach (FileDictionaryFolder folder in FileDictionary.Folders)
                {
                    if (folder.Name != Path.GetFileNameWithoutExtension(pkn.FilePath)) continue;
                    PKNFiles.Add(new PKNFile(Path.Combine(RippedDirectory, pkn.FilePath), folder));
                    break;
                }
            }
        }

        public bool CheckPKNs()
        {
            foreach (ConanImageFile pkn in ConanImage.PKNFiles)
            {
                string path = Path.Combine(RippedDirectory, pkn.FilePath);
                PKNFile pknFile = PKNFiles.FirstOrDefault(p => p.FilePath == path);
                if (pknFile == null)
                {
                    return false;
                }
            }
            return true;
        }

        ///  <summary>
        ///  Reads the file indexes of the PSX executable
        ///  </summary>
        ///  <param name="executablePath">Path of the PSX executable</param>
        ///  <param name="offset">Offset in the PSX executable where the file listing starts in bytes</param>
        ///  <param name="length">Length of the file listing in bytes</param>
        public void LoadExecutable(string executablePath)
        {
            RippedExecutablePath = executablePath;
            FileDictionary.Offset = DictionaryOffset;
            FileDictionary.Length = DictionaryLength;

            byte[] listing = new byte[DictionaryLength];
            using (BinaryReader reader = new BinaryReader(new FileStream(RippedExecutablePath, FileMode.Open)))
            {
                reader.BaseStream.Seek(DictionaryOffset, SeekOrigin.Begin);
                reader.Read(listing, 0, DictionaryLength);
            }

            for (int i = 0; i < DictionaryLength; i += 0x24)
            {
                byte[] entryData = new byte[0x24];
                Array.Copy(listing, i, entryData, 0, 0x24);
                FileDictionary.Files.Add(new FileDictionaryFile(entryData));
            }
            FileDictionary.SortFiles();
        }

        /// <summary>
        /// Saves the PSX executable with modified indexes to match the current game files
        /// </summary>
        public void SaveExecutable()
        {
            if (FileDictionary == null) return;

            uint counter = 24;
            string lastFolder = "BG";
            foreach (FileDictionaryFile file in FileDictionary.Files)
            {
                if (lastFolder != file.Folder) counter++;
                lastFolder = file.Folder;

                if (file.IsInsidePkn)
                {
                    string fileName = RippedDirectory + "\\" + file.Folder + "\\" + file.FullPath;
                    FileInfo fileInfo = new FileInfo(fileName);

                    file.Offset = counter;
                    file.Length = (uint)fileInfo.Length / 2048;
                    uint modLength = (uint)fileInfo.Length % 2048;
                    if (modLength != 0)
                    {
                        file.Length++;
                    }
                    if (!IgnoreMemoryOptimization)
                    {
                        file.SectorOverhead = DisableMemoryOptimization ? 0 : modLength / 4;
                    }
                    
                    counter += file.Length;
                }
                else
                {
                    string fileName = RippedDirectory + file.FullPath;
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (fileInfo.Length % 2336 != 0)
                    {
                        if (MessageBox.Show(String.Format("The file \"{0}\" does not have the correct size.\n Do you want to add zero padding?", fileInfo.FullName), "Incorrect Size!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            long rest = 2336 - fileInfo.Length % 2336;
                            using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write)))
                            {
                                for (int i = 0; i < rest; i++)
                                {
                                    writer.Write('\0');
                                }
                            }
                            fileInfo = new FileInfo(fileName);
                        }
                        else
                        {
                            MessageBox.Show("The build process was cancelled!", "Build process cancelled!");
                            return;
                        }
                        //MessageBox.Show(String.Format("The file \"{0}\" does not have the correct size.", fileInfo.FullName));
                        //return;
                    }

                    file.Offset = counter;
                    file.Length = (uint)fileInfo.Length / 2336;
                    counter += file.Length;
                }
            }

            FileDictionary.SortFiles();

            using (BinaryWriter writer = new BinaryWriter(new FileStream(RippedExecutablePath, FileMode.Open)))
            {
                writer.BaseStream.Seek(FileDictionary.Offset, SeekOrigin.Begin);
                foreach (FileDictionaryFile file in FileDictionary.Files)
                {
                    writer.Write(file.GetBytes());
                }

                int CodeCaveSegment = 585512;

                //Apply Assembler patches (write as single hex because of endianess)
                //Patch 01 - Allow sjis 2+1 more char in GMAP "school" string
                writer.BaseStream.Seek(0x03D5CB, SeekOrigin.Begin);
                writer.Write((byte)0x24);
                writer.Write((byte)0x03);
                writer.BaseStream.Seek(0x03D5D0, SeekOrigin.Begin);
                writer.Write((byte)0xA8);
                writer.Write((byte)0x03);

                //Patch 02 - Mod save string to way longer (Speichern)
                //Move pointer to a code cave ("old seq data" printf strings 1/2)
                writer.BaseStream.Seek(0x0375F8, SeekOrigin.Begin);
                writer.Write(new byte[] { 0x01, 0x80, 0x04, 0x3C, 0xC4, 0x69, 0x84, 0x24 });
                //nop out the lib printf part
                writer.BaseStream.Seek(0x6BCB4, SeekOrigin.Begin);
                writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

                //Patch 03 - Move pointer (gmap wohnheim) to a code cave ("old seq data" printf string 2/2)
                writer.BaseStream.Seek(0x03D630, SeekOrigin.Begin);
                writer.Write(new byte[] { 0x01, 0x80, 0x04, 0x3C, 0xD0, 0x69, 0x84, 0x24 });

                //Patch 03.1
                writer.BaseStream.Seek(685204, SeekOrigin.Begin);
                writer.Write(new byte[] {
                    0x44, 0x45, 0x52, 0x50, 0x4c, 0x41, 0x59, 0x45,
                    0x52, 0x20, 0x57, 0x41, 0x53, 0x20, 0x48, 0x45,
                    0x52, 0x45
                });

                //Patch 04 - Resize the GMAP Draw area for text (0x70 is the hardware limit for width)
                //We increase the draw rectangle with for one more char
                writer.BaseStream.Seek(0xF944, SeekOrigin.Begin);
                writer.Write((byte)0x30); //0x40->0x30
                //We overwrite (8001F288 move $a3, $zero) to change the width register back for the other rectangles
                //Should not crash or cause the game to be unstable, but never say never (should be tested on real hardware later)
                //alternative for this is the usage of the t2 register (but value is over 0x70, so not tested)
                writer.BaseStream.Seek(0xF988, SeekOrigin.Begin);
                writer.Write(new byte[] { 0x40, 0x00, 0x14, 0x24 }); //0x30->0x40

                //Patch 05 - Increase the chars in buffer for GMAP draw area (CPU)
                writer.BaseStream.Seek(0x03D638, SeekOrigin.Begin);
                writer.Write((byte)0x05); //0x03->0x05

                byte libsegptr = 0x28;
                //dehnen    - 584152 - orginal hex: A4 6C 0B 80
                //wasser    - 584156 - orginal hex: C4 3F 01 80
                //stein     - 584188 - orginal hex: 3C 3F 01 80
                //aufricht  - 584208 - orginal hex: 9C 6C 0B 80
                //binden    - 584212 - orginal hex: 94 6C 0B 80

                //Patch 06.1 - Move minigame label pointer (dehnen) to a code cave (sony lib header text)
                writer.BaseStream.Seek(584152, SeekOrigin.Begin);
                writer.Write(new byte[] { libsegptr, 0xE7, 0x09, 0x80 });
                libsegptr += 12;

                //Patch 06.2 - Move minigame label pointer (wasser) to a code cave (sony lib header text)
                writer.BaseStream.Seek(584156, SeekOrigin.Begin);
                writer.Write(new byte[] { libsegptr, 0xE7, 0x09, 0x80 });
                libsegptr += 22;

                //Patch 06.3 - Move minigame label pointer (stein) to a code cave (sony lib header text)
                writer.BaseStream.Seek(584188, SeekOrigin.Begin);
                writer.Write(new byte[] { libsegptr, 0xE7, 0x09, 0x80 });
                libsegptr += 22;

                //Patch 06.4 - Move minigame label pointer (aufricht) to a code cave (sony lib header text)
                writer.BaseStream.Seek(584208, SeekOrigin.Begin);
                writer.Write(new byte[] { libsegptr, 0xE7, 0x09, 0x80 });
                libsegptr += 12;

                //Patch 06.5 - Move minigame label pointer (aufricht) to a code cave (sony lib header text)
                writer.BaseStream.Seek(584212, SeekOrigin.Begin);
                writer.Write(new byte[] { libsegptr, 0xE7, 0x09, 0x80 });
                libsegptr += 16;

                //----------

                //Patch 0x - dialog color for minigame 3 (totally useless ;)
                //writer.BaseStream.Seek(0x057AFC, SeekOrigin.Begin);
                //writer.Write((byte)0xA); //Blue 0x02, Red 0x04, Yellow 0x06
            }
        }

        /// <summary>
        /// Checks the ripped image for having all the needed files
        /// </summary>
        /// <param name="checkSize">True for also checking if the files are the correct size</param>
        /// <returns></returns>
        public bool CheckRip(bool checkSize = false)
        {
            if (String.IsNullOrEmpty(RippedCatPath)) return false;
            if (ConanImage.Check(RippedDirectory, checkSize)) return true;
            return false;
        }

        /// <summary>
        /// TODO MIT license friendly
        /// </summary>
        /// <param name="destinationFolder"></param>
        public void RipNew(string destinationFolder)
        {
            RippedDirectory = destinationFolder;
            RippedCatPath = Path.GetDirectoryName(ImageBinPath) + "\\" + Path.GetFileNameWithoutExtension(ImageBinPath) + ".cat";
            if (Path.GetExtension(ImageBinPath).ToLower() != ".bin") return;
            if (!File.Exists(ImageBinPath)) return;

            using (FileStream fileStream = File.Open(ImageCuePath, FileMode.Open))
            {
                CDReader cdReader = new CDReader(fileStream, true);
                string[] files = GetFiles(cdReader.Root);
                
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
        }

        private string[] GetFiles(DiscDirectoryInfo directory)
        {
            List<string> result = new List<string>();

            foreach (DiscFileInfo file in directory.GetFiles())
            {
                result.Add(file.FullName);
            }

            foreach (DiscDirectoryInfo dir in directory.GetDirectories())
            {
                result.AddRange(GetFiles(dir));
            }

            return result.ToArray();
        }

        /// <summary>
        /// This method will rip the image to the given directory
        /// </summary>
        /// <param name="destinationFolder"></param>
        public void Rip(string destinationFolder)
        {
            //RipNew(destinationFolder);
            RippedDirectory = destinationFolder;
            RippedCatPath = Path.GetDirectoryName(ImageBinPath) + "\\" + Path.GetFileNameWithoutExtension(ImageBinPath) + ".cat";
            if (Path.GetExtension(ImageBinPath).ToLower() != ".bin") return;
            if (!File.Exists(ImageBinPath)) return;

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = PSXRipExecutable,
                Arguments = String.Format("\"{0}\" \"{1}\"", ImageBinPath, RippedDirectory)
            };
            Process psxrip = new Process { StartInfo = info };
            psxrip.Start();
            psxrip.WaitForExit();

            FileDictionary = new FileDictionary();
            LoadExecutable(RippedDirectory + "\\SLPS_016.90");
            InitPKNs();

            foreach (PKNFile pknFile in PKNFiles)
            {
                pknFile.Unpack();
            }
        }

        public void InitFiles()
        {
            FileDictionaryFolder voice1 = FileDictionary.Folders.FirstOrDefault(d => d.Name == "VOICE1");
            FileDictionaryFolder voice2 = FileDictionary.Folders.FirstOrDefault(d => d.Name == "VOICE2");
            FileDictionaryFolder voice3 = FileDictionary.Folders.FirstOrDefault(d => d.Name == "VOICE3");
            FileDictionaryFolder xstr = FileDictionary.Folders.FirstOrDefault(d => d.Name == "XSTR");

            List<FileDictionaryFile> files = new List<FileDictionaryFile>();
            files.AddRange(voice1.Files);
            files.AddRange(voice2.Files);
            files.AddRange(voice3.Files);
            files.AddRange(xstr.Files);
        }

        /// <summary>
        /// This method will build an image from the directory of the ripped image files
        /// </summary>
        public void Build()
        {
            if (String.IsNullOrEmpty(RippedCatPath)) { return; }

            SaveExecutable();
            CheckPKNs();

            foreach (PKNFile pknFile in PKNFiles)
            {
                pknFile.Pack();
            }

            Process psxbuild = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = PSXBuildExecutable;
            info.Arguments = String.Format("\"{0}\" \"{1}\"", RippedCatPath, ImageBinPath);
            psxbuild.StartInfo = info;
            psxbuild.Start();
            psxbuild.WaitForExit();
        }

        /// <summary>
        /// This method will clean the directory of the ripped image
        /// </summary>
        /// <param name="resetVariables">True for clearing out the ripped variables</param>
        public void Clear(bool resetVariables = true)
        {
            Directory.Delete(RippedDirectory, true);
            if (resetVariables)
            {
                RippedDirectory = "";
                RippedCatPath = "";
            }
        }

        public override string ToString()
        {
            return ImageName;
        }

    }
}
