using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TuringMachine.Core;
using TuringMachine.Forms;

namespace TuringMachine
{
    public partial class FMain : Form
    {
        Fuzzer _Fuzzer;

        public FMain()
        {
            InitializeComponent();

            _Fuzzer = new Fuzzer();
            _Fuzzer.OnInputsChange += _Fuzzer_OnInputsChange;
            _Fuzzer.OnConfigurationsChange += _Fuzzer_OnConfigurationsChange;
            _Fuzzer.OnListenChange += _Fuzzer_OnListenChange;

            ConfigureGrid(gridConfig);
            ConfigureGrid(gridInput);

            _Fuzzer_OnListenChange(null, null);
        }
        void _Fuzzer_OnListenChange(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = _Fuzzer.Listen.ToString();
            toolStripStatusLabel2.ForeColor = Color.Black;
        }
        void ConfigureGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
        }
        void _Fuzzer_OnConfigurationsChange(object sender, EventArgs e)
        {
            gridConfig.DataSource = _Fuzzer.Configurations.ToArray();
            toolStripLabel3.Text = "Configurations" + (_Fuzzer.Configurations.Count <= 0 ? "" : " (" + _Fuzzer.Configurations.Count.ToString() + ")");
        }
        void _Fuzzer_OnInputsChange(object sender, EventArgs e)
        {
            gridInput.DataSource = _Fuzzer.Inputs.ToArray();
            toolStripLabel1.Text = "Inputs" + (_Fuzzer.Inputs.Count <= 0 ? "" : " (" + _Fuzzer.Inputs.Count.ToString() + ")");
        }
        void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = "*",
                Filter = "All files|*"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    foreach (string s in dialog.FileNames)
                        _Fuzzer.AddFileInput(s);
            }
        }
        void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    foreach (FileInfo s in new DirectoryInfo(dialog.SelectedPath).GetFiles())
                        _Fuzzer.AddFileInput(s.FullName);
            }
        }
        void socketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EndPointDialog dialog = new EndPointDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    _Fuzzer.AddTcpInput(dialog.EndPoint);
            }
        }
        void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = "*",
                Filter = "Turing Machine Mutation files|*.fmut"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    foreach (string file in dialog.FileNames)
                        _Fuzzer.AddConfig(file);
            }
        }
        void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    foreach (FileInfo s in new DirectoryInfo(dialog.SelectedPath).GetFiles())
                        _Fuzzer.AddConfig(s.FullName);
            }
        }
        void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            using (EndPointDialog dialog = new EndPointDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    try { _Fuzzer.Listen = dialog.EndPoint; }
                    catch
                    {
                        toolStripStatusLabel2.ForeColor = Color.Red;
                    }
            }
        }
    }
}