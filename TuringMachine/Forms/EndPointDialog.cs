using System;
using System.Net;
using System.Windows.Forms;

namespace TuringMachine.Forms
{
    public partial class EndPointDialog : Form
    {
        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint EndPoint { get; internal set; }

        public EndPointDialog()
        {
            InitializeComponent();

            numericUpDown1.Minimum = ushort.MinValue;
            numericUpDown1.Maximum = ushort.MaxValue;
        }
        void EndPointDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK) return;

            IPAddress ip;
            if (!IPAddress.TryParse(textBox1.Text, out ip))
            {
                errorProvider1.SetError(textBox1, "Invalid ip address");
                e.Cancel = true;
                return;
            }

            if (numericUpDown1.Value > ushort.MaxValue || numericUpDown1.Value < ushort.MinValue)
            {
                errorProvider1.SetError(textBox1, "Invalid port");
                e.Cancel = true;
                return;
            }

            EndPoint = new IPEndPoint(ip, Convert.ToInt32(numericUpDown1.Value));
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
                        if (numericUpDown1.Focused)
                        {
                            e.SuppressKeyPress = true;
                            e.Handled = true;
                            numericUpDown1.Validate();

                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            if (textBox1.Focused)
                            {
                                e.SuppressKeyPress = true;
                                e.Handled = true;

                                numericUpDown1.Focus();
                                try { numericUpDown1.Select(0, numericUpDown1.Value.ToString().Length); } catch { }
                            }
                        }
                        break;
                    }
            }
        }
    }
}