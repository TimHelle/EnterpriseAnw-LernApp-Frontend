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
            Model.Database.SQLiteHelper db = new Model.Database.SQLiteHelper();
            db.initializeSQLiteDatabase();
            db.initializeSavedSettings();
            Fragenkatalog.katalog.Clear();
            ListViewOfAllQuestionItems.ItemsSource = Fragen.fragen;
        }
        private void ButtonDeleteItem(object sender, EventArgs e)
        {
            //TODO delelte benutzererstellte frage
        }
    }
}