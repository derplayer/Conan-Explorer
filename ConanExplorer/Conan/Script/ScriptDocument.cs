using System.ComponentModel;
using System.IO;
using System.Text;
using ConanExplorer.Conan.Filetypes;

namespace ConanExplorer.Conan.Script
{
    public class ScriptDocument : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Encoding _shiftJist = Encoding.GetEncoding("shift_jis");
        private string _textBuffer = string.Empty;

        public BaseFile BaseFile { get; set; }

        public string TextBuffer
        {
            get { return _textBuffer; }
            set
            {
                _textBuffer = value;
                OnPropertyChanged("TextBuffer");
            }
        }

        public ScriptDocument() {}

        public ScriptDocument(BaseFile baseFile)
        {
            BaseFile = baseFile;

            string filePath;
            if (baseFile.GetType() == typeof(LZBFile))
            {
                LZBFile lzbFile = (LZBFile)baseFile;
                if (lzbFile.DecompressedFile == null) { lzbFile.Decompress(); }
                filePath = lzbFile.DecompressedFile.FilePath;
            }
            else
            {
                filePath = baseFile.FilePath;
            }

            using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                byte[] charArray = new byte[reader.BaseStream.Length];
                reader.Read(charArray, 0, charArray.Length);
                TextBuffer = _shiftJist.GetString(charArray);
                TextBuffer = TextBuffer.Trim('\0');
            }
        }

        public void WriteToOriginalFile()
        {
            string outputPath = BaseFile.FilePath;
            if (BaseFile.GetType() == typeof (LZBFile))
            {
                LZBFile lzbFile = (LZBFile) BaseFile;
                outputPath = lzbFile.DecompressedFile.FilePath;
            }
            using (BinaryWriter writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create)))
            {
                writer.Write(_shiftJist.GetBytes(TextBuffer));
                if (BaseFile.GetType() != typeof(LZBFile))
                {
                    long rest = 2048 - writer.BaseStream.Length % 2048;
                    for (int i = 0; i < rest; i++)
                    {
                        writer.Write('\0');
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return BaseFile.FileName;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
