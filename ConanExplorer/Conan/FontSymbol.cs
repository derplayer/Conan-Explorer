using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan
{
    public class FontSymbol
    {
        public char Character1 { get; set; }
        public char Character2 { get; set; }

        /// <summary>
        /// True if symbol consists of two characters
        /// </summary>
        public bool Splitted { get; private set; }

        public string Symbol
        {
            get
            {
                if (Splitted) return "" + Character1 + Character2;
                return "" + Character1;
            }
        }

        public FontSymbol(char char1)
        {
            Character1 = char1;
            Splitted = false;
        }

        public FontSymbol(char char1, char char2)
        {
            Character1 = char1;
            Character2 = char2;
            Splitted = true;
        }
    }
}
