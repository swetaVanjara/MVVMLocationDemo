using System;
using System.IO;
using MVVMPlaceDemo.Helpers;
using Xamarin.Forms;


namespace MVVMPlaceDemo
{
    public partial class App : Application
    {
     
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
