using Acr.UserDialogs;
using MatrixXamarinApp.Models;
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
    public partial class ViewsMessageListPage2 : ContentPage
    {
        public ViewsMessageListPage2()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<viewsMessageL2>();
                var inbox = conn.Table<viewsMessageL2>().ToList();

                ViewMessageListView.ItemsSource = inbox;
            }
        }


        private async void Change_View_Clicked(object sender, EventArgs e)
        {
            var vID = SecureStorage.Remove("ViewID2");
            var isConnected = CrossConnectivity.Current.IsConnected;

            if (isConnected)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.DeleteAll<ViewsModel2>();
                    conn.DeleteAll<viewsMessageL2>();
                }
                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    await Navigation.PushAsync(new ViewPage2());
                }
               
            }

            else if (isConnected == false)
            {
                await DisplayAlert("Alert", "Check your internet connection", "Ok");
            }

            
        }
    }
}