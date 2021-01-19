using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearnModePage : ContentPage
    {
        public Fragen currentQuestion = new Fragen();
        public List<int> randomNumberListOfAnswers = new List<int>();
        public int counter = Fragenkatalog.fragenAnzahl;

        public LearnModePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            labelCounterForQuestions.Text = "Verbleibende Fragen: " + (counter - 1);
            loadNextQuestion();
        }

        #region Methoden der Buttons
        private void buttonAnswerClicked(object sender, EventArgs e)
        {
            buttonAnswerA.BackgroundColor = Color.Red;
            buttonAnswerB.BackgroundColor = Color.Red;
            buttonAnswerC.BackgroundColor = Color.Red;
            buttonAnswerD.BackgroundColor = Color.Red;
            foreach (var item in currentQuestion.getAntwort())
            {
                if (item.getStatus() == true)
                {
                    if (item.getText().Equals(buttonAnswerA.Text))
                    {
                        buttonAnswerA.BackgroundColor = Color.Green;
                    }
                    if (item.getText().Equals(buttonAnswerB.Text))
                    {
                        buttonAnswerB.BackgroundColor = Color.Green;
                    }
                    if (item.getText().Equals(buttonAnswerC.Text))
                    {
                        buttonAnswerC.BackgroundColor = Color.Green;
                    }
                    if (item.getText().Equals(buttonAnswerD.Text))
                    {
                        buttonAnswerD.BackgroundColor = Color.Green;
                    }
                }
            }
            buttonExplanation.IsVisible = true;
            if (counter > 1)
            {
                NextButton.IsVisible = true;
            }
            else
            {
                NextButton.IsVisible = false;
            }
        }
        private void buttonExplanationClicked(object sender, EventArgs e)
        {
            if (currentQuestion.getErklärung() == null || currentQuestion.getErklärung().Equals(""))
            {
                DisplayAlert("Hinweis", "Es ist keine Erklärung für diese Frage hinterlegt.", "Okay");
            }
            else
            {
                DisplayAlert("Hinweis", currentQuestion.getErklärung(), "Okay");
            }
        }
        private void buttonNextQuestionClicked(object sender, EventArgs e)
        {
            if (counter > 1)
            {
                counter--;
                Fragenkatalog.katalog.Remove(currentQuestion);
                randomNumberListOfAnswers.Clear();
                buttonAnswerA.BackgroundColor = Color.Gray;
                buttonAnswerB.BackgroundColor = Color.Gray;
                buttonAnswerC.BackgroundColor = Color.Gray;
                buttonAnswerD.BackgroundColor = Color.Gray;
                loadNextQuestion();
                buttonExplanation.IsVisible = false;
                NextButton.IsVisible = false;
                labelCounterForQuestions.Text = "Verbleibende Fragen: " + (counter - 1);
            }
        }
        protected override bool OnBackButtonPressed() => true;
        #endregion

        private void randomNumbers()
        {
            int number;
            var rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                do
                {
                    number = rand.Next(0, 4);
                } while (randomNumberListOfAnswers.Contains(number));
                randomNumberListOfAnswers.Add(number);
            }
        }
        private void loadNextQuestion()
        {
            randomNumbers();
            currentQuestion = Fragenkatalog.katalog[counter - 1];
            labelQuestion.Text = currentQuestion.getText().ToString();
            buttonAnswerA.Text = currentQuestion.getAntwort()[randomNumberListOfAnswers[0]].getText();
            buttonAnswerB.Text = currentQuestion.getAntwort()[randomNumberListOfAnswers[1]].getText();
            buttonAnswerC.Text = currentQuestion.getAntwort()[randomNumberListOfAnswers[2]].getText();
            buttonAnswerD.Text = currentQuestion.getAntwort()[randomNumberListOfAnswers[3]].getText();
        }
    }
}