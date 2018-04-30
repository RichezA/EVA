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

    public partial class AppliPage : ContentPage
    {

        public MainPage Instance;
        public string InfoButtonText;

        public AppliPage(MainPage main)
        {
            this.Instance = main;
            this.Title = this.Instance.lang.GetLanguageResult("AppliPageTitle") ;
            //InitializeComponent();
            List<Group> groups = (from g in this.Instance.groups where g.Type == "Application" select g).ToList();

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
                    imageCell.SetBinding(Image.SourceProperty, "Miniature");
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
            await Navigation.PushAsync(new GroupPage(this.Instance, group.Nom, group.Image, group.Description));
        }

        async private void AppliList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Group group = ((ListView)sender).SelectedItem as Group;
                ((ListView)sender).SelectedItem = null;
                await Navigation.PushAsync(new GroupPage(this.Instance, group.Nom, group.Image, group.Description));
            }
        }
    }
}