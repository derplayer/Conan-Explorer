using ConanExplorer.Conan.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Script.Elements
{
    public class ScriptCommand : IScriptElement
    {
        protected bool _linebreak;
        public virtual string Text
        {
            get
            {
                if (_linebreak)
                {
                    return String.Format("#{0}:{1}\r\n", Name, String.Join(",", Parameters));
                }
                return String.Format("#{0}:{1}", Name, String.Join(",", Parameters));
            }
        }
        public int Length
        {
            get { return Text.Length; }
        }

        public string Name { get; }
        public string DisplayName
        {
            get { return String.Format("#{0}:{1}", Name, String.Join(",", Parameters)); }
        }
        public string[] Parameters { get; set; }

        public ScriptCommand(Match match, bool linebreak = true)
        {
            _linebreak = linebreak;
            Name = match.Groups[1].Value;
            Parameters = match.Groups[2].Value.Split(',');
        }
    }
}
