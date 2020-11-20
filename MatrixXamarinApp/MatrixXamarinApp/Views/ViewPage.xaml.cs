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
    public partial class ViewPage : ContentPage
    {

        public ViewPage()
        {
           

                InitializeComponent();

            
        }

        //ViewsModel inbox = new ViewsModel();

        protected async override void OnAppearing()
        {
            try
            {

                base.OnAppearing();


                List<ViewsModel> vm = new List<ViewsModel>();

                var httpClient = new HttpClient();
                UserDetailCredentials userDetailCredentials = new UserDetailCredentials();
                var username = await SecureStorage.GetAsync("userName");
                var webguid = await SecureStorage.GetAsync("webGuid");
                var url = await SecureStorage.GetAsync("url");
                userDetailCredentials.userName = username; // register.userName;
                userDetailCredentials.webGuid = webguid; // register.webGuid;
                var response = await httpClient.GetAsync(url + "/Message/views?" + "userName=" + userDetailCredentials.userName + "&" + "webGuid=" + userDetailCredentials.webGuid);
                //var response = await httpClient.GetAsync("http://webapi.sd-matrix.com:5000/Message/Get?msgIds=1412547&messageDir=INC&userName=CALEBO&webGuid=5084ee33-4e6e-4bef-b537-a5be923e7f2e&msgPart=0")
                // http://webapi.sd-matrix.com:5000/Message/views?userName=calebo&webGUID=fd754570-1251-4d4d-8377-b227b34abd47
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    vm = JsonConvert.DeserializeObject<List<ViewsModel>>(responseContent);
                    ViewsModel viewsModel = new ViewsModel();

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                        var cmd = conn.CreateCommand(cmdText, typeof(ViewsModel).Name);


                        if (cmd.ExecuteScalar<string>() != null)
                        {
                            conn.DeleteAll<ViewsModel>();
                            for (var i = 0; i < vm.Count; i++)
                            {
                                viewsModel = new ViewsModel()
                                {
                                    ViewID = vm[i].ViewID,
                                    ViewName = vm[i].ViewName

                                };
                                conn.CreateTable<ViewsModel>();
                                int rowsAdded = conn.Insert(viewsModel);


                            }
                        }
                        else
                        {
                            for (var i = 0; i < vm.Count; i++)
                            {

                                viewsModel = new ViewsModel()
                                {
                                    ViewID = vm[i].ViewID,
                                    ViewName = vm[i].ViewName

                                };

                                conn.CreateTable<ViewsModel>();
                                int rowsAdded = conn.Insert(viewsModel);


                            }
                        }
                    }


                }


                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<ViewsModel>();
                    var inbox = conn.Table<ViewsModel>().OrderBy(x => x.ViewName).ToList();

                    viewsListView.ItemsSource = inbox;
                }
            }
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {
            try
            {

                var inmail = e.Item as ViewsModel;
                var vIDs = inmail.ViewID.ToString();
                var vName = inmail.ViewName.ToString();
                await SecureStorage.SetAsync("ViewID1", vIDs);
                await SecureStorage.SetAsync("ViewName1", vName);

                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    var isConnected = CrossConnectivity.Current.IsConnected;
                    var vID = await SecureStorage.GetAsync("ViewID1");

                    if (isConnected == true)
                    {
                        Message services = new Message();
                        var a = "";
                        var b = "";
                        var messageNum = await services.CheckOutViewMessageIfExists(a, b);

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
                    var getCaf = conn.Table<ViewsModel>().Where(x => x.ViewName.ToLower().Contains(keyword.ToLower())).ToList();

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