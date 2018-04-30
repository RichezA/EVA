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
    public partial class AfterLogin : TabbedPage
    {

        public AfterLogin(MainPage main)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            Children.Add(new GamePage(main));
            Children.Add(new AppliPage(main));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}
