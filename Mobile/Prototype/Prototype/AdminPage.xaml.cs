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
    public partial class AdminPage : ContentPage
    {
        public MainPage instance;

        public AdminPage(MainPage main)
        {
            this.instance = main;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            AdressEntry.Text = main.ipServer;
            YearEntry.Text = main.Number;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            this.instance.ipServer = AdressEntry.Text;
            this.instance.Number = YearEntry.Text;
            await Navigation.PopToRootAsync();
        }
    }
}
