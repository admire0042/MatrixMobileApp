using Acr.UserDialogs;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
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
    public partial class GetMessagesPage2 : ContentPage
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public GetMessagesPage2()
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
                    conn.CreateTable<GetMessages2>();
                    var message = await con.Table<GetMessages2>().OrderByDescending(x => x.CreatedTime).ToListAsync();

                    ViewMessageListView.ItemsSource = message;
                    SQLiteAsyncConnection.ResetPool();
                }
                if ((await SecureStorage.GetAsync("viewTwo")) != null)
                {
                    var nav = await SecureStorage.GetAsync("viewTwo");
                    Nav.Text = nav.ToString();
                }
            }

        }

        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {
            try
            {
                var inmail = e.Item as GetMessages2;
                var messID = inmail.MessageId;
                // await SecureStorage.SetAsync("messID", messID);
                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var Eml = conn.Table<GetFullMessages2>()
                                   .Where(x => x.MessageId == messID)
                                    .FirstOrDefault()?
                                    .EML;


                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                        var cmd = conn.CreateCommand(cmdText, typeof(GetFullMessages2).Name);


                        if (Eml == null)
                        {
                            await DisplayAlert("Note", "Eml is not available", "Cancel");
                        }
                        else
                        {
                            await Navigation.PushAsync(new GetFullViewPage2());
                            await SecureStorage.SetAsync("Eml2", Eml);
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