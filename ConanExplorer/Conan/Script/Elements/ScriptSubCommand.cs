using ConanExplorer.Conan.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Script.Elements
{
    public class ScriptSubCommand : IScriptElement
    {
        public string Text
        {
            get
            {
                if (!HasParameter) return String.Format("%{0}:", Name);
                return String.Format("%{0}({1}):", Name, Parameter);
            }
        }

        public int Length
        {
            get { return Text.Length; }
        }

        public string Name { get; }
        public string DisplayName
        {
            get { return Name; }
        }
        public string Parameter { get; set; }
        public bool HasParameter { get; }

        public ScriptSubCommand(Match match)
        {
            Name = match.Groups[1].Value;
            if (match.Groups.Count == 2) return;
            HasParameter = true;
            Parameter = match.Groups[2].Value;
        }
    }
}
