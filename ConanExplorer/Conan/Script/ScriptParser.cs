using ConanExplorer.Conan.Script.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Script
{
    public static class ScriptParser
    {
        private static readonly Regex _regexCommand = new Regex(@"\#(.*?)\:([\d\w\,\.\=\<\>\+]*)");

        public static string[] TextToLines(string text)
        {
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\r')
                {
                    if (i + 1 == text.Length) continue;
                    if (text[i + 1] == '\n')
                    {
                        lines.Add(sb.ToString());
                        sb.Clear();
                        i++;
                    }
                }
                else
                {
                    sb.Append(text[i]);
                }
            }
            if (sb.Length != 0) lines.Add(sb.ToString());
            return lines.ToArray();
        }

        public static void Parse(ScriptDocument script, List<IScriptElement> elements)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (IScriptElement element in elements)
            {
                stringBuilder.Append(element.Text);
            }
            script.TextBuffer = stringBuilder.ToString();
        }

        public static List<IScriptElement> Parse(ScriptDocument script)
        {
            List<IScriptElement> result = new List<IScriptElement>();

            //add linebreak to text property to keep content linebreak free
            //last line linebreak only works with scripttext for now

            bool isMessage = false;
            ScriptMessage message = null;

            string[] lines;
            if (script.TextBuffer.Contains("\r"))
            {
                lines = TextToLines(script.TextBuffer);
            }
            else
            {
                lines = script.TextBuffer.Split('\n'); //fix for XML serialization fucking up CR LF
            }
            for (int i = 0; i < lines.Length; i++)
            {
                bool eof = (i == lines.Length - 1);
                string line = lines[i];
                if (String.IsNullOrEmpty(line))
                {
                    isMessage = false;
                    message = null;

                    result.Add(new ScriptText(line, !eof));
                    continue;
                }

                if (isMessage)
                {
                    if (line.EndsWith("%ME:") || line.EndsWith("%END:"))
                    {
                        message.Content += line;
                        isMessage = false;
                        message = null;
                    }
                    else
                    {
                        message.Content += line + "\r\n";
                    }
                    continue;
                }

                Match matchCommand = _regexCommand.Match(line);
                if (matchCommand.Success)
                {
                    if (matchCommand.Index != 0)
                    {
                        result.Add(new ScriptText(line.Substring(0, matchCommand.Index)));
                        //add flag to command for not doing line break
                    }
                    if (matchCommand.Groups[1].Value == "MESSAGE")
                    {
                        isMessage = true;
                        message = new ScriptMessage(matchCommand);
                        result.Add(message);
                        continue;
                    }
                    
                    if (matchCommand.Index + matchCommand.Length != line.Length)
                    {
                        result.Add(new ScriptCommand(matchCommand, false));
                        int index = matchCommand.Index + matchCommand.Length;
                        result.Add(new ScriptText(line.Substring(index, line.Length - index), !eof));
                    }
                    else
                    {
                        result.Add(new ScriptCommand(matchCommand));
                    }
                }
                else
                {
                    result.Add(new ScriptText(line, !eof));
                }
            }
            return result;
        }
    }
}
