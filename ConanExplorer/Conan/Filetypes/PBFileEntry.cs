using ConanExplorer.Conan.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Filetypes
{
    /// <summary>
    /// File entry of the PB file format
    /// </summary>
    public class PBFileEntry
    {
        /// <summary>
        /// Sub-Header
        /// </summary>
        public PBFileHeader Header { get; set; }

        /// <summary>
        /// Extracted file that belongs to this entry
        /// </summary>
        public BaseFile File { get; set; }

        public PBFileEntry() { }

        public PBFileEntry(PBFileHeader header, BaseFile file)
        {
            Header = header;
            File = file;
        }
    }
}
