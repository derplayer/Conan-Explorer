using System;
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

        public static readonly HardCodedText[] Texts =
        {
            //New Game Dialog
            new HardCodedText(0x00000C9C, 0x12, "最初から始めます。", "Starting new game."), //starting from the beginning.
            new HardCodedText(0x00000CB0, 0x10, "よろしいですか？", "Are you sure?"), //is it OK?
            new HardCodedText(0x000A6E90, 0x4, "はい", "Yes"), //yes
            new HardCodedText(0x000A6E98, 0x6, "いいえ", "No"), //no

            //Options
            new HardCodedText(0x000A72E8, 0x6, "セーブ", "Save"), //save
            new HardCodedText(0x000A72F0, 0x6, "ロード", "Load"), //load
            new HardCodedText(0x0000337C, 0x12, "マップ移動スピード", "Walking-Speed"), //walk-speed
            new HardCodedText(0x00003390, 0x16, "メッセージ表示スピ-ド", "Text-Speed"), //text-speed
            new HardCodedText(0x000033A8, 0x10, "メッセージボイス", "Voices"), //voices
            new HardCodedText(0x000033BC, 0x8, "サウンド", "Sound"), //sound
            new HardCodedText(0x00003338, 0x8, "ステレオ", "Stereo"), //sound - stereo
            new HardCodedText(0x0000332C, 0x8, "モノラル", "Mono"), //sound - mono

            //Saving
            new HardCodedText(0x00003390, 0x1C, "どこにデータをセーブしますか", "Where do you want to save?"), //where should be saved?
            new HardCodedText(0x000033A8, 0x4, "新規", "New"), //new
            new HardCodedText(0x000033BC, 0xE, "セーブ中です。", "Saving in progress"), //saving in progress...
            new HardCodedText(0x00003338, 0x1A, "セーブ中はメモリーカードを", "While saving on memory card"), //while saving the memory card
            new HardCodedText(0x0000332C, 0x18, "抜き差ししないで下さい。", "do not unplug it"), //please do not plug in / out.

            //Ingame
            new HardCodedText(0x000032FE, 0x6, "拠検証", "Evidences"), //evaluation / search evidence
            new HardCodedText(0x00003308, 0xA, "オプション", "Options") //options

        };

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
