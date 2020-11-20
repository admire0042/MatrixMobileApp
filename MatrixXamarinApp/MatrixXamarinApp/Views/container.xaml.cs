using System;
using System.Collections.Generic;
using MatrixXamarinApp.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Net.Http;

namespace MatrixXamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class container : ContentPage
    {
        public container()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
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

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                vm = JsonConvert.DeserializeObject<List<ViewsModel>>(responseContent);
                ViewsModel viewsModel = new ViewsModel();

                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.DeleteAll<ViewsModel>();
                }

                for (var i = 0; i < vm.Count; i++)
                {
                    viewsModel = new ViewsModel()
                    {
                        ViewID = vm[i].ViewID,
                        ViewName = vm[i].ViewName

                    };

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<ViewsModel>();
                        int rowsAdded = conn.Insert(viewsModel);
                    }

                };
            }


            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<ViewsModel>();
                var inbox = conn.Table<ViewsModel>().ToList();

                ShipToListView.ItemsSource = inbox;
            }
        }
    }
}