using MatrixXamarinApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixXamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InboxMessagesPage : ContentPage
    {
        public InboxMessagesPage()
        {
            InitializeComponent();
        }

        //void NewMessage_Clicked(object select, EventArgs e)
        //{
        //    Navigation.PushAsync(new MessagePage());
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<InboxMessages>();
                var inbox = conn.Table<InboxMessages>().ToList();

                inboxListView.ItemsSource = inbox;
            }
        }

        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {
              var inmail = e.Item as InboxMessages;
                await Navigation.PushAsync(new InboxDetailMessagePage(inmail.subject, inmail.number, inmail.direction));
            
        }
    }
}