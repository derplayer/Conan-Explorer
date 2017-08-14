using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan;
using ConanExplorer.Conan.Filetypes;
using ConanExplorer.Conan.Headers;
using ConanExplorer.Controls;
using ConanExplorer.ExtensionMethods;
using ConanExplorer.Properties;

namespace ConanExplorer.Windows
{
    public partial class MainWindow : Form
    {
        private ApplicationState _state = ApplicationState.Instance;

        public MainWindow()
        {
            InitializeComponent();
            dynamicControl.Initialize();

            Text = _state.WindowTitle;
            _state.WindowTitleChanged += _state_WindowTitleChanged;
            _state.EmulatorStarted += _state_EmulatorStarted;
            _state.EmulatorStopped += _state_EmulatorStopped;
        }


        private void UpdateTreeNodes()
        {
            treeView1.Nodes.Clear();
            if (_state.ProjectFile == null) return;

            PSXImage image = _state.ProjectFile.ModifiedImage;
            TreeNode imageNode = new TreeNode(image.ToString());
            imageNode.Tag = image;
            imageNode.Checked = false;

            foreach (string directory in ConanImage.Directories)
            {
                TreeNode directoryNode = new TreeNode(directory);
                directoryNode.ImageIndex = 0;

                PKNFile pkn = image.PKNFiles.Find(e => e.Name == directory);
                if (pkn != null)
                {
                    TreeNode pknNode = new TreeNode(pkn.FileName);
                    pknNode.ImageIndex = 1;
                    pknNode.SelectedImageIndex = 1;
                    pknNode.Tag = pkn;

                    if (pkn.Files.Count != 0)
                    {
                        foreach (BaseFile file in pkn.Files)
                        {
                            TreeNode fileNode = new TreeNode(file.FileName);
                            fileNode.Tag = file;

                            if (file.GetType() == typeof(PBFile))
                            {
                                PBFile pbFile = (PBFile)file;
                                foreach (PBFileEntry entry in pbFile.Files)
                                {
                                    TreeNode entryNode = new TreeNode(entry.File.FileName);
                                    entryNode.Tag = entry.File;
                                    fileNode.Nodes.Add(entryNode);
                                }
                            }
                            if (file.GetType() == typeof(BGFile))
                            {
                                BGFile bgFile = (BGFile)file;
                                foreach (BaseFile entry in bgFile.Files)
                                {
                                    TreeNode entryNode = new TreeNode(entry.FileName);
                                    entryNode.Tag = entry;
                                    fileNode.Nodes.Add(entryNode);
                                }
                            }
                            pknNode.Nodes.Add(fileNode);
                        }
                    }
                    directoryNode.Nodes.Add(pknNode);
                }
                else
                {
                    foreach (ConanImageFile file in ConanImage.Files)
                    {
                        if (Path.GetDirectoryName(file.FilePath) == directory)
                        {
                            
                            TreeNode fileNode = new TreeNode(Path.GetFileName(file.FilePath));
                            fileNode.Tag = file;
                            //fileNode.Tag = HeaderList.GetTypeFromFile(Path.Combine(_state.ProjectFile.ModifiedImage.RippedDirectory,file.FilePath)); //LAAAAG
                            directoryNode.Nodes.Add(fileNode);
                        }
                    }
                }
                imageNode.Nodes.Add(directoryNode);
            }
            treeView1.Nodes.Add(imageNode);
        }

        private void _state_WindowTitleChanged(object sender, string e)
        {
            Text = e;
        }

        private void _state_EmulatorStopped(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                runModifiedImageToolStripMenuItem.Enabled = true;
                stopModifiedImageToolStripMenuItem.Enabled = false;
                buildModifiedImageToolStripMenuItem.Enabled = true;
                revertModifiedImageToolStripMenuItem.Enabled = true;
                deleteProjectToolStripMenuItem.Enabled = true;
            });
        }

        private void _state_EmulatorStarted(object sender, EventArgs e)
        {
            runModifiedImageToolStripMenuItem.Enabled = false;
            stopModifiedImageToolStripMenuItem.Enabled = true;
            buildModifiedImageToolStripMenuItem.Enabled = false;
            revertModifiedImageToolStripMenuItem.Enabled = false;
            deleteProjectToolStripMenuItem.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Conan Explorer Project (*.cep)|*.cep|PSX Image (*.cue/*.bin)|*.cue;*.bin";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileExtension = Path.GetExtension(openFileDialog.FileName).ToLower();

                if (fileExtension == ".cep")
                {
                    if (!ApplicationState.Instance.LoadProject(openFileDialog.FileName))
                    {
                        MessageBox.Show("The project could not be loaded correctly!");
                    }
                }
                else if (fileExtension == ".cue" || fileExtension == ".bin")
                {
                    if (!ApplicationState.Instance.CreateProject(openFileDialog.FileName))
                    {
                        MessageBox.Show("The project could not be created correctly!");
                    }
                }
                else
                {
                    MessageBox.Show("UNKNOWN FORMAT!");
                }
            }
            closeToolStripMenuItem.Enabled = true;
            projectToolStripMenuItem.Enabled = true;
            UpdateTreeNodes();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _state.CloseProject();
            closeToolStripMenuItem.Enabled = false;
            projectToolStripMenuItem.Enabled = false;
            UpdateTreeNodes();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void runModifiedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ApplicationState.Instance.ProjectFile.ProjectFilePath)) return;
            ApplicationState.Instance.RunEmulator();
        }

        private void stopModifiedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ApplicationState.Instance.ProjectFile.ProjectFilePath)) return;
            ApplicationState.Instance.StopEmulator();
        }

        private void buildModifiedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationState.Instance.ProjectFile.ModifiedImage.Build();
        }

        private void deleteProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Oy vey!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ApplicationState.Instance.DeleteProject();
                projectToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = false;
                UpdateTreeNodes();
            }
        }

        private void fontEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontEditorWindow fontEditorWindow = new FontEditorWindow();
            fontEditorWindow.ShowDialog();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.Node.Tag == null) return;
            if (e.Node.Tag.GetType() == typeof (PKNFile))
            {
                treeView1.SelectedNode = e.Node;
                ContextMenu cm = new ContextMenu();
                cm.MenuItems.Add("Reset", OnClickPKNReset);
                cm.MenuItems.Add("Unpack/Decompress All", OnClickPKNUnpackDecompAll);
                cm.Show(treeView1, e.Location);
            }

            if (e.Node.Level >= 3)
            {
                treeView1.SelectedNode = e.Node;
                ContextMenu cm = new ContextMenu();

                if (e.Node.Tag.GetType() == typeof(LZBFile))
                {
                    cm.MenuItems.Add("Compress", OnClickLZBCompress);
                    cm.MenuItems.Add("Decompress", OnClickLZBDecompress);
                    cm.MenuItems.Add("Clear", OnClickLZBClear);
                    cm.MenuItems.Add("-");
                }
                if (e.Node.Tag.GetType() == typeof(PBFile))
                {
                    cm.MenuItems.Add("Pack", OnClickPBPack);
                    cm.MenuItems.Add("Unpack", OnClickPBUnpack);
                    cm.MenuItems.Add("-");
                }
                if (e.Node.Tag.GetType() == typeof(BGFile))
                {
                    cm.MenuItems.Add("Pack", OnClickBGPack);
                    cm.MenuItems.Add("Unpack", OnClickBGUnpack);
                    cm.MenuItems.Add("-");
                }

                cm.MenuItems.Add("Revert To Original", OnClickRevertToOriginal);
                cm.Show(treeView1, e.Location);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                compareControl.Update(null); 
                dynamicControl.Update(null);
                return;
            }
            if (e.Node.Tag.GetType().IsSubclassOf(typeof(BaseFile)))
            {
                BaseFile baseFile = (BaseFile)e.Node.Tag;
                compareControl.Update(baseFile);
                dynamicControl.Update(baseFile);
                return;
            }
            compareControl.Update(null);
            dynamicControl.Update(null);
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;
            if (e.Node.Tag.GetType().IsSubclassOf(typeof(BaseFile)))
            {
                BaseFile baseFile = (BaseFile) e.Node.Tag;
                System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", baseFile.FilePath));
            }
        }

        private void OnClickRevertToOriginal(object sender, EventArgs eventArgs)
        {
            TreeNode pknNode = treeView1.SelectedNode;
            BaseFile baseFile = (BaseFile) pknNode.Tag;

            foreach (PKNFile pknFile in _state.ProjectFile.OriginalImage.PKNFiles)
            {
                foreach (BaseFile file in pknFile.Files)
                {
                    if (file.FileName == baseFile.FileName)
                    {
                        File.Copy(file.FilePath, baseFile.FilePath, true);
                        if (baseFile.GetType() == typeof (LZBFile))
                        {
                            LZBFile lzbFile = (LZBFile) baseFile;
                            if (lzbFile.DecompressedFile != null)
                            {
                                lzbFile.DecompressedFile.Delete();
                                lzbFile.DecompressedFile = null;
                            }
                            if (lzbFile.HeaderFile != null)
                            {
                                lzbFile.HeaderFile.Delete();
                                lzbFile.HeaderFile = null;
                            }
                        }
                        return;
                    }
                }
            }

            MessageBox.Show("Couldn't find the original file", "Hue");
        }

        private void OnClickPKNReset(object sender, EventArgs eventArgs)
        {
            TreeNode pknNode = treeView1.SelectedNode;
            pknNode.Nodes.Clear();

            PKNFile pkn = (PKNFile)pknNode.Tag;
            pkn.Clear();
            pkn.Unpack();
            _state.SaveProject();

            if (pkn.Files.Count != 0)
            {
                foreach (BaseFile file in pkn.Files)
                {
                    TreeNode fileNode = new TreeNode(file.FileName);
                    fileNode.Tag = file;
                    pknNode.Nodes.Add(fileNode);
                }
            }
            pknNode.Expand();
        }

        private void OnClickPKNUnpackDecompAll(object sender, EventArgs eventArgs)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.Tag == null) return;

            Enabled = false;

            if (treeView1.SelectedNode.Tag.GetType() == typeof (PKNFile))
            {
                PKNFile pknFile = (PKNFile) treeView1.SelectedNode.Tag;
                foreach (BaseFile file in pknFile.Files)
                {
                    if (file.GetType() == typeof (LZBFile))
                    {
                        LZBFile lzbFile = (LZBFile) file;
                        lzbFile.Decompress();
                    }
                }
            }
            _state.SaveProject();
            Enabled = true;
        }


        private void OnClickLZBCompress(object sender, EventArgs eventArgs)
        {
            LZBFile lzb = (LZBFile)treeView1.SelectedNode.Tag;

            Enabled = false;
            if (!lzb.Compress())
            {
                MessageBox.Show("There is nothing to compress!");
                Enabled = true;
                return;
            }
            _state.SaveProject();
            Enabled = true;

            //UpdateTreeNodes();
        }

        private void OnClickLZBDecompress(object sender, EventArgs eventArgs)
        {
            TreeNode lzbNode = treeView1.SelectedNode;
            //lzbNode.Nodes.Clear();

            LZBFile lzb = (LZBFile)lzbNode.Tag;

            Enabled = false;
            lzb.Decompress();
            Enabled = true;
            _state.SaveProject();

            //UpdateTreeNodes();
        }

        private void OnClickLZBClear(object sender, EventArgs eventArgs)
        {
            TreeNode lzbNode = treeView1.SelectedNode;
            //lzbNode.Nodes.Clear();

            LZBFile lzb = (LZBFile)lzbNode.Tag;
            lzb.DecompressedFile = null;
            _state.SaveProject();

            //UpdateTreeNodes();
        }

        private void OnClickPBPack(object sender, EventArgs eventArgs)
        {
            TreeNode pbNode = treeView1.SelectedNode;
            PBFile pb = (PBFile)pbNode.Tag;

            Enabled = false;
            pb.Pack();
            Enabled = true;
            _state.SaveProject();
        }

        private void OnClickPBUnpack(object sender, EventArgs eventArgs)
        {
            TreeNode pbNode = treeView1.SelectedNode;
            PBFile pb = (PBFile)pbNode.Tag;

            Enabled = false;
            pb.Unpack();
            Enabled = true;
            _state.SaveProject();
        }

        private void OnClickBGPack(object sender, EventArgs eventArgs)
        {
            TreeNode pbNode = treeView1.SelectedNode;
            BGFile bg = (BGFile)pbNode.Tag;

            Enabled = false;
            bg.Pack();
            Enabled = true;
            _state.SaveProject();
        }

        private void OnClickBGUnpack(object sender, EventArgs eventArgs)
        {
            TreeNode pbNode = treeView1.SelectedNode;
            BGFile bg = (BGFile)pbNode.Tag;

            Enabled = false;
            bg.Unpack();
            Enabled = true;
            _state.SaveProject();
        }



        private void lZBDeCompressorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LZB File (*.LZB)|*.LZB|OUT File (*.OUT)|*.OUT|Other Files|*.*";
            openFileDialog.FilterIndex = 0;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(openFileDialog.FileName).ToUpper();
                if (extension == ".LZB")
                {
                    LZBFile lzbFile = new LZBFile(openFileDialog.FileName);

                    TaskProgress progress = new TaskProgress();
                    progress.TaskDone += ProgressOnTaskDone;
                    Enabled = false;
                    lzbFile.Decompress();
                    Enabled = true;
                }
                else if (extension == ".OUT")
                {
                    LZBFile lzbFile = new LZBFile();
                    lzbFile.DecompressedFile = new BaseFile(openFileDialog.FileName);
                    lzbFile.HeaderFile = new BaseFile(Path.GetDirectoryName(openFileDialog.FileName) + "\\" + Path.GetFileNameWithoutExtension(openFileDialog.FileName) + ".HEADER");
                    lzbFile.FilePath = Path.GetDirectoryName(openFileDialog.FileName) + "\\" + Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                    TaskProgress progress = new TaskProgress();
                    progress.TaskDone += ProgressOnTaskDone;
                    Enabled = false;
                    lzbFile.Compress();
                    Enabled = true;
                }
                else
                {
                    LZBFile lzbFile = new LZBFile();
                    SelectModeWindow selectModeWindow = new SelectModeWindow();
                    if (selectModeWindow.ShowDialog() == DialogResult.OK)
                    {
                        string headerPath = openFileDialog.FileName + ".HEADER";
                        using (BinaryWriter writer = new BinaryWriter(new FileStream(headerPath, FileMode.Create)))
                        {
                            writer.Write(LZSSHeader.Empty(selectModeWindow.Mode).GetBytes());
                        }
                        string outputPath = openFileDialog.FileName + ".OUT";
                        File.Copy(openFileDialog.FileName, outputPath);

                        lzbFile.HeaderFile = new BaseFile(headerPath);
                        lzbFile.DecompressedFile = new BaseFile(outputPath);
                        lzbFile.FilePath = openFileDialog.FileName + ".LZB";

                        TaskProgress progress = new TaskProgress();
                        progress.TaskDone += ProgressOnTaskDone;
                        Enabled = false;
                        lzbFile.Compress();
                        Enabled = true;
                    }
                }
            }
        }

        private void ProgressOnTaskDone(object sender, EventArgs eventArgs)
        {
            Enabled = true;
        }

        private void revertModifiedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PSXImage modifiedImage = _state.ProjectFile.ModifiedImage;
            modifiedImage.Clear(false);
            modifiedImage.Rip(_state.ProjectFile.ModifiedImage.RippedDirectory);
            UpdateTreeNodes();
        }

        private void revertToOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PSXImage modifiedImage = _state.ProjectFile.ModifiedImage;
            PSXImage originalImage = _state.ProjectFile.OriginalImage;            
            File.Copy(originalImage.ImageBinPath, modifiedImage.ImageBinPath, true);
            modifiedImage.Clear(false);
            modifiedImage.Rip(modifiedImage.RippedDirectory);
            UpdateTreeNodes();
        }

        private void fileIndexViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ApplicationState.Instance.ProjectFile == null)
            {
                MessageBox.Show("There is no project maaaan.");
                return;
            }
            FileDictionary fileDirectory = ApplicationState.Instance.ProjectFile.ModifiedImage.FileDictionary;
            if (fileDirectory == null)
            {
                MessageBox.Show("There are no file indices maaaan.");
                return;
            }
            FileIndexViewerWindow fileIndexViewerWindow = new FileIndexViewerWindow();
            fileIndexViewerWindow.ShowDialog();
        }

        private void scriptEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptEditorWindow scriptEditorWindow = new ScriptEditorWindow();
            scriptEditorWindow.ShowDialog();
        }

        private void debugToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DebugXaToPCM bmp2tim = new DebugXaToPCM();
            bmp2tim.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", @"¯\_(ツ)_/¯");
        }
    }
}
