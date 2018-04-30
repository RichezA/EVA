using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Prototype.Services;
using Xamarin.Forms;

namespace Prototype
{
    public partial class GamePage : ContentPage
    {
        public MainPage Instance;
        public string InfoButtonText;

        public GamePage(MainPage main)
        {
            this.Instance = main;
            this.Title = this.Instance.lang.GetLanguageResult("GamePageTitle");
            List<Group> groups = (from g in this.Instance.groups where g.Type == "Jeux" select g).ToList();
            /*List<Group> groups = new List<Group>
            {
                new Group{Studio = "MeteorShower", UrlImage = "https://scontent.fbru1-1.fna.fbcdn.net/v/t1.0-9/28277420_565544467147226_8712051723323277030_n.png?oh=33d1687d71a438f11a1d3b0d0091279a&oe=5B143C0A", Description = "Gagnant de la BOC"},
            };*/
            
            

            this.InfoButtonText = this.Instance.lang.GetLanguageResult("InfoButton");
            ListView listView = new ListView
            {
                RowHeight = 100,
                HasUnevenRows = true,
                // Source of data items.
                ItemsSource = groups,

                // Define template for displaying each item.
                // (Argument of DataTemplate constructor is called for 
                //      each item; it must return a Cell derivative.)
                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Image GroupImage = new Image
                    {
                        Margin = new Thickness(10, 0, 5, 0),
                        HeightRequest = 80,
                        WidthRequest = 80,
                        //DownsampleToViewSize = true,
                        Aspect = Aspect.Fill,
                        //Source = ImageSource.FromUri(new Uri("https://www.battleofcodes.com/wp-content/uploads/2017/07/DSCN3589-e1499422261438-300x350.jpg"))
                        //Source = "http://www.battleofcodes.com/ave/appsobjects/getallobj/Test3Mini.png"
                        //Source = "http://meteorshower.king-hosting.fr/Test3Mini.png"
                        //Source = "/data/user/0/com.Aveva.Eva/files/MoonBlood Studio.png"
                        //Source = "https://www.chicagotraveler.com/sites/default/files/concerts-chicago-big-1.jpg"
                    };
                    //ImageService.Instance.LoadUrl("http://www.battleofcodes.com/ave/appsobjects/getallobj/Test3Mini.png").Into(listView);
                    GroupImage.SetBinding(Image.SourceProperty, "Miniature");
                    /*imageCell.SetBinding(Image.MarginProperty, "10, 0, 5, 0");
                    imageCell.SetBinding(Image.HeightRequestProperty, ""*/

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
                                        GroupImage
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
            await Navigation.PushAsync(new GroupPage(this.Instance, group.Studio, group.Image, group.Description));
        }

        async private void AppliList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Group group = ((ListView)sender).SelectedItem as Group;
                ((ListView)sender).SelectedItem = null;
                await Navigation.PushAsync(new GroupPage(this.Instance, group.Studio, group.Image, group.Description));
            }
        }
    }
}
