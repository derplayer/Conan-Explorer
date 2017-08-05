using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan
{
    public class ConanPath
    {
        public string RelativePath { get; set; }
        [XmlIgnore]
        public string RootPath { get; set; }
        [XmlIgnore]
        public string FullPath
        {
            get { return Path.Combine(RootPath, RelativePath); }
        }
    }
}
