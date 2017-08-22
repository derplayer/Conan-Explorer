using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ConanExplorer.Conan.Script.Elements
{
    public class ScriptGmap : ScriptCommand
    {
        private static Encoding _shiftJis = Encoding.GetEncoding("shift_jis");

        private string _content;

        public override string Text
        {
            get
            {
                if (_linebreak)
                {
                    return String.Format("#MESSAGE:{0}\r\n{1}\r\n", Parameters[0], Content);
                }
                return String.Format("#MESSAGE:{0}\r\n{1}", Parameters[0], Content);
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
            }
        }

        public string ContentText
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                string[] lines = ScriptParser.TextToLines(Content);
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] cols = lines[i].Split(',');
                    if (cols.Length != 3) throw new Exception("NO!");
                    stringBuilder.AppendLine(cols[2]);
                }
                return stringBuilder.ToString();
            }
        }


        public ScriptGmap(Match match, int lineIndex, bool linebreak = true) : base(match, lineIndex, linebreak) { }

        public void Format(ScriptFile script, bool ignoreMissing = true)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string[] lines = ScriptParser.TextToLines(Content);

            stringBuilder.AppendLine(lines[0]);
            for (int l = 1; l < lines.Length; l++)
            {
                string[] splitted = lines[l].Split(',');
                if (splitted.Length != 3) throw new Exception("NO!");
                string displayText = splitted[2];

                stringBuilder.Append(splitted[0] + "," + splitted[1] + ",");

                for (int i = 0; i < displayText.Length; i++)
                {
                    bool pair = false;
                    string neededSymbol = displayText[i].ToString();
                    if (i + 1 < displayText.Length)
                    {
                        if (script.IsValidChar(displayText[i + 1]))
                        {
                            neededSymbol += displayText[i + 1];
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

                stringBuilder.Append("\r\n");
            }

            Content = stringBuilder.ToString();
        }

        public void DeFormat(ScriptFile script)
        {

        }
    }
}
