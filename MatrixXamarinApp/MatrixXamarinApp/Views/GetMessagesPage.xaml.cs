﻿using Acr.UserDialogs;
using MatrixXamarinApp.Models;
using MatrixXamarinApp.ServicesHandler;
using MimeKit;
using MimeKit.Utils;
using SQLite;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.SfDataGrid.XForms.DataPager;
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
    public partial class GetMessagesPage : ContentPage
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public GetMessagesPage()
        {
            InitializeComponent();
         
        }



        protected async override void OnAppearing()
        {
            
            using (UserDialogs.Instance.Loading("Loading..."))
            {
                base.OnAppearing();
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


                            //---------------------

                            //var Eml = await SecureStorage.GetAsync("Eml");
                            var mesId = await SecureStorage.GetAsync("messID");



                            GetFulledMessages getMessages = new GetFulledMessages();

                            using (SQLiteConnection connn = new SQLiteConnection(App.FilePath))
                            {


                                const string cmdTexts = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                                var cmds = connn.CreateCommand(cmdTexts, typeof(GetFulledMessages).Name);

                                //--------

                                using (var memory = new MemoryStream(Encoding.UTF8.GetBytes(Eml), false))
                                {
                                    var builder = new StringBuilder();
                                    var parser = new MimeParser(memory, MimeFormat.Default);
                                    var message = parser.ParseMessage();

                                    var to = message.To;
                                    var reply_to = message.ReplyTo;
                                    var date = DateUtils.FormatDate(message.Date);
                                    var from = message.From;
                                    var subject = message.Subject;
                                    var body = message.HtmlBody;
                                    var bodytext = message.TextBody;





                                    if (cmds.ExecuteScalar<string>() != null)
                                    {
                                        conn.DeleteAll<GetFulledMessages>();
                                    }

                                    if (to != null)
                                        getMessages.To = to.ToString();
                                    if (reply_to != null)
                                        getMessages.Reply_To = reply_to.ToString();
                                    if (date != null)
                                        getMessages.Date = DateUtils.FormatDate(message.Date);
                                    if (from != null)
                                        getMessages.From = from.ToString();
                                    if (subject != null)
                                        getMessages.Subject = subject.ToString();
                                    if (body != null)
                                    {
                                        getMessages.Body = body.ToString();
                                    }

                                    if (bodytext != null && body == null)
                                        getMessages.BodyText = bodytext.ToString();

                                    await con.CreateTableAsync<GetFulledMessages>();
                                    int rowsAdded = await con.InsertAsync(getMessages);
                                    var messageDetail = await con.Table<GetFulledMessages>().ToListAsync();
                                    var nav = await SecureStorage.GetAsync("messIDdisplay");
                                    Nav.Text = nav.ToString();
                                    //viewsListView.ItemsSource = messageDetail;

                                    SQLiteAsyncConnection.ResetPool();

                                }

                                //var browser = new WebView();
                                //var htmlSource = new HtmlWebViewSource();
                                //htmlSource.Html = @"<html><body>
                                //                  <h1>Xamarin.Forms</h1>
                                //                  <p>Welcome to WebView.</p>
                                //                  </body></html>";
                                //browser.Source = htmlSource;
                                //html.BindingContext = browser.Source;

                                //--------



                            }

                            //---------------------

                            await SecureStorage.SetAsync("messIDdisplay", messID.ToString());
                            await Navigation.PushAsync(new GetFullViewPage());

                        }

                    }
                }
            }
            catch
            {
            }
        



        }

    }
}