using System.Windows.Forms;

namespace TuringMachine.Forms
{
    public partial class ExecuteDialog : Form
    {
        /// <summary>
        /// FileName
        /// </summary>
        public string FileName { get; internal set; }
        /// <summary>
        /// Arguments
        /// </summary>
        public string Arguments { get; internal set; }

        public ExecuteDialog()
        {
            InitializeComponent();

            textBox1.Text = "";
            textBox2.Text = "";
        }
        void EndPointDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK) return;

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "FileName required");
                e.Cancel = true;
                return;
            }

            FileName = textBox1.Text;
            Arguments = textBox2.Text;
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
                        if (textBox2.Focused)
                        {
                            e.SuppressKeyPress = true;
                            e.Handled = true;

                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            if (textBox1.Focused)
                            {
                                e.SuppressKeyPress = true;
                                e.Handled = true;

                                textBox2.Focus();
                                textBox2.SelectAll();
                            }
                        }
                        break;
                    }
            }
        }
    }
}