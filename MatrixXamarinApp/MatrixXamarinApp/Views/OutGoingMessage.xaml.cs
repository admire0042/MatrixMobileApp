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
    public partial class OutGoingMessage : ContentPage
    {
        public OutGoingMessage()
        {
            InitializeComponent();
        }

        private async void ButtonNum_Clicked(object sender, EventArgs e)
        {
            Outbox reg = new Outbox();
            Message services = new Message();
            var messageNum = await services.CheckOutMailMessageIfExists(EntryNumber.Text);
            var isConnected = CrossConnectivity.Current.IsConnected;

            if (isConnected == true)
            {
                if (messageNum)
                {
                    var mes = await SecureStorage.GetAsync("subj");
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