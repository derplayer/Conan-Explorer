using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan
{
    public class FontSettings
    {
        public List<char> AllowedSymbols = new List<char>();
        public List<char> AllowedSplittedSymbols = new List<char>();
        public string FontName { get; set; }
        public int FontSize { get; set; } = 2;

        public static FontSettings ASCII()
        {
            FontSettings settings = new FontSettings();
            settings.AllowedSplittedSymbols = ("!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~").ToList();
            settings.FontName = "Quarlow";
            settings.FontSize = 12;
            return settings;
        }

        public static FontSettings Latin1()
        {
            FontSettings settings = new FontSettings();
            settings.AllowedSplittedSymbols = ("!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~" +
                                               "¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ").ToList();
            settings.FontName = "Quarlow";
            settings.FontSize = 12;
            return settings;
        }

        public static FontSettings DE()
        {
            FontSettings settings = new FontSettings();
            settings.AllowedSplittedSymbols = ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZöäüÖÄÜß\"&.,-':!?()1234567890").ToList();
            settings.FontName = "Quarlow";
            settings.FontSize = 12;
            return settings;
        }
    }
}
