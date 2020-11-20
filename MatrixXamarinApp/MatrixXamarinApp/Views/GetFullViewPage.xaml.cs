using Acr.UserDialogs;
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




                        if (cmd.ExecuteScalar<string>() != null)
                        {
                            conn.DeleteAll<GetFulledMessages>();

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
                                getMessages.Body = body.ToString();
                            if (bodytext != null && body == null)
                                getMessages.BodyText = bodytext.ToString();

                            await con.CreateTableAsync<GetFulledMessages>();
                            int rowsAdded = await con.InsertAsync(getMessages);
                            var messageDetail = await con.Table<GetFulledMessages>().ToListAsync();
                            viewsListView.ItemsSource = messageDetail;
                            SQLiteAsyncConnection.ResetPool();
                        }
                        else
                        {
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
                                getMessages.Body = body.ToString();
                            if (bodytext != null)
                                getMessages.BodyText = bodytext.ToString();

                            await con.CreateTableAsync<GetFulledMessages>();
                            int rowsAdded = await con.InsertAsync(getMessages);
                            var messageDetail = await con.Table<GetFulledMessages>().ToListAsync();
                            viewsListView.ItemsSource = messageDetail;
                            SQLiteAsyncConnection.ResetPool();
                        }

                    }

                    //--------



                }
            }
            catch
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

    }
}