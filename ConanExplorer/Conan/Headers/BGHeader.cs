using ConanExplorer.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Headers
{
    /// <summary>
    /// BG file header class.
    /// Not fully understand the index repetition and the mask(?) for that but it works.
    /// </summary>
    public class BGHeader
    {
        /// <summary>
        /// Header signature.
        /// </summary>
        public static byte[] Signature = new byte[] { 0x42, 0x55, 0x2D, 0x4D, 0x41, 0x50, 0x30, 0x00 };
        /// <summary>
        /// Weird part of the header i have no description yet for.
        /// </summary>
        public byte[] Data { get; set; } = new byte[16];
        /// <summary>
        /// All indices including the repeated ones.
        /// </summary>
        public int[] Indices { get; set; } = new int[10];
        /// <summary>
        /// Indices excluding the repeated indices.
        /// </summary>
        public List<int> RealIndices
        {
            get
            {
                bool[] mask = IndicesMask;
                if (mask.Length != 10) return new List<int>();

                List<int> indices = new List<int>();
                for (int i = 0; i < 10; i++)
                {
                    if (mask[i])
                    {
                        indices.Add(Indices[i]);
                    }
                }
                return indices;
            }
            set
            {
                if (value.Count != FileCount) return;

                int index = -1;
                bool[] mask = IndicesMask;
                for (int i = 0; i < 10; i++)
                {
                    if (mask[i])
                    {
                        index++;
                        Indices[i] = value[index];
                    }
                    else
                    {
                        Indices[i] = value[index];
                    }
                }
            }
        }
        /// <summary>
        /// Gets the repetition mask based on some magic bullshit.
        /// True = Normal
        /// False = Repetition
        /// </summary>
        public bool[] IndicesMask
        {
            get
            {
                List<bool> mask = new List<bool>();
                switch (Data[1])
                {
                    case 0:
                        mask.AddRange(new bool[] { true, true, false, false });
                        break;
                    case 1:
                        mask.AddRange(new bool[] { true, true, true, false });
                        break;
                    case 2:
                        mask.AddRange(new bool[] { true, true, false, true });
                        break;
                    case 3:
                        mask.AddRange(new bool[] { true, true, true, true });
                        break;
                }
                switch (Data[8])
                {
                    case 1:
                        mask.AddRange(new bool[] { true, false, false, false, false });
                        break;
                    case 2:
                        mask.AddRange(new bool[] { true, true, false, false, false });
                        break;
                    case 3:
                        mask.AddRange(new bool[] { true, true, true, false, false });
                        break;
                    case 4:
                        mask.AddRange(new bool[] { true, true, true, true, false });
                        break;
                }
                switch (Data[9])
                {
                    case 0:
                        mask.Add(false);
                        break;
                    case 4:
                        mask.Add(true);
                        break;
                }
                return mask.ToArray();
            }
        }
        /// <summary>
        /// File count.
        /// </summary>
        public int FileCount
        {
            get
            {
                int count = 0;
                foreach (bool value in IndicesMask)
                {
                    if(value) count++;
                }
                return count;
            }
        }


        /// <summary>
        /// Loads the BG header from a buffer.
        /// </summary>
        /// <param name="data">Buffer</param>
        /// <returns></returns>
        public bool Load(byte[] data)
        {
            if (data.Length != 64) return false;
            byte[] signature = new byte[8];
            Array.Copy(data, 0, signature, 0, 8);
            if (!signature.SequenceEqual(Signature)) return false;

            Array.Copy(data, 8, Data, 0, 16);

            for (int i = 0; i < 10; i++)
            {
                Indices[i] = BitConverter.ToInt32(data, 24 + i * 4);
            }

            return true;
        }

        /// <summary>
        /// Gets the bytes for writing the BG header.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] result = new byte[64];
            Array.Copy(Signature, 0, result, 0, 8);
            Array.Copy(Data, 0, result, 8, 16);

            for (int i = 0; i < 10; i++)
            {
                byte[] bytes = BitConverter.GetBytes(Indices[i]);
                Array.Copy(bytes, 0, result, 24 + i * 4, 4);
            }

            return result;
        }

    }
}
