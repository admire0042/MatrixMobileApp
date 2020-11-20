using Acr.UserDialogs;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPage2 : ContentPage
    {
        public ViewPage2()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            try
            {


                base.OnAppearing();

                List<ViewsModel2> vm = new List<ViewsModel2>();

                var httpClient = new HttpClient();
                UserDetailCredentials userDetailCredentials = new UserDetailCredentials();
                var username = await SecureStorage.GetAsync("userName");
                var webguid = await SecureStorage.GetAsync("webGuid");
                var url = await SecureStorage.GetAsync("url");
                userDetailCredentials.userName = username; // register.userName;
                userDetailCredentials.webGuid = webguid; // register.webGuid;
                var response = await httpClient.GetAsync(url + "/Message/views?" + "userName=" + userDetailCredentials.userName + "&" + "webGuid=" + userDetailCredentials.webGuid);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    vm = JsonConvert.DeserializeObject<List<ViewsModel2>>(responseContent);
                    ViewsModel2 viewsModel = new ViewsModel2();

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                        var cmd = conn.CreateCommand(cmdText, typeof(ViewsModel2).Name);


                        if (cmd.ExecuteScalar<string>() != null)
                        {
                            conn.DeleteAll<ViewsModel2>();
                            for (var i = 0; i < vm.Count; i++)
                            {
                                viewsModel = new ViewsModel2()
                                {
                                    ViewID = vm[i].ViewID,
                                    ViewName = vm[i].ViewName

                                };

                                conn.CreateTable<ViewsModel2>();
                                int rowsAdded = conn.Insert(viewsModel);


                            }
                        }
                        else
                        {
                            for (var i = 0; i < vm.Count; i++)
                            {
                                viewsModel = new ViewsModel2()
                                {
                                    ViewID = vm[i].ViewID,
                                    ViewName = vm[i].ViewName

                                };

                                conn.CreateTable<ViewsModel2>();
                                int rowsAdded = conn.Insert(viewsModel);

                            }
                        }
                    }
                }


                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<ViewsModel2>();
                    var inbox = conn.Table<ViewsModel2>().OrderBy(x => x.ViewName).ToList();

                    viewsListView.ItemsSource = inbox;
                }
            }
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var inmail = e.Item as ViewsModel2;
                var vIDs = inmail.ViewID.ToString();
                var vName = inmail.ViewName.ToString();
                await SecureStorage.SetAsync("ViewName2", vName);
                await SecureStorage.SetAsync("ViewID2", vIDs);

                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    var isConnected = CrossConnectivity.Current.IsConnected;
                    var vID = await SecureStorage.GetAsync("ViewID2");

                    if (isConnected == true)
                    {
                        Message services = new Message();
                        var a = "";
                        var b = "";
                        var messageNum = await services.CheckOutViewMessageIfExists2(a, b);

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
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var keyword = MainSearchBar.Text;


                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    var getCaf = conn.Table<ViewsModel2>().Where(x => x.ViewName.ToLower().Contains(keyword.ToLower())).ToList();

                    var resultCount = getCaf.Count();

                    if (resultCount > 0)
                    {
                        viewsListView.ItemsSource = getCaf;
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