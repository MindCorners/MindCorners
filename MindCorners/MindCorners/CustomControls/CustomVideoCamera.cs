using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomVideoCamera : Page
    {
        private VideoChatItemViewModel ViewModel;
        public CustomVideoCamera(VideoChatItemViewModel vm)
        {
            ClassId = "MindCorners.CustomControls.CustomVideoCamera";
            BackgroundColor=Color.FromHex("#5A5454");
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = vm;
            ViewModel = vm;
            OnPhotoResult += CustomCamera_OnPhotoResult;
        }

        private async void CustomCamera_OnPhotoResult(VideoResultEventArgs result)
        {  
            if (result.Success)
            {
                ViewModel.VideoSelected(result.VideoFilePath, result.Video);
            }
            else
            {
                await App.NavigationPage.PopAsync();
                //await Navigation.PopModalAsync();
                //if (!result.Success)
                //    return;

                //Photo.Source = ImageSource.FromStream(() => new MemoryStream(result.Image));
            }
        }

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(propertyName: "Camera",returnType: typeof(CameraOptionsNino),declaringType: typeof(CustomVideoCamera),defaultValue: CameraOptionsNino.Rear);
        public CameraOptionsNino Camera
        {
            get { return (CameraOptionsNino)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public static readonly BindableProperty TakePhotoCommandProperty = BindableProperty.Create(propertyName: "TakePhotoCommand", returnType: typeof(Command), declaringType: typeof(CustomVideoCamera));
        public Command TakePhotoCommand { 

            get { return (Command)GetValue(TakePhotoCommandProperty); }
            set { SetValue(TakePhotoCommandProperty, value); }
        }

        public delegate void VideoResultEventHandler(VideoResultEventArgs result);

        public event VideoResultEventHandler OnPhotoResult;

        public void SetPhotoResult(string videoFilePath, byte[] video, int width = -1, int height = -1)
        {
            OnPhotoResult?.Invoke(new VideoResultEventArgs(videoFilePath, video, width, height));
        }

        public void Cancel()
        {
            OnPhotoResult?.Invoke(new VideoResultEventArgs());
        }
        

        public static readonly BindableProperty StartVideoProperty = BindableProperty.Create("StartVideoImage", typeof(string), typeof(CustomVideoCamera));
        public string StartVideoImage
        {

            get { return (string)GetValue(StartVideoProperty); }
            set { SetValue(StartVideoProperty, value); }
        }


        public static readonly BindableProperty StopVideoProperty = BindableProperty.Create("StopVideoImage", typeof(string), typeof(CustomVideoCamera));
        public string StopVideoImage
        {

            get { return (string)GetValue(StopVideoProperty); }
            set { SetValue(StopVideoProperty, value); }
        }



        public static readonly BindableProperty PauseVideoProperty = BindableProperty.Create("PauseVideoImage", typeof(string), typeof(CustomVideoCamera));
        public string PauseVideoImage
        {

            get { return (string)GetValue(PauseVideoProperty); }
            set { SetValue(PauseVideoProperty, value); }
        }


        public static readonly BindableProperty CancelImageProperty = BindableProperty.Create("CancelImage", typeof(string), typeof(CustomVideoCamera));
        public string CancelImage
        {

            get { return (string)GetValue(CancelImageProperty); }
            set { SetValue(CancelImageProperty, value); }
        }

        public static readonly BindableProperty FlashLightOnImageProperty = BindableProperty.Create("FlashLightOnImage", typeof(string), typeof(CustomVideoCamera));
        public string FlashLightOnImage
        {

            get { return (string)GetValue(FlashLightOnImageProperty); }
            set { SetValue(FlashLightOnImageProperty, value); }
        }
        public static readonly BindableProperty FlashLightOffImageProperty = BindableProperty.Create("FlashLightOffImage", typeof(string), typeof(CustomVideoCamera));
        public string FlashLightOffImage
        {

            get { return (string)GetValue(FlashLightOffImageProperty); }
            set { SetValue(FlashLightOffImageProperty, value); }
        }
        public static readonly BindableProperty FlashLightAutoImageProperty = BindableProperty.Create("FlashLightAutoImage", typeof(string), typeof(CustomVideoCamera));
        public string FlashLightAutoImage
        {

            get { return (string)GetValue(FlashLightAutoImageProperty); }
            set { SetValue(FlashLightAutoImageProperty, value); }
        }

        public static readonly BindableProperty CameraRotateImageProperty = BindableProperty.Create("CameraRotateImage", typeof(string), typeof(CustomVideoCamera));
        public string CameraRotateImage
        {

            get { return (string)GetValue(CameraRotateImageProperty); }
            set { SetValue(CameraRotateImageProperty, value); }
        }

        public static readonly BindableProperty PictureGallaryImageProperty = BindableProperty.Create("PictureGallaryImage", typeof(string), typeof(CustomVideoCamera));
        public string PictureGallaryImage
        {

            get { return (string)GetValue(PictureGallaryImageProperty); }
            set { SetValue(PictureGallaryImageProperty, value); }
        }
    }

    public class VideoResultEventArgs : EventArgs
    {

        public VideoResultEventArgs()
        {
            Success = false;
        }

        public VideoResultEventArgs(string videoFilePath, byte[] video, int width, int height)
        {
            Success = true;
            VideoFilePath = videoFilePath;
            Video = video;
            Width = width;
            Height = height;
        }
        public string VideoFilePath { get; private set; }
        public byte[] Video { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Success { get; private set; }
    }
}
