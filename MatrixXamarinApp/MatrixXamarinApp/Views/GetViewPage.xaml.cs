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
    public partial class GetViewPage : ContentPage
    {
        public GetViewPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("MessageID");
            SecureStorage.Remove("MessageDirection");
            await SecureStorage.SetAsync("MessageID", MessageID.Text);
           await SecureStorage.SetAsync("MessageDirection", MessageDirection.Text);
            var a = MessageID.Text;
            var b = MessageDirection.Text;

            Message services = new Message();
            var messageNum = await services.CheckGetviewsIfExists(a,b);
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected)
            {

                if (messageNum)
                {
                    await Navigation.PushAsync(new GetMessagesPage());
                }
                else
                {
                    await DisplayAlert("Message failed", "Message ID/Message direction incorrect  not correct", "Okay");
                }
            }
            else
            {
                await DisplayAlert("No internet", "", "Ok");
            }
        }
    }
}