using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan
{
    public class ConanScriptMessage
    {
        private Encoding _shiftJis = Encoding.GetEncoding("shift_jis");

        public string ScriptName { get; set; }
        public int LineIndex { get; set; }
        public int LineCount { get; set; }
        public int NewLineCount { get; set; }
        public string RawText { get; set; }

        /// <summary>
        /// Returns only the message text without the logic
        /// </summary>
        public string MessageText
        {
            get
            {
                string result = "";
                bool open = true;
                bool isSelectionWindow = false;
                string[] lines = RawText.Split('\n');
                foreach (string line in lines)
                {
                    if (line.StartsWith("#MESSAGE:")) continue;
                    if (isSelectionWindow)
                    {
                        if (line.StartsWith("%END:"))
                        {
                            isSelectionWindow = false;
                            continue;
                        }
                        string[] blocks = line.Split(',');
                        result += blocks[blocks.Length - 1] + "\n";
                        continue;
                    }
                    foreach (char character in line)
                    {
                        if (character == '%')
                        {
                            if (line.Contains("%SEL:"))
                            {
                                isSelectionWindow = true;
                                break;
                            }
                            open = false;
                        }

                        if (open)
                        {
                            result += character;
                        }

                        if (character == ':' && open == false) open = true;
                    }
                    if (result.Length != 0) result += "\n";
                }
                return result;
            }
        }

        public ConanScriptMessage(string rawText, int lineIndex, string scriptName)
        {
            RawText = rawText;
            LineCount = rawText.Split('\n').Length;
            LineIndex = lineIndex;
            ScriptName = scriptName;
        }

        public void Format(ScriptFile scriptFile)
        {
            StringBuilder sb = new StringBuilder();

            bool open = true;
            string[] lines = RawText.Split('\n');
            foreach (string line in lines)
            {
                if (line.StartsWith("#MESSAGE:"))
                {
                    sb.Append(line + "\n");
                    continue;
                }
                for (int i = 0; i < line.Length; i++)
                {
                    char character = line[i];
                    if (character == '%') open = false;

                    if (open)
                    {
                        bool pair = false;
                        bool found = false;
                        string neededSymbol = line[i].ToString();
                        if (i + 1 < line.Length)
                        {
                            if (scriptFile.IsValidChar(line[i + 1]))
                            {
                                neededSymbol += line[i + 1];
                                pair = true;
                            }
                            else
                            {
                                neededSymbol += " ";
                            }
                        }

                        foreach (FontCharacter fontCharacter in scriptFile.GeneratedFont)
                        {
                            if (fontCharacter.Symbol == neededSymbol)
                            {
                                byte[] bytes = BitConverter.GetBytes(fontCharacter.Index);
                                sb.Append(_shiftJis.GetString(bytes));
                                found = true;
                                if (pair) i++;
                                break;
                            }
                        }

                        if (!found)
                        {
                            //Console.WriteLine("PANIC");
                        }
                    }
                    else
                    {
                        sb.Append(character);
                    }

                    if (character == ':' && open == false) open = true;
                }
                sb.Append("\n");
            }
            RawText = sb.ToString();
            NewLineCount = RawText.Split('\n').Length;
        }

        public void DeFormat(ScriptFile scriptFile)
        {
            
        }

    }
}
