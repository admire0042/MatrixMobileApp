using Acr.UserDialogs;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using Plugin.Connectivity;
using SQLite;
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
    public partial class ViewMessagesPage : ContentPage
    {
        public ViewMessagesPage()
        {
            InitializeComponent();
        }

        private async void ButtonNum_Clicked(object sender, EventArgs e)
        {
            
            
            var isConnected = CrossConnectivity.Current.IsConnected;
            var vID = await SecureStorage.GetAsync("ViewID1");

            if (isConnected == true)
            {
                Message services = new Message();
                await SecureStorage.SetAsync("dir1", Direction.Text);
                await SecureStorage.SetAsync("con1", Condition.Text);
                var messageNum = await services.CheckOutViewMessageIfExists(Direction.Text, Condition.Text);

                if (messageNum)
                {
                    using (UserDialogs.Instance.Loading("Loading..."))
                    {
                        await Navigation.PushModalAsync(new MainMenu());
                    }
                    //await Navigation.PushAsync(new ViewsMessageListPage());
                    //await  Navigation.PushAsync(new ViewsMessageListPage());
                    
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