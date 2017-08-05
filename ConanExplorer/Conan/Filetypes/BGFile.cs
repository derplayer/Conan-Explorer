using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Filetypes
{
    public class BGFile : BaseFile
    {
        List<Object> PackedFiles { get; set; }

        public BGFile() { }
        public BGFile(string filePath) : base(filePath)
        {
            
        }

        public void Pack()
        {

        }

        public void Unpack()
        {

        }
    }
}
