using jpsxdec.adpcm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Filetypes
{
    public class XAFile : BaseFile
    {
        private static short[] positiveXaAdpcmTable = new short[] { 0, 60, 115, 98, 122 };
        private static short[] negativeXaAdpcmTable = new short[] { 0, 0, -52, -55, -60 };

        public XAFile() { }
        public XAFile(string filePath, bool dummyTest) : base(filePath)
        {
            int iSector = 0;

            using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(filePath + ".bin", FileMode.Create)))
                {
                    //test
                    //var baseStreamWrapper = new ikvm.io.InputStreamWrapper(reader.BaseStream);
                    //java.io.OutputStream baseStreamWrapperOut = new java.io.FileOutputStream(filePath + ".raw");

                    if (reader.BaseStream.Length == 0) return;
                    int numberOfsu_s = 4;
                    List<byte> decoded = new List<byte>();
                    var test = new XaAdpcmDecoder(4, false, 100);

                    byte[] UnusedForADPCM;
                    byte[] ECC; //Error code correction is in conan there!
                    bool stopLoop = false;

                    while (true)
                    {
                        //2336 is per sector, sync value is padding?
                        var header = new ADPCMSubHeader();
                        header.Load(reader);
                        var sgs = new List<ADPCMSoundGroup>();

                        if (header.EOF == true)
                            stopLoop = true;

                        //This needs to be done 18 times (2304 bytes)                        
                        for (int sg = 0; sg < 18; sg++)
                        {
                            var soundGroup = new ADPCMSoundGroup();
                            soundGroup.Load(reader);    //conan has no sync block?
                            sgs.Add(soundGroup);
                        }

                        //Read leftover
                        UnusedForADPCM = reader.ReadBytes(20);
                        ECC = reader.ReadBytes(4);

                        reader.BaseStream.Seek(-2336, SeekOrigin.Current);
                        byte[] xaSector = reader.ReadBytes(2336);

                        short dstLeft = 0, oldLeft = 0, olderLeft = 0, dstRight = 1, oldRight = 0, olderRight = 0;

                        //deode hier
                        java.io.InputStream inStream = new java.io.ByteArrayInputStream(xaSector);
                        java.io.ByteArrayOutputStream outStream = new java.io.ByteArrayOutputStream();
                        test.decode(inStream, outStream, iSector);

                        var tmpArr = outStream.toByteArray();
                        foreach (var data in tmpArr)
                        {
                            decoded.Add(data);
                        }

                        iSector++;
                        //List<short> l = new List<short>();
                        //List<short> r = new List<short>();

                        //foreach (var sg in sgs)
                        //{
                        //    for (byte b = 0; b < 4; b++) //4bits adpcm
                        //    {
                        //        if (header.AudioMode == AudioType.Stereo)
                        //        {
                        //            l.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 1, dstLeft, ref oldLeft, ref olderLeft));
                        //            r.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 0, dstRight, ref oldRight, ref olderRight));
                        //        }
                        //        else
                        //        {
                        //            l.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 1, dstLeft, ref oldLeft, ref olderLeft));
                        //            l.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 0, dstRight, ref oldRight, ref olderRight));
                        //        }
                        //    }
                        //}

                        //hacky decode output
                        //TODO: resample to 44100 for easier edit
                        //if (header.AudioMode == AudioType.Stereo)
                        //{
                        //    for (int sample = 0; sample < l.Count; sample++)
                        //    {
                        //        decoded.Add((byte)(l[sample]));
                        //        decoded.Add((byte)(l[sample] >> 8));
                        //        decoded.Add((byte)(r[sample]));
                        //        decoded.Add((byte)(r[sample] >> 8));
                        //    }
                        //}
                        //else
                        //{
                        //    for (int sample = 0; sample < l.Count; sample++)
                        //    {
                        //        byte tmpNum = (byte)((short)((l[sample] & 15) << 12));
                        //        decoded.Add(tmpNum);
                        //        tmpNum = (byte)((short)((l[sample] & 240) << 8));
                        //        decoded.Add(tmpNum);
                        //    }
                        //}

                        if (stopLoop) break;
                    }

                    foreach (var data in decoded)
                    {
                        bw.Write(data);
                    }
                }
            }
        }
        public XAFile(string filePath) : base(filePath)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(filePath + ".bin", FileMode.Create)))
                {
                    if (reader.BaseStream.Length == 0) return;
                    int numberOfsu_s = 4;
                    List<byte> decoded = new List<byte>();

                    byte[] UnusedForADPCM;
                    byte[] ECC; //Error code correction is in conan there!
                    bool stopLoop = false;

                    while (true)
                    {
                        //2336 is per sector, sync value is padding?
                        var header = new ADPCMSubHeader();
                        header.Load(reader);
                        var sgs = new List<ADPCMSoundGroup>();

                        if (header.EOF == true)
                            stopLoop = true;

                        //This needs to be done 18 times (2304 bytes)                        
                        for (int sg = 0; sg < 18; sg++)
                        {
                            var soundGroup = new ADPCMSoundGroup();
                            soundGroup.Load(reader);    //conan has no sync block?
                            sgs.Add(soundGroup);
                        }

                        //Read leftover
                        UnusedForADPCM = reader.ReadBytes(20);
                        ECC = reader.ReadBytes(4);

                        short dstLeft = 0, oldLeft = 0, olderLeft = 0, dstRight = 1, oldRight = 0, olderRight = 0;

                        List<short> l = new List<short>();
                        List<short> r = new List<short>();

                        foreach (var sg in sgs)
                        {
                            for (byte b = 0; b < 4; b++) //4bits adpcm
                            {
                                if (header.AudioMode == AudioType.Stereo)
                                {
                                    l.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 1, dstLeft, ref oldLeft, ref olderLeft));
                                    r.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 0, dstRight, ref oldRight, ref olderRight));
                                }
                                else
                                {
                                    l.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 1, dstLeft, ref oldLeft, ref olderLeft));
                                    l.AddRange(DecodeBlock(sg.ADPCMSoundUnits, sg.SoundParameters, b, 0, dstRight, ref oldRight, ref olderRight));
                                }
                            }
                        }

                        //hacky decode output
                        //TODO: resample to 44100 for easier edit
                        if (header.AudioMode == AudioType.Stereo)
                        {
                            for (int sample = 0; sample < l.Count; sample++)
                            {
                                decoded.Add((byte)(l[sample]));
                                decoded.Add((byte)(l[sample] >> 8));
                                decoded.Add((byte)(r[sample]));
                                decoded.Add((byte)(r[sample] >> 8));
                            }
                        }
                        else
                        {
                            for (int sample = 0; sample < l.Count; sample++)
                            {
                                byte tmpNum = (byte)((short)((l[sample] & 15) << 12));
                                decoded.Add(tmpNum);
                                tmpNum = (byte)((short)((l[sample] & 240) << 8));
                                decoded.Add(tmpNum);
                            }
                        }

                        if (stopLoop) break;
                    }

                    foreach (var data in decoded)
                    {
                        bw.Write(data);
                    }

                }
            }
        }

        private List<short> DecodeBlock(byte[] samples, byte[] parameters, byte block, byte nibble, short dst, ref short old, ref short older)
        {
            List<short> list = new List<short>();
            byte shift = (byte)(12 - (parameters[4 + block * 2 + nibble] & 0xF));
            sbyte filter = (sbyte)((parameters[4 + block * 2 + nibble] & 0x30) >> 4);

            short f0 = positiveXaAdpcmTable[filter];
            short f1 = negativeXaAdpcmTable[filter];

            for (int d = 0; d < 28; d++)
            {
                //Signed4bit((byte)((samples[block + d * 4] >> (nibble * 4)) & 0xF));
                sbyte t = Signed4bit((byte)((samples[block + d * 4] >> (nibble * 4)) & 0xF));
                short s = (short)((t << shift) + ((old * f0 + older * f1 + 32) / 64));

                short sample = (short)MinMax(s, -0x8000, 0x7FFF);

                list.Add(sample);
                older = old;
                old = sample;
            }

            return list;
        }

        private int MinMax(int number, int min, int max)
        {
            if (number < min)
                return min;
            if (number > max)
                return max;

            return number;
        }

        private sbyte Signed4bit(byte number)
        {
            if ((number & 0x8) == 0x8)
                return (sbyte)((number & 0x7) - 8);
            else
                return (sbyte)number;
        }

    }

    public class ADPCMSoundGroup
    {
        public ADPCMSoundGroup() { }

        public byte[] SoundParameters { get; set; }
        public byte[] ADPCMSoundUnits { get; set; }

        public bool Load(BinaryReader reader)
        {
            SoundParameters = reader.ReadBytes(16);
            ADPCMSoundUnits = reader.ReadBytes(112);

            return true;
        }

    }

    public enum BPSType
    {
        Bit4,   //level B/C
        Bit8    //level A
    }

    public enum SampleRateType
    {
        Hz37,     //37.8kHz, level A/B
        Hz18      //18.9kHz, level C
    }

    public enum AudioType
    {
        Mono,
        Stereo,
        Reserved
    }

    public class ADPCMSubHeader
    {
        public byte FileNumber { get; set; }
        public byte ChannelNumber { get; set; }     //ADPCM data can only be up to 16 channels (0-15)

        public bool EOF { get; set; }               //is set when last sector of one file appears
        public bool RealTimeSector { get; set; }
        public bool From { get; set; }
        public bool Trigger { get; set; }
        public bool Data { get; set; }
        public bool Audio { get; set; }
        public bool Video { get; set; }
        public bool EOR { get; set; }               //end of record

        public bool Emphasis { get; set; }          //wave-filter, not used in any known game
        public BPSType BPS { get; set; }
        public SampleRateType SampleRate { get; set; }
        public AudioType AudioMode { get; set; }

        public ADPCMSubHeader() { }

        public bool Load(BinaryReader reader)
        {
            FileNumber = reader.ReadByte();
            ChannelNumber = reader.ReadByte();

            byte subMode = reader.ReadByte();
            BitArray subModeBits = new BitArray(new byte[] { subMode });
            EOF = subModeBits[7];
            RealTimeSector = subModeBits[6];
            From = subModeBits[5];
            Trigger = subModeBits[4];
            Data = subModeBits[3];
            Audio = subModeBits[2];
            Video = subModeBits[1];
            EOR = subModeBits[0];

            byte codingInfo = reader.ReadByte();
            BitArray codingInfoBits = new BitArray(new byte[] { codingInfo });
            Emphasis = codingInfoBits[6];

            if (codingInfoBits[4])
                BPS = BPSType.Bit8;
            else
                BPS = BPSType.Bit4;

            if (codingInfoBits[2])
                SampleRate = SampleRateType.Hz18;
            else
                SampleRate = SampleRateType.Hz37;

            if (codingInfoBits[0])
                AudioMode = AudioType.Stereo;
            else
                AudioMode = AudioType.Mono;

            //???
            var FileNumberCopy = reader.ReadByte();
            var ChannelCopy = reader.ReadByte();
            var SubmodeCopy = reader.ReadByte();
            var CodinginfoCopy = reader.ReadByte();

            return true;
        }

        public static bool GetBit(byte pByte, int bitNo)
        {
            return (pByte & (1 << bitNo)) != 0;
        }
    }


}
