using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Script
{
    public class ScriptCollection
    {
        public BindingList<ScriptDocument> Scripts { get; set; } = new BindingList<ScriptDocument>();
    }
}
