using ConanExplorer.Conan;
using ConanExplorer.Conan.Filetypes;
using ConanExplorer.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace ConanExplorer.Windows
{
    public partial class LockedCharactersWindow : Form
    {
        private ScriptFile _scriptFile;
        private FONTFile _fontFile;

        const int LVM_FIRST = 0x1000;
        const int LVM_SETICONSPACING = LVM_FIRST + 53;

        public LockedCharactersWindow(ScriptFile scriptFile)
        {
            InitializeComponent();
            SetSpacing(32, 36);

            _scriptFile = scriptFile;

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label_LoadingFont.Visible = false;
            listView_FontCharacters.Enabled = true;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitializeCharacters();
        }

        private void InitializeCharacters()
        {
            _fontFile = ApplicationState.Instance.ProjectFile.ModifiedImage.FONTFile;
            _fontFile.Load();

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(16, 16);
            foreach (FontCharacter character in _fontFile.Characters)
            {
                imageList.Images.Add(character.Index.ToString(), character.GetBitmap());
            }

            Invoke((MethodInvoker)delegate ()
            {
                listView_FontCharacters.LargeImageList = imageList;
            });


            List<ListViewItem> items = new List<ListViewItem>();
            foreach (FontCharacter character in _fontFile.Characters)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageKey = character.Index.ToString();
                lvi.Text = character.IndexString;

                if (_scriptFile.LockedCharacters.Any(c => c.Index == character.Index))
                {
                    lvi.Checked = true;
                }

                //Blacklist for chars, that crash the game-engine!
                //88 is safe for the engine, but just to be sure, i moved the font segment (crashes begin at 0x8995+)
                if (lvi.Text.StartsWith("899") == true) lvi.Checked = true;     //8995-899F cause crashes?

                items.Add(lvi);
            }
            Invoke((MethodInvoker)delegate ()
            {
                listView_FontCharacters.Items.AddRange(items.ToArray());
            });
        }

        private void LockedCharactersWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _scriptFile.LockedCharacters.Clear();
            foreach (ListViewItem lvi in listView_FontCharacters.Items)
            {
                if (lvi.Checked)
                {
                    FontCharacter fontCharacter = _fontFile.Characters.FirstOrDefault(c => c.IndexString == lvi.Text);
                    if (fontCharacter == null) continue;
                    _scriptFile.LockedCharacters.Add(fontCharacter);
                }
            }
        }

        private void listView_FontCharacters_Click(object sender, EventArgs e)
        {
            //if not disabled then...
            if(listView_FontCharacters.Enabled == true) {
                try
                {
                    string selectedCharId = listView_FontCharacters.SelectedItems[0].Text;
                    foreach (var element in _scriptFile.LockedCharacters)
                    {
                        if (element.IndexString == selectedCharId)
                        {
                            Clipboard.SetText(element.Symbol, TextDataFormat.UnicodeText);
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    //Leave clipboard empty
                }
            }
        }

        private int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        private void SetSpacing(short x, short y)
        {
            const int LVM_FIRST = 0x1000;
            const int LVM_SETICONSPACING = LVM_FIRST + 53;
            Win32.SendMessage(listView_FontCharacters.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(x, y));
            listView_FontCharacters.Refresh();
        }
    }
}
