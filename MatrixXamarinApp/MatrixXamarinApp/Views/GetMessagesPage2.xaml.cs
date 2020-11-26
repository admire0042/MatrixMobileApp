﻿using Acr.UserDialogs;
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
    public partial class GetMessagesPage2 : ContentPage
    {
        SQLiteAsyncConnection con = new SQLiteAsyncConnection(App.FilePath);
        public GetMessagesPage2()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
           
            using (UserDialogs.Instance.Loading("Loading..."))
            {
                base.OnAppearing();
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
                            await SecureStorage.SetAsync("Eml2", Eml);

                            //---

                            GetFulledMessages2 getMessages = new GetFulledMessages2();

                            using (SQLiteConnection connn = new SQLiteConnection(App.FilePath))
                            {


                                const string cmdTexts = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
                                var cmds = connn.CreateCommand(cmdTexts, typeof(GetFulledMessages2).Name);

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
                                        connn.DeleteAll<GetFulledMessages2>();
                                    }

                                    if (to != null)
                                        getMessages.To = to.ToString();
                                    if (reply_to != null)
                                        getMessages.Reply_To = reply_to.ToString();
                                    if (date != null)
                                        getMessages.Date = date.ToString();
                                    if (from != null)
                                        getMessages.From = from.ToString();
                                    if (subject != null)
                                        getMessages.Subject = subject.ToString();
                                    if (body != null)
                                        getMessages.Body = body.ToString();
                                    else if (bodytext != null && body == null)
                                        getMessages.BodyText = bodytext.ToString();


                                    await con.CreateTableAsync<GetFulledMessages2>();
                                    int rowsAdded = await con.InsertAsync(getMessages);

                                }

                                //--------



                            }

                            //---

                            await SecureStorage.SetAsync("messIDdisplay2", messID.ToString());
                            await Navigation.PushAsync(new GetFullViewPage2());
                           
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