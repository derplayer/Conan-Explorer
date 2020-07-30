﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ConanExplorer.Conan
{
    public class HardCodedText
    {
        private static Encoding _shiftJis = Encoding.GetEncoding("shift_jis");
        public static List<HardCodedText> Texts { get; set; }

        public int Offset { get; set; }
        public int Length { get; set; }
        public string OriginalString { get; set; }
        [XmlIgnore]
        public string CurrentString
        {
            get
            {
                string executablePath = ApplicationState.Instance.ProjectFile.ModifiedImage.RippedExecutablePath;
                using (BinaryReader reader = new BinaryReader(new FileStream(executablePath, FileMode.Open)))
                {
                    reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
                    byte[] data = reader.ReadBytes(Length);
                    return _shiftJis.GetString(data);
                }
            }
            set
            {
                string executablePath = ApplicationState.Instance.ProjectFile.ModifiedImage.RippedExecutablePath;
                using (BinaryWriter writer = new BinaryWriter(new FileStream(executablePath, FileMode.Open)))
                {
                    writer.BaseStream.Seek(Offset, SeekOrigin.Begin);
                    byte[] data = _shiftJis.GetBytes(value);
                    writer.Write(data);

                    for (int i = 0; i < Length - data.Length; i++)
                    {
                        writer.Write('\0');
                    }
                }
            }
        }
        public string NewString { get; set; }

        public string Translation { get; set; }

        public HardCodedText() { }

        public HardCodedText(int offset, int length, string originalString, string translation)
        {
            Offset = offset;
            Length = length;
            OriginalString = originalString;
            Translation = translation;
        }

        public void Format(ScriptFile script, bool ignoreMissing = true)
        {
            if (NewString == null) return; //skip if value was not edited
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < NewString.Length; i++)
            {
                bool pair = false;
                string neededSymbol = NewString[i].ToString();
                if (i + 1 < NewString.Length)
                {
                    if (script.IsValidChar(NewString[i + 1]))
                    {
                        neededSymbol += NewString[i + 1];
                        pair = true;
                    }
                    else
                    {
                        neededSymbol += " ";
                    }
                }
                else
                {
                    neededSymbol += " ";
                }

                bool found = false;
                foreach (FontCharacter fontCharacter in script.GeneratedFont)
                {
                    if (fontCharacter.Symbol == neededSymbol)
                    {
                        byte[] bytes = BitConverter.GetBytes(fontCharacter.Index);
                        stringBuilder.Append(_shiftJis.GetString(bytes));
                        found = true;
                        if (pair) i++;
                        break;
                    }
                }
                if (!found)
                {
                    if (!ignoreMissing)
                    {
                        MessageBox.Show("Couldn't find the needed symbol in the generated font.\nGenerate the font before formating!", "Format Error!");
                        return;
                    }
                }
            }
            CurrentString = stringBuilder.ToString();
        }

        public override string ToString()
        {
            return String.Format("[0x{0} - {1} Chars]: \"{2}\" (\"{3}\")", Offset.ToString("X").PadLeft(8, '0'), Length.ToString().PadLeft(2, '0'), OriginalString, Translation);
        }
    }
}
