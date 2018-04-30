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
        public String GroupName { get; private set; }
        public ImageSource imageSource { get; private set; }
        public String Description { get; private set; }
        public MainPage Instance;

        public GroupPage(MainPage main, String GroupName, ImageSource imageSource, String Description)
        {
            InitializeComponent();
            this.Instance = main;
            VoteButton.Text = this.Instance.lang.GetLanguageResult("VoteButton");
            this.GroupName = GroupName;
            this.imageSource = imageSource;
            this.Description = Description;
            this.Group.Text = GroupName;  //Label Group
            this.Image.Source = imageSource; //Image
            this.Desc.Text = Description; //Description
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return true;
        }
        async private void Vote_Clicked(object sender, EventArgs e)
        {
            bool alert = await DisplayAlert("VOTE", "Etes vous sur de voter pour " + this.GroupName + "?", "Oui", "Non");
            if (alert)
            {
                await Navigation.PopToRootAsync();
            }
        }
        
    }
}