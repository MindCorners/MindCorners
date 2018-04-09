using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomCamera : Page
    {
        private ICamera ViewModel;
        public CustomCamera(ICamera vm)
        {
            ClassId = "MindCorners.CustomControls.CustomCamera";
            BackgroundColor=Color.Black;
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = vm;
            ViewModel = vm;
            OnPhotoResult += CustomCamera_OnPhotoResult;
        }

        private async void CustomCamera_OnPhotoResult(PhotoResultEventArgs result)
        {  
            if (result.Success)
            {
                ViewModel.PhotoSelected(result.Image);
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

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(propertyName: "Camera",returnType: typeof(CameraOptionsNino),declaringType: typeof(CustomCamera),defaultValue: CameraOptionsNino.Rear);
        public CameraOptionsNino Camera
        {
            get { return (CameraOptionsNino)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public static readonly BindableProperty TakePhotoCommandProperty = BindableProperty.Create(propertyName: "TakePhotoCommand", returnType: typeof(Command), declaringType: typeof(CustomCamera));
        public Command TakePhotoCommand { 

            get { return (Command)GetValue(TakePhotoCommandProperty); }
            set { SetValue(TakePhotoCommandProperty, value); }
        }

        public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);

        public event PhotoResultEventHandler OnPhotoResult;

        public void SetPhotoResult(byte[] image, int width = -1, int height = -1)
        {
            OnPhotoResult?.Invoke(new PhotoResultEventArgs(image, width, height));
        }

        public void Cancel()
        {
            OnPhotoResult?.Invoke(new PhotoResultEventArgs());
        }
        

        public static readonly BindableProperty TakePhotoImageProperty = BindableProperty.Create("TakePhotoImage", typeof(string), typeof(CustomCamera));
        public string TakePhotoImage
        {

            get { return (string)GetValue(TakePhotoImageProperty); }
            set { SetValue(TakePhotoImageProperty, value); }
        }

        public static readonly BindableProperty CancelImageProperty = BindableProperty.Create("CancelImage", typeof(string), typeof(CustomCamera));
        public string CancelImage
        {

            get { return (string)GetValue(CancelImageProperty); }
            set { SetValue(CancelImageProperty, value); }
        }

        public static readonly BindableProperty FlashLightOnImageProperty = BindableProperty.Create("FlashLightOnImage", typeof(string), typeof(CustomCamera));
        public string FlashLightOnImage
        {

            get { return (string)GetValue(FlashLightOnImageProperty); }
            set { SetValue(FlashLightOnImageProperty, value); }
        }
        public static readonly BindableProperty FlashLightOffImageProperty = BindableProperty.Create("FlashLightOffImage", typeof(string), typeof(CustomCamera));
        public string FlashLightOffImage
        {

            get { return (string)GetValue(FlashLightOffImageProperty); }
            set { SetValue(FlashLightOffImageProperty, value); }
        }
        public static readonly BindableProperty FlashLightAutoImageProperty = BindableProperty.Create("FlashLightAutoImage", typeof(string), typeof(CustomCamera));
        public string FlashLightAutoImage
        {

            get { return (string)GetValue(FlashLightAutoImageProperty); }
            set { SetValue(FlashLightAutoImageProperty, value); }
        }

        public static readonly BindableProperty CameraRotateImageProperty = BindableProperty.Create("CameraRotateImage", typeof(string), typeof(CustomCamera));
        public string CameraRotateImage
        {

            get { return (string)GetValue(CameraRotateImageProperty); }
            set { SetValue(CameraRotateImageProperty, value); }
        }

        public static readonly BindableProperty PictureGallaryImageProperty = BindableProperty.Create("PictureGallaryImage", typeof(string), typeof(CustomCamera));
        public string PictureGallaryImage
        {

            get { return (string)GetValue(PictureGallaryImageProperty); }
            set { SetValue(PictureGallaryImageProperty, value); }
        }
    }

    public class PhotoResultEventArgs : EventArgs
    {

        public PhotoResultEventArgs()
        {
            Success = false;
        }

        public PhotoResultEventArgs(byte[] image, int width, int height)
        {
            Success = true;
            Image = image;
            Width = width;
            Height = height;
        }

        public byte[] Image { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Success { get; private set; }
    }

    public enum CameraOptionsNino
    {
        Rear,
        Front
    }
}
