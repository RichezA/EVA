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
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Prototype
{
	public partial class MainPage : ContentPage
	{
        public Language lang = new Language { Short = "fr" };
        public List<Group> groups;
        public TcpClient client;
        private TcpListener tcpListener;
        private Thread listenThread;
        public bool voteOk = false;
        public IPAddress myIpv4;
        public string ipServer = "192.168.43.73";
        public string year = "";

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
            IPAddress[] ipv4Addresses = Array.FindAll(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => a.AddressFamily == AddressFamily.InterNetwork);
            this.myIpv4 = ipv4Addresses[0].MapToIPv4();
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoginButton.Text = this.lang.GetLanguageResult("LoginButton");
            LogEntry.Placeholder = this.lang.GetLanguageResult("LogEntry");
            PasswordEntry.Placeholder = this.lang.GetLanguageResult("PasswordEntry");
            BOCSiteBtn.Text = this.lang.GetLanguageResult("BOCSiteBtn");
            LanSelector.Text = this.lang.GetLanguageResult("LanSelector");
            Task.Factory.StartNew(() => this.GetJson());
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Network.SendPacket("CHECKVOTE:"+this.myIpv4, this.ipServer);
            }
            catch
            {

            }
            if (LogEntry.Text == "admin" && PasswordEntry.Text == "casciot")
            {
                await Navigation.PushAsync(new AfterLogin(this));
                return;
            }
            if(this.voteOk == false)
            {
                await DisplayAlert("Erreur", "Les votes n'ont pas encore commencé ou sont terminés", "OK");
            }
            else {
                Scanner();
            }
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
            Network.SendPacket("PAGE:" + this.myIpv4 + ":SCANNER", this.ipServer);
            ScannerPage.OnScanResult += (result) => {

                ScannerPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    Network.SendPacket("CONNEXION:" + this.myIpv4+ ":" +result.Text, this.ipServer);
                });
            };


            await Navigation.PushAsync(ScannerPage);

        }

        public void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();
                while (true)
                {
                    //blocks until a client has connected to the server
                    TcpClient client = this.tcpListener.AcceptTcpClient();

                    //create a thread to handle communication 
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);

                }

            }
            catch (SocketException) { }

        }

        public void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            this.client = tcpClient;
            NetworkStream clientStream = tcpClient.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            //clientStream.Flush();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }
                if (encoder.GetString(message, 0, bytesRead) == "VOTEOK")
                {
                    this.voteOk = true;
                    clientStream.Flush();
                }
                if(encoder.GetString(message, 0, bytesRead) == "VOTEOFF")
                {
                    this.voteOk = false;
                    clientStream.Flush();
                }

            }

        }

        async private void LanSelector_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LanguagePage(this));
            Network.SendPacket("PAGE:" + this.myIpv4 + ":LANGUAGEPAGE", this.ipServer);

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