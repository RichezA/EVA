using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Services;
using Xamarin.Forms;

namespace Prototype
{
    public partial class App : Application
    {
        public MainPage instance;

        public App()
        {
            InitializeComponent();
            instance = new MainPage();
            MainPage = new NavigationPage(instance);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            try
            {
                Network.SendPacket("PAGE:MAINPAGE", instance.ipServer);
            }
            catch
            {

            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            try
            {
                Network.SendPacket("PAGE:ERROR/QUIT", instance.ipServer);
            }
            catch
            {

            }

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            try
            {
                Network.SendPacket("PAGE:???", instance.ipServer);
            }
            catch
            {

            }
        }
    }
}
