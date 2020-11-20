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
    public partial class AllViewsPage : ContentPage
    {
        public AllViewsPage()
        {
            InitializeComponent();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var btn1 = await SecureStorage.GetAsync("ViewName1");
            var btn2 = await SecureStorage.GetAsync("ViewName2");
            if(btn1 != null)
            {
                Button1.Text = btn1;
            }
            if (btn2 != null)
            {
                Button2.Text = btn2;
            }

        }


            private async void Button_Cliked1(object sender, EventArgs e)
        {
            var vID = await SecureStorage.GetAsync("ViewID1");
            if (vID != null)
            {
                await Navigation.PushAsync(new ViewsMessageListPage());
            }
            else if (vID == null)
            {
                await Navigation.PushAsync(new ViewPage());
            }
        }

        private async void Button_Cliked2(object sender, EventArgs e)
        {
            var vID = await SecureStorage.GetAsync("ViewID2");
            if (vID != null)
            {
                await Navigation.PushAsync(new ViewsMessageListPage2());
            }
            else if (vID == null)
            {
                await Navigation.PushAsync(new ViewPage2());
            }
        }

    }
}