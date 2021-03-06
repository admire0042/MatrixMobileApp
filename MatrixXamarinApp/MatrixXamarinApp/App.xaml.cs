﻿using Matcha.BackgroundService;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.RestAPIClient;
using MatrixXamarinApp.ServicesHandler;
using MatrixXamarinApp.Views;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp
{
    public partial class App : Application
    {
        public static string FilePath;

       
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new RegisterPage());
            // MainPage = new MessagePage();
            //MainPage = new MainMenu();
            MainPage = new NavigationPage(new SplashPage());

        }

       

        public App(string filePath)
        {
            InitializeComponent();

            FilePath = filePath;
            MainPage = new NavigationPage(new SplashPage());
        }


        private async void FullSync()
        {
            Device.StartTimer(new TimeSpan(1, 0, 0), () =>
            {
                var isConnected = CrossConnectivity.Current.IsConnected;
                if (isConnected)
                {
                    BackgroundService2 back = new BackgroundService2();
                    Task.Run(() => back.FullSync())
                   .ContinueWith(t =>
                   {
                       if (t.IsFaulted)
                       {
                       }
                       else
                           Task.Run(() => back.FullSync2());

                   });
                }
                return true; // runs again, or false to stop

            });
        }

        public void Background()
        {
            var us = SecureStorage.GetAsync("userName");
            var web = SecureStorage.GetAsync("webGuid");
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected && us.Result != null && web.Result != null)
            {
                BackgroundAggregatorService.StartBackgroundService();
            }
            else
            {
                BackgroundAggregatorService.StopBackgroundService();
            }
        }
        protected async override void OnStart()
        {
          
            // Register
            BackgroundAggregatorService.Add(() => new BackgroundService(300));



            //await Task.Delay(2000);
            // Starts
            await Task.Run(() => Background());
            await Task.Run(() => FullSync());


        }

        protected async override void OnSleep()
        {
            await Task.Run(() => Background());
            await Task.Run(() => FullSync());

        }

        protected async  override void OnResume()
        {
            await Task.Run(() => Background());
            await Task.Run(() => FullSync());
        }
    }
}
