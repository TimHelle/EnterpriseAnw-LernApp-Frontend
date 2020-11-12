using App.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            labelQuestions.Text = Fragen.fragen.Count.ToString();
            labelCategories.Text = Kategorien.kategorien.Count.ToString();
            //if (Fragen.fragen.Count == 0)
            //{
            //    buttonLoadDataFromCloudDB.IsEnabled = true;
            //}
            //else if (Fragen.fragen.Count > 0)
            //{
            //    buttonLoadDataFromCloudDB.IsEnabled = false;
            //}
        }

        private async void loadDataFromCloudDBClicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("Achtung","Möchten Sie das alle Daten auf dem Handy mit den Daten aus der Cloud überschrieben werden?","Yes","No");
            if(action == true)
            {
                try
                {
                    Model.Database.SQLiteHelper db = new Model.Database.SQLiteHelper();
                    db.initializeSQLiteDatabase();
                    db.initializeSavedSettings();
                    db.wipeDataAndFillFromRemote();
                    var test = SQLiteHelper.getAllFragen();
                    DisplayAlert("Fragen", "" + SQLiteHelper.getAllFragen().Count(), "Okay");
                    DisplayAlert("Kategorien", "" + db.getAllKategorien().Count(), "Okay");
                }
                catch (Exception ex)
                {
                    DisplayAlert("Fehler", "Es konnten keine Daten geladen werden!", "Okay");
                }
            }
        }
        protected override bool OnBackButtonPressed() => true;
    }
}