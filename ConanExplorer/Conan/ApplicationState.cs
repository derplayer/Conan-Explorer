using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.Conan
{
    /// <summary>
    /// This singleton pattern class holds information about the current application state
    /// </summary>
    public class ApplicationState
    {
        public event EventHandler<string> WindowTitleChanged;
        public event EventHandler EmulatorStarted;
        public event EventHandler EmulatorStopped;

        public string ProjectPath
        {
            get
            {
                if (_projectFile == null) return "";
                return Path.GetDirectoryName(_projectFile.ProjectFilePath);
            }
        }
        
        private const string _xebraExecutable = "Tools\\xebra\\xebra.exe";
        private const string _xebraExecutablePath = "Tools\\xebra\\";

        // TODO: http://drhell.web.fc2.com/ps1/xebra190322a.zip
        // Upgrade xebra  & migrate the emulator settings (incompatible with new version)
        private const string _xebraDownload = "http://drhell.web.fc2.com/ps1/xebra170711.zip";
        private readonly string _zipTempPath = Path.GetTempPath() + "xebra.zip";
        private Process _xebraProcess;

        private static ApplicationState _instance;
        private static readonly object SyncRoot = new object();

        private ProjectFile _projectFile;

        private ApplicationState() { }

        public static ApplicationState Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (SyncRoot)
                {
                    if (_instance == null) { _instance = new ApplicationState(); }
                }
                return _instance;
            }
        }

        public ProjectFile ProjectFile
        {
            get { return _projectFile; }
            set
            {
                _projectFile = value;
                OnWindowTitleChanged(WindowTitle);
            }
        }

        public string WindowTitle
        {
            get
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string name = Assembly.GetExecutingAssembly().GetName().Name;
                if (!String.IsNullOrEmpty(ProjectFile?.ProjectFilePath))
                {
                    return String.Format("{0} {1} - {2}", name, version, ProjectFile.ProjectFilePath);
                }
                return String.Format("{0} {1}", name, version);
            }
        }


        public bool CreateProject(string filePath)
        {
            ProjectFile = new ProjectFile();
            ProjectFile.CreateProject(filePath);
            OnWindowTitleChanged(WindowTitle);
            return true;
        }

        public bool LoadProject(string filePath)
        {
            ProjectFile = new ProjectFile();
            ProjectFile.LoadProject(filePath);
            OnWindowTitleChanged(WindowTitle);

            if (!File.Exists(ProjectFile.OriginalImage.ImageBinPath))
            {
                MessageBox.Show("Where is my original image file?\n" + ProjectFile.OriginalImage.ImageBinPath);
                return false;
            }
            if (!File.Exists(ProjectFile.ModifiedImage.ImageBinPath))
            {
                MessageBox.Show("Where is my modified image file?\n" + ProjectFile.ModifiedImage.ImageBinPath);
                return false;
            }

            return true;
        }

        public bool SaveProject()
        {
            ProjectFile.SaveProject();
            return true;
        }

        public bool CloseProject()
        {
            ProjectFile = null;
            return true;
        }

        public bool DeleteProject()
        {
            if (String.IsNullOrEmpty(ProjectFile.ProjectFilePath)) return false;
            ProjectFile.DeleteProject();
            OnWindowTitleChanged(WindowTitle);
            ProjectFile = null;
            return true;
        }

        public bool RunEmulator()
        {
            //Check for xebra instance to download from the internet if not there
            if (File.Exists(_xebraExecutable) == false)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "PSX Emulator (XEBRA) missing.\r\n\nDo you want to download it?",
                    "XEBRA not found!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Asterisk
                );

                if (dialogResult == DialogResult.Yes)
                {
                    using (var client = new WebClient())
                    {
                        //TODO: Use DownloadFileAsync for UX feedback & fallback when offline?
                        client.DownloadFile(_xebraDownload, _zipTempPath);
                        System.IO.Compression.ZipFile.ExtractToDirectory(_zipTempPath, _xebraExecutablePath);  
                    }
                }
                else
                    return false;
            }
            //Run xebra emlator instance with game cue/bin path as parameter
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = _xebraExecutable,
                Arguments = String.Format("\"{0}\"", ProjectFile.ModifiedImage.ImageBinPath)
            };
            _xebraProcess = new Process { StartInfo = info };
            _xebraProcess.EnableRaisingEvents = true;
            _xebraProcess.Exited += _xebraProcess_Exited;
            _xebraProcess.Start();            
            OnEmulatorStarted();
            return true;
        }

        public bool StopEmulator()
        {
            _xebraProcess.Kill();
            return true;
        }

        private void _xebraProcess_Exited(object sender, EventArgs e)
        {
            OnEmulatorStopped();
        }


        private void OnWindowTitleChanged(string e)
        {
            WindowTitleChanged?.Invoke(null, e);
        }

        private void OnEmulatorStarted()
        {
            EmulatorStarted?.Invoke(null, EventArgs.Empty);
        }

        private void OnEmulatorStopped()
        {
            EmulatorStopped?.Invoke(null, EventArgs.Empty);
        }
    }
}
