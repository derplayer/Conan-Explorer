using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Script.Elements
{
    public interface IScriptElement
    {
        string Text { get; }
        int Length { get; }
        string DisplayName { get; }
    }
}
