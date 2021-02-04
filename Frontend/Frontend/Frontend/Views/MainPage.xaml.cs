using App.Views;
using Frontend.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        //Funktion wird ausgeführt wenn die Seite angezeigt wird
        protected override void OnAppearing()
        {
            Model.Database.SQLiteHelper db = new Model.Database.SQLiteHelper();
            db.initializeSQLiteDatabase();
            db.initializeSavedSettings();
            NumberOfCategoriePicker.SelectedItem = null;
            NumberOfCategoriePicker.ItemsSource = null;
            Fragenkatalog.katalog.Clear();
            Kategorien.auswahl.Clear();
            ListViewOfCategorieItems.ItemsSource = Kategorien.kategorien;
        }

        #region Methoden für die Buttons
        private void ButtonStartLearnMode(object sender, EventArgs e)
        {
            if (Kategorien.auswahl.Count > 0 && NumberOfCategoriePicker.SelectedItem != null)
            {
                Fragenkatalog.katalog = new Collection<Fragen>(Fragenkatalog.katalog.OrderBy(x => Guid.NewGuid()).ToList());
                Navigation.PushAsync(new LearnModePage());
            }
            else
            {
                DisplayAlert("Achtung", "Sie müssen ein Modul und eine Anzahl gewählt haben um den Modus zu starten.", "Okay");
            }
        }
        private void ButtonAddNewItem(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddNewItem());
        }
        private void ButtonSettingPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Settings());
        }
        private void ButtonDetailModus_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestionDetail());
        }
        protected override bool OnBackButtonPressed() => true;
        #endregion

        #region Methoden für die Switch
        private void SwitchToggled(object sender, ToggledEventArgs e)
        {
            var senderAsSwitch = (Switch)sender;
            Label label = senderAsSwitch.Parent.FindByName<Label>("labelItem");
            setSeletion(label.Text, senderAsSwitch);
            Fragenkatalog.katalog.Clear();
            foreach (Fragen frage in Fragen.fragen)
            {
                foreach (Kategorien kategorie in Kategorien.auswahl)
                {
                    foreach (Kategorien fragenKategorie in frage.getKategorie())
                    {
                        if (fragenKategorie.hash.Equals(kategorie.hash))
                        {
                            Fragenkatalog.katalog.Add(frage);
                        }
                    }
                }
            }
            NumberOfCategoriePicker.ItemsSource = initializePickerForCategorieNumbers();
        }
        private void setSeletion(string name, Switch senderSwitch)
        {
            if (senderSwitch.IsToggled == true)
            {
                addItemToSelection(name);
            }
            else if (senderSwitch.IsToggled == false)
            {
                removeItemFromSelection(name);
            }
        }
        private void addItemToSelection(string name)
        {
            bool check = false;
            foreach (var item in Kategorien.auswahl)
            {
                if (item.titel.Equals(name))
                {
                    check = true;
                    break;
                }
            }
            foreach (var item in Kategorien.kategorien)
            {
                if (check == false && item.titel == name)
                {
                    Kategorien.auswahl.Add(item);
                    break;
                }
            }
        }
        private void removeItemFromSelection(string name)
        {
            bool check = false;
            foreach (var item in Kategorien.auswahl)
            {
                if (item.titel == name)
                {
                    check = true;
                    break;
                }
            }
            foreach (var item in Kategorien.kategorien)
            {
                if (check == true && item.titel == name)
                {
                    Kategorien.auswahl.Remove(item);
                    break;
                }
            }
        }
        #endregion

        #region Methoden für CategorieNumberPicker
        private List<int> initializePickerForCategorieNumbers()
        {
            List<int> FragenkatalogCounter = new List<int>();
            for (int i = 1; i <= Fragenkatalog.katalog.Count; i++)
            {
                FragenkatalogCounter.Add(i);
            }
            return FragenkatalogCounter;
        }
        private void NumberOfCategoriePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (NumberOfCategoriePicker.SelectedItem != null)
            {
                Fragenkatalog.fragenAnzahl = Int32.Parse(NumberOfCategoriePicker.SelectedItem.ToString());
            }
        }
        #endregion

    }
}
