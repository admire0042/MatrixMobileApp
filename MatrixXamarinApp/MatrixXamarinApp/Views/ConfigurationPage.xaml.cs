﻿using Acr.UserDialogs;
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
    public partial class ConfigurationPage : ContentPage
    {
        public ConfigurationPage()
        {
            InitializeComponent();
  
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if ((await SecureStorage.GetAsync("lastUpdate")) != null)
            {
                var time = await SecureStorage.GetAsync("lastUpdate");
               
                backgroundSync.Text = time.ToString();
            }

        }
        private void Unregister_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("userName");
            SecureStorage.Remove("webGuid");

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                var cmd1 = conn.CreateCommand(cmdText, typeof(ViewsMessageList2).Name);
                var cmd2 = conn.CreateCommand(cmdText, typeof(ViewsModel).Name);
                var cmd3 = conn.CreateCommand(cmdText, typeof(ViewsMessageList).Name);
                var cmd4 = conn.CreateCommand(cmdText, typeof(ViewsModel2).Name);
                var cmd5 = conn.CreateCommand(cmdText, typeof(viewsMessageL).Name);
                var cmd6 = conn.CreateCommand(cmdText, typeof(viewsMessageL2).Name); 
                var cmd7 = conn.CreateCommand(cmdText, typeof(GetMessages).Name);
                var cmd8 = conn.CreateCommand(cmdText, typeof(GetMessages2).Name);
                var cmd9 = conn.CreateCommand(cmdText, typeof(GetFullMessages).Name);
                var cmd10 = conn.CreateCommand(cmdText, typeof(GetFullMessages2).Name);


                if (cmd1.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<ViewsMessageList2>();
                }
                if (cmd2.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<ViewsModel>();
                }
                if (cmd3.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<ViewsMessageList>();
                }
                if (cmd4.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<ViewsModel2>();
                }
                if (cmd5.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<viewsMessageL>();
                }
                if (cmd6.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<viewsMessageL2>();
                }
                if (cmd7.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<GetMessages>();
                }
                if (cmd8.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<GetMessages2>();
                }
                if (cmd9.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<GetFullMessages>();
                }
                if (cmd10.ExecuteScalar<string>() != null)
                {
                    conn.DeleteAll<GetFullMessages2>();
                }

                //if (SecureStorage.GetAsync("ViewID2") != null)
                //{
                //    SecureStorage.Remove("ViewID2");
                //}
                //if (SecureStorage.GetAsync("ViewID1") != null)
                //{
                //    SecureStorage.Remove("ViewID1");
                //}
                //if (SecureStorage.GetAsync("ViewName1") != null)
                //{
                //    SecureStorage.Remove("ViewName1");
                //}
                //if (SecureStorage.GetAsync("ViewName2") != null)
                //{
                //    SecureStorage.Remove("ViewName2");
                //}
                SecureStorage.RemoveAll();
            }

            Navigation.PushAsync(new RegisterPage());
            //Detail = new RegisterPage();
            //IsPresented = false;
        }

        private void ChangeView_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangeViewPage());
        }

        private async void pages_Clicked(object sender, EventArgs e)
        {
            var num = pageEntry.Text;
            
            if(num == "")
            {
                await DisplayAlert("Note:", "Enter number between 5 and 40", "Cancel");
            }
            else
            {
                var numInt = int.Parse(num);
                if (5 <= numInt && numInt <= 40)
                {
                    await SecureStorage.SetAsync("number", num);
                    pageEntry.Text = "";
                    await DisplayAlert("Note:", "Input recorded", "Okay");
                }

                else
                {
                    await DisplayAlert("Note:", "Enter number between 5 and 40", "Cancel");
                }

            }


        }
    }
}