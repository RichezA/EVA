using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prototype.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{

    public partial class ListPage : ContentPage
    {

        public MainPage Instance;
        public string InfoButtonText;
        public List<Group> groups;
        public int Id_Price;

        public ListPage(MainPage main, int Id_Price)
        {
            this.Instance = main;
            this.Id_Price = Id_Price;
            NavigationPage.SetHasBackButton(this, false);
            //InitializeComponent();
            switch (Id_Price)
            {
                //APP
                case 1: //Prix meilleur fonctionnalité
                    this.groups = (from g in this.Instance.copyGroup where g.Type == "Application" select g).ToList();
                    this.Title = this.Instance.lang.GetLanguageResult("AppliPageTitle1");
                    break;
                case 2: //Prix de l'app la plus originale
                    this.groups = (from g in this.Instance.copyGroup where g.Type == "Application" select g).ToList();
                    this.Title = this.Instance.lang.GetLanguageResult("AppliPageTitle2");
                    break;
                //JEU
                case 3: //Prix du meilleur gameplay
                    this.groups = (from g in this.Instance.copyGroup where g.Type == "Jeux" select g).ToList();
                    this.Title = this.Instance.lang.GetLanguageResult("AppliPageTitle3");
                    break;
                case 4: //Prix de la meilleur direction artistique
                    this.groups = (from g in this.Instance.copyGroup where g.Type == "Jeux" select g).ToList();
                    this.Title = this.Instance.lang.GetLanguageResult("AppliPageTitle4");
                    break;
                case 5: //Prix du meilleur jeu
                    this.groups = (from g in this.Instance.copyGroup where g.Type == "Jeux" select g).ToList();
                    this.Title = this.Instance.lang.GetLanguageResult("AppliPageTitle5");
                    break;
            }

            this.InfoButtonText = this.Instance.lang.GetLanguageResult("InfoButton");
            ListView listView = new ListView
            {
                RowHeight = 100,
                HasUnevenRows = false,
                // Source of data items.
                ItemsSource = groups,

                // Define template for displaying each item.
                // (Argument of DataTemplate constructor is called for 
                //      each item; it must return a Cell derivative.)
                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Image imageCell = new Image
                    {
                        Margin = new Thickness(10, 0, 5, 0),
                        HeightRequest = 80,
                        WidthRequest = 80
                    };
                    imageCell.SetBinding(Image.SourceProperty, "UrlStudio");
                    /*imageCell.SetBinding(Image.MarginProperty, "10, 0, 5, 0");
                    imageCell.SetBinding(Image.HeightRequestProperty, "")*/

                    Label GroupName = new Label
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 20,
                    };
                    GroupName.SetBinding(Label.TextProperty, "Studio");

                    Button InfoButton = new Button
                    {
                        Text = this.InfoButtonText,
                        WidthRequest = 100
                    };
                    InfoButton.Clicked += InfoButton_Clicked;
                    //InfoButton.SetBinding(Button.TextProperty, this.InfoButtonText);

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new StackLayout
                                {
                                    Padding = new Thickness(0, 10),
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        imageCell
                                    }
                                },
                                new StackLayout
                                {
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                       GroupName
                                    }
                                },
                                new StackLayout
                                {
                                    Margin = new Thickness(0,0,10,0),
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        InfoButton                                            
                                    }
                                }
                            }
                        }
                    };
                })
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    listView
                }
            };
            listView.ItemSelected += AppliList_ItemSelected;
        }

        async private void InfoButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Group group = button.BindingContext as Group;
            await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":GROUPPAGE", this.Instance.ipServer);
            await Navigation.PushAsync(new GroupPage(this.Instance, group, this.Id_Price));
        }

        async private void AppliList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Group group = ((ListView)sender).SelectedItem as Group;
                ((ListView)sender).SelectedItem = null;
                await Network.SendPacket("PAGE:" + this.Instance.Number + ":" + this.Instance.myIpv4 + ":GROUPPAGE", this.Instance.ipServer);
                await Navigation.PushAsync(new GroupPage(this.Instance, group, this.Id_Price));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}