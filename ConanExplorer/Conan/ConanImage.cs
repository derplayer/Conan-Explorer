using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan
{
    /// <summary>
    /// Contains the file list of the original Meitantei Conan (SLPS_01690)
    /// </summary>
    public static class ConanImage
    {
        /// <summary>
        /// Checks the ripped image for having all the needed files
        /// </summary>
        /// <param name="rootDirectory">Directory path of the image</param>
        /// <param name="checkSize">True for also checking if the files are the correct size</param>
        /// <returns></returns>
        public static bool Check(string rootDirectory, bool checkSize = false)
        {
            foreach (string directory in Directories)
            {
                if (!Directory.Exists(Path.Combine(rootDirectory, directory)))
                {
                    return false;
                }
            }

            foreach (ConanImageFile file in Files)
            {
                if (!File.Exists(Path.Combine(rootDirectory, file.FilePath)))
                {
                    return false;
                }
                if (!checkSize) continue;
                FileInfo fileInfo = new FileInfo(Path.Combine(rootDirectory, file.FilePath));
                if (fileInfo.Length != file.FileSize)
                {
                    return false;
                }
            }
            return true;
        }

        public static readonly ConanImageDialogue[] Dialogues =
        {
            new ConanImageDialogue(0x00000C9C, 0x12, "最初から始めます。"),
            new ConanImageDialogue(0x00000CB0, 0x10, "よろしいですか？"),
            new ConanImageDialogue(0x000A6E90, 0x4, "はい"),
            new ConanImageDialogue(0x000A6E98, 0x6, "いいえ")
        };

        public static readonly string[] Directories =
        {
            "BG",
            "GRAPH",
            "ITEM",
            "MAP1",
            "MAP2",
            "REACT",
            "SCRIPT",
            "SD",
            "SOUND",
            "VOICE1",
            "VOICE2",
            "VOICE3",
            "XSTR"
        };

        public static readonly ConanImageFile[] PKNFiles =
        {
            new ConanImageFile("BG\\BG.PKN", 3022848),
            new ConanImageFile("GRAPH\\GRAPH.PKN", 2910208),
            new ConanImageFile("ITEM\\ITEM.PKN", 307200),
            new ConanImageFile("MAP1\\MAP1.PKN", 4022272),
            new ConanImageFile("MAP2\\MAP2.PKN", 1288192),
            new ConanImageFile("REACT\\REACT.PKN", 2379776),
            new ConanImageFile("SCRIPT\\SCRIPT.PKN", 350208),
            new ConanImageFile("SD\\SD.PKN", 409600),
            new ConanImageFile("SOUND\\SOUND.PKN", 7948288)
        };

        public static readonly ConanImageFile[] Files = 
        {
            new ConanImageFile("DUMMY.DAT", 32768000),
            new ConanImageFile("SLPS_016.90", 686080),
            new ConanImageFile("SYSTEM.CNF", 68),

            new ConanImageFile("BG\\BG.PKN", 3022848),
            new ConanImageFile("GRAPH\\GRAPH.PKN", 2910208),
            new ConanImageFile("ITEM\\ITEM.PKN", 307200),
            new ConanImageFile("MAP1\\MAP1.PKN", 4022272),
            new ConanImageFile("MAP2\\MAP2.PKN", 1288192),
            new ConanImageFile("REACT\\REACT.PKN", 2379776),
            new ConanImageFile("SCRIPT\\SCRIPT.PKN", 350208),
            new ConanImageFile("SD\\SD.PKN", 409600),
            new ConanImageFile("SOUND\\SOUND.PKN", 7948288),

            new ConanImageFile("VOICE1\\ADD00.XA", 1048576),
            new ConanImageFile("VOICE1\\ADD01.XA", 1441792),
            new ConanImageFile("VOICE1\\ADD02.XA", 1769472),
            new ConanImageFile("VOICE1\\ADD03.XA", 2359296),
            new ConanImageFile("VOICE1\\ADD04.XA", 3211264),
            new ConanImageFile("VOICE1\\MUJIN00.XA", 393216),
            new ConanImageFile("VOICE1\\MUJIN01.XA", 458752),
            new ConanImageFile("VOICE1\\MUJIN02.XA", 524288),
            new ConanImageFile("VOICE1\\MUJIN03.XA", 655360),
            new ConanImageFile("VOICE1\\MUJIN04.XA", 655360),
            new ConanImageFile("VOICE1\\MUJIN05.XA", 720896),
            new ConanImageFile("VOICE1\\MUJIN06.XA", 786432),
            new ConanImageFile("VOICE1\\MUJIN07.XA", 851968),
            new ConanImageFile("VOICE1\\MUJIN08.XA", 1179648),
            new ConanImageFile("VOICE1\\MUJIN09.XA", 983040),
            new ConanImageFile("VOICE1\\MUJIN10.XA", 1048576),
            new ConanImageFile("VOICE1\\MUJIN11.XA", 1114112),
            new ConanImageFile("VOICE1\\MUJIN12.XA", 1179648),
            new ConanImageFile("VOICE1\\MUJIN13.XA", 1310720),
            new ConanImageFile("VOICE1\\MUJIN14.XA", 1376256),
            new ConanImageFile("VOICE1\\MUJIN15.XA", 1441792),
            new ConanImageFile("VOICE1\\MUJIN16.XA", 1572864),
            new ConanImageFile("VOICE1\\MUJIN17.XA", 1638400),
            new ConanImageFile("VOICE1\\MUJIN18.XA", 1769472),
            new ConanImageFile("VOICE1\\MUJIN19.XA", 1900544),
            new ConanImageFile("VOICE1\\MUJIN20.XA", 2097152),
            new ConanImageFile("VOICE1\\MUJIN21.XA", 2359296),
            new ConanImageFile("VOICE1\\MUJIN22.XA", 3211264),
            new ConanImageFile("VOICE1\\SEXA00.XA", 1966080),
            new ConanImageFile("VOICE1\\SEXA01.XA", 8060928),

            new ConanImageFile("VOICE2\\LADY100.XA", 393216),
            new ConanImageFile("VOICE2\\LADY101.XA", 458752),
            new ConanImageFile("VOICE2\\LADY102.XA", 524288),
            new ConanImageFile("VOICE2\\LADY103.XA", 589824),
            new ConanImageFile("VOICE2\\LADY104.XA", 655360),
            new ConanImageFile("VOICE2\\LADY105.XA", 720896),
            new ConanImageFile("VOICE2\\LADY106.XA", 786432),
            new ConanImageFile("VOICE2\\LADY107.XA", 851968),
            new ConanImageFile("VOICE2\\LADY108.XA", 1114112),
            new ConanImageFile("VOICE2\\LADY109.XA", 983040),
            new ConanImageFile("VOICE2\\LADY110.XA", 1048576),
            new ConanImageFile("VOICE2\\LADY111.XA", 1114112),
            new ConanImageFile("VOICE2\\LADY112.XA", 1179648),
            new ConanImageFile("VOICE2\\LADY113.XA", 1245184),
            new ConanImageFile("VOICE2\\LADY114.XA", 1310720),
            new ConanImageFile("VOICE2\\LADY115.XA", 1441792),
            new ConanImageFile("VOICE2\\LADY116.XA", 1507328),
            new ConanImageFile("VOICE2\\LADY117.XA", 1507328),
            new ConanImageFile("VOICE2\\LADY118.XA", 1572864),
            new ConanImageFile("VOICE2\\LADY119.XA", 1703936),
            new ConanImageFile("VOICE2\\LADY120.XA", 1769472),
            new ConanImageFile("VOICE2\\LADY121.XA", 1835008),
            new ConanImageFile("VOICE2\\LADY122.XA", 1900544),
            new ConanImageFile("VOICE2\\LADY123.XA", 2031616),
            new ConanImageFile("VOICE2\\LADY124.XA", 2162688),
            new ConanImageFile("VOICE2\\LADY125.XA", 2359296),
            new ConanImageFile("VOICE2\\LADY126.XA", 2686976),
            new ConanImageFile("VOICE2\\LADY127.XA", 3735552),

            new ConanImageFile("VOICE3\\LADY200.XA", 327680),
            new ConanImageFile("VOICE3\\LADY202.XA", 458752),
            new ConanImageFile("VOICE3\\LADY201.XA", 524288),
            new ConanImageFile("VOICE3\\LADY203.XA", 655360),
            new ConanImageFile("VOICE3\\LADY204.XA", 720896),
            new ConanImageFile("VOICE3\\LADY205.XA", 786432),
            new ConanImageFile("VOICE3\\LADY206.XA", 851968),
            new ConanImageFile("VOICE3\\LADY207.XA", 917504),
            new ConanImageFile("VOICE3\\LADY208.XA", 983040),
            new ConanImageFile("VOICE3\\LADY209.XA", 1310720),
            new ConanImageFile("VOICE3\\LADY210.XA", 1114112),
            new ConanImageFile("VOICE3\\LADY211.XA", 1179648),
            new ConanImageFile("VOICE3\\LADY212.XA", 1245184),
            new ConanImageFile("VOICE3\\LADY213.XA", 1310720),
            new ConanImageFile("VOICE3\\LADY214.XA", 1376256),
            new ConanImageFile("VOICE3\\LADY215.XA", 1441792),
            new ConanImageFile("VOICE3\\LADY216.XA", 1507328),
            new ConanImageFile("VOICE3\\LADY217.XA", 1572864),
            new ConanImageFile("VOICE3\\LADY218.XA", 1638400),
            new ConanImageFile("VOICE3\\LADY219.XA", 1703936),
            new ConanImageFile("VOICE3\\LADY220.XA", 1835008),
            new ConanImageFile("VOICE3\\LADY221.XA", 1900544),
            new ConanImageFile("VOICE3\\LADY222.XA", 2031616),
            new ConanImageFile("VOICE3\\LADY223.XA", 2097152),
            new ConanImageFile("VOICE3\\LADY224.XA", 2228224),
            new ConanImageFile("VOICE3\\LADY225.XA", 2424832),
            new ConanImageFile("VOICE3\\LADY226.XA", 2686976),
            new ConanImageFile("VOICE3\\LADY227.XA", 3211264),

            new ConanImageFile("XSTR\\J1.STR", 24623104),
            new ConanImageFile("XSTR\\J2.STR", 40339456),
            new ConanImageFile("XSTR\\J3.STR", 13019136),
            new ConanImageFile("XSTR\\J4.STR", 21833728),
            new ConanImageFile("XSTR\\J5.STR", 38750208),
            new ConanImageFile("XSTR\\J6.STR", 17119232),
            new ConanImageFile("XSTR\\M01.STR", 20484096),
            new ConanImageFile("XSTR\\M02.STR", 18616320),
            new ConanImageFile("XSTR\\O01.STR", 11327488),
            new ConanImageFile("XSTR\\O02.STR", 11405312),
            new ConanImageFile("XSTR\\OV.STR", 1540096),
            new ConanImageFile("XSTR\\XBANDAI.STR", 1851392)
        };


    }


    public class ConanImageFile
    {
        public string FilePath;
        public long FileSize;
        public bool Critical;

        public ConanImageFile(string filePath, long fileSize)
        {
            FilePath = filePath;
            FileSize = fileSize;
        }
    }

    public class ConanImageDialogue
    {
        public int Offset;
        public int Length;
        public string OriginalString;

        public ConanImageDialogue(int offset, int length, string originalString)
        {
            Offset = offset;
            Length = length;
            OriginalString = originalString;
        }
    }
}
