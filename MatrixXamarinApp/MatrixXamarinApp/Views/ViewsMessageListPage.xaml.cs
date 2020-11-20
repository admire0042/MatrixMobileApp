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
    public partial class ViewsMessageListPage : ContentPage
    {
        public ViewsMessageListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (UserDialogs.Instance.Loading("Loading..."))
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<viewsMessageL>();
                    var inbox = conn.Table<viewsMessageL>().ToList();

                    ViewMessageListView.ItemsSource = inbox;
                }
            }
           
        }


        private async  void Change_View_Clicked(object sender, EventArgs e)
        {
           
            var isConnected = CrossConnectivity.Current.IsConnected;
            try
            {
                if (isConnected)
                {

                    var vID = SecureStorage.Remove("ViewID1");
                    SecureStorage.Remove("dir1");
                    SecureStorage.Remove("con1");
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.DeleteAll<ViewsModel>();
                        conn.DeleteAll<viewsMessageL>();
                    }
                    using (UserDialogs.Instance.Loading("Loading..."))
                    {
                        await Navigation.PushAsync(new ViewPage());
                    }
                    
                }
                else if(isConnected == false)
                {
                    await DisplayAlert("Alert", "Check your internet connection", "Ok");
                }
            }
            catch { }
           

           
        }
    }
}