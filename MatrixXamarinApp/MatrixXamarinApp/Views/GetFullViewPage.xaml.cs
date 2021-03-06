﻿using Acr.UserDialogs;
using MailKit;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using MimeKit;
using MimeKit.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetFullViewPage : ContentPage
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public GetFullViewPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                var Eml = await SecureStorage.GetAsync("Eml");
                var mesId = await SecureStorage.GetAsync("messID");



                GetFulledMessages getMessages = new GetFulledMessages();

                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {


                    const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                    var cmd = conn.CreateCommand(cmdText, typeof(GetFulledMessages).Name);

                //--------

                //var browser = new WebView();
                //var htmlSource = new HtmlWebViewSource();
                //htmlSource.Html = @"<html><body>
                //                  <h1>Xamarin.Forms</h1>
                //                  <p>Welcome to WebView.</p>
                //                  </body></html>";
                //browser.Source = htmlSource;
                //html.BindingContext = browser.Source;

                //--------

                //------
                var messageDetail = await con.Table<GetFulledMessages>().ToListAsync();
                var nav = await SecureStorage.GetAsync("messIDdisplay");
                    var nav2 = int.Parse(nav);

                    //----


                    var saa = conn.Table<GetMessages>().Where(x => x.MessageId == nav2).FirstOrDefault().IO == "I";
                    var sab = conn.Table<GetMessages>().Where(x => x.MessageId == nav2).FirstOrDefault().IO == "O";
                    if (saa)
                    {
                        outgoing.IsVisible = false;
                        incoming.ItemsSource = messageDetail;
                    }
                    if (sab)
                    {
                        incoming.IsVisible = false;
                        outgoing.ItemsSource = messageDetail;
                    }

                    //----

                    Nav.Text = nav.ToString();
                   
                    
                SQLiteAsyncConnection.ResetPool();
                //------

            }
        }
            catch
            {
            }
        }

    }
}