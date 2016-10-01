using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TuringMachine.Core;
using TuringMachine.Core.Enums;
using TuringMachine.Forms;

namespace TuringMachine
{
    public partial class FMain : Form
    {
        int _Test = 0;
        int _Crash = 0;
        int _Fails = 0;
        FuzzerServer _Fuzzer;

        public FMain()
        {
            InitializeComponent();

            _Fuzzer = new FuzzerServer();
            _Fuzzer.OnInputsChange += _Fuzzer_OnInputsChange;
            _Fuzzer.OnConfigurationsChange += _Fuzzer_OnConfigurationsChange;
            _Fuzzer.OnListenChange += _Fuzzer_OnListenChange;
            _Fuzzer.OnTestEnd += _Fuzzer_OnTestEnd;

            ConfigureGrid(gridConfig);
            ConfigureGrid(gridInput);

            // Default values
            for (int x = 0; x < 100; x++)
            {
                AddToSerie(chart1.Series["Test"], 0);
                AddToSerie(chart1.Series["Crash"], 0);
                AddToSerie(chart1.Series["Fails"], 0);
            }

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
                    _Fuzzer.AddTcpQueryInput(dialog.EndPoint);
            }
        }
        void socketToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (EndPointDialog dialog = new EndPointDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    _Fuzzer.AddTcpProxyInput(dialog.EndPoint);
            }
        }
        void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ExecuteDialog dialog = new ExecuteDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    _Fuzzer.AddExecuteInput(dialog.FileName, dialog.Arguments);
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
        void _Fuzzer_OnTestEnd(object sender, ETestResult result)
        {
            _Test++;
            switch (result)
            {
                case ETestResult.Crash: _Crash++; break;
                case ETestResult.Fail: _Fails++; break;
            }
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            AddToSerie(chart1.Series["Test"], _Test);
            AddToSerie(chart1.Series["Crash"], _Crash);
            AddToSerie(chart1.Series["Fails"], _Fails);

            _Test = _Crash = _Fails = 0;
        }
        void AddToSerie(Series series, int r)
        {
            if (series.Points.Count > 100)
                series.Points.RemoveAt(0);

            series.Points.Add(r);
        }
        void tbPlay_Click(object sender, EventArgs e)
        {
            if (_Fuzzer.Play())
            {
                tbPlay.Enabled = false;
                tbPause.Enabled = true;
                tbStop.Enabled = true;
            }
        }
        void tbPause_Click(object sender, EventArgs e)
        {
            if (_Fuzzer.Pause())
            {
                tbPause.Enabled = false;
                tbPlay.Enabled = true;
            }
        }
        void tbStop_Click(object sender, EventArgs e)
        {
            if (_Fuzzer.Stop())
            {
                tbPlay.Enabled = true;
                tbPause.Enabled = false;
                tbStop.Enabled = false;
            }
        }
    }
}