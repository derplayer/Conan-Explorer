using ConanExplorer.Conan;
using ConanExplorer.Conan.Script;
using ConanExplorer.Conan.Script.Elements;
using ConanExplorer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.Windows
{
    public partial class SearchCommandWindow : Form
    {
        private ScriptFile _scriptFile;
        private SortableBindingList<CommandViewModel> _commands = new SortableBindingList<CommandViewModel>();

        public SearchCommandWindow(ScriptFile scriptFile)
        {
            _scriptFile = scriptFile;
            InitializeComponent();
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            dataGridView_Result.DataSource = null;
            _commands.Clear();
            foreach (ScriptDocument scriptDocument in _scriptFile.Scripts)
            {
                List<IScriptElement> elements = ScriptParser.Parse(scriptDocument);

                foreach (IScriptElement element in elements)
                {
                    if (element.GetType() == typeof(ScriptCommand))
                    {
                        ScriptCommand command = (ScriptCommand)element;
                        if (command.Name == textBox_Search.Text)
                        {
                            _commands.Add(new CommandViewModel(command, scriptDocument));
                        }
                    }
                }
            }
            dataGridView_Result.DataSource = _commands;
            foreach (DataGridViewColumn column in dataGridView_Result.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }
    }

    public class CommandViewModel
    {
        public string Script { get; private set; }
        public string Name { get; private set; }
        public string Parameters { get; private set; }

        public CommandViewModel(ScriptCommand command, ScriptDocument scriptDocument)
        {
            Script = scriptDocument.Name;
            Name = command.Name;
            Parameters = command.DisplayParameters;
        }
    }
}
