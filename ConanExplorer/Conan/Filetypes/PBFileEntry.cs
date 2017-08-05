using ConanExplorer.Conan.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Filetypes
{
    public class PBFileEntry
    {
        public PACKFileHeader Header { get; set; }
        public BaseFile File { get; set; }

        public PBFileEntry() { }

        public PBFileEntry(PACKFileHeader header, BaseFile file)
        {
            Header = header;
            File = file;
        }
    }
}
