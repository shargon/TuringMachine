using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TuringMachine.Core.FuzzingMethods.Mutational;
using TuringMachine.Core.FuzzingMethods.Patchs;
using TuringMachine.Core.Interfaces;

namespace TuringMachine.Generator
{
    public partial class FMain : Form
    {
        string _LastFile = "";
        IFuzzingConfig _Cur;

        public FMain()
        {
            InitializeComponent();
            NewFile();
        }
        void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }
        void newToolStripMenuItem_Click(object sender, EventArgs e) { NewFile(); }
        void saveToolStripMenuItem_Click(object sender, EventArgs e) { Save(_LastFile); }
        void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "All fuzzing files|*.fmut;*.fpatch|Mutational fuzzing file|*.fmut|Patch fuzzing file|*.fpatch",
                DefaultExt = "*.fmut",
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    Save(dialog.FileName);
            }
        }
        void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "All fuzzing files|*.fmut;*.fpatch|Mutational fuzzing file|*.fmut|Patch fuzzing file|*.fpatch",
                DefaultExt = "*.fmut;*.fpatch",
                CheckFileExists = true,
                CheckPathExists = true
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    LoadFile(dialog.FileName);
            }
        }
        void LoadFile(string file)
        {
            _Cur = null;
            switch (Path.GetExtension(file).ToLowerInvariant())
            {
                case ".fmut":
                    {
                        _Cur = MutationConfig.FromJson(File.ReadAllText(file, Encoding.UTF8));
                        break;
                    }
                case ".fpatch":
                    {
                        _Cur = PatchConfig.FromJson(File.ReadAllText(file, Encoding.UTF8));
                        break;
                    }
            }

            if (_Cur != null)
            {
                propertyGrid1.SelectedObject = _Cur;
                propertyGrid1.ExpandAllGridItems();
                _LastFile = file;

                saveToolStripMenuItem.Enabled = true;
            }
        }
        void NewFile()
        {
            saveToolStripMenuItem.Enabled = false;
            _LastFile = "";

            MutationConfig c = new MutationConfig();
            c.Mutations.Add(new MutationalOffset());
            _Cur = c;
            propertyGrid1.SelectedObject = _Cur;
            propertyGrid1.ExpandAllGridItems();
        }
        void Save(string file)
        {
            string json = _Cur.ToJson();
            File.WriteAllText(file, json, Encoding.UTF8);
            _LastFile = file;
            saveToolStripMenuItem.Enabled = true;
        }
        void FMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;

                        Close();
                        break;
                    }
            }
        }
    }
}