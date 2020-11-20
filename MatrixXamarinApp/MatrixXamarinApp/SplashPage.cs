using MatrixXamarinApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MatrixXamarinApp
{
    public class SplashPage : ContentPage
    {
        Image splasImage;

        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splasImage = new Image
            {
                Source = "Matrix.png",
                WidthRequest = 100,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutFlags(splasImage,
            AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splasImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splasImage);

            this.BackgroundColor = Color.FromHex("#429de3");
            this.Content = sub;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splasImage.ScaleTo(1, 2000); //Time-consuming process such as initialization
            await splasImage.ScaleTo(0.9, 1500, Easing.SpringOut);
            await splasImage.FadeTo(150, 1200, Easing.SpringOut);

            var username = await SecureStorage.GetAsync("userName");
            var webguid = await SecureStorage.GetAsync("webGuid");
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(webguid))
            {

                Application.Current.MainPage = new MainMenu();
                // Application.Current.MainPage = new NavigationPage(new MainMenu());
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new RegisterPage());
            }


        }

        



    }
}
