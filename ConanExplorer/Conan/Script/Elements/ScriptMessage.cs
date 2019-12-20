using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ConanExplorer.Conan.Script.Elements
{
    public class ScriptMessage : ScriptCommand
    {
        public event EventHandler ContentChanged;

        private static readonly Regex _regexSubCommandClr = new Regex(@"%CLR:?");
        private static readonly Regex _regexSubCommand = new Regex(@"\%(\w*)(?:\((\d*|\w*)\))?\:");
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

        public int WindowCount
        {
            get
            {
                return _regexSubCommandClr.Matches(Content).Count + 1;
            }
        }

        public bool IsSelectionWindow
        {
            get
            {
                return Content.Contains("%SEL:");
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnContentChanged();
            }
        }

        public int ContentLineCount
        {
            get
            {
                return ScriptParser.TextToLines(_content).Length;
            }
        }

        public string ContentText
        {
            get
            {
                if (Content.Contains("%SEL:"))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    bool selection = false;
                    string[] lines = ScriptParser.TextToLines(Content);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("%SEL:"))
                        {
                            selection = true;
                            stringBuilder.AppendLine("");
                            continue;
                        }

                        if (line.StartsWith("%END:"))
                        {
                            selection = false;
                            stringBuilder.AppendLine("");
                            continue;
                        }

                        if (selection)
                        {
                            stringBuilder.AppendLine(line.Split(',')[2]);
                        }
                        else
                        {
                            stringBuilder.AppendLine(_regexSubCommand.Replace(line, ""));
                        }
                    }
                    return stringBuilder.ToString();
                }
                return _regexSubCommand.Replace(Content, "");
            }
        }

        public string[] ContentTextArray
        {
            get
            {
                try
                {
                    string cleanContent = _regexSubCommand.Replace(Content, "");
                    return cleanContent.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
                }
                catch (Exception e)
                {
                    return new string[] { }; //empty so that the loop skips it
                }

            }
        }

        public MatchCollection Matches
        {
            get
            {
                return _regexSubCommand.Matches(Content);
            }
        }


        public ScriptMessage(Match match, int lineIndex, bool linebreak = true) : base(match, lineIndex, linebreak) { }
    

        public void Format(ScriptFile script, bool ignoreMissing = true)
        {
            MatchCollection matches = _regexSubCommand.Matches(Content);
            StringBuilder stringBuilder = new StringBuilder();

            if (Content.Contains("%SEL:"))
            {
                bool open = false;
                string[] lines = ScriptParser.TextToLines(Content);
                for (int l = 0; l < lines.Length; l++)
                {
                    string line = lines[l];
                    if (line.StartsWith("%SEL:"))
                    {
                        stringBuilder.Append(line + "\r\n");
                        open = true;
                        continue;
                    }

                    if (line.StartsWith("%END:"))
                    {
                        if (l == lines.Length - 1)
                        {
                            stringBuilder.Append(line);
                        }
                        else
                        {
                            stringBuilder.Append(line + "\r\n");
                        }
                        open = false;
                        continue;
                    }

                    if (open)
                    {
                        string[] splitted = line.Split(',');
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
                    }
                    else
                    {
                        MatchCollection lineMatches = _regexSubCommand.Matches(line);
                        int matchIndex = 0;
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (lineMatches[matchIndex].Index == i)
                            {
                                i += lineMatches[matchIndex].Length - 1;
                                stringBuilder.Append(lineMatches[matchIndex].Value);
                                matchIndex++;
                                continue;
                            }

                            bool pair = false;
                            string neededSymbol = line[i].ToString();
                            if (i + 1 < line.Length)
                            {
                                if (script.IsValidChar(line[i + 1]))
                                {
                                    neededSymbol += line[i + 1];
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
                    }
                    stringBuilder.Append("\r\n");
                }
            }
            else
            {
                int matchIndex = 0;
                for (int i = 0; i < Content.Length; i++)
                {
                    if (Content[i] == '\n')
                    {
                        stringBuilder.Append('\n');
                        continue;
                    }

                    if (Content[i] == '\r')
                    {
                        stringBuilder.Append('\r');
                        continue;
                    }

                    if (matches[matchIndex].Index == i)
                    {
                        i += matches[matchIndex].Length - 1;
                        stringBuilder.Append(matches[matchIndex].Value);
                        matchIndex++;
                        continue;
                    }

                    bool pair = false;
                    string neededSymbol = Content[i].ToString();
                    if (i + 1 < Content.Length)
                    {
                        if (script.IsValidChar(Content[i + 1]))
                        {
                            neededSymbol += Content[i + 1];
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
            }
            Content = stringBuilder.ToString();
        }

        public void DeFormat(ScriptFile script)
        {
            MatchCollection matches = _regexSubCommand.Matches(Content);
            StringBuilder stringBuilder = new StringBuilder();

            int matchIndex = 0;
            for (int i = 0; i < Content.Length; i++)
            {
                if (Content[i] == '\n')
                {
                    stringBuilder.Append('\n');
                    continue;
                }

                if (Content[i] == '\r')
                {
                    stringBuilder.Append('\r');
                    continue;
                }

                if (matches[matchIndex].Index == i)
                {
                    i += matches[matchIndex].Length;
                    stringBuilder.Append(matches[matchIndex].Value);
                    matchIndex++;
                    continue;
                }

                string neededSymbol = Content[i].ToString();
                int fontCharacterIndex = BitConverter.ToInt16(_shiftJis.GetBytes(neededSymbol), 0);

                FontCharacter fontCharacter = script.GeneratedFont.FirstOrDefault(fc => fc.Index == fontCharacterIndex);
                if (fontCharacter == null)
                {
                    MessageBox.Show("Font character index out of range!", "DeFormat Error!");
                    return;
                }
                stringBuilder.Append(fontCharacter.Symbol);
            }
            Content = stringBuilder.ToString();
        }

        protected void OnContentChanged()
        {
            ContentChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
