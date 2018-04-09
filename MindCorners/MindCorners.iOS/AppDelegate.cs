using System;
using System.Collections.Generic;
using System.Linq;
using CarouselView.FormsPlugin.iOS;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using UIKit;
using XLabs.Forms;
using XLabs.Forms.Services;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Email;
using XLabs.Platform.Services.Media;

namespace MindCorners.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            SetIoc();
            //app.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
            LoadApplication(new App());
           
            return base.FinishedLaunching(app, options);
        }

        private void SetIoc()
        {

            var resolverContainer = new global::XLabs.Ioc.SimpleContainer();

            var app = new XFormsAppiOS();
            app.Init(this);

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
