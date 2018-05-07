using System;
using System.Collections.Generic;
using System.Text;
using Prototype.Services;
using Xamarin.Forms;

namespace Prototype
{
    public partial class LanguagePage : ContentPage
    {
        public MainPage Instance { get; private set; }

        public LanguagePage(MainPage main)
        {
            this.Instance = main;
            List<Language> langs = new List<Language>
            {
                new Language{Name = "Français", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/Flag_of_France.svg/225px-Flag_of_France.svg.png", Short = "fr"},
                new Language{Name = "English", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Flag_of_Great_Britain_%281707-1800%29.svg/2000px-Flag_of_Great_Britain_%281707-1800%29.svg.png", Short = "en"},
                new Language{Name = "Italiano", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/03/Flag_of_Italy.svg/225px-Flag_of_Italy.svg.png", Short = "it"}
            };
            ListView listView = new ListView
            {
                RowHeight = 100,
                HasUnevenRows = false,
                // Source of data items.
                ItemsSource = langs,

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
                    imageCell.SetBinding(Image.SourceProperty, "ImageUrl");
                    /*imageCell.SetBinding(Image.MarginProperty, "10, 0, 5, 0");
                    imageCell.SetBinding(Image.HeightRequestProperty, ""*/

                    Label GroupName = new Label
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 20,
                    };
                    GroupName.SetBinding(Label.TextProperty, "Name");

                    Button SelectButton = new Button
                    {
                        Text = this.Instance.lang.GetLanguageResult("SelectButton"),
                        WidthRequest = 100
                    };
                    SelectButton.Clicked += SelectButton_Clicked;
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
                                        SelectButton
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
            listView.ItemSelected += ListItem_ItemSelected;
        }

        async private void ListItem_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Language lang = ((ListView)sender).SelectedItem as Language;
                ((ListView)sender).SelectedItem = null;
                this.Instance.lang.Short = lang.Short;
                Network.SendPacket("PAGE:MAINPAGE", this.Instance.ipServer);
                await Navigation.PopToRootAsync();
                this.Instance.ReloadPage();
            }
        }

        async private void SelectButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            this.Instance.lang.Short = ((Language)button.BindingContext).Short;
            Network.SendPacket("PAGE:MAINPAGE", this.Instance.ipServer);
            await Navigation.PopToRootAsync();
            this.Instance.ReloadPage();
        }
    }
}
