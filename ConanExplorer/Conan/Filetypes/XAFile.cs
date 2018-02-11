using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Filetypes
{
    /// <summary>
    /// [WIP]
    /// </summary>
    public class XAFile : BaseFile
    {
        public XAFile() { }
        public XAFile(string filePath) : base(filePath)
        {

        }

    }

    public class ADPCMSoundGroup
    {
        public ADPCMSoundGroup() { }


    }

    public class ADPCMSubHeader
    {
        public byte FileNumber { get; set; }
        public byte ChannelNumber { get; set; }
  
        public bool EOF { get; set; }
        public bool RealTimeSector { get; set; }
        public bool From { get; set; }
        public bool Trigger { get; set; }
        public bool Data { get; set; }
        public bool Audio { get; set; }
        public bool Video { get; set; }
        public bool EOR { get; set; }

        public bool Emphasis { get; set; }
        public bool BPS { get; set; }
        public bool SampleRate { get; set; }
        public bool Stereo { get; set; }

        public ADPCMSubHeader() {}

        public bool Load(BinaryReader reader)
        {
            FileNumber = reader.ReadByte();
            ChannelNumber = reader.ReadByte();

            return true;
        }
    }


}
