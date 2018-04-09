using System;
using System.Reflection;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using ImageCircle.Forms.Plugin.Droid;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Octane.Xam.VideoPlayer.Android;
using Plugin.Permissions;
using Xamarin.Forms;
using ImageCircleRenderer = ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer;


namespace MindCorners.Droid
{   
    [Activity(Label = "MindCorners", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait) ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            
            var cv = typeof(Xamarin.Forms.CarouselView);
            var assembly = Assembly.Load(cv.FullName);
            LoadApplication(new App());

           FormsVideoPlayer.Init();
            
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

