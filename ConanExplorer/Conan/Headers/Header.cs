using System;
using System.IO;

namespace ConanExplorer.Conan.Headers
{
    public class Header
    {
        public string Name;
        public string Description;
        public string Extension;
        public byte[] Signature;
        public Type FileType;

        public bool Compare(byte[] buffer)
        {
            if (buffer.Length < Signature.Length) return false;
            for (int i = 0; i < Signature.Length; i++)
            {
                if (buffer[i] != Signature[i]) return false;
            }
            return true;
        }

        public bool Compare(string fileName)
        {
            if (Path.GetExtension(fileName).Replace(".", "") == Extension) return true;
            return false;
        }
    }
}
