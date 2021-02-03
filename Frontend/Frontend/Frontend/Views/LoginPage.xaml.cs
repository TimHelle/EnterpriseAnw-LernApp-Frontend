using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void CheckLogin(object sender, EventArgs e)
        {
            String key = "Enterprise";
            if (entryPassword.Text == key)
            {
                Navigation.PushAsync(new MainPage());
            }            
        }
    }
}