using Acr.UserDialogs;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using SQLite;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.SfDataGrid.XForms.DataPager;
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
    public partial class GetMessagesPage : ContentPage
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public GetMessagesPage()
        {
            InitializeComponent();
         
        }



        protected async override void OnAppearing()
        {
            base.OnAppearing();
            using (UserDialogs.Instance.Loading("Loading..."))
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    await con.CreateTableAsync<GetMessages>();
                    var message = await con.Table<GetMessages>().OrderByDescending(x => x.CreatedTime).ToListAsync();

                    ViewMessageListView.ItemsSource = message;
                    SQLiteAsyncConnection.ResetPool();
                }
                if ((await SecureStorage.GetAsync("viewOne")) != null)
                {
                    var nav = await SecureStorage.GetAsync("viewOne");
                    Nav.Text = nav.ToString();
                }
            }

        }

        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {
            try
            {
                var inmail = e.Item as GetMessages;
                var messID = inmail.MessageId;
                // await SecureStorage.SetAsync("messID", messID);
                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var Eml = conn.Table<GetFullMessages>()
                                   .Where(x => x.MessageId == messID)
                                    .FirstOrDefault()?
                                    .EML;


                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                        var cmd = conn.CreateCommand(cmdText, typeof(GetFullMessages).Name);


                        if (Eml == null)
                        {
                            await DisplayAlert("Note", "Eml is not available", "Cancel");
                        }
                        else
                        {
                            await SecureStorage.SetAsync("Eml", Eml);
                            await Navigation.PushAsync(new GetFullViewPage());

                        }

                    }
                }
            }
            catch
            {
               await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        



        }

    }
}