using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureTake : ContentPage
    {
        public PictureTake()
        {
            InitializeComponent();
        }
        public PictureTake(CameraViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void OnBackButtonClick(object sender, EventArgs e)
        {
            await App.NavigationPage.PopAsync();
        }

        private async void PictureTake_OnOnPhotoResult(PhotoResultEventArgs result)
        {
            //throw new NotImplementedException();
            await Navigation.PopModalAsync();
            if (!result.Success)
                return;

            //Photo.Source = ImageSource.FromStream(() => new MemoryStream(result.Image));
        }
    }
}