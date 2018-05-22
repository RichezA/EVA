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
        public bool userOK = false;
        public IPAddress myIpv4;
        public string User;
        public string ipServer = "192.168.43.171";
        public string Number = "3";
        public Thread checkingVote;
        public List<Group> copyGroup;

        /*static async Task<List<Group>> GetJson()
        {
            HttpClient test = new HttpClient();
            return JsonConvert.DeserializeObject<List<Group>>(await new HttpClient().GetStringAsync(new Uri("https://www.battleofcodes.com/ave/appsobjects/getallobj/ave.json")));
        }*/

        private async void GetJson()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            //var json = await client.GetStringAsync(new Uri("https://s3-eu-west-1.amazonaws.com/battleofcodes.prj/ave.json"));
            var json = await client.GetStringAsync(new Uri("http://www.battleofcodes.com/ave/appsobjects/getallobj/eva.json"));
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
            this.checkingVote = new Thread(new ThreadStart(CheckVote));
            this.checkingVote.Start();
        }

        private async void CheckVote()
        {
            while (true)
            {
                try
                {
                    if (this.voteOk == true)
                    {
                        this.checkingVote.Abort();
                        return;
                    }
                    await Network.SendPacket("CHECKVOTE:" + this.myIpv4, this.ipServer);
                }
                catch (ThreadAbortException)
                {

                }
                catch
                {

                }
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (LogEntry.Text == "admin" && PasswordEntry.Text == "casciot")
            {
                await Navigation.PushAsync(new AdminPage(this));
                LogEntry.Placeholder = this.lang.GetLanguageResult("LogEntry");
                PasswordEntry.Placeholder = this.lang.GetLanguageResult("PasswordEntry");
                return;
            }
            if (this.voteOk == false)
            {
                await DisplayAlert("Erreur", "Les votes n'ont pas encore commencé ou sont terminés", "OK");
            }
            else
            {
                Scanner();
                //this.groups.CopyTo(this.copyGroup);

                this.copyGroup = new List<Group>(this.groups); //BETTER COPY
            }
            //if(this.userOK == false)
            //{
            //    await DisplayAlert("Erreur", "Vous avez déjà voté", "OK");
            //}
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
            await Network.SendPacket("PAGE:" + this.Number + ":" + this.myIpv4 + ":SCANNER", this.ipServer);
            /*ScannerPage.OnScanResult += (result) => {

                ScannerPage.IsScanning = false;

                
                Device.BeginInvokeOnMainThread(async () => {
                    this.User = result.Text;
                    if (!Lock)
                    {
                        await Network.SendPacket("CHECKUSER:" + this.myIpv4 + ":" + this.User, this.ipServer);
                        await Task.Delay(5000);
                        Lock = true;
                    }
                    if (this.userOK)
                    {
                        Navigation.PushAsync(new ListPage(this, 1));
                        Network.SendPacket("CONNEXION:" + this.Number + ":" + this.myIpv4 + ":" + result.Text, this.ipServer);
                        this.userOK = false;
                        return;
                    }
                    else
                    {
                        Navigation.PopToRootAsync();
                        DisplayAlert("Erreur", "Vous avez déjà voté", "OK");
                        return;
                    }
                });
            };*/

            ScannerPage.OnScanResult += (result) =>
            {

                ScannerPage.IsScanning = false;

                /*Device.BeginInvokeOnMainThread(() =>
                {
                    this.User = result.Text;
                    Network.SendPacket("CHECKUSER:" + this.myIpv4 + ":" + this.User, this.ipServer);
                    Task<bool> test = Task<bool>.Factory.StartNew(() => WaitSecond());
                    Task.WaitAny(test);
                    //Task.WaitAll();
                    //Thread.Sleep(5000);
                    //if (this.userOK)
                    if(test.Result)
                    {
                        Navigation.PushAsync(new ListPage(this, 1));
                        Network.SendPacket("CONNEXION:" + this.Number + ":" + this.myIpv4 + ":" + result.Text, this.ipServer);
                        this.userOK = false;
                        return;
                    }
                    else
                    {
                        Navigation.PopToRootAsync();
                        DisplayAlert("Erreur", "Vous avez déjà voté", "OK");
                        return;
                    }
                });*/
                this.User = result.Text;
                Network.SendPacket("CHECKUSER:" + this.myIpv4 + ":" + this.User, this.ipServer);
                Task<bool> test = Task<bool>.Factory.StartNew(() => WaitSecond());
                Task.WaitAny(test);

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (test.Result)
                    {
                        Navigation.PushAsync(new ListPage(this, 1));
                        Network.SendPacket("CONNEXION:" + this.Number + ":" + this.myIpv4 + ":" + result.Text, this.ipServer);
                        this.userOK = false;
                        return;
                    }
                    else
                    {
                        Navigation.PopToRootAsync();
                        DisplayAlert("Erreur", "Vous avez déjà voté", "OK");
                        return;
                    }
                });
                
                //this.User = result.Text;
                //Network.SendPacket("CHECKUSER:" + this.myIpv4 + ":" + this.User, this.ipServer);
                //Task.Delay(5000);

            };


            await Navigation.PushAsync(ScannerPage);

        }

        public bool WaitSecond()
        {
            Task.WaitAny(Task.Delay(1000));
            if (this.userOK)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                    //Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    //clientThread.Start(client);
                    Task.Factory.StartNew(() => HandleClientComm(client));

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
                if (encoder.GetString(message, 0, bytesRead) == "VOTEOFF")
                {
                    this.voteOk = false;
                    clientStream.Flush();
                    //this.checkingVote.Start();
                }
                if (encoder.GetString(message, 0, bytesRead) == "USEROK")
                {
                    this.userOK = true;
                    clientStream.Flush();
                }

            }

        }

        async private void LanSelector_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LanguagePage(this));
            await Network.SendPacket("PAGE:" + this.Number + ":" + this.myIpv4 + ":LANGUAGEPAGE", this.ipServer);

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