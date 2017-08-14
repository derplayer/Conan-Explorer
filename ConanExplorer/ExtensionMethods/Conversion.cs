using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.ExtensionMethods
{
    public static class Conversion
    {
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba) { hex.AppendFormat("{0:x2}", b); }
            return hex.ToString();
        }

        public static string BoolArrayToBinary(bool[] array)
        {
            StringBuilder result = new StringBuilder();
            foreach (bool value in array)
            {
                if (value)
                {
                    result.Append('1');
                }
                else
                {
                    result.Append('0');
                }
            }
            return result.ToString();
        }
    }
}
