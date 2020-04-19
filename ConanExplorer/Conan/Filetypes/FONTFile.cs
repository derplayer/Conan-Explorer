﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan.Filetypes
{
    /// <summary>
    /// FONT.BIN file class
    /// </summary>
    public class FONTFile : BaseFile
    {
        private readonly int _originalSize = 0x1b690;

        private readonly int _indexLength = 0x19CC;
        private readonly int _startOffset = 0x19D0;
        private readonly int _startLength = 0x19CC0;

        /// <summary>
        /// List of all the characters inside the FONT file
        /// </summary>
        [XmlIgnore]
        public List<FontCharacter> Characters = new List<FontCharacter>();

        public FONTFile() { }
        public FONTFile(string filePath) : base(filePath) { }

        /// <summary>
        /// Loads the characters of the FONT file.
        /// </summary>
        /// <param name="useMetadata"></param>
        /// <returns></returns>
        public bool Load(bool useMetadata = false)
        {
            if (!File.Exists(FilePath)) return false;
            
            if (File.Exists(FilePath + ".INFO") && useMetadata)
            {
                using (StreamReader reader = new StreamReader(new FileStream(FilePath + ".INFO", FileMode.Open)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<FontCharacter>));
                    Characters = (List<FontCharacter>)serializer.Deserialize(reader);
                }
            }
            else
            {
                byte[] buffer = new byte[_originalSize];
                using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
                {
                    if (reader.BaseStream.Length != _originalSize) return false;
                    reader.Read(buffer, 0, _originalSize);
                }

                Encoding encoder = Encoding.GetEncoding("shift_jis");
                Characters.Clear();
                for (int i = 0; i < 6604; i += 2)
                {
                    byte[] data = new byte[32];
                    short index = BitConverter.ToInt16(buffer, i);

                    Array.Copy(buffer, 6608 + i / 2 * 32, data, 0, 32);

                    FontCharacter character = new FontCharacter(data, index, encoder.GetString(BitConverter.GetBytes(index)));
                    Characters.Add(character);
                }
            }
            return true;
        }

        /// <summary>
        /// Generates characters from the given dictionary and font.
        /// </summary>
        /// <param name="dictionary">Dictionary containing max 2 chars per string.</param>
        /// <param name="font">.NET font</param>
        /// <returns></returns>
        public bool Generate(string[] dictionary, Font font)
        {
            foreach (FontCharacter fontCharacter in Characters) { fontCharacter.Data = new byte[32]; }
            for (int i = 0; i < dictionary.Length; i++)
            {
                FontCharacter fontCharacter = Characters[i];
                fontCharacter.Symbol = dictionary[i];

                Bitmap bitmap = fontCharacter.GetBitmap();
                using (Graphics graphic = Graphics.FromImage(bitmap))
                {
                    RectangleF firstChar = new RectangleF(-2, 0, 16, 16);
                    RectangleF secondChar = new RectangleF(6, 0, 16, 16);

                    graphic.SmoothingMode = SmoothingMode.None;
                    graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphic.PixelOffsetMode = PixelOffsetMode.None;
                    graphic.DrawString("" + fontCharacter.Symbol[0], font, Brushes.White, firstChar);
                    if (fontCharacter.Symbol.Length == 1) continue; //should not appear actually
                    graphic.DrawString("" + fontCharacter.Symbol[1], font, Brushes.White, secondChar);
                }
                fontCharacter.SetBitmap(bitmap);
            }
            return true;
        }

        /// <summary>
        /// Writes the characters into the FONT file.
        /// </summary>
        /// <param name="keepMetadata"></param>
        /// <returns></returns>
        public bool Save(bool keepMetadata = true)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.Create)))
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    FontCharacter fontCharacter = Characters[i];
                    writer.Seek(i*2, SeekOrigin.Begin);
                    writer.Write(BitConverter.GetBytes(fontCharacter.Index));
                    writer.Seek(6608 + i*32, SeekOrigin.Begin);
                    writer.Write(fontCharacter.Data);
                }
            }

            if (!keepMetadata) return true;
            using (StreamWriter writer = new StreamWriter(new FileStream(FilePath + ".INFO", FileMode.Create)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<FontCharacter>));
                serializer.Serialize(writer, Characters);
            }
            return true;
        }

        /// <summary>
        /// Returns a new list of empty characters with the predetermined indices.
        /// </summary>
        /// <returns></returns>
        public static List<FontCharacter> EmptyFontCharacters()
        {
            return FontIndices.Select(index => new FontCharacter((short) index)).ToList();
        }

        /// <summary>
        /// Predetermined indices.
        /// Those are hardcoded and can not be changed.
        /// </summary>
        public static ushort[] FontIndices =
        {
            0x4081, 0x4181, 0x4281, 0x4381, 0x4481, 0x4581, 0x4681, 0x4781, 0x4881, 0x4981, 0x4A81, 0x4B81, 0x4C81, 0x4D81, 0x4E81, 0x4F81,
            0x5081, 0x5181, 0x5281, 0x5381, 0x5481, 0x5581, 0x5681, 0x5781, 0x5881, 0x5981, 0x5A81, 0x5B81, 0x5C81, 0x5D81, 0x5E81, 0x5F81,
            0x6081, 0x6181, 0x6281, 0x6381, 0x6481, 0x6581, 0x6681, 0x6781, 0x6881, 0x6981, 0x6A81, 0x6B81, 0x6C81, 0x6D81, 0x6E81, 0x6F81,
            0x7081, 0x7181, 0x7281, 0x7381, 0x7481, 0x7581, 0x7681, 0x7781, 0x7881, 0x7981, 0x7A81, 0x7B81, 0x7C81, 0x7D81, 0x7E81, 0x8081,
            0x8181, 0x8281, 0x8381, 0x8481, 0x8581, 0x8681, 0x8781, 0x8881, 0x8981, 0x8A81, 0x8B81, 0x8C81, 0x8D81, 0x8E81, 0x8F81, 0x9081,
            0x9181, 0x9281, 0x9381, 0x9481, 0x9581, 0x9681, 0x9781, 0x9981, 0x9A81, 0x9B81, 0x9C81, 0x9D81, 0x9E81, 0x9F81, 0xA081, 0xA181,
            0xA281, 0xA381, 0xA481, 0xA581, 0xA681, 0xA781, 0xA881, 0xA981, 0xAA81, 0xAB81, 0x4F82, 0x5082, 0x5182, 0x5282, 0x5382, 0x5482,
            0x5582, 0x5682, 0x5782, 0x5882, 0x6082, 0x6182, 0x6282, 0x6382, 0x6482, 0x6582, 0x6682, 0x6782, 0x6882, 0x6982, 0x6A82, 0x6B82,
            0x6C82, 0x6D82, 0x6E82, 0x6F82, 0x7082, 0x7182, 0x7282, 0x7382, 0x7482, 0x7582, 0x7682, 0x7782, 0x7882, 0x7982, 0x8182, 0x8282,
            0x8382, 0x8482, 0x8582, 0x8682, 0x8782, 0x8882, 0x8982, 0x8A82, 0x8B82, 0x8C82, 0x8D82, 0x8E82, 0x8F82, 0x9082, 0x9182, 0x9282,
            0x9382, 0x9482, 0x9582, 0x9682, 0x9782, 0x9882, 0x9982, 0x9A82, 0x9F82, 0xA082, 0xA182, 0xA282, 0xA382, 0xA482, 0xA582, 0xA682,
            0xA782, 0xA882, 0xA982, 0xAA82, 0xAB82, 0xAC82, 0xAD82, 0xAE82, 0xAF82, 0xB082, 0xB182, 0xB282, 0xB382, 0xB482, 0xB582, 0xB682,
            0xB782, 0xB882, 0xB982, 0xBA82, 0xBB82, 0xBC82, 0xBD82, 0xBE82, 0xBF82, 0xC082, 0xC182, 0xC282, 0xC382, 0xC482, 0xC582, 0xC682,
            0xC782, 0xC882, 0xC982, 0xCA82, 0xCB82, 0xCC82, 0xCD82, 0xCE82, 0xCF82, 0xD082, 0xD182, 0xD282, 0xD382, 0xD482, 0xD582, 0xD682,
            0xD782, 0xD882, 0xD982, 0xDA82, 0xDB82, 0xDC82, 0xDD82, 0xDE82, 0xDF82, 0xE082, 0xE182, 0xE282, 0xE382, 0xE482, 0xE582, 0xE682,
            0xE782, 0xE882, 0xE982, 0xEA82, 0xEB82, 0xEC82, 0xED82, 0xEE82, 0xEF82, 0xF082, 0xF182, 0x4083, 0x4183, 0x4283, 0x4383, 0x4483,
            0x4583, 0x4683, 0x4783, 0x4883, 0x4983, 0x4A83, 0x4B83, 0x4C83, 0x4D83, 0x4E83, 0x4F83, 0x5083, 0x5183, 0x5283, 0x5383, 0x5483,
            0x5583, 0x5683, 0x5783, 0x5883, 0x5983, 0x5A83, 0x5B83, 0x5C83, 0x5D83, 0x5E83, 0x5F83, 0x6083, 0x6183, 0x6283, 0x6383, 0x6483,
            0x6583, 0x6683, 0x6783, 0x6883, 0x6983, 0x6A83, 0x6B83, 0x6C83, 0x6D83, 0x6E83, 0x6F83, 0x7083, 0x7183, 0x7283, 0x7383, 0x7483,
            0x7583, 0x7683, 0x7783, 0x7883, 0x7983, 0x7A83, 0x7B83, 0x7C83, 0x7D83, 0x7E83, 0x8083, 0x8183, 0x8283, 0x8383, 0x8483, 0x8583,
            0x8683, 0x8783, 0x8883, 0x8983, 0x8A83, 0x8B83, 0x8C83, 0x8D83, 0x8E83, 0x8F83, 0x9083, 0x9183, 0x9283, 0x9383, 0x9483, 0x9583,
            0x9683, 0x9F88, 0xA088, 0xA188, 0xA288, 0xA388, 0xA488, 0xA588, 0xA688, 0xA788, 0xA888, 0xA988, 0xAA88, 0xAB88, 0xAC88, 0xAD88,
            0xAE88, 0xAF88, 0xB088, 0xB188, 0xB288, 0xB388, 0xB488, 0xB588, 0xB688, 0xB788, 0xB888, 0xB988, 0xBA88, 0xBB88, 0xBC88, 0xBD88,
            0xBE88, 0xBF88, 0xC088, 0xC188, 0xC288, 0xC388, 0xC488, 0xC588, 0xC688, 0xC788, 0xC888, 0xC988, 0xCA88, 0xCB88, 0xCC88, 0xCD88,
            0xCE88, 0xCF88, 0xD088, 0xD188, 0xD288, 0xD388, 0xD488, 0xD588, 0xD688, 0xD788, 0xD888, 0xD988, 0xDA88, 0xDB88, 0xDC88, 0xDD88,
            0xDE88, 0xDF88, 0xE088, 0xE188, 0xE288, 0xE388, 0xE488, 0xE588, 0xE688, 0xE788, 0xE888, 0xE988, 0xEA88, 0xEB88, 0xEC88, 0xED88,
            0xEE88, 0xEF88, 0xF088, 0xF188, 0xF288, 0xF388, 0xF488, 0xF588, 0xF688, 0xF788, 0xF888, 0xF988, 0xFA88, 0xFB88, 0xFC88, 0x4089,
            0x4189, 0x4289, 0x4389, 0x4489, 0x4589, 0x4689, 0x4789, 0x4889, 0x4989, 0x4A89, 0x4B89, 0x4C89, 0x4D89, 0x4E89, 0x4F89, 0x5089,
            0x5189, 0x5289, 0x5389, 0x5489, 0x5589, 0x5689, 0x5789, 0x5889, 0x5989, 0x5A89, 0x5B89, 0x5C89, 0x5D89, 0x5E89, 0x5F89, 0x6089,
            0x6189, 0x6289, 0x6389, 0x6489, 0x6589, 0x6689, 0x6789, 0x6889, 0x6989, 0x6A89, 0x6B89, 0x6C89, 0x6D89, 0x6E89, 0x6F89, 0x7089,
            0x7189, 0x7289, 0x7389, 0x7489, 0x7589, 0x7689, 0x7789, 0x7889, 0x7989, 0x7A89, 0x7B89, 0x7C89, 0x7D89, 0x7E89, 0x8089, 0x8189,
            0x8289, 0x8389, 0x8489, 0x8589, 0x8689, 0x8789, 0x8889, 0x8989, 0x8A89, 0x8B89, 0x8C89, 0x8D89, 0x8E89, 0x8F89, 0x9089, 0x9189,
            0x9289, 0x9389, 0x9489, 0x9589, 0x9689, 0x9789, 0x9889, 0x9989, 0x9A89, 0x9B89, 0x9C89, 0x9D89, 0x9E89, 0x9F89, 0xA089, 0xA189,
            0xA289, 0xA389, 0xA489, 0xA589, 0xA689, 0xA789, 0xA889, 0xA989, 0xAA89, 0xAB89, 0xAC89, 0xAD89, 0xAE89, 0xAF89, 0xB089, 0xB189,
            0xB289, 0xB389, 0xB489, 0xB589, 0xB689, 0xB789, 0xB889, 0xB989, 0xBA89, 0xBB89, 0xBC89, 0xBD89, 0xBE89, 0xBF89, 0xC089, 0xC189,
            0xC289, 0xC389, 0xC489, 0xC589, 0xC689, 0xC789, 0xC889, 0xC989, 0xCA89, 0xCB89, 0xCC89, 0xCD89, 0xCE89, 0xCF89, 0xD089, 0xD189,
            0xD289, 0xD389, 0xD489, 0xD589, 0xD689, 0xD789, 0xD889, 0xD989, 0xDA89, 0xDB89, 0xDC89, 0xDD89, 0xDE89, 0xDF89, 0xE089, 0xE189,
            0xE289, 0xE389, 0xE489, 0xE589, 0xE689, 0xE789, 0xE889, 0xE989, 0xEA89, 0xEB89, 0xEC89, 0xED89, 0xEE89, 0xEF89, 0xF089, 0xF189,
            0xF289, 0xF389, 0xF489, 0xF589, 0xF689, 0xF789, 0xF889, 0xF989, 0xFA89, 0xFB89, 0xFC89, 0x408A, 0x418A, 0x428A, 0x438A, 0x448A,
            0x458A, 0x468A, 0x478A, 0x488A, 0x498A, 0x4A8A, 0x4B8A, 0x4C8A, 0x4D8A, 0x4E8A, 0x4F8A, 0x508A, 0x518A, 0x528A, 0x538A, 0x548A,
            0x558A, 0x568A, 0x578A, 0x588A, 0x598A, 0x5A8A, 0x5B8A, 0x5C8A, 0x5D8A, 0x5E8A, 0x5F8A, 0x608A, 0x618A, 0x628A, 0x638A, 0x648A,
            0x658A, 0x668A, 0x678A, 0x688A, 0x698A, 0x6A8A, 0x6B8A, 0x6C8A, 0x6D8A, 0x6E8A, 0x6F8A, 0x708A, 0x718A, 0x728A, 0x738A, 0x748A,
            0x758A, 0x768A, 0x778A, 0x788A, 0x798A, 0x7A8A, 0x7B8A, 0x7C8A, 0x7D8A, 0x7E8A, 0x808A, 0x818A, 0x828A, 0x838A, 0x848A, 0x858A,
            0x868A, 0x878A, 0x888A, 0x898A, 0x8A8A, 0x8B8A, 0x8C8A, 0x8D8A, 0x8E8A, 0x8F8A, 0x908A, 0x918A, 0x928A, 0x938A, 0x948A, 0x958A,
            0x968A, 0x978A, 0x988A, 0x998A, 0x9A8A, 0x9B8A, 0x9C8A, 0x9D8A, 0x9E8A, 0x9F8A, 0xA08A, 0xA18A, 0xA28A, 0xA38A, 0xA48A, 0xA58A,
            0xA68A, 0xA78A, 0xA88A, 0xA98A, 0xAA8A, 0xAB8A, 0xAC8A, 0xAD8A, 0xAE8A, 0xAF8A, 0xB08A, 0xB18A, 0xB28A, 0xB38A, 0xB48A, 0xB58A,
            0xB68A, 0xB78A, 0xB88A, 0xB98A, 0xBA8A, 0xBB8A, 0xBC8A, 0xBD8A, 0xBE8A, 0xBF8A, 0xC08A, 0xC18A, 0xC28A, 0xC38A, 0xC48A, 0xC58A,
            0xC68A, 0xC78A, 0xC88A, 0xC98A, 0xCA8A, 0xCB8A, 0xCC8A, 0xCD8A, 0xCE8A, 0xCF8A, 0xD08A, 0xD18A, 0xD28A, 0xD38A, 0xD48A, 0xD58A,
            0xD68A, 0xD78A, 0xD88A, 0xD98A, 0xDA8A, 0xDB8A, 0xDC8A, 0xDD8A, 0xDE8A, 0xDF8A, 0xE08A, 0xE18A, 0xE28A, 0xE38A, 0xE48A, 0xE58A,
            0xE68A, 0xE78A, 0xE88A, 0xE98A, 0xEA8A, 0xEB8A, 0xEC8A, 0xED8A, 0xEE8A, 0xEF8A, 0xF08A, 0xF18A, 0xF28A, 0xF38A, 0xF48A, 0xF58A,
            0xF68A, 0xF78A, 0xF88A, 0xF98A, 0xFA8A, 0xFB8A, 0xFC8A, 0x408B, 0x418B, 0x428B, 0x438B, 0x448B, 0x458B, 0x468B, 0x478B, 0x488B,
            0x498B, 0x4A8B, 0x4B8B, 0x4C8B, 0x4D8B, 0x4E8B, 0x4F8B, 0x508B, 0x518B, 0x528B, 0x538B, 0x548B, 0x558B, 0x568B, 0x578B, 0x588B,
            0x598B, 0x5A8B, 0x5B8B, 0x5C8B, 0x5D8B, 0x5E8B, 0x5F8B, 0x608B, 0x618B, 0x628B, 0x638B, 0x648B, 0x658B, 0x668B, 0x678B, 0x688B,
            0x698B, 0x6A8B, 0x6B8B, 0x6C8B, 0x6D8B, 0x6E8B, 0x6F8B, 0x708B, 0x718B, 0x728B, 0x738B, 0x748B, 0x758B, 0x768B, 0x778B, 0x788B,
            0x798B, 0x7A8B, 0x7B8B, 0x7C8B, 0x7D8B, 0x7E8B, 0x808B, 0x818B, 0x828B, 0x838B, 0x848B, 0x858B, 0x868B, 0x878B, 0x888B, 0x898B,
            0x8A8B, 0x8B8B, 0x8C8B, 0x8D8B, 0x8E8B, 0x8F8B, 0x908B, 0x918B, 0x928B, 0x938B, 0x948B, 0x958B, 0x968B, 0x978B, 0x988B, 0x998B,
            0x9A8B, 0x9B8B, 0x9C8B, 0x9D8B, 0x9E8B, 0x9F8B, 0xA08B, 0xA18B, 0xA28B, 0xA38B, 0xA48B, 0xA58B, 0xA68B, 0xA78B, 0xA88B, 0xA98B,
            0xAA8B, 0xAB8B, 0xAC8B, 0xAD8B, 0xAE8B, 0xAF8B, 0xB08B, 0xB18B, 0xB28B, 0xB38B, 0xB48B, 0xB58B, 0xB68B, 0xB78B, 0xB88B, 0xB98B,
            0xBA8B, 0xBB8B, 0xBC8B, 0xBD8B, 0xBE8B, 0xBF8B, 0xC08B, 0xC18B, 0xC28B, 0xC38B, 0xC48B, 0xC58B, 0xC68B, 0xC78B, 0xC88B, 0xC98B,
            0xCA8B, 0xCB8B, 0xCC8B, 0xCD8B, 0xCE8B, 0xCF8B, 0xD08B, 0xD18B, 0xD28B, 0xD38B, 0xD48B, 0xD58B, 0xD68B, 0xD78B, 0xD88B, 0xD98B,
            0xDA8B, 0xDB8B, 0xDC8B, 0xDD8B, 0xDE8B, 0xDF8B, 0xE08B, 0xE18B, 0xE28B, 0xE38B, 0xE48B, 0xE58B, 0xE68B, 0xE78B, 0xE88B, 0xE98B,
            0xEA8B, 0xEB8B, 0xEC8B, 0xED8B, 0xEE8B, 0xEF8B, 0xF08B, 0xF18B, 0xF28B, 0xF38B, 0xF48B, 0xF58B, 0xF68B, 0xF78B, 0xF88B, 0xF98B,
            0xFA8B, 0xFB8B, 0xFC8B, 0x408C, 0x418C, 0x428C, 0x438C, 0x448C, 0x458C, 0x468C, 0x478C, 0x488C, 0x498C, 0x4A8C, 0x4B8C, 0x4C8C,
            0x4D8C, 0x4E8C, 0x4F8C, 0x508C, 0x518C, 0x528C, 0x538C, 0x548C, 0x558C, 0x568C, 0x578C, 0x588C, 0x598C, 0x5A8C, 0x5B8C, 0x5C8C,
            0x5D8C, 0x5E8C, 0x5F8C, 0x608C, 0x618C, 0x628C, 0x638C, 0x648C, 0x658C, 0x668C, 0x678C, 0x688C, 0x698C, 0x6A8C, 0x6B8C, 0x6C8C,
            0x6D8C, 0x6E8C, 0x6F8C, 0x708C, 0x718C, 0x728C, 0x738C, 0x748C, 0x758C, 0x768C, 0x778C, 0x788C, 0x798C, 0x7A8C, 0x7B8C, 0x7C8C,
            0x7D8C, 0x7E8C, 0x808C, 0x818C, 0x828C, 0x838C, 0x848C, 0x858C, 0x868C, 0x878C, 0x888C, 0x898C, 0x8A8C, 0x8B8C, 0x8C8C, 0x8D8C,
            0x8E8C, 0x8F8C, 0x908C, 0x918C, 0x928C, 0x938C, 0x948C, 0x958C, 0x968C, 0x978C, 0x988C, 0x998C, 0x9A8C, 0x9B8C, 0x9C8C, 0x9D8C,
            0x9E8C, 0x9F8C, 0xA08C, 0xA18C, 0xA28C, 0xA38C, 0xA48C, 0xA58C, 0xA68C, 0xA78C, 0xA88C, 0xA98C, 0xAA8C, 0xAB8C, 0xAC8C, 0xAD8C,
            0xAE8C, 0xAF8C, 0xB08C, 0xB18C, 0xB28C, 0xB38C, 0xB48C, 0xB58C, 0xB68C, 0xB78C, 0xB88C, 0xB98C, 0xBA8C, 0xBB8C, 0xBC8C, 0xBD8C,
            0xBE8C, 0xBF8C, 0xC08C, 0xC18C, 0xC28C, 0xC38C, 0xC48C, 0xC58C, 0xC68C, 0xC78C, 0xC88C, 0xC98C, 0xCA8C, 0xCB8C, 0xCC8C, 0xCD8C,
            0xCE8C, 0xCF8C, 0xD08C, 0xD18C, 0xD28C, 0xD38C, 0xD48C, 0xD58C, 0xD68C, 0xD78C, 0xD88C, 0xD98C, 0xDA8C, 0xDB8C, 0xDC8C, 0xDD8C,
            0xDE8C, 0xDF8C, 0xE08C, 0xE18C, 0xE28C, 0xE38C, 0xE48C, 0xE58C, 0xE68C, 0xE78C, 0xE88C, 0xE98C, 0xEA8C, 0xEB8C, 0xEC8C, 0xED8C,
            0xEE8C, 0xEF8C, 0xF08C, 0xF18C, 0xF28C, 0xF38C, 0xF48C, 0xF58C, 0xF68C, 0xF78C, 0xF88C, 0xF98C, 0xFA8C, 0xFB8C, 0xFC8C, 0x408D,
            0x418D, 0x428D, 0x438D, 0x448D, 0x458D, 0x468D, 0x478D, 0x488D, 0x498D, 0x4A8D, 0x4B8D, 0x4C8D, 0x4D8D, 0x4E8D, 0x4F8D, 0x508D,
            0x518D, 0x528D, 0x538D, 0x548D, 0x558D, 0x568D, 0x578D, 0x588D, 0x598D, 0x5A8D, 0x5B8D, 0x5C8D, 0x5D8D, 0x5E8D, 0x5F8D, 0x608D,
            0x618D, 0x628D, 0x638D, 0x648D, 0x658D, 0x668D, 0x678D, 0x688D, 0x698D, 0x6A8D, 0x6B8D, 0x6C8D, 0x6D8D, 0x6E8D, 0x6F8D, 0x708D,
            0x718D, 0x728D, 0x738D, 0x748D, 0x758D, 0x768D, 0x778D, 0x788D, 0x798D, 0x7A8D, 0x7B8D, 0x7C8D, 0x7D8D, 0x7E8D, 0x808D, 0x818D,
            0x828D, 0x838D, 0x848D, 0x858D, 0x868D, 0x878D, 0x888D, 0x898D, 0x8A8D, 0x8B8D, 0x8C8D, 0x8D8D, 0x8E8D, 0x8F8D, 0x908D, 0x918D,
            0x928D, 0x938D, 0x948D, 0x958D, 0x968D, 0x978D, 0x988D, 0x998D, 0x9A8D, 0x9B8D, 0x9C8D, 0x9D8D, 0x9E8D, 0x9F8D, 0xA08D, 0xA18D,
            0xA28D, 0xA38D, 0xA48D, 0xA58D, 0xA68D, 0xA78D, 0xA88D, 0xA98D, 0xAA8D, 0xAB8D, 0xAC8D, 0xAD8D, 0xAE8D, 0xAF8D, 0xB08D, 0xB18D,
            0xB28D, 0xB38D, 0xB48D, 0xB58D, 0xB68D, 0xB78D, 0xB88D, 0xB98D, 0xBA8D, 0xBB8D, 0xBC8D, 0xBD8D, 0xBE8D, 0xBF8D, 0xC08D, 0xC18D,
            0xC28D, 0xC38D, 0xC48D, 0xC58D, 0xC68D, 0xC78D, 0xC88D, 0xC98D, 0xCA8D, 0xCB8D, 0xCC8D, 0xCD8D, 0xCE8D, 0xCF8D, 0xD08D, 0xD18D,
            0xD28D, 0xD38D, 0xD48D, 0xD58D, 0xD68D, 0xD78D, 0xD88D, 0xD98D, 0xDA8D, 0xDB8D, 0xDC8D, 0xDD8D, 0xDE8D, 0xDF8D, 0xE08D, 0xE18D,
            0xE28D, 0xE38D, 0xE48D, 0xE58D, 0xE68D, 0xE78D, 0xE88D, 0xE98D, 0xEA8D, 0xEB8D, 0xEC8D, 0xED8D, 0xEE8D, 0xEF8D, 0xF08D, 0xF18D,
            0xF28D, 0xF38D, 0xF48D, 0xF58D, 0xF68D, 0xF78D, 0xF88D, 0xF98D, 0xFA8D, 0xFB8D, 0xFC8D, 0x408E, 0x418E, 0x428E, 0x438E, 0x448E,
            0x458E, 0x468E, 0x478E, 0x488E, 0x498E, 0x4A8E, 0x4B8E, 0x4C8E, 0x4D8E, 0x4E8E, 0x4F8E, 0x508E, 0x518E, 0x528E, 0x538E, 0x548E,
            0x558E, 0x568E, 0x578E, 0x588E, 0x598E, 0x5A8E, 0x5B8E, 0x5C8E, 0x5D8E, 0x5E8E, 0x5F8E, 0x608E, 0x618E, 0x628E, 0x638E, 0x648E,
            0x658E, 0x668E, 0x678E, 0x688E, 0x698E, 0x6A8E, 0x6B8E, 0x6C8E, 0x6D8E, 0x6E8E, 0x6F8E, 0x708E, 0x718E, 0x728E, 0x738E, 0x748E,
            0x758E, 0x768E, 0x778E, 0x788E, 0x798E, 0x7A8E, 0x7B8E, 0x7C8E, 0x7D8E, 0x7E8E, 0x808E, 0x818E, 0x828E, 0x838E, 0x848E, 0x858E,
            0x868E, 0x878E, 0x888E, 0x898E, 0x8A8E, 0x8B8E, 0x8C8E, 0x8D8E, 0x8E8E, 0x8F8E, 0x908E, 0x918E, 0x928E, 0x938E, 0x948E, 0x958E,
            0x968E, 0x978E, 0x988E, 0x998E, 0x9A8E, 0x9B8E, 0x9C8E, 0x9D8E, 0x9E8E, 0x9F8E, 0xA08E, 0xA18E, 0xA28E, 0xA38E, 0xA48E, 0xA58E,
            0xA68E, 0xA78E, 0xA88E, 0xA98E, 0xAA8E, 0xAB8E, 0xAC8E, 0xAD8E, 0xAE8E, 0xAF8E, 0xB08E, 0xB18E, 0xB28E, 0xB38E, 0xB48E, 0xB58E,
            0xB68E, 0xB78E, 0xB88E, 0xB98E, 0xBA8E, 0xBB8E, 0xBC8E, 0xBD8E, 0xBE8E, 0xBF8E, 0xC08E, 0xC18E, 0xC28E, 0xC38E, 0xC48E, 0xC58E,
            0xC68E, 0xC78E, 0xC88E, 0xC98E, 0xCA8E, 0xCB8E, 0xCC8E, 0xCD8E, 0xCE8E, 0xCF8E, 0xD08E, 0xD18E, 0xD28E, 0xD38E, 0xD48E, 0xD58E,
            0xD68E, 0xD78E, 0xD88E, 0xD98E, 0xDA8E, 0xDB8E, 0xDC8E, 0xDD8E, 0xDE8E, 0xDF8E, 0xE08E, 0xE18E, 0xE28E, 0xE38E, 0xE48E, 0xE58E,
            0xE68E, 0xE78E, 0xE88E, 0xE98E, 0xEA8E, 0xEB8E, 0xEC8E, 0xED8E, 0xEE8E, 0xEF8E, 0xF08E, 0xF18E, 0xF28E, 0xF38E, 0xF48E, 0xF58E,
            0xF68E, 0xF78E, 0xF88E, 0xF98E, 0xFA8E, 0xFB8E, 0xFC8E, 0x408F, 0x418F, 0x428F, 0x438F, 0x448F, 0x458F, 0x468F, 0x478F, 0x488F,
            0x498F, 0x4A8F, 0x4B8F, 0x4C8F, 0x4D8F, 0x4E8F, 0x4F8F, 0x508F, 0x518F, 0x528F, 0x538F, 0x548F, 0x558F, 0x568F, 0x578F, 0x588F,
            0x598F, 0x5A8F, 0x5B8F, 0x5C8F, 0x5D8F, 0x5E8F, 0x5F8F, 0x608F, 0x618F, 0x628F, 0x638F, 0x648F, 0x658F, 0x668F, 0x678F, 0x688F,
            0x698F, 0x6A8F, 0x6B8F, 0x6C8F, 0x6D8F, 0x6E8F, 0x6F8F, 0x708F, 0x718F, 0x728F, 0x738F, 0x748F, 0x758F, 0x768F, 0x778F, 0x788F,
            0x798F, 0x7A8F, 0x7B8F, 0x7C8F, 0x7D8F, 0x7E8F, 0x808F, 0x818F, 0x828F, 0x838F, 0x848F, 0x858F, 0x868F, 0x878F, 0x888F, 0x898F,
            0x8A8F, 0x8B8F, 0x8C8F, 0x8D8F, 0x8E8F, 0x8F8F, 0x908F, 0x918F, 0x928F, 0x938F, 0x948F, 0x958F, 0x968F, 0x978F, 0x988F, 0x998F,
            0x9A8F, 0x9B8F, 0x9C8F, 0x9D8F, 0x9E8F, 0x9F8F, 0xA08F, 0xA18F, 0xA28F, 0xA38F, 0xA48F, 0xA58F, 0xA68F, 0xA78F, 0xA88F, 0xA98F,
            0xAA8F, 0xAB8F, 0xAC8F, 0xAD8F, 0xAE8F, 0xAF8F, 0xB08F, 0xB18F, 0xB28F, 0xB38F, 0xB48F, 0xB58F, 0xB68F, 0xB78F, 0xB88F, 0xB98F,
            0xBA8F, 0xBB8F, 0xBC8F, 0xBD8F, 0xBE8F, 0xBF8F, 0xC08F, 0xC18F, 0xC28F, 0xC38F, 0xC48F, 0xC58F, 0xC68F, 0xC78F, 0xC88F, 0xC98F,
            0xCA8F, 0xCB8F, 0xCC8F, 0xCD8F, 0xCE8F, 0xCF8F, 0xD08F, 0xD18F, 0xD28F, 0xD38F, 0xD48F, 0xD58F, 0xD68F, 0xD78F, 0xD88F, 0xD98F,
            0xDA8F, 0xDB8F, 0xDC8F, 0xDD8F, 0xDE8F, 0xDF8F, 0xE08F, 0xE18F, 0xE28F, 0xE38F, 0xE48F, 0xE58F, 0xE68F, 0xE78F, 0xE88F, 0xE98F,
            0xEA8F, 0xEB8F, 0xEC8F, 0xED8F, 0xEE8F, 0xEF8F, 0xF08F, 0xF18F, 0xF28F, 0xF38F, 0xF48F, 0xF58F, 0xF68F, 0xF78F, 0xF88F, 0xF98F,
            0xFA8F, 0xFB8F, 0xFC8F, 0x4090, 0x4190, 0x4290, 0x4390, 0x4490, 0x4590, 0x4690, 0x4790, 0x4890, 0x4990, 0x4A90, 0x4B90, 0x4C90,
            0x4D90, 0x4E90, 0x4F90, 0x5090, 0x5190, 0x5290, 0x5390, 0x5490, 0x5590, 0x5690, 0x5790, 0x5890, 0x5990, 0x5A90, 0x5B90, 0x5C90,
            0x5D90, 0x5E90, 0x5F90, 0x6090, 0x6190, 0x6290, 0x6390, 0x6490, 0x6590, 0x6690, 0x6790, 0x6890, 0x6990, 0x6A90, 0x6B90, 0x6C90,
            0x6D90, 0x6E90, 0x6F90, 0x7090, 0x7190, 0x7290, 0x7390, 0x7490, 0x7590, 0x7690, 0x7790, 0x7890, 0x7990, 0x7A90, 0x7B90, 0x7C90,
            0x7D90, 0x7E90, 0x8090, 0x8190, 0x8290, 0x8390, 0x8490, 0x8590, 0x8690, 0x8790, 0x8890, 0x8990, 0x8A90, 0x8B90, 0x8C90, 0x8D90,
            0x8E90, 0x8F90, 0x9090, 0x9190, 0x9290, 0x9390, 0x9490, 0x9590, 0x9690, 0x9790, 0x9890, 0x9990, 0x9A90, 0x9B90, 0x9C90, 0x9D90,
            0x9E90, 0x9F90, 0xA090, 0xA190, 0xA290, 0xA390, 0xA490, 0xA590, 0xA690, 0xA790, 0xA890, 0xA990, 0xAA90, 0xAB90, 0xAC90, 0xAD90,
            0xAE90, 0xAF90, 0xB090, 0xB190, 0xB290, 0xB390, 0xB490, 0xB590, 0xB690, 0xB790, 0xB890, 0xB990, 0xBA90, 0xBB90, 0xBC90, 0xBD90,
            0xBE90, 0xBF90, 0xC090, 0xC190, 0xC290, 0xC390, 0xC490, 0xC590, 0xC690, 0xC790, 0xC890, 0xC990, 0xCA90, 0xCB90, 0xCC90, 0xCD90,
            0xCE90, 0xCF90, 0xD090, 0xD190, 0xD290, 0xD390, 0xD490, 0xD590, 0xD690, 0xD790, 0xD890, 0xD990, 0xDA90, 0xDB90, 0xDC90, 0xDD90,
            0xDE90, 0xDF90, 0xE090, 0xE190, 0xE290, 0xE390, 0xE490, 0xE590, 0xE690, 0xE790, 0xE890, 0xE990, 0xEA90, 0xEB90, 0xEC90, 0xED90,
            0xEE90, 0xEF90, 0xF090, 0xF190, 0xF290, 0xF390, 0xF490, 0xF590, 0xF690, 0xF790, 0xF890, 0xF990, 0xFA90, 0xFB90, 0xFC90, 0x4091,
            0x4191, 0x4291, 0x4391, 0x4491, 0x4591, 0x4691, 0x4791, 0x4891, 0x4991, 0x4A91, 0x4B91, 0x4C91, 0x4D91, 0x4E91, 0x4F91, 0x5091,
            0x5191, 0x5291, 0x5391, 0x5491, 0x5591, 0x5691, 0x5791, 0x5891, 0x5991, 0x5A91, 0x5B91, 0x5C91, 0x5D91, 0x5E91, 0x5F91, 0x6091,
            0x6191, 0x6291, 0x6391, 0x6491, 0x6591, 0x6691, 0x6791, 0x6891, 0x6991, 0x6A91, 0x6B91, 0x6C91, 0x6D91, 0x6E91, 0x6F91, 0x7091,
            0x7191, 0x7291, 0x7391, 0x7491, 0x7591, 0x7691, 0x7791, 0x7891, 0x7991, 0x7A91, 0x7B91, 0x7C91, 0x7D91, 0x7E91, 0x8091, 0x8191,
            0x8291, 0x8391, 0x8491, 0x8591, 0x8691, 0x8791, 0x8891, 0x8991, 0x8A91, 0x8B91, 0x8C91, 0x8D91, 0x8E91, 0x8F91, 0x9091, 0x9191,
            0x9291, 0x9391, 0x9491, 0x9591, 0x9691, 0x9791, 0x9891, 0x9991, 0x9A91, 0x9B91, 0x9C91, 0x9D91, 0x9E91, 0x9F91, 0xA091, 0xA191,
            0xA291, 0xA391, 0xA491, 0xA591, 0xA691, 0xA791, 0xA891, 0xA991, 0xAA91, 0xAB91, 0xAC91, 0xAD91, 0xAE91, 0xAF91, 0xB091, 0xB191,
            0xB291, 0xB391, 0xB491, 0xB591, 0xB691, 0xB791, 0xB891, 0xB991, 0xBA91, 0xBB91, 0xBC91, 0xBD91, 0xBE91, 0xBF91, 0xC091, 0xC191,
            0xC291, 0xC391, 0xC491, 0xC591, 0xC691, 0xC791, 0xC891, 0xC991, 0xCA91, 0xCB91, 0xCC91, 0xCD91, 0xCE91, 0xCF91, 0xD091, 0xD191,
            0xD291, 0xD391, 0xD491, 0xD591, 0xD691, 0xD791, 0xD891, 0xD991, 0xDA91, 0xDB91, 0xDC91, 0xDD91, 0xDE91, 0xDF91, 0xE091, 0xE191,
            0xE291, 0xE391, 0xE491, 0xE591, 0xE691, 0xE791, 0xE891, 0xE991, 0xEA91, 0xEB91, 0xEC91, 0xED91, 0xEE91, 0xEF91, 0xF091, 0xF191,
            0xF291, 0xF391, 0xF491, 0xF591, 0xF691, 0xF791, 0xF891, 0xF991, 0xFA91, 0xFB91, 0xFC91, 0x4092, 0x4192, 0x4292, 0x4392, 0x4492,
            0x4592, 0x4692, 0x4792, 0x4892, 0x4992, 0x4A92, 0x4B92, 0x4C92, 0x4D92, 0x4E92, 0x4F92, 0x5092, 0x5192, 0x5292, 0x5392, 0x5492,
            0x5592, 0x5692, 0x5792, 0x5892, 0x5992, 0x5A92, 0x5B92, 0x5C92, 0x5D92, 0x5E92, 0x5F92, 0x6092, 0x6192, 0x6292, 0x6392, 0x6492,
            0x6592, 0x6692, 0x6792, 0x6892, 0x6992, 0x6A92, 0x6B92, 0x6C92, 0x6D92, 0x6E92, 0x6F92, 0x7092, 0x7192, 0x7292, 0x7392, 0x7492,
            0x7592, 0x7692, 0x7792, 0x7892, 0x7992, 0x7A92, 0x7B92, 0x7C92, 0x7D92, 0x7E92, 0x8092, 0x8192, 0x8292, 0x8392, 0x8492, 0x8592,
            0x8692, 0x8792, 0x8892, 0x8992, 0x8A92, 0x8B92, 0x8C92, 0x8D92, 0x8E92, 0x8F92, 0x9092, 0x9192, 0x9292, 0x9392, 0x9492, 0x9592,
            0x9692, 0x9792, 0x9892, 0x9992, 0x9A92, 0x9B92, 0x9C92, 0x9D92, 0x9E92, 0x9F92, 0xA092, 0xA192, 0xA292, 0xA392, 0xA492, 0xA592,
            0xA692, 0xA792, 0xA892, 0xA992, 0xAA92, 0xAB92, 0xAC92, 0xAD92, 0xAE92, 0xAF92, 0xB092, 0xB192, 0xB292, 0xB392, 0xB492, 0xB592,
            0xB692, 0xB792, 0xB892, 0xB992, 0xBA92, 0xBB92, 0xBC92, 0xBD92, 0xBE92, 0xBF92, 0xC092, 0xC192, 0xC292, 0xC392, 0xC492, 0xC592,
            0xC692, 0xC792, 0xC892, 0xC992, 0xCA92, 0xCB92, 0xCC92, 0xCD92, 0xCE92, 0xCF92, 0xD092, 0xD192, 0xD292, 0xD392, 0xD492, 0xD592,
            0xD692, 0xD792, 0xD892, 0xD992, 0xDA92, 0xDB92, 0xDC92, 0xDD92, 0xDE92, 0xDF92, 0xE092, 0xE192, 0xE292, 0xE392, 0xE492, 0xE592,
            0xE692, 0xE792, 0xE892, 0xE992, 0xEA92, 0xEB92, 0xEC92, 0xED92, 0xEE92, 0xEF92, 0xF092, 0xF192, 0xF292, 0xF392, 0xF492, 0xF592,
            0xF692, 0xF792, 0xF892, 0xF992, 0xFA92, 0xFB92, 0xFC92, 0x4093, 0x4193, 0x4293, 0x4393, 0x4493, 0x4593, 0x4693, 0x4793, 0x4893,
            0x4993, 0x4A93, 0x4B93, 0x4C93, 0x4D93, 0x4E93, 0x4F93, 0x5093, 0x5193, 0x5293, 0x5393, 0x5493, 0x5593, 0x5693, 0x5793, 0x5893,
            0x5993, 0x5A93, 0x5B93, 0x5C93, 0x5D93, 0x5E93, 0x5F93, 0x6093, 0x6193, 0x6293, 0x6393, 0x6493, 0x6593, 0x6693, 0x6793, 0x6893,
            0x6993, 0x6A93, 0x6B93, 0x6C93, 0x6D93, 0x6E93, 0x6F93, 0x7093, 0x7193, 0x7293, 0x7393, 0x7493, 0x7593, 0x7693, 0x7793, 0x7893,
            0x7993, 0x7A93, 0x7B93, 0x7C93, 0x7D93, 0x7E93, 0x8093, 0x8193, 0x8293, 0x8393, 0x8493, 0x8593, 0x8693, 0x8793, 0x8893, 0x8993,
            0x8A93, 0x8B93, 0x8C93, 0x8D93, 0x8E93, 0x8F93, 0x9093, 0x9193, 0x9293, 0x9393, 0x9493, 0x9593, 0x9693, 0x9793, 0x9893, 0x9993,
            0x9A93, 0x9B93, 0x9C93, 0x9D93, 0x9E93, 0x9F93, 0xA093, 0xA193, 0xA293, 0xA393, 0xA493, 0xA593, 0xA693, 0xA793, 0xA893, 0xA993,
            0xAA93, 0xAB93, 0xAC93, 0xAD93, 0xAE93, 0xAF93, 0xB093, 0xB193, 0xB293, 0xB393, 0xB493, 0xB593, 0xB693, 0xB793, 0xB893, 0xB993,
            0xBA93, 0xBB93, 0xBC93, 0xBD93, 0xBE93, 0xBF93, 0xC093, 0xC193, 0xC293, 0xC393, 0xC493, 0xC593, 0xC693, 0xC793, 0xC893, 0xC993,
            0xCA93, 0xCB93, 0xCC93, 0xCD93, 0xCE93, 0xCF93, 0xD093, 0xD193, 0xD293, 0xD393, 0xD493, 0xD593, 0xD693, 0xD793, 0xD893, 0xD993,
            0xDA93, 0xDB93, 0xDC93, 0xDD93, 0xDE93, 0xDF93, 0xE093, 0xE193, 0xE293, 0xE393, 0xE493, 0xE593, 0xE693, 0xE793, 0xE893, 0xE993,
            0xEA93, 0xEB93, 0xEC93, 0xED93, 0xEE93, 0xEF93, 0xF093, 0xF193, 0xF293, 0xF393, 0xF493, 0xF593, 0xF693, 0xF793, 0xF893, 0xF993,
            0xFA93, 0xFB93, 0xFC93, 0x4094, 0x4194, 0x4294, 0x4394, 0x4494, 0x4594, 0x4694, 0x4794, 0x4894, 0x4994, 0x4A94, 0x4B94, 0x4C94,
            0x4D94, 0x4E94, 0x4F94, 0x5094, 0x5194, 0x5294, 0x5394, 0x5494, 0x5594, 0x5694, 0x5794, 0x5894, 0x5994, 0x5A94, 0x5B94, 0x5C94,
            0x5D94, 0x5E94, 0x5F94, 0x6094, 0x6194, 0x6294, 0x6394, 0x6494, 0x6594, 0x6694, 0x6794, 0x6894, 0x6994, 0x6A94, 0x6B94, 0x6C94,
            0x6D94, 0x6E94, 0x6F94, 0x7094, 0x7194, 0x7294, 0x7394, 0x7494, 0x7594, 0x7694, 0x7794, 0x7894, 0x7994, 0x7A94, 0x7B94, 0x7C94,
            0x7D94, 0x7E94, 0x8094, 0x8194, 0x8294, 0x8394, 0x8494, 0x8594, 0x8694, 0x8794, 0x8894, 0x8994, 0x8A94, 0x8B94, 0x8C94, 0x8D94,
            0x8E94, 0x8F94, 0x9094, 0x9194, 0x9294, 0x9394, 0x9494, 0x9594, 0x9694, 0x9794, 0x9894, 0x9994, 0x9A94, 0x9B94, 0x9C94, 0x9D94,
            0x9E94, 0x9F94, 0xA094, 0xA194, 0xA294, 0xA394, 0xA494, 0xA594, 0xA694, 0xA794, 0xA894, 0xA994, 0xAA94, 0xAB94, 0xAC94, 0xAD94,
            0xAE94, 0xAF94, 0xB094, 0xB194, 0xB294, 0xB394, 0xB494, 0xB594, 0xB694, 0xB794, 0xB894, 0xB994, 0xBA94, 0xBB94, 0xBC94, 0xBD94,
            0xBE94, 0xBF94, 0xC094, 0xC194, 0xC294, 0xC394, 0xC494, 0xC594, 0xC694, 0xC794, 0xC894, 0xC994, 0xCA94, 0xCB94, 0xCC94, 0xCD94,
            0xCE94, 0xCF94, 0xD094, 0xD194, 0xD294, 0xD394, 0xD494, 0xD594, 0xD694, 0xD794, 0xD894, 0xD994, 0xDA94, 0xDB94, 0xDC94, 0xDD94,
            0xDE94, 0xDF94, 0xE094, 0xE194, 0xE294, 0xE394, 0xE494, 0xE594, 0xE694, 0xE794, 0xE894, 0xE994, 0xEA94, 0xEB94, 0xEC94, 0xED94,
            0xEE94, 0xEF94, 0xF094, 0xF194, 0xF294, 0xF394, 0xF494, 0xF594, 0xF694, 0xF794, 0xF894, 0xF994, 0xFA94, 0xFB94, 0xFC94, 0x4095,
            0x4195, 0x4295, 0x4395, 0x4495, 0x4595, 0x4695, 0x4795, 0x4895, 0x4995, 0x4A95, 0x4B95, 0x4C95, 0x4D95, 0x4E95, 0x4F95, 0x5095,
            0x5195, 0x5295, 0x5395, 0x5495, 0x5595, 0x5695, 0x5795, 0x5895, 0x5995, 0x5A95, 0x5B95, 0x5C95, 0x5D95, 0x5E95, 0x5F95, 0x6095,
            0x6195, 0x6295, 0x6395, 0x6495, 0x6595, 0x6695, 0x6795, 0x6895, 0x6995, 0x6A95, 0x6B95, 0x6C95, 0x6D95, 0x6E95, 0x6F95, 0x7095,
            0x7195, 0x7295, 0x7395, 0x7495, 0x7595, 0x7695, 0x7795, 0x7895, 0x7995, 0x7A95, 0x7B95, 0x7C95, 0x7D95, 0x7E95, 0x8095, 0x8195,
            0x8295, 0x8395, 0x8495, 0x8595, 0x8695, 0x8795, 0x8895, 0x8995, 0x8A95, 0x8B95, 0x8C95, 0x8D95, 0x8E95, 0x8F95, 0x9095, 0x9195,
            0x9295, 0x9395, 0x9495, 0x9595, 0x9695, 0x9795, 0x9895, 0x9995, 0x9A95, 0x9B95, 0x9C95, 0x9D95, 0x9E95, 0x9F95, 0xA095, 0xA195,
            0xA295, 0xA395, 0xA495, 0xA595, 0xA695, 0xA795, 0xA895, 0xA995, 0xAA95, 0xAB95, 0xAC95, 0xAD95, 0xAE95, 0xAF95, 0xB095, 0xB195,
            0xB295, 0xB395, 0xB495, 0xB595, 0xB695, 0xB795, 0xB895, 0xB995, 0xBA95, 0xBB95, 0xBC95, 0xBD95, 0xBE95, 0xBF95, 0xC095, 0xC195,
            0xC295, 0xC395, 0xC495, 0xC595, 0xC695, 0xC795, 0xC895, 0xC995, 0xCA95, 0xCB95, 0xCC95, 0xCD95, 0xCE95, 0xCF95, 0xD095, 0xD195,
            0xD295, 0xD395, 0xD495, 0xD595, 0xD695, 0xD795, 0xD895, 0xD995, 0xDA95, 0xDB95, 0xDC95, 0xDD95, 0xDE95, 0xDF95, 0xE095, 0xE195,
            0xE295, 0xE395, 0xE495, 0xE595, 0xE695, 0xE795, 0xE895, 0xE995, 0xEA95, 0xEB95, 0xEC95, 0xED95, 0xEE95, 0xEF95, 0xF095, 0xF195,
            0xF295, 0xF395, 0xF495, 0xF595, 0xF695, 0xF795, 0xF895, 0xF995, 0xFA95, 0xFB95, 0xFC95, 0x4096, 0x4196, 0x4296, 0x4396, 0x4496,
            0x4596, 0x4696, 0x4796, 0x4896, 0x4996, 0x4A96, 0x4B96, 0x4C96, 0x4D96, 0x4E96, 0x4F96, 0x5096, 0x5196, 0x5296, 0x5396, 0x5496,
            0x5596, 0x5696, 0x5796, 0x5896, 0x5996, 0x5A96, 0x5B96, 0x5C96, 0x5D96, 0x5E96, 0x5F96, 0x6096, 0x6196, 0x6296, 0x6396, 0x6496,
            0x6596, 0x6696, 0x6796, 0x6896, 0x6996, 0x6A96, 0x6B96, 0x6C96, 0x6D96, 0x6E96, 0x6F96, 0x7096, 0x7196, 0x7296, 0x7396, 0x7496,
            0x7596, 0x7696, 0x7796, 0x7896, 0x7996, 0x7A96, 0x7B96, 0x7C96, 0x7D96, 0x7E96, 0x8096, 0x8196, 0x8296, 0x8396, 0x8496, 0x8596,
            0x8696, 0x8796, 0x8896, 0x8996, 0x8A96, 0x8B96, 0x8C96, 0x8D96, 0x8E96, 0x8F96, 0x9096, 0x9196, 0x9296, 0x9396, 0x9496, 0x9596,
            0x9696, 0x9796, 0x9896, 0x9996, 0x9A96, 0x9B96, 0x9C96, 0x9D96, 0x9E96, 0x9F96, 0xA096, 0xA196, 0xA296, 0xA396, 0xA496, 0xA596,
            0xA696, 0xA796, 0xA896, 0xA996, 0xAA96, 0xAB96, 0xAC96, 0xAD96, 0xAE96, 0xAF96, 0xB096, 0xB196, 0xB296, 0xB396, 0xB496, 0xB596,
            0xB696, 0xB796, 0xB896, 0xB996, 0xBA96, 0xBB96, 0xBC96, 0xBD96, 0xBE96, 0xBF96, 0xC096, 0xC196, 0xC296, 0xC396, 0xC496, 0xC596,
            0xC696, 0xC796, 0xC896, 0xC996, 0xCA96, 0xCB96, 0xCC96, 0xCD96, 0xCE96, 0xCF96, 0xD096, 0xD196, 0xD296, 0xD396, 0xD496, 0xD596,
            0xD696, 0xD796, 0xD896, 0xD996, 0xDA96, 0xDB96, 0xDC96, 0xDD96, 0xDE96, 0xDF96, 0xE096, 0xE196, 0xE296, 0xE396, 0xE496, 0xE596,
            0xE696, 0xE796, 0xE896, 0xE996, 0xEA96, 0xEB96, 0xEC96, 0xED96, 0xEE96, 0xEF96, 0xF096, 0xF196, 0xF296, 0xF396, 0xF496, 0xF596,
            0xF696, 0xF796, 0xF896, 0xF996, 0xFA96, 0xFB96, 0xFC96, 0x4097, 0x4197, 0x4297, 0x4397, 0x4497, 0x4597, 0x4697, 0x4797, 0x4897,
            0x4997, 0x4A97, 0x4B97, 0x4C97, 0x4D97, 0x4E97, 0x4F97, 0x5097, 0x5197, 0x5297, 0x5397, 0x5497, 0x5597, 0x5697, 0x5797, 0x5897,
            0x5997, 0x5A97, 0x5B97, 0x5C97, 0x5D97, 0x5E97, 0x5F97, 0x6097, 0x6197, 0x6297, 0x6397, 0x6497, 0x6597, 0x6697, 0x6797, 0x6897,
            0x6997, 0x6A97, 0x6B97, 0x6C97, 0x6D97, 0x6E97, 0x6F97, 0x7097, 0x7197, 0x7297, 0x7397, 0x7497, 0x7597, 0x7697, 0x7797, 0x7897,
            0x7997, 0x7A97, 0x7B97, 0x7C97, 0x7D97, 0x7E97, 0x8097, 0x8197, 0x8297, 0x8397, 0x8497, 0x8597, 0x8697, 0x8797, 0x8897, 0x8997,
            0x8A97, 0x8B97, 0x8C97, 0x8D97, 0x8E97, 0x8F97, 0x9097, 0x9197, 0x9297, 0x9397, 0x9497, 0x9597, 0x9697, 0x9797, 0x9897, 0x9997,
            0x9A97, 0x9B97, 0x9C97, 0x9D97, 0x9E97, 0x9F97, 0xA097, 0xA197, 0xA297, 0xA397, 0xA497, 0xA597, 0xA697, 0xA797, 0xA897, 0xA997,
            0xAA97, 0xAB97, 0xAC97, 0xAD97, 0xAE97, 0xAF97, 0xB097, 0xB197, 0xB297, 0xB397, 0xB497, 0xB597, 0xB697, 0xB797, 0xB897, 0xB997,
            0xBA97, 0xBB97, 0xBC97, 0xBD97, 0xBE97, 0xBF97, 0xC097, 0xC197, 0xC297, 0xC397, 0xC497, 0xC597, 0xC697, 0xC797, 0xC897, 0xC997,
            0xCA97, 0xCB97, 0xCC97, 0xCD97, 0xCE97, 0xCF97, 0xD097, 0xD197, 0xD297, 0xD397, 0xD497, 0xD597, 0xD697, 0xD797, 0xD897, 0xD997,
            0xDA97, 0xDB97, 0xDC97, 0xDD97, 0xDE97, 0xDF97, 0xE097, 0xE197, 0xE297, 0xE397, 0xE497, 0xE597, 0xE697, 0xE797, 0xE897, 0xE997,
            0xEA97, 0xEB97, 0xEC97, 0xED97, 0xEE97, 0xEF97, 0xF097, 0xF197, 0xF297, 0xF397, 0xF497, 0xF597, 0xF697, 0xF797, 0xF897, 0xF997,
            0xFA97, 0xFB97, 0xFC97, 0x4098, 0x4198, 0x4298, 0x4398, 0x4498, 0x4598, 0x4698, 0x4798, 0x4898, 0x4998, 0x4A98, 0x4B98, 0x4C98,
            0x4D98, 0x4E98, 0x4F98, 0x5098, 0x5198, 0x5298, 0x5398, 0x5498, 0x5598, 0x5698, 0x5798, 0x5898, 0x5998, 0x5A98, 0x5B98, 0x5C98,
            0x5D98, 0x5E98, 0x5F98, 0x6098, 0x6198, 0x6298, 0x6398, 0x6498, 0x6598, 0x6698, 0x6798, 0x6898, 0x6998, 0x6A98, 0x6B98, 0x6C98,
            0x6D98, 0x6E98, 0x6F98, 0x7098, 0x7198, 0x7298
        };
    }
}
