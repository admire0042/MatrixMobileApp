using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public partial class MainMenu : MasterDetailPage
    {
        public List<MainMenuItem> MainMenuItems { get; set; }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if ((await SecureStorage.GetAsync("lastUpdate")) != null)
            {
                var time = await SecureStorage.GetAsync("lastUpdate");
                backgroundSync.Text = time.ToString();
            }
        }
        public async void ViewName()
        {

            var viewOne = "View One";
            var bb = await SecureStorage.GetAsync("ViewName1");
            if (bb != null)
            {

                viewOne = bb.ToString();
            };
             await SecureStorage.SetAsync("viewOne", viewOne);
            var view1 = await SecureStorage.GetAsync("viewOne");

            var viewTwo = "View Two";
            var cc = await SecureStorage.GetAsync("ViewName2");
            if (cc != null)
            {

                viewTwo = cc.ToString();
            };
            await SecureStorage.SetAsync("viewTwo", viewTwo);
            var view2 = await SecureStorage.GetAsync("viewTwo");

            MainMenuItems = new List<MainMenuItem>()
            {

            new MainMenuItem() { Title = view1.ToString(), Icon = "menu_stock.png", TargetType = typeof(ViewsMessageListPage) },
                new MainMenuItem() { Title = view2.ToString(), Icon = "menu_stock.png", TargetType = typeof(ViewsMessageListPage2) }
            };

        }
        public  MainMenu()
        {
            // Set the binding context to this code behind.
            BindingContext = this;
            // Build the Menu
            ViewName();
            // Set the default page, this is the "home" page.
            Detail = new NavigationPage(new GetMessagesPage());



                InitializeComponent();
        }

     

    // When a MenuItem is selected.
    public async void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            if ((await SecureStorage.GetAsync("lastUpdate")) != null)
            {
                var time = await SecureStorage.GetAsync("lastUpdate");
                backgroundSync.Text = time.ToString();
            }

            var item = e.SelectedItem as MainMenuItem;
            if (item != null)
            {
                ViewName();
                var view1 = await SecureStorage.GetAsync("viewOne");
                var view2 = await SecureStorage.GetAsync("viewTwo");
                if (item.Title.Equals(view1.ToString()))
                {
                    var vID = await SecureStorage.GetAsync("ViewID1");
                    if (vID != null)
                    {
                        Detail = new NavigationPage(new GetMessagesPage());
                    }
                    else if (vID == null)
                    {
                        Detail = new NavigationPage(new ViewPage());
                    }
                    
                   
                }
                else if (item.Title.Equals(view2.ToString()))
                {
                    var vID = await SecureStorage.GetAsync("ViewID2");
                    if (vID != null)
                    {
                        Detail = new NavigationPage(new GetMessagesPage2());
                    }
                    else if (vID == null)
                    {
                        Detail = new NavigationPage(new ViewPage2());
                    }
                }
               
                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }

        private async void ConfigButton_Clicked(object sender, EventArgs e)
        {
            if ((await SecureStorage.GetAsync("lastUpdate")) != null)
            {
                var time = await SecureStorage.GetAsync("lastUpdate");
                backgroundSync.Text = time.ToString();
            }
            Detail = new NavigationPage(new ConfigurationPage());
            MenuListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}