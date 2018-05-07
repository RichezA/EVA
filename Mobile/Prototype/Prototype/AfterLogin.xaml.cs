using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AfterLogin : ContentPage
    {
        public MainPage instance;

        public AfterLogin(MainPage main)
        {
            this.instance = main;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            AdressEntry.Text = main.ipServer;
            YearEntry.Text = main.year;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            this.instance.ipServer = AdressEntry.Text;
            this.instance.year = YearEntry.Text;
            await Navigation.PopToRootAsync();
        }
    }
}
