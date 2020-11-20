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
    public partial class OutboxMessagesPage : ContentPage
    {
        public OutboxMessagesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<OutboxMessages>();
                var outbox = conn.Table<OutboxMessages>().ToList();

                outboxListView.ItemsSource = outbox;
            }
        }

        private async void OnItemSelected(Object sender, ItemTappedEventArgs e)
        {
            var inmail = e.Item as OutboxMessages;
            await Navigation.PushAsync(new OutboxDetailMessagePage(inmail.subject, inmail.number, inmail.direction));

        }
    }
}