using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace TuringMachine.Forms
{
    public partial class EndPointDialog : Form
    {
        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        /// Request file
        /// </summary>
        public string RequestFile { get; private set; }
        /// <summary>
        /// Show RequestFile
        /// </summary>
        public bool ShowRequestFile { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndPointDialog()
        {
            InitializeComponent();

            tPort.Minimum = ushort.MinValue;
            tPort.Maximum = ushort.MaxValue;
        }
        void EndPointDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK) return;

            if (tRequest.Visible)
            {
                if (!string.IsNullOrEmpty(tRequest.Text))
                {
                    if (!File.Exists(tRequest.Text))
                    {
                        errorProvider1.SetError(button3, "File not found");
                        e.Cancel = true;
                        return;
                    }

                    RequestFile = tRequest.Text;
                }
            }

            IPAddress ip;
            if (!IPAddress.TryParse(tAddress.Text, out ip))
            {
                errorProvider1.SetError(tAddress, "Invalid ip address");
                e.Cancel = true;
                return;
            }

            if (tPort.Value > ushort.MaxValue || tPort.Value < ushort.MinValue)
            {
                errorProvider1.SetError(tPort, "Invalid port");
                e.Cancel = true;
                return;
            }

            EndPoint = new IPEndPoint(ip, Convert.ToInt32(tPort.Value));
        }
        void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            errorProvider1.Clear();
        }
        void EndPointDialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;

                        DialogResult = DialogResult.Cancel;
                        break;
                    }
                case Keys.Enter:
                    {
                        if (tRequest.Focused)
                        {
                            e.SuppressKeyPress = true;
                            e.Handled = true;
                            tAddress.Focus();
                        }
                        else
                        {
                            if (tPort.Focused)
                            {
                                e.SuppressKeyPress = true;
                                e.Handled = true;
                                tPort.Validate();

                                DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                if (tAddress.Focused)
                                {
                                    e.SuppressKeyPress = true;
                                    e.Handled = true;

                                    tPort.Focus();
                                    tPort.Select(0, tPort.Text.Length);
                                }
                            }
                        }
                        break;
                    }
            }
        }
        void EndPointDialog_Load(object sender, EventArgs e)
        {
            if (!ShowRequestFile)
            {
                label3.Visible = false;
                tRequest.Visible = false;
                Height -= tRequest.Height;
                button3.Visible = false;
            }
        }
        void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "All files|*.*",
                DefaultExt = "*.*"
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;

                tRequest.Text = dialog.FileName;
            }
        }
    }
}