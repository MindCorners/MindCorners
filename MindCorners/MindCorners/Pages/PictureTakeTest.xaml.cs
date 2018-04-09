using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureTakeTest : ContentPage
    {
        public PictureTakeTest()
        {
            InitializeComponent();
            
        }

        private async void OnBackButtonClick(object sender, EventArgs e)
        {
            await App.NavigationPage.PopAsync();
        }
    }
}