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
        private int guessCount;
        private const int MaximumAllowableGuesses = 6;

        public MainForm() 
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.DimGray;

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
                letterLabel.ForeColor = Color.White;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnter_Click(object sender, EventArgs e)
        {
            string guess = this.txtGuess.Text.ToUpper();
            string solution = this.solution.ToUpper();

            //Word is not in dictionary
            if (!WurdleDictionary.IsInDictionary(guess))
            {
                this.errorProvider.SetError(this.txtGuess, "Not in the word list.");
                return;
            }

            this.guessCount += 1;

            foreach (char guessedLetter in guess)
            {
                Control[] controls = this.flpAlphabet.Controls.Find(guessedLetter.ToString(), true);
                if (controls.Length > 0 )
                {
                    Label letterLabel = (Label)controls[0];
                    letterLabel.Enabled = false;    
                }
            }

            //Word is in dictionary
            for (int index = 0; index < guess.Length; index++)
            {
                Color backgroundColor = Color.Gray;

                //Is the current guess letter equal to the current solution letter
                
                if (guess[index] == solution[index])
                {
                    backgroundColor = Color.Green;
                }
                else
                {
                    int searchIndex = solution.IndexOf(guess[index]);

                    if(searchIndex != -1)
                    {
                        backgroundColor = Color.Orange;
                    }
                }

                AddGuessLetterToTheGuessPanel(guess[index], backgroundColor);
               
            }

            this.txtGuess.Text = string.Empty;

            if (guess.Equals(solution))
            {
                MessageBox.Show("Congratulations!", this.Text);
                this.txtGuess.Enabled = false;
            }
            else if (this.guessCount == MaximumAllowableGuesses)
            {
                MessageBox.Show($"The word is {solution}.", this.Text);
                this.txtGuess.Enabled = false;
            }
        }

        /// <summary>
        /// add documentation later .. 
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

            this.guessCount = 0;    

            Debug.WriteLine(this.solution);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="backgroundColor"></param>
        private void AddGuessLetterToTheGuessPanel(char letter, Color backgroundColor)
        {
            Label letterLabel = new Label();
            letterLabel.Name = letter.ToString();
            letterLabel.Text = letter.ToString();
            letterLabel.TextAlign = ContentAlignment.MiddleCenter;
            letterLabel.Font = new Font("Arial", 20f, FontStyle.Bold);
            letterLabel.Size = new Size(50, 50);
            letterLabel.BorderStyle = BorderStyle.FixedSingle;
            letterLabel.ForeColor = Color.White;    
            letterLabel.BackColor = backgroundColor;
            this.flpGuesses.Controls.Add(letterLabel);
        }
    }
}
