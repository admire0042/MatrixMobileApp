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


        private void FullSync()
        {
            Device.StartTimer(new TimeSpan(0, 2, 0), () =>
            {
                BackgroundService2 back = new BackgroundService2();
                back.FullSync();
                return true; // runs again, or false to stop
            });
        }


        protected override void OnStart()
        {
          
            // Register
            BackgroundAggregatorService.Add(() => new BackgroundService(300));

            var us = SecureStorage.GetAsync("userName");
            var web = SecureStorage.GetAsync("webGuid");


            // Starts
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected && us.Result != null && web.Result != null)
            {
                BackgroundAggregatorService.StartBackgroundService();
                FullSync();
            }
            else
            {
                BackgroundAggregatorService.StopBackgroundService();
            }

        }

        protected override void OnSleep()
        {
            var us = SecureStorage.GetAsync("userName");
            var web = SecureStorage.GetAsync("webGuid");

            // Starts
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected && us != null && web != null)
            {
                BackgroundAggregatorService.StartBackgroundService();
                FullSync();
            }
            else
            {
                BackgroundAggregatorService.StopBackgroundService();
            }

        }

        protected  override void OnResume()
        {
            var us = SecureStorage.GetAsync("userName");
            var web = SecureStorage.GetAsync("webGuid");

            // Starts
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected && us != null && web != null)
            {
                BackgroundAggregatorService.StartBackgroundService();
                FullSync();
            }
            else
            {
                BackgroundAggregatorService.StopBackgroundService();
            }
        }
    }
}