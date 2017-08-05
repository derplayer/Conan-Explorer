using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ConanExplorer.Conan.Filetypes;

namespace ConanExplorer.Conan
{
    /// <summary>
    /// This class holds all the project information
    /// </summary>
    public class ProjectFile
    {
        [XmlIgnore]
        public string ProjectFilePath;
        public PSXImage OriginalImage { get; set; }
        public PSXImage ModifiedImage { get; set; }

        /// <summary>
        /// Constructor for creating new Project
        /// </summary>
        /// <param name="filePath">Path of where the image bin or cue is located</param>
        public bool CreateProject(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("The given file does not exist how did you do that?");
                return false;
            }

            string projectDirectory = Path.GetDirectoryName(filePath);
            string originalDirectory = projectDirectory + "\\original\\";
            string modifiedDirectory = projectDirectory + "\\modified\\";
            string inputFileName = Path.GetFileNameWithoutExtension(filePath);
            string inputExtension = Path.GetExtension(filePath).ToLower();
            string imageBinFilePath = "";
            string imageCueFilePath = "";

            if (inputExtension == ".cue")
            {
                imageCueFilePath = filePath;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string firstLine = reader.ReadLine();
                    string[] splitted = firstLine.Split('"');
                    if (splitted.Length != 3)
                    {
                        MessageBox.Show("Looks not like the .cue i was looking for.");
                        return false;
                    }
                    imageBinFilePath = projectDirectory + "\\" + splitted[1];
                }

                if (!File.Exists(imageBinFilePath))
                {
                    MessageBox.Show(String.Format("Couldn't find the .bin file for the given .cue file: {0}", filePath));
                    return false;
                }
            }
            else if (inputExtension == ".bin")
            {
                imageBinFilePath = filePath;
                imageCueFilePath = String.Format("{0}\\{1}.cue", projectDirectory, inputFileName);
                using (StreamWriter writer = new StreamWriter(imageCueFilePath))
                {
                    writer.WriteLine("FILE \"{0}.bin\" BINARY", inputFileName);
                    writer.WriteLine("  TRACK 01 MODE2/2352");
                    writer.WriteLine("    INDEX 01 00:00:00");
                }
            }
            else
            {
                MessageBox.Show("The given file is not a bin/cue how could this happen?");
                return false;
            }

            Directory.CreateDirectory(originalDirectory);
            File.Move(imageBinFilePath, originalDirectory + Path.GetFileName(imageBinFilePath));
            File.Move(imageCueFilePath, originalDirectory + Path.GetFileName(imageCueFilePath));

            Directory.CreateDirectory(modifiedDirectory);
            File.Copy(originalDirectory + Path.GetFileName(imageBinFilePath), modifiedDirectory + Path.GetFileName(imageBinFilePath));
            File.Copy(originalDirectory + Path.GetFileName(imageCueFilePath), modifiedDirectory + Path.GetFileName(imageCueFilePath));

            ProjectFilePath = String.Format("{0}\\{1}.cep", projectDirectory, inputFileName);
            OriginalImage = new PSXImage(originalDirectory + Path.GetFileName(imageBinFilePath), originalDirectory + Path.GetFileName(imageCueFilePath));
            ModifiedImage = new PSXImage(modifiedDirectory + Path.GetFileName(imageBinFilePath), modifiedDirectory + Path.GetFileName(imageCueFilePath));

            OriginalImage.Rip(originalDirectory + inputFileName);
            if (!OriginalImage.CheckRip())
            {
                MessageBox.Show("The given image does not resemble my definition of an \"Meitantei Conan(SLPS_01690)\" image.");
                DeleteProject();
                return false;
            }

            ModifiedImage.Rip(modifiedDirectory + inputFileName);

            SaveProject();
            return true;
        }

        /// <summary>
        /// Serializes the current instance to the ProjectFilePath
        /// </summary>
        public void SaveProject()
        {
            if (String.IsNullOrEmpty(ProjectFilePath)) return;
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectFile));
            using (StreamWriter writer = new StreamWriter(ProjectFilePath))
            {
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// De-serializes the given file path to an instance which values will than be copied to the current instance
        /// </summary>
        /// <param name="filePath">Path of the project file</param>
        public void LoadProject(string filePath)
        {
            ProjectFilePath = filePath;
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectFile));
            using (StreamReader reader = new StreamReader(filePath))
            {
                ProjectFile projectFile = (ProjectFile)serializer.Deserialize(reader);
                OriginalImage = projectFile.OriginalImage;
                ModifiedImage = projectFile.ModifiedImage;
            }
        }

        /// <summary>
        /// This method deletes the whole project and moves the original image back to it's place
        /// </summary>
        public void DeleteProject()
        {
            if (String.IsNullOrEmpty(ProjectFilePath)) return;
            string root = Path.GetDirectoryName(ProjectFilePath);

            File.Move(OriginalImage.ImageCuePath, Path.Combine(Path.GetDirectoryName(ProjectFilePath), Path.GetFileName(OriginalImage.ImageCuePath)));
            File.Move(OriginalImage.ImageBinPath, Path.Combine(Path.GetDirectoryName(ProjectFilePath), Path.GetFileName(OriginalImage.ImageBinPath)));

            try
            {
                Directory.Delete(Path.GetDirectoryName(OriginalImage.ImageCuePath), true);
                Directory.Delete(Path.GetDirectoryName(ModifiedImage.ImageCuePath), true);
            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Process.Start("cmd.exe", "/c " + @"rmdir /s/q " + Path.GetDirectoryName(OriginalImage.ImageCuePath) + "");
                System.Diagnostics.Process.Start("cmd.exe", "/c " + @"rmdir /s/q " + Path.GetDirectoryName(ModifiedImage.ImageCuePath) + "");
            }

            File.Delete(ProjectFilePath);
            OriginalImage = null;
            ModifiedImage = null;
            ProjectFilePath = "";
        }
    }
}
