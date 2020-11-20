using Acr.UserDialogs;
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
    public partial class ViewMessagesPage2 : ContentPage
    {
        public ViewMessagesPage2()
        {
            InitializeComponent();
        }

        private async void ButtonNum_Clicked(object sender, EventArgs e)
        {
            Message services = new Message();
            await SecureStorage.SetAsync("dir2", Direction.Text);
            await SecureStorage.SetAsync("con2", Condition.Text);
            var messageNum = await services.CheckOutViewMessageIfExists2(Direction.Text, Condition.Text);
            var isConnected = CrossConnectivity.Current.IsConnected;
            var vID = await SecureStorage.GetAsync("ViewID2");

            if (isConnected == true)
            {

                if (messageNum)
                {
                    //await Navigation.PushAsync(new ViewsMessageListPage2());

                    // await new ViewsMessageListPage2();
                    using (UserDialogs.Instance.Loading("Loading..."))
                    {
                        await Navigation.PushModalAsync(new MainMenu());
                    }
                    
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