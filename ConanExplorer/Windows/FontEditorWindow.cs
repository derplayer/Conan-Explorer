using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan;
using ConanExplorer.Conan.Filetypes;
using ConanExplorer.ExtensionMethods;
using System.IO;
using System.Security.Cryptography;

namespace ConanExplorer.Windows
{
    public partial class FontEditorWindow : Form
    {
        private PSXImage _modifiedImage;
        private FONTFile _fontFile;

        private List<Bitmap> FontList; //3302
        public List<FontCharacter> FontCharacterList = new List<FontCharacter>();
        public List<FontCharacter> FontCharacterList_temp = new List<FontCharacter>();

        public FontEditorWindow()
        {
            InitializeComponent();

            if (ApplicationState.Instance.ProjectFile == null) return;
            _modifiedImage = ApplicationState.Instance.ProjectFile.ModifiedImage;
        }

        private void button_LoadFont_Click(object sender, EventArgs e)
        {
            if (_modifiedImage == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Conan Font File|FONT.BIN";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _fontFile = new FONTFile(openFileDialog.FileName);
                }
            }
            else
            {
                PKNFile graphPkn = _modifiedImage.PKNFiles?.Find(pkn => pkn.Name == "GRAPH");
                BaseFile fontFile = graphPkn?.Files?.Find(file => file.FileName == "FONT.BIN");
                if (fontFile == null) return;

                _fontFile = new FONTFile(fontFile.FilePath);
            }
            _fontFile.Load();

            FontList = _fontFile.Characters.Select(character => character.GetBitmap()).ToList();
            pictureBox_CharacterMap.Image = Graphic.CombineBitmaps(FontList, 42);
            MessageBox.Show("Symbols loaded: " + FontList.Count + " (default engine maximum)");
        }

        private void button_GenerateFont_Click(object sender, EventArgs e)
        {
            //TODO?: Foreach for more fonts
            object O = Properties.Resources.ResourceManager.GetObject("font1");
            Bitmap font_Bitmap = (Bitmap)O;

            int font_x_index = font_Bitmap.Width / 8;
            int font_y_index = font_Bitmap.Height / 16;

            List<Bitmap> indexed_letters = new List<Bitmap>();

            for (int i = 0; i < font_y_index; i++)
            {

                for (int j = 0; j < font_x_index; j++)
                {

                    //absolute disgusting font item blacklist
                    //each row hat 16 chars
                    
                    if (i == 1 && j == 13) continue; // Free Space 1
                    if (i == 1 && j == 14) continue; // Free Space 2
                    if (i == 1 && j == 15) continue; // Free Space 3
                    /*
                    if (i == 3 && j == 13) continue; //ß
                    */
                    //if (i == 3 && j == 14) continue; //(empty space for uneven words ends , also __ as 16x16 space)
                    if (i == 3 && j == 15) continue; //Free Space 4


                    Rectangle srcRect = new Rectangle((j * 8), (i * 16), 8, 16);
                    Bitmap picArea = Copy(font_Bitmap, srcRect);

                    indexed_letters.Add(picArea);

                }

            }
            int result = 0;
            List<Tuple<Bitmap, Bitmap, string>> theCharacters_temp = new List<Tuple<Bitmap, Bitmap, string>>();
            List<Tuple<Bitmap, Bitmap, string>> theCharacters_temp_special = new List<Tuple<Bitmap, Bitmap, string>>();

            //Index generated out of String and bitmap data from font
            for (int i = 0; i < textBox_Symbols.Text.Length; i++)
            {
                for (int j = 0; j < textBox_Symbols.Text.Length; j++)
                {
                    char ch1 = this.textBox_Symbols.Text.ElementAt<char>(i);
                    char ch2 = this.textBox_Symbols.Text.ElementAt<char>(j);

                    if (ch2 == 32 || ch1 == 32)
                    {
                        theCharacters_temp_special.Add(new Tuple<Bitmap, Bitmap, string>(indexed_letters[i], indexed_letters[j], ch1.ToString() + ch2.ToString()));
                    }else
                    {
                        theCharacters_temp.Add(new Tuple<Bitmap, Bitmap, string>(indexed_letters[i], indexed_letters[j], ch1.ToString() + ch2.ToString()));
                    }

                    result++;
                }
            }

            foreach (var item in theCharacters_temp_special)
            {
                theCharacters_temp.Add(item);
            }
            //

            //TODO: Add at the end special chars like !?.""

            //
            //Tuple<Bitmap, Bitmap, string>[] combinationsList = theCharacters_temp.ToArray();

            //for (int i = 0; i < combinationsList.Length; i++)
            //{
            //    List<Bitmap> combination = new List<Bitmap>() { combinationsList[i].Item1, combinationsList[i].Item2 };
            //    string Symbolstring = combinationsList[i].Item3;

            //    FontCharacter mergedFontCharacter = new FontCharacter((short)i, Symbolstring);
            //    mergedFontCharacter.SetBitmap(Graphic.CombineBitmaps(combination, 2));
            //    FontCharacterList.Add(mergedFontCharacter);
            //}

            //FontList = FontCharacterList.Select((character => character.GetBitmap())).ToList();
            //pictureBox_CharacterMap.Image = Graphic.CombineBitmaps(FontList, 42);

            //if (FontList.Count <= 3302) { MessageBox.Show("Symbols created: " + FontList.Count); }
            //else { MessageBox.Show("Symbols created: " + FontList.Count + Environment.NewLine + "WARNING: Engine will crash with this amount of chars!"); }

        }

        static public Bitmap Copy(Bitmap srcBitmap, Rectangle srcRect)
        {

            // Create the new bitmap and associated graphics object
            Bitmap bmp = new Bitmap(8, 16);
            Graphics g = Graphics.FromImage(bmp);

            //Overwrite default smoothing settings
            g.InterpolationMode = InterpolationMode.NearestNeighbor;      //raw 1:1
            g.PixelOffsetMode = PixelOffsetMode.None;                   
            GraphicsUnit units = GraphicsUnit.Pixel;

            Rectangle destRect = new Rectangle(0, 0, 8, 16);
            g.DrawImage(srcBitmap, destRect, srcRect, units);

            g.Dispose();

            // Return the bitmap
            return bmp;
        }

        private void pictureBox_CharacterMap_MouseMove(object sender, MouseEventArgs e)
        {
            int xCoordinates = e.Location.X / 16;
            int yCoordinates = e.Location.Y / 16;
            int index = yCoordinates * 42 + xCoordinates;

            toolStripStatusLabel_Character.Text = "";
            if (_fontFile != null)
            {
                if (index >= _fontFile.Characters.Count) return;
                short charIndex = _fontFile.Characters[index].Index;
                Console.WriteLine(charIndex);
                charIndex = (short)(((charIndex & 0xFF00) >> 8) | ((charIndex & 0x00FF) << 8));
                toolStripStatusLabel_Character.Text = String.Format("Index: {0:X}, Symbol: {1}", charIndex, _fontFile.Characters[index].Symbol);
            }
            if (FontCharacterList.Count != 0)
            {
                if (index >= FontCharacterList.Count) return;
                short charIndex = FontCharacterList[index].Index;
                charIndex = (short)(((charIndex & 0xFF00) >> 8) | ((charIndex & 0x00FF) << 8));
                toolStripStatusLabel_Character.Text = String.Format("Index: {0:X}, Symbol: {1}", charIndex, FontCharacterList[index].Symbol);
            }
        }

        private void button_Filter_Click(object sender, EventArgs e)
        {
            List<FontCharacter> FontCharacterList_temp = new List<FontCharacter>(FontCharacterList);

            List<string> whiteList;
            byte[] checksum;
            try { 
            checksum = Serializae.ReadFromBinaryFile<byte[]>(Application.StartupPath + "\\whitelist_check.bin");
            }catch(System.IO.FileNotFoundException)
            {
                checksum = null;
            }

            if (checksum == null) { //its okay for now
            string wordlist = ConanExplorer.Properties.Resources.wordlist_long;
            whiteList = new List<string>();
            char c1;
            char c2;
            
            for (int i = 0; i < wordlist.Length; i += 2)
            {
                c1 = wordlist.ElementAt<char>(i);
                try { c2 = wordlist.ElementAt<char>(i + 1); } catch { c2 = '_'; } //placeholder
                
                //Safe mode every combination possible
                string combination1 = c1.ToString() + c2.ToString();
                string combination2 = c2.ToString() + c1.ToString();
                whiteList.Add(combination1);
                whiteList.Add(combination2);
            }
                //Spaced Letters into whitelist (8x16 letter + space)
                for (int i = 0; i < textBox_Symbols.Text.Length; i++)
                {
                    char ch1 = this.textBox_Symbols.Text.ElementAt<char>(i);
                    whiteList.Add(ch1.ToString() + " ");
                    whiteList.Add(" " + ch1.ToString());
                }

                Serializae.WriteToBinaryFile<List<string>>(Application.StartupPath + "\\whitelist.bin", whiteList);
                MD5 md5 = MD5.Create();
                    using (var stream = File.OpenRead(Application.StartupPath + "\\whitelist.bin"))
                    {
                        md5.ComputeHash(stream);
                    }

                Serializae.WriteToBinaryFile<byte[]>(Application.StartupPath + "\\whitelist_check.bin", md5.Hash);

            }else {
                whiteList = Serializae.ReadFromBinaryFile<List<string>>(Application.StartupPath + "\\whitelist.bin");
            }

            FontCharacterList.Clear();
            //every generated character list
            foreach (var item in FontCharacterList_temp)
            {
                foreach (var whileListItem in whiteList)
                {
                    if (whileListItem == item.Symbol)
                    {
                        FontCharacterList.Add(item);
                        break;
                    }
                }
            }

            MessageBox.Show("The Filterd Characters are now: " + FontCharacterList.Count);

            FontList.Clear();
            FontList = FontCharacterList.Select((character => character.GetBitmap())).ToList();

            pictureBox_CharacterMap.Image = Graphic.CombineBitmaps(FontList, 42);
        }

        private void button_SaveFont_Click(object sender, EventArgs e)
        {
            _fontFile.Save();
        }
    }
}
