using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConanExplorer.Conan;
using ConanExplorer.Conan.Filetypes;
using ConanExplorer.Conan.Script;

namespace ConanExplorer.Windows
{
    /// <summary>
    /// OUTDATED NOT IN USE
    /// </summary>
    public partial class ScriptAnalyseWindow : Form
    {
        private ScriptCollection _scriptFileCollection;
        private List<char> _allowedSymbols = new List<char> { '.', ',', '!', '?', '(', ')', ':' }; //make editable in GUI
        private FONTFile _fontFile;
        private PSXImage _modifiedImage;

        public ScriptAnalyseWindow(ScriptCollection scriptFileCollection)
        {
            InitializeComponent();
            _scriptFileCollection = scriptFileCollection;
            List<string> lines = ParseScripts();

            foreach (string line in lines)
            {
                richTextBox1.AppendText(line + "\n");
            }

            HashSet<string> dictionary = CreateDictionary(lines);

            richTextBox1.AppendText("\nDictionary( " + dictionary.Count + " Symbols ):");
            foreach (string characters in dictionary)
            {
                richTextBox1.AppendText(characters + "\n");
            }

            string[] unsortedDictionary = dictionary.ToArray();
            string[] sortedDictionary = dictionary.OrderBy(entry => entry).ToArray();

            if (ApplicationState.Instance.ProjectFile != null)
            {
                _modifiedImage = ApplicationState.Instance.ProjectFile.ModifiedImage;
            }

            if (_modifiedImage == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Conan Font File|FONT.BIN";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _fontFile = new FONTFile(openFileDialog.FileName);
                }
            }
            else
            {
                PKNFile graphPkn = _modifiedImage.PKNFiles?.Find(pkn => pkn.Name == "GRAPH");
                BaseFile fontFile = graphPkn?.Files?.Find(file => file.FileName == "FONT.BIN");
                if (fontFile == null) return;

                _fontFile = new FONTFile(fontFile.FilePath);
            }
            _fontFile.Load();

            _fontFile.Generate(unsortedDictionary, new Font("ConanFont", 12));
            _fontFile.Save();

        }

        public HashSet<string> CreateDictionary(List<string> lines)
        {
            //chop up the lines into 2 char entries
            HashSet<string> result = new HashSet<string>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                int counter = 0;
                string chunk = "";
                for (int i = 0; i < line.Length; i++)
                {
                    //if (IsValidChar(line[i]))
                    //{
                    //    chunk += line[i];
                    //    counter++;
                    //}

                    if (i == line.Length - 1 && counter == 1)
                    {
                        result.Add("" + line[i] + " ");
                    }

                    if (counter == 2)
                    {
                        result.Add(chunk);
                        chunk = "";
                        counter = 0;
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// Splits all the scripts text parts from the logic parts
        /// </summary>
        /// <returns></returns>
        public List<string> ParseScripts()
        {
            List<string> result = new List<string>();

            foreach (ScriptDocument script in _scriptFileCollection.Scripts)
            {
                bool selectWindow = false;
                bool open = false;
                foreach (string line in script.TextBuffer.Split('\n'))
                {
                    if (line.StartsWith("#MESSAGE:"))
                    {
                        open = true;
                    }
                    if (open)
                    {
                        if (line == "SPEECH:2,2") { continue; }
                        if (line == "")
                        {
                            open = false;
                            continue;
                        }
                        if (!line.StartsWith("#"))
                        {
                            if (line.StartsWith("%END:"))
                            {
                                selectWindow = false;
                                open = false;
                                continue;
                            }
                            if (line.StartsWith("%SEL:"))
                            {
                                selectWindow = true;
                                continue;
                            }

                            if (selectWindow)
                            {
                                result.Add(line.Split(',')[2]);
                            }
                            else
                            {
                                string tmp = "";
                                bool gate = true;
                                foreach (char character in line)
                                {
                                    if (character == '%') gate = false;
                                    if (gate) tmp += character;
                                    if (character == ':') gate = true;
                                }
                                result.Add(tmp);
                            }
                        }
                    }
                    if (line.EndsWith("%ME:") || line.EndsWith("%END:")) open = false;
                }
            }
            return result;
        }
    }
}
