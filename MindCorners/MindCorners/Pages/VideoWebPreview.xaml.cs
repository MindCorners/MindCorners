using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.Models;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class VideoWebPreview : ContentPage
    { 

        public VideoWebPreview(PostAttachment vm)
        {
            InitializeComponent();
            BindingContext = vm;
            //WebView wv = new WebView();
            //wv.VerticalOptions=LayoutOptions.FillAndExpand;
            //wv.HorizontalOptions = LayoutOptions.FillAndExpand;
            //var html = @"<!DOCTYPE html><html><body><span>testtesttest</span><video src=""" + "http://192.168.0.95:50000/api/Post/GetFileByName?fileName=8a1acc40-b222-435e-8eeb-df9c57a0c3c2.mp4&type=3" +
            //          "\" controls height=\"150\" width=\"150\"></body></html>";

            ////http://192.168.0.95:50000/api/Post/GetFileByName?fileName=8aca2b81-da32-4023-a751-0f1ea51d9617.mp4&type=3
            ////http://192.168.0.95:50000/api/Post/GetFileByName?fileName=e78ef8f0-a800-4442-beb8-a26c2d426eb7.mp4&type=3
            //wv.Source = html;

            //Content = wv;
           
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        

    }
}
