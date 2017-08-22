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
        public List<char> AllowedSymbols { get; set; } = new List<char>();
        public List<char> AllowedSplittedSymbols { get; set; } = new List<char>();
        public string FontName { get; set; }
        public int FontSize { get; set; } = 2;
        [XmlIgnore]
        public Font Font => new Font(FontName, FontSize);
        public List<FontCharacter> GeneratedFont { get; set; } = new List<FontCharacter>(3304);
        public List<ScriptDocument> Scripts { get; set; } = new List<ScriptDocument>();


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
                        stringBuilder.Append(message.ContentText);
                    }
                    else if (element.GetType() == typeof(ScriptGmap))
                    {
                        ScriptGmap gmap = (ScriptGmap)element;
                        stringBuilder.Append(gmap.ContentText);
                    }
                }
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

        public void GenerateFont(HashSet<string> dictionary, Font font)
        {
            GeneratedFont = FONTFile.EmptyFontCharacters();
            string[] unsortedDictionary = dictionary.ToArray();
            string[] sortedDictionary = dictionary.OrderBy(e => e).ToArray();
            foreach (FontCharacter fontCharacter in GeneratedFont) { fontCharacter.Data = new byte[32]; }
            for (int i = 0; i < unsortedDictionary.Length; i++)
            {
                FontCharacter fontCharacter = GeneratedFont[i + 1];
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
        }

        public void DeFormat()
        {

        }

        public HashSet<string> CreateDictionary(string[] lines) 
        {
            //chop up the lines into 2 char entries
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
