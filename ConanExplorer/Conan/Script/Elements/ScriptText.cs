using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Script.Elements
{
    public class ScriptText : IScriptElement
    {
        private bool _linebreak;
        public string Text
        {
            get
            {
                if (_linebreak) return Content + "\r\n";
                return Content;
            }
        }
        public string Content { get; set; }
        public string DisplayName
        {
            get { return "Text"; }
        }
        public int Length
        {
            get { return Text.Length; }
        }

        public ScriptText(string text, bool linebreak = true)
        {
            Content = text;
            _linebreak = linebreak;
        }
    }
}
