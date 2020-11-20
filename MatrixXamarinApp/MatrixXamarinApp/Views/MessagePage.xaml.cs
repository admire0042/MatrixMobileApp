using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePage : ContentPage
    {
        public MessagePage()
        {

            InitializeComponent();
        }

        private async void ButtonNum_Clicked(object sender, EventArgs e)
        {
            Inbox reg = new Inbox();
            Message services = new Message();
            var messageNum = await services.CheckMessageIfExists(EntryNumber.Text);
            var isConnected = CrossConnectivity.Current.IsConnected;

            if(isConnected == true)
            {
                if (messageNum)
                {
                    var mes = await SecureStorage.GetAsync("subject");
                    await DisplayAlert("Message success", mes, "Okay", "Cancel");
                }
                else
                {
                    await DisplayAlert("Message failed", "Message ID incorrect or not exists", "Okay", "Cancel");
                }
            }
           else
            {
                await DisplayAlert("No internet", "", "Ok");
            }
           

        }
    }
}

            