using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.RightsManagement;
using System.Xml.Serialization;
using ConanExplorer.Conan.Filetypes;
using ConanExplorer.Conan.Script;
using System.Text;
using ConanExplorer.Conan.Script.Elements;
using System.Windows;

namespace ConanExplorer.Conan
{
    /// <summary>
    /// Conan Explorer Script File
    /// </summary>
    public class ScriptFile
    {
        [XmlIgnore]
        public FontSettings FontSettings { get; set; } = new FontSettings();

        public List<char> AllowedSymbols
        {
            get
            {
                if (FontSettings.AllowedSymbols.Count > 256)
                    FontSettings = FontSettings.DE(); //dirty bugfix for older broken scripts...
                return FontSettings.AllowedSymbols;
            }
            set
            {
                FontSettings.AllowedSymbols = value;
            }
        }

        public List<char> AllowedSplittedSymbols
        {
            get
            {
                if (FontSettings.AllowedSplittedSymbols.Count > 256)
                    FontSettings = FontSettings.DE(); //dirty bugfix for older broken scripts...
                return FontSettings.AllowedSplittedSymbols;
            }
            set
            {
                FontSettings.AllowedSplittedSymbols = value;
            }
        }

        public string FontName
        {
            get
            {
                return FontSettings.FontName;
            }
            set
            {
                FontSettings.FontName = value;
            }
        }

        public int FontSize
        {
            get
            {
                return FontSettings.FontSize;
            }
            set
            {
                FontSettings.FontSize = value;
            }
        }
        public Font Font => new Font(FontName, FontSize);
        public List<FontCharacter> GeneratedFont { get; set; } = new List<FontCharacter>(3304);
        public List<ScriptDocument> Scripts { get; set; } = new List<ScriptDocument>();
        public List<HardCodedText> HardCodedTexts { get; set; } = new List<HardCodedText>();
        public List<FontCharacter> LockedCharacters { get; set; }

        public ScriptFile() { }

        public void AnalyseScripts()
        {
            
        }

        public bool IsValidChar(char character)
        {
            if (character == ' ') return true;
            foreach (char c in AllowedSymbols)
            {
                if (c == character) return true;
            }
            foreach (char c in AllowedSplittedSymbols)
            {
                if (c == character) return true;
            }
            return false;
        }

        public bool IsLockedChar(char character)
        {
            foreach (var c in LockedCharacters)
            {
                if (c.Symbol[0] == character) return true;
            }
            return false;
        }

        /// <summary>
        /// Generates the font characters for the current script file.
        /// </summary>
        public void GenerateFont()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ScriptDocument script in Scripts)
            {
                List<IScriptElement> elements = ScriptParser.Parse(script);
                foreach (IScriptElement element in elements)
                {
                    if (element.GetType() == typeof(ScriptMessage))
                    {
                        ScriptMessage message = (ScriptMessage)element;
                        stringBuilder.AppendLine(message.ContentText);
                    }
                    else if (element.GetType() == typeof(ScriptGmap))
                    {
                        ScriptGmap gmap = (ScriptGmap)element;
                        stringBuilder.AppendLine(gmap.ContentText);
                    }
                }
            }
            foreach (HardCodedText text in HardCodedTexts)
            {
                if (text.NewString == null)
                    stringBuilder.AppendLine(text.Translation);
                else
                    stringBuilder.AppendLine(text.NewString);
            }

            HashSet<string> dictionary = CreateDictionary(ScriptParser.TextToLines(stringBuilder.ToString()));

            //DEBUG OUTPUT START
            Console.WriteLine("Dictionary Size: {0} Symbols", dictionary.Count);
            if (dictionary.Count > 3302)
            {
                Console.WriteLine("Dictionary size is exceeding the 3302 limit!");
                MessageBox.Show("Dictionary size is exceeding the 3302 limit!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //DEBUG OUTPUT END

            GenerateFont(dictionary, Font);
        }

        /// <summary>
        /// Generates the font characters for the given 2 char dictionary and font.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="font"></param>
        public void GenerateFont(HashSet<string> dictionary, Font font)
        {
            FONTFile fontFile = ApplicationState.Instance.ProjectFile.ModifiedImage.FONTFile;
            fontFile.Load();
            GeneratedFont = fontFile.Characters;

            //GeneratedFont = FONTFile.EmptyFontCharacters();
            //foreach (FontCharacter fontCharacter in GeneratedFont) { fontCharacter.Data = new byte[32]; }

            int fontOffset = 1;
            string[] unsortedDictionary = dictionary.ToArray();
            string[] sortedDictionary = dictionary.OrderBy(e => e).ToArray();
            for (int i = 0; i < unsortedDictionary.Length; i++)
            {
                FontCharacter fontCharacter = GeneratedFont[i + fontOffset];
                while (LockedCharacters.Any(c => c.Index == fontCharacter.Index))
                {
                    fontOffset++;
                    fontCharacter = GeneratedFont[i + fontOffset];
                }

                fontCharacter.Data = new byte[32];
                fontCharacter.Symbol = unsortedDictionary[i];

                Bitmap bitmap = fontCharacter.GetBitmap();
                using (Graphics graphic = Graphics.FromImage(bitmap))
                {
                    RectangleF firstChar = new RectangleF(-2, 0, 16, 16);
                    RectangleF secondChar = new RectangleF(6, 0, 16, 16);

                    graphic.SmoothingMode = SmoothingMode.None;
                    graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphic.PixelOffsetMode = PixelOffsetMode.None;
                    graphic.DrawString("" + fontCharacter.Symbol[0], font, Brushes.White, firstChar);
                    if (fontCharacter.Symbol.Length == 1) continue; //should not appear actually
                    graphic.DrawString("" + fontCharacter.Symbol[1], font, Brushes.White, secondChar);
                }
                fontCharacter.SetBitmap(bitmap);
            }
        }

        /// <summary>
        /// Formats the messages with the generated font by replacing the text with shift-jis characters.
        /// </summary>
        /// <param name="save"></param>
        public void Format(bool save = true)
        {
            foreach (ScriptDocument script in Scripts)
            {
                if (script.BaseFile.GetType() != typeof(LZBFile)) continue;
                List<IScriptElement> elements = ScriptParser.Parse(script);
                foreach (IScriptElement element in elements)
                {
                    if (element.GetType() == typeof(ScriptMessage))
                    {
                        ScriptMessage message = (ScriptMessage)element;
                        message.Format(this);
                    }
                    else if (element.GetType() == typeof(ScriptGmap))
                    {
                        ScriptGmap gmap = (ScriptGmap)element;
                        gmap.Format(this);
                    }
                }
                ScriptParser.Parse(script, elements);
                if (save) script.WriteToOriginalFile();
            }

            foreach (HardCodedText text in HardCodedTexts)
            {
                text.Format(this);
            }
        }

        /// <summary>
        /// [UNUSED] Will not be used because makes not alot of sense.
        /// </summary>
        public void DeFormat() { }

        /// <summary>
        /// Chops up the lines into 2 char entries.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public HashSet<string> CreateDictionary(string[] lines) 
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                int counter = 0;
                string chunk = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (IsValidChar(line[i]))
                    {
                        chunk += line[i];
                        counter++;
                    }

                    if (i == line.Length - 1 && counter == 1)
                    {
                        result.Add(line[i] + " ");
                    }

                    if (counter == 2)
                    {
                        result.Add(chunk);
                        chunk = "";
                        counter = 0;
                    }
                }
            }
            return result;
        }

    }


}
