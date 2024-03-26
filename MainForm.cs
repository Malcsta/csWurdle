using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Module5Library;
using Module5Library.Forms;

namespace Module5Wurdle
{
    public class MainForm : WurdleForm
    {
        private string solution;

        public MainForm() 
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.mnuNew.ShortcutKeys = (Keys)(Keys.Control | Keys.N);
            this.mnuExit.ShortcutKeys = (Keys)(Keys.Alt | Keys.F4);

            this.flpGuesses.BorderStyle = BorderStyle.FixedSingle;

            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach (char c in alphabet)
            {
                Label letterLabel = new Label();
                letterLabel.Name = c.ToString();
                letterLabel.Text = c.ToString();
                letterLabel.TextAlign = ContentAlignment.MiddleCenter;
                letterLabel.Font = new Font("Arial", 8.25f, FontStyle.Bold);
                letterLabel.Size = new Size(30, 30);
                letterLabel.BorderStyle = BorderStyle.FixedSingle;

                this.flpAlphabet.Controls.Add(letterLabel);
            }

            this.errorProvider.SetIconPadding(this.txtGuess, 3);

            this.btnEnter.Enabled = false;

            this.AcceptButton = this.btnEnter;

            this.Text = "Wurdle";

            StartGame();

            this.txtGuess.KeyPress += TxtGuess_KeyPress;
            this.txtGuess.TextChanged += TxtGuess_TextChanged;
            this.btnEnter.Click += BtnEnter_Click;

            this.mnuExit.Click += MnuExit_Click;
            this.mnuAbout.Click += MnuAbout_Click;
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void MnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {

            //Not in dictionary
            if (!WurdleDictionary.IsInDictionary(this.txtGuess.Text.ToLower()))
            {
                this.errorProvider.SetError(this.txtGuess, "Not in the word list.");
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtGuess_TextChanged(object sender, EventArgs e)
        {
            this.btnEnter.Enabled = this.txtGuess.Text.Length == 5;

            this.errorProvider.SetError(this.txtGuess, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtGuess_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartGame()
        {
            this.solution = Module5Library.WurdleDictionary.GetSolution();

            Debug.WriteLine(this.solution);
        }
    }
}
