using System;
using System.Windows.Forms;

namespace TuringMachine.Forms
{
    public partial class LongDialog : Form
    {
        /// <summary>
        /// EndPoint
        /// </summary>
        public long Input { get; set; }

        /// <summary>
        /// Min Value
        /// </summary>
        public long MinValue { get; set; }
        /// <summary>
        /// Max Value
        /// </summary>
        public long MaxValue { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public LongDialog()
        {
            InitializeComponent();

            MinValue = 0;
            MaxValue = long.MaxValue;
            Title = "";
        }
        void EndPointDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK) return;

            if (tPort.Value > MaxValue || tPort.Value < MinValue)
            {
                errorProvider1.SetError(tPort, "Invalid input");
                e.Cancel = true;
                return;
            }

            Input = Convert.ToInt64(tPort.Value);
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
                        if (tPort.Focused)
                        {
                            e.SuppressKeyPress = true;
                            e.Handled = true;
                            tPort.Validate();

                            DialogResult = DialogResult.OK;
                        }
                        break;
                    }
            }
        }
        void EndPointDialog_Load(object sender, EventArgs e)
        {
            tPort.Minimum = MinValue;
            tPort.Maximum = MaxValue;
            label2.Text = Title;
            tPort.Value = Input;
            tPort.Select(0, tPort.Text.Length);
        }
    }
}