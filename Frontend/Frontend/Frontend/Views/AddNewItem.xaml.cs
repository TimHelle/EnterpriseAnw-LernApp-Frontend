using App.Model.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewItem : TabbedPage
    {

        private static String getSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }


        public List<string> ListOfSelectedCategories = new List<string>();
        public AddNewItem()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            setPickerData();
            pickerCategory.ItemsSource = ListOfSelectedCategories;
        }

        #region Methoden für Buttons
        private void buttonSaveCategoryClicked(object sender, EventArgs e)
        {
            //Prüft ob alle Felder gefüllt sind
            if (entryCategoryTitel.Text != null && !entryCategoryTitel.Text.Trim().Equals(""))
            {
                if (entryCategoryTitel.Text.Length <= 50)
                {
                    Kategorien category = new Kategorien();
                    category.userCreated = true;
                    category.titel = entryCategoryTitel.Text;
                    category.beschreibung = entryCategoryDescribtion.Text;

                    //hash berechnen
                    String catText = category.titel + category.beschreibung;
                    Console.WriteLine("-------------------------");
                    Console.WriteLine(getSHA256Hash(catText).ToLower());
                    Console.WriteLine("-------------------------");

                    category.hash = getSHA256Hash(catText).ToLower();

                    Kategorien.kategorien.Add(category);
                    SQLiteHelper db = new SQLiteHelper();
                    db.AddCategoryToDatabase(category);
                    Navigation.PushAsync(new MainPage());
                }
            }
        }
        private void buttonSaveQuestionClicked(object sender, EventArgs e)
        {
            //Prüft ob alle Felder gefüllt sind
            if (entryQuestion.Text != null && !entryQuestion.Text.Trim().Equals("") &&
                pickerCategory.SelectedItem != null &&
                entryRightAnwser.Text != null && !entryRightAnwser.Text.Trim().Equals("") &&
                entryWrongAnwser_1.Text != null && !entryWrongAnwser_1.Text.Trim().Equals("") &&
                entryWrongAnwser_2.Text != null && !entryWrongAnwser_2.Text.Trim().Equals("") &&
                entryWrongAnwser_3.Text != null && !entryWrongAnwser_3.Text.Trim().Equals(""))
            {
                if (entryQuestion.Text.Length <= 128 &&
                   entryRightAnwser.Text.Length <= 128 &&
                   entryWrongAnwser_1.Text.Length <= 128 &&
                   entryWrongAnwser_2.Text.Length <= 128 &&
                   entryWrongAnwser_3.Text.Length <= 128)
                {
                    Fragen question = new Fragen();
                    Antworten a1 = new Antworten();
                    Antworten a2 = new Antworten();
                    Antworten a3 = new Antworten();
                    Antworten a4 = new Antworten();
                    question.setText(entryQuestion.Text);
                    a1.setText(entryRightAnwser.Text);
                    a1.setStatus(true);
                    question.setAntworten(a1);
                    a2.setText(entryWrongAnwser_1.Text);
                    a2.setStatus(false);
                    question.setAntworten(a2);
                    a3.setText(entryWrongAnwser_2.Text);
                    a3.setStatus(false);
                    question.setAntworten(a3);
                    a4.setText(entryWrongAnwser_3.Text);
                    a4.setStatus(false);
                    question.setAntworten(a4);
                    foreach (Kategorien k in Kategorien.kategorien)
                    {
                        if (k.titel.Equals(pickerCategory.SelectedItem))
                        {
                            question.setKategorie(k);
                        }
                    }
                    Model.Database.SQLiteHelper db = new Model.Database.SQLiteHelper();
                    question.userCreated = true;
                    //hash here
                    String questionText = question.getText() + question.getErklärung();
                    Console.WriteLine("-------------------------");
                    Console.WriteLine(getSHA256Hash(questionText).ToLower());
                    Console.WriteLine("-------------------------");
                    question.hash = getSHA256Hash(questionText).ToLower();



                    db.AddFrageToDatabase(question);
                    Navigation.PushAsync(new MainPage());
                }
                else
                {
                    DisplayAlert("Achtung", "Sie dürfen nicht mehr als 127 Zeichen benutzten!", "Okay");
                }
            }
            else
            {
                DisplayAlert("Achtung", "Sie müssen alle Felder mit Werten füllen, wenn Sie speichern wollen!", "Okay");
            }
        }
        protected override bool OnBackButtonPressed() => true;
        #endregion
        private void setPickerData()
        {
            foreach (var item in Kategorien.kategorien)
            {
                ListOfSelectedCategories.Add(item.titel);
            }
        }
    }
}