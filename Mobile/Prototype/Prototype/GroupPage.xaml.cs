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
        public int Id_Price;

        public GroupPage(MainPage main, Group group, int Id_Price)
        {
            InitializeComponent();
            this.Instance = main;
            this.Id_Price = Id_Price;
            this.group = group;
            VoteButton.Text = this.Instance.lang.GetLanguageResult("VoteButton");
            this.Group.Text = group.Nom;  //Label Group
            this.Image.Source = group.UrlMiniature; //Image
            this.Desc.Text = group.Description; //Description
            this.Devs.Text = group.Devs;
        }

        protected override bool OnBackButtonPressed()
        {
            Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":LISTPAGE"+this.Id_Price, this.Instance.ipServer);
            Navigation.PopAsync();
            return true;
        }
        async private void Vote_Clicked(object sender, EventArgs e)
        {
            bool alert = await DisplayAlert("VOTE", "Etes vous sur de voter pour " + this.group.Nom + "?", "Oui", "Non");
            if (alert)
            {
                switch (this.Id_Price)
                {
                    case 1:
                        this.Instance.copyGroup.Remove(this.group);
                        await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":LISTPAGE1", this.Instance.ipServer);
                        await Network.SendPacket("VOTE1:" + this.Instance.myIpv4 + ":" + this.Instance.User + ":" + this.group.Studio, this.Instance.ipServer);
                        await Navigation.PushAsync(new ListPage(this.Instance, 2));
                        break;
                    case 2:
                        this.Instance.copyGroup.Remove(this.group);
                        await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":LISTPAGE2", this.Instance.ipServer);
                        await Network.SendPacket("VOTE2:" + this.Instance.myIpv4 + ":" + this.Instance.User + ":" + this.group.Studio, this.Instance.ipServer);
                        await Navigation.PushAsync(new ListPage(this.Instance, 3));
                        break;
                    case 3:
                        this.Instance.copyGroup.Remove(this.group);
                        await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":LISTPAGE3", this.Instance.ipServer);
                        await Network.SendPacket("VOTE3:" + this.Instance.myIpv4 + ":" + this.Instance.User + ":" + this.group.Studio, this.Instance.ipServer);
                        await Navigation.PushAsync(new ListPage(this.Instance, 4));
                        break;
                    case 4:
                        this.Instance.copyGroup.Remove(this.group);
                        await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":LISTPAGE4", this.Instance.ipServer);
                        await Network.SendPacket("VOTE4:" + this.Instance.myIpv4 + ":" + this.Instance.User + ":" + this.group.Studio, this.Instance.ipServer);
                        await Navigation.PushAsync(new ListPage(this.Instance, 5));
                        break;
                    case 5:
                        this.Instance.copyGroup.Remove(this.group);
                        await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":LISTPAGE5", this.Instance.ipServer);
                        await Network.SendPacket("VOTE5:" + this.Instance.myIpv4 + ":" + this.Instance.User + ":" + this.group.Studio, this.Instance.ipServer);
                        await Navigation.PopToRootAsync();
                        break;
                }
            }
        }
        
    }
}