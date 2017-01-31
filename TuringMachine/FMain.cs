using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TuringMachine.Core;
using TuringMachine.Core.Enums;
using TuringMachine.Core.Inputs;
using TuringMachine.Core.Interfaces;
using TuringMachine.Forms;
using TuringMachine.Generator;

namespace TuringMachine
{
    public partial class FMain : Form
    {
        class dummyStat : IType { public string Type { get { return "Dummy"; } } }

        FuzzerStat<dummyStat> _Stat = new FuzzerStat<dummyStat>(null);
        TuringServer _Fuzzer;
        Font GridBoldFont;

        /// <summary>
        /// Allow edit when is playing
        /// </summary>
        const bool AllowHotEdit = true;

        public FMain()
        {
            InitializeComponent();

            // Create fuzzer
            _Fuzzer = new TuringServer(this);
            _Fuzzer.Inputs.ListChanged += _Fuzzer_OnInputsChange;
            _Fuzzer.Configurations.ListChanged += _Fuzzer_OnConfigurationsChange;
            _Fuzzer.OnListenChange += _Fuzzer_OnListenChange;
            _Fuzzer.OnTestEnd += _Fuzzer_OnTestEnd;
            _Fuzzer.OnCrashLog += _Fuzzer_OnCrashLog;

            chart1.ChartAreas[0].AxisY.Maximum = double.NaN;

            // Configure grids
            gridConfig.AutoGenerateColumns = false;
            gridInput.AutoGenerateColumns = false;
            gridLog.AutoGenerateColumns = false;
            GridBoldFont = new Font(gridLog.Font, FontStyle.Bold);

            // Default values
            for (int x = 0; x < 100; x++)
            {
                AddToSerie(chart1.Series["Test"], 0);
                AddToSerie(chart1.Series["Crash"], 0);
                AddToSerie(chart1.Series["Fails"], 0);
            }

            _Fuzzer_OnListenChange(null, null);

            gridInput.DataSource = new BindingSource() { DataSource = _Fuzzer.Inputs };
            gridConfig.DataSource = new BindingSource() { DataSource = _Fuzzer.Configurations };
            gridLog.DataSource = new BindingSource() { DataSource = _Fuzzer.Logs };
        }
        void _Fuzzer_OnPercentFactor(FuzzingStream stream, ref double percentFactor)
        {
            // TODO: Extract percentage in secuence
            //percentFactor = uPercentWave1.GetPercentFactor(stream);
        }
        void _Fuzzer_OnListenChange(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = _Fuzzer.Listen.ToString();
            toolStripStatusLabel2.ForeColor = Color.Black;
        }
        #region Refresh bars
        void _Fuzzer_OnCrashLog(object sender, FuzzerLog log)
        {
            if (InvokeRequired)
            {
                Invoke(new TuringServer.delOnCrashLog(_Fuzzer_OnCrashLog), sender, log);
                return;
            }
            toolStripLabel2.Text = "Crashes" + (_Fuzzer.Logs.Count <= 0 ? "" : " (" + _Fuzzer.Logs.Count.ToString() + ")");
        }
        void _Fuzzer_OnConfigurationsChange(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(_Fuzzer_OnConfigurationsChange), sender, e);
                return;
            }
            toolStripLabel3.Text = "Configurations" + (_Fuzzer.Configurations.Count <= 0 ? "" : " (" + _Fuzzer.Configurations.Count.ToString() + ")");
        }
        void _Fuzzer_OnInputsChange(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(_Fuzzer_OnInputsChange), sender, e);
                return;
            }
            toolStripLabel1.Text = "Inputs" + (_Fuzzer.Inputs.Count <= 0 ? "" : " (" + _Fuzzer.Inputs.Count.ToString() + ")");
        }
        #endregion
        #region Add Inputs
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
                        _Fuzzer.AddInput(new FileFuzzingInput(s));
            }
        }
        void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    foreach (FileInfo s in new DirectoryInfo(dialog.SelectedPath).GetFiles())
                    {
                        if (File.Exists(s.FullName))
                            _Fuzzer.AddInput(new FileFuzzingInput(s.FullName));
                    }
            }
        }
        void socketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EndPointDialog dialog = new EndPointDialog() { ShowRequestFile = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.RequestFile) || File.Exists(dialog.RequestFile))
                        _Fuzzer.AddInput(new TcpQueryFuzzingInput(dialog.EndPoint, dialog.RequestFile));
                }
            }
        }
        void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ExecuteDialog dialog = new ExecuteDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    _Fuzzer.AddInput(new ExecutionFuzzingInput(dialog.FileName, dialog.Arguments));
            }
        }
        void manualStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Fuzzer.AddInput(new EmptyFuzzingInput());
        }
        void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long from, to;

            using (LongDialog dialog = new LongDialog()
            {
                Text = "From",
                Title = "Stream length:",
                MinValue = 0,
                MaxValue = long.MaxValue,
                Input = 1024 * 1024
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                from = dialog.Input;
            }

            using (LongDialog dialog = new LongDialog()
            {
                Text = "To",
                Title = "Stream length:",
                MinValue = from,
                MaxValue = long.MaxValue,
                Input = 1024 * 1024 * 10
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                to = dialog.Input;
            }

            _Fuzzer.AddInput(new RandomFuzzingInput(new FromToValue<long>(from, to)));
        }
        #endregion
        #region Add Configs
        void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = "*.fmut;*.fpatch",
                Filter = "All fuzzing files|*.fmut;*.fpatch|Mutational fuzzing file|*.fmut|Patch fuzzing file|*.fpatch",
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
        #endregion
        #region Test End
        void _Fuzzer_OnTestEnd(object sender, EFuzzingReturn result, FuzzerStat<IFuzzingInput>[] sinput, FuzzerStat<IFuzzingConfig>[] sconfig)
        {
            _Stat.Increment(result);
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            AddToSerie(chart1.Series["Test"], _Stat.Tests);
            AddToSerie(chart1.Series["Crash"], _Stat.Crashes);
            AddToSerie(chart1.Series["Fails"], _Stat.Fails);

            chart1.ChartAreas[0].RecalculateAxesScale();
            _Stat.Reset();

            gridInput.Invalidate();
            gridConfig.Invalidate();
        }
        void AddToSerie(Series series, int r)
        {
            if (series.Points.Count > 100)
                series.Points.RemoveAt(0);

            series.Points.Add(r);
        }
        #endregion
        void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            using (EndPointDialog dialog = new EndPointDialog()
            {
                EndPoint = _Fuzzer == null ? null : _Fuzzer.Listen
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    try { _Fuzzer.Listen = dialog.EndPoint; }
                    catch
                    {
                        toolStripStatusLabel2.ForeColor = Color.Red;
                    }
            }
        }
        void tbStop_Click(object sender, EventArgs e)
        {
            if (_Fuzzer.Stop())
            {
                tbPlay.Enabled = true;
                tbPause.Enabled = false;
                tbStop.Enabled = false;
                toolStripStatusLabel2.Enabled = true;

                toolStripDropDownButton1.Enabled = true;
                toolStripDropDownButton2.Enabled = true;

                gridInput_SelectionChanged(null, null);
                gridConfig_SelectionChanged(null, null);
            }
        }
        void tbPlay_Click(object sender, EventArgs e)
        {
            if (_Fuzzer.Start())
            {
                tbPlay.Enabled = false;
                tbPause.Enabled = true;
                tbStop.Enabled = true;
                toolStripStatusLabel2.Enabled = false;

                if (!AllowHotEdit)
                {
                    toolStripDropDownButton1.Enabled = false;
                    toolStripDropDownButton2.Enabled = false;
                }

                gridInput_SelectionChanged(null, null);
                gridConfig_SelectionChanged(null, null);
            }
        }
        void tbPause_Click(object sender, EventArgs e)
        {
            if (_Fuzzer.Pause())
            {
                tbPause.Enabled = false;
                tbPlay.Enabled = true;

                toolStripDropDownButton1.Enabled = true;
                toolStripDropDownButton2.Enabled = true;

                gridInput_SelectionChanged(null, null);
                gridConfig_SelectionChanged(null, null);
            }
        }
        void toolStripButton1_Click(object sender, EventArgs e)
        {
            RemoveSelected(gridInput, _Fuzzer.Inputs);
        }
        void toolStripButton2_Click(object sender, EventArgs e)
        {
            RemoveSelected(gridConfig, _Fuzzer.Configurations);
        }
        void RemoveSelected(DataGridView grid, IList collection)
        {
            if (!AllowHotEdit && _Fuzzer.State != EFuzzerState.Stopped) return;

            lock (collection)
            {
                List<object> l = new List<object>();
                foreach (DataGridViewRow r in grid.SelectedRows)
                    l.Add(r.DataBoundItem);

                foreach (object o in l)
                    collection.Remove(o);
            }
        }
        void gridInput_SelectionChanged(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = gridInput.SelectedRows.Count > 0 && (AllowHotEdit || _Fuzzer.State != EFuzzerState.Started);
        }
        void gridConfig_SelectionChanged(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = gridConfig.SelectedRows.Count > 0 && (AllowHotEdit || _Fuzzer.State != EFuzzerState.Started);
        }
        void originalInputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem it = (ToolStripItem)sender;
            SaveSelectedInputWith(it.Tag == null ? null : ((FuzzerStat<IFuzzingConfig>)it.Tag).Source);
        }
        void SaveSelectedInputWith(IFuzzingConfig config)
        {
            if (gridInput.SelectedRows.Count != 1) return;

            FuzzerStat<IFuzzingInput> inp = (FuzzerStat<IFuzzingInput>)gridInput.SelectedRows[0].DataBoundItem;
            if (inp == null) return;

            using (SaveFileDialog s = new SaveFileDialog()
            {
                Filter = "Dat file|*.dat",
                DefaultExt = "*.dat",
            })
            {
                if (s.ShowDialog() != DialogResult.OK) return;

                try
                {
                    if (File.Exists(s.FileName))
                        File.Delete(s.FileName);

                    using (FileStream fs = File.OpenWrite(s.FileName))
                    {
                        byte[] stream = inp.Source.GetStream();
                        if (config != null)
                        {
                            using (Stream fzs = new FuzzingStream(stream, config))
                                fzs.CopyTo(fs);
                        }
                        else
                        {
                            fs.Write(stream, 0, stream.Length);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void saveInputWithToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            // Clear
            for (int x = saveInputWithToolStripMenuItem.DropDownItems.Count - 2; x >= 0; x--)
                saveInputWithToolStripMenuItem.DropDownItems.RemoveAt(x);

            // Add items
            foreach (FuzzerStat<IFuzzingConfig> c in _Fuzzer.Configurations)
            {
                ToolStripItem it = new ToolStripMenuItem(c.Source.ToString());
                saveInputWithToolStripMenuItem.DropDownItems.Insert(0, it);
                it.Tag = c;
                it.Click += originalInputToolStripMenuItem_Click;
            }

            // Separator
            if (saveInputWithToolStripMenuItem.DropDownItems.Count != 1)
                saveInputWithToolStripMenuItem.DropDownItems.Insert(saveInputWithToolStripMenuItem.DropDownItems.Count - 1, new ToolStripSeparator());
        }
        void gridInput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            int currentMouseOverRow = gridInput.HitTest(e.X, e.Y).RowIndex;
            if (currentMouseOverRow < 0) return;

            contextMenuStrip1.Show(gridInput, new Point(e.X, e.Y));
        }
        void gridLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridLog.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                FuzzerLog log = (FuzzerLog)gridLog.Rows[e.RowIndex].DataBoundItem;
                if (File.Exists(log.Path))
                    Process.Start(log.Path);
            }
        }
        void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            tbStop_Click(sender, e);
        }
        void gridLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = gridLog.Rows[e.RowIndex];
            DataGridViewCell c = row.Cells["cExploitable"];
            string v = c.Value.ToString();

            if (v == "UNKNOWN")
            {
                c.Style.ForeColor = Color.Black;
                c.Style.Font = gridLog.Font;
                c.Style.SelectionBackColor = row.DefaultCellStyle.SelectionBackColor;
                c.Style.SelectionForeColor = row.DefaultCellStyle.SelectionForeColor;
            }
            else
            {
                c.Style.BackColor = Color.Yellow;
                c.Style.ForeColor = Color.Red;
                c.Style.Font = GridBoldFont;

                c.Style.SelectionBackColor = Color.Orange;
                c.Style.SelectionForeColor = Color.Brown;
            }
        }
        void generatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FGenerator f = new FGenerator();
            f.Show();
        }
        void gridInput_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        void gridInput_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                object d = e.Data.GetData(DataFormats.FileDrop);
                if (d == null) return;

                string file = (d is string[]) ? ((string[])d).FirstOrDefault() : d.ToString();
                if (File.Exists(file))
                {
                    if (sender == gridConfig) _Fuzzer.AddConfig(file);
                    else if (sender == gridInput) _Fuzzer.AddInput(new FileFuzzingInput(file));
                }
            }
        }
    }
}