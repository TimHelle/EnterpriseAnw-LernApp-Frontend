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
    public partial class QuestionDetail : ContentPage
    {
        public QuestionDetail()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            Fragenkatalog.katalog.Clear();
            ListViewOfAllQuestionItems.ItemsSource = Fragen.fragen;
        }
        private void ButtonDeleteItem(object sender, EventArgs e)
        {
            Model.Database.SQLiteHelper db = new Model.Database.SQLiteHelper();
            var senderAsButton = (Button)sender;
            Label label = senderAsButton.Parent.FindByName<Label>("labelItemText");
            foreach (Fragen item in Fragen.fragen)
            {
                if (label.Text.Equals(item.getText()))
                {
                    db.DeleteQuestionFromDatabase(item);
                    Fragen.fragen.Remove(item);
                    break;
                }
            }
            Navigation.PushAsync(new MainPage());
        }
    }
}