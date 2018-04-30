using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prototype.Services;
using ZXing.Net.Mobile.Forms;

namespace Prototype
{
	public partial class MainPage : ContentPage
	{
        public Language lang = new Language { Short = "fr" };
        public List<Group> groups;

        /*static async Task<List<Group>> GetJson()
        {
            HttpClient test = new HttpClient();
            return JsonConvert.DeserializeObject<List<Group>>(await new HttpClient().GetStringAsync(new Uri("https://www.battleofcodes.com/ave/appsobjects/getallobj/ave.json")));
        }*/

        private async void GetJson()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            var json = await client.GetStringAsync(new Uri("http://www.battleofcodes.com/ave/appsobjects/getallobj/ave.json"));
            //var json = await client.GetStringAsync(new Uri("http://meteorshower.king-hosting.fr/ave.json"));
            this.groups = JsonConvert.DeserializeObject<List<Group>>(json.ToString());
        }

        public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoginButton.Text = this.lang.GetLanguageResult("LoginButton");
            LogEntry.Placeholder = this.lang.GetLanguageResult("LogEntry");
            PasswordEntry.Placeholder = this.lang.GetLanguageResult("PasswordEntry");
            BOCSiteBtn.Text = this.lang.GetLanguageResult("BOCSiteBtn");
            LanSelector.Text = this.lang.GetLanguageResult("LanSelector");
            Task.Factory.StartNew(() => this.GetJson());
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            Scanner();
            /*if (!String.IsNullOrEmpty(LogEntry.Text) && !String.IsNullOrEmpty(PasswordEntry.Text))
            {
                //if(LogEntry.Text.Contains("@gmail.com") || LogEntry.Text.Contains("@hotmail.com") || LogEntry.Text.Contains("@hotmail.be") || LogEntry.Text.Contains("@outlook.com") || LogEntry.Text.Contains("@outlook.be"))
                //{
                    await Navigation.PushAsync(new AfterLogin(this));
                    Network.SendPacket(LogEntry.Text + " Connected");
                    LogEntry.Text = "";
                    PasswordEntry.Text = "";
                /*}
                else
                {
                    await DisplayAlert("Erreur", "Merci de spécifier une adresse mail valide", "OK");
                }*/

            /*}
            else
            {
                await DisplayAlert("Erreur", "Merci de remplir les champs", "OK");
            }*/
            
        }

        public async void Scanner()
        {

            var ScannerPage = new ZXingScannerPage();
            ScannerPage.DefaultOverlayTopText = "Scannez votre ticket";
            ScannerPage.OnScanResult += (result) => {
                // Parar de escanear
                ScannerPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    Network.SendPacket(result.Text + " connected");
                });
            };


            await Navigation.PushAsync(ScannerPage);

        }

        async private void LanSelector_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LanguagePage(this));
        }

        private void BOCSiteBtn_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://battleofcodes.com"));   
        }

        public void ReloadPage()
        {
            LoginButton.Text = this.lang.GetLanguageResult("LoginButton");
            LogEntry.Placeholder = this.lang.GetLanguageResult("LogEntry");
            PasswordEntry.Placeholder = this.lang.GetLanguageResult("PasswordEntry");
            BOCSiteBtn.Text = this.lang.GetLanguageResult("BOCSiteBtn");
            LanSelector.Text = this.lang.GetLanguageResult("LanSelector");
        }

    }
}