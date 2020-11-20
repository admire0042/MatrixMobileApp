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
    public partial class InboxDetailMessagePage : ContentPage
    {
        public InboxDetailMessagePage( string sub, int num, int dir)
        {
            InitializeComponent();
            SubjectMessage.Text = sub;
            NumberMessage.Text = num.ToString();
            DirectionMessage.Text = dir.ToString();
        }
    }
}