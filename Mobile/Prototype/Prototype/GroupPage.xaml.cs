using Prototype.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GroupPage : ContentPage
	{
        public MainPage Instance;
        public Group group;

        public GroupPage(MainPage main, Group group)
        {
            InitializeComponent();
            this.Instance = main;
            this.group = group;
            VoteButton.Text = this.Instance.lang.GetLanguageResult("VoteButton");
            this.Group.Text = group.Nom;  //Label Group
            this.Image.Source = group.Image; //Image
            this.Desc.Text = group.Description; //Description
        }

        protected override bool OnBackButtonPressed()
        {
            Network.SendPacket("PAGE:???PAGE", this.Instance.ipServer);
            Navigation.PopAsync();
            return true;
        }
        async private void Vote_Clicked(object sender, EventArgs e)
        {
            bool alert = await DisplayAlert("VOTE", "Etes vous sur de voter pour " + this.group.Nom + "?", "Oui", "Non");
            if (alert)
            {
                Network.SendPacket("PAGE:MAINPAGE", this.Instance.ipServer);
                await Navigation.PopToRootAsync();
            }
        }
        
    }
}