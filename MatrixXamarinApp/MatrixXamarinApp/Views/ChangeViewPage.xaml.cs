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
    public partial class ChangeViewPage : ContentPage
    {
        public ChangeViewPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var btn1 = await SecureStorage.GetAsync("ViewName1");
            var btn2 = await SecureStorage.GetAsync("ViewName2");
            if (btn1 != null)
            {
                Button1.Text = btn1;
            }
            if (btn2 != null)
            {
                Button2.Text = btn2;
            }

        }


        private async void Button_Cliked1(object sender, EventArgs e)
        {
            var isConnected = CrossConnectivity.Current.IsConnected;
            try
            {
                using (UserDialogs.Instance.Loading("Loading..."))
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
                            conn.DeleteAll<GetMessages>();
                            conn.DeleteAll<GetFullMessages>();
                        }
                   
                            await Navigation.PushAsync(new ViewPage());
                    }

                
                        else if (isConnected == false)
                        {
                            await DisplayAlert("Alert", "Check your internet connection", "Ok");
                        }
                }
            }
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }


        }

        private async void Button_Cliked2(object sender, EventArgs e)
        {
            try
            {
                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    var vID = SecureStorage.Remove("ViewID2");
                    var isConnected = CrossConnectivity.Current.IsConnected;

                    if (isConnected)
                    {
                    
                            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                            {
                                conn.CreateTable<ViewsModel2>();
                                conn.CreateTable<viewsMessageL2>();
                                conn.CreateTable<GetMessages2>();
                                conn.CreateTable<GetFullMessages2>();
                                conn.DeleteAll<ViewsModel2>();
                                conn.DeleteAll<viewsMessageL2>();
                                conn.DeleteAll<GetMessages2>();
                                conn.DeleteAll<GetFullMessages2>();
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
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        
        }

        private void ConfigButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ConfigurationPage());
        }
    }
}