using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            Register reg = new Register();
            RegisterService services = new RegisterService();
            var getLoginDetails = await services.CheckRegisterIfExists(EntryUrl.Text,EntryUsername.Text, EntryPassword.Text);

            Uri uriResult;
           bool result = Uri.TryCreate(EntryUrl.Text, UriKind.RelativeOrAbsolute, out uriResult)
               || (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected == true)
            {
                if (!result)
                {
                    await DisplayAlert("Information", "Enter a Valid URL!", "OK");
                    return;
                }
                else if (getLoginDetails)
                {
                    //await DisplayAlert("Login success", "You are login", "Okay", "Cancel");
                    Application.Current.MainPage = new MainMenu();
                }

                else
                {
                    await DisplayAlert("Login failed", "Incorrect registration details", "Okay", "Cancel");
                }
            }
            else
            {
                await DisplayAlert("No internet", "Check your internet connection", "Ok");
            }



        }

       
    }
}
//Uri.TryCreate(EntryUrl.Text, UriKind.RelativeOrAbsolute, out uriResult