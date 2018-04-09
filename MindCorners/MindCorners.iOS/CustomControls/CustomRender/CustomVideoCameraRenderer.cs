using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetsLibrary;
using AVFoundation;
using CoreAnimation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using Plugin.Media;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CustomVideoCamera), typeof(CustomVideoCameraRenderer))]

namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class CustomVideoCameraRenderer : PageRenderer
    {
        Boolean weAreRecording;
        bool flashOn = false;
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureDeviceInput audioInput;
        AVCaptureStillImageOutput stillImageOutput;
        AVCaptureMovieFileOutput output;
        UIView liveCameraStream;
        NSTimer recordTimeTimer;

        UILabel recordTimeLabel;
        UIButton videoButton;
        UIButton cancelPhotoButton;
        UIButton flashButton;
        UIButton rotateButton;
        UIButton photoGallaryButton;
		UIActivityIndicatorView activitySpinner;
        // UIPaintCodeButton takePhotoButton;
        // UIPaintCodeButton cancelPhotoButton;

        //protected override async void OnElementChanged(ElementChangedEventArgs<View> e)
        //{
        //    base.OnElementChanged(e);

        //    SetupUserInterface();
        //    SetupEventHandlers();
        //    await AuthorizeCameraUse();
        //    SetupLiveCameraStream();

        //}

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            long totalSeconds = 10000;
            Int32 preferredTimeScale = 30;
            CMTime maxDuration = new CMTime(totalSeconds, preferredTimeScale);

            output = new AVCaptureMovieFileOutput();
            //output.MinFreeDiskSpaceLimit = 1024 * 1024;
           // output.MaxRecordedDuration = maxDuration;

            SetupUserInterface();
            SetupEventHandlers();
            await AuthorizeCameraUse();
            SetupLiveCameraStream();




        }

        private void SetupUserInterface()
        {
            var centerButtonX = View.Bounds.GetMidX() - 35f;
            var bottomButtonY = View.Bounds.Bottom - 85;

            var topRightX = View.Bounds.Right - 65;
            var topLeftX = View.Bounds.X + 25;
            var topButtonY = View.Bounds.Top + 25;
            var buttonWidth = 70;
            var buttonHeight = 70;

            liveCameraStream = new UIView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
				Frame = new CGRect(0, 0, View.Frame.Width, View.Frame.Height)
            };

            View.AddSubview(liveCameraStream);

            liveCameraStream.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            liveCameraStream.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
            liveCameraStream.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            liveCameraStream.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;


            var element = (Element as CustomVideoCamera);
            if (element != null)
            {
                recordTimeLabel = new UILabel();
                recordTimeLabel.TranslatesAutoresizingMaskIntoConstraints = false;
                recordTimeLabel.TextAlignment = UITextAlignment.Center;
                //recordTimeLabel.Frame = new CGRect(centerButtonX, topButtonY, 100, 37);
                //recordTimeLabel.Text = "00:00:00";
                recordTimeLabel.TextColor = Color.FromHex("#EF1846").ToUIColor();
                recordTimeLabel.SizeToFit();

                videoButton = UIButton.FromType(UIButtonType.Custom);
                videoButton.TranslatesAutoresizingMaskIntoConstraints = false;
                //videoButton.Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight);
                videoButton.SetImage(UIImage.FromFile(element.StartVideoImage), UIControlState.Normal);


                cancelPhotoButton = UIButton.FromType(UIButtonType.Custom);
                cancelPhotoButton.TranslatesAutoresizingMaskIntoConstraints = false;
                //cancelPhotoButton.Frame = new CGRect(topLeftX, topButtonY, 37, 37);
                cancelPhotoButton.SetImage(UIImage.FromFile(element.CancelImage), UIControlState.Normal);
                cancelPhotoButton.SizeToFit();


                flashButton = UIButton.FromType(UIButtonType.Custom);
                flashButton.TranslatesAutoresizingMaskIntoConstraints = false;
                //flashButton.Frame = new CGRect(topRightX, topButtonY, 37, 37);
                flashButton.SetImage(UIImage.FromFile(element.FlashLightOnImage), UIControlState.Normal);
                flashButton.SizeToFit();


                photoGallaryButton = UIButton.FromType(UIButtonType.Custom);
                photoGallaryButton.TranslatesAutoresizingMaskIntoConstraints = false;
                //photoGallaryButton.Frame = new CGRect(topLeftX, bottomButtonY + 23, 37, 37);
                photoGallaryButton.SetImage(UIImage.FromFile(element.PictureGallaryImage), UIControlState.Normal);
                photoGallaryButton.SizeToFit();

                rotateButton = UIButton.FromType(UIButtonType.Custom);
                rotateButton.TranslatesAutoresizingMaskIntoConstraints = false;
                //rotateButton.Frame = new CGRect(topRightX, bottomButtonY + 23, 37, 37);
                rotateButton.SetImage(UIImage.FromFile(element.CameraRotateImage), UIControlState.Normal);
                rotateButton.SizeToFit();

				activitySpinner =  new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
				activitySpinner.TranslatesAutoresizingMaskIntoConstraints = false;
				activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
				activitySpinner.SizeToFit();
				activitySpinner.HidesWhenStopped = true;
				activitySpinner.BackgroundColor = Color.FromHex("#E2E8F1").ToUIColor();
				activitySpinner.Alpha = 0.8f;
					
                View.AddSubview(recordTimeLabel);
                recordTimeLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
                recordTimeLabel.TopAnchor.ConstraintEqualTo(View.TopAnchor, 25).Active = true;
                recordTimeLabel.WidthAnchor.ConstraintEqualTo(100).Active = true;



                View.AddSubview(cancelPhotoButton);
                cancelPhotoButton.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, 25).Active = true;
                cancelPhotoButton.TopAnchor.ConstraintEqualTo(View.TopAnchor, 25).Active = true;
                //cancelPhotoButton.RightAnchor.ConstraintEqualTo(recordTimeLabel.LeftAnchor).Active = true;

                View.AddSubview(flashButton);
                flashButton.RightAnchor.ConstraintEqualTo(View.RightAnchor, -25).Active = true;
                flashButton.TopAnchor.ConstraintEqualTo(View.TopAnchor, 25).Active = true;
                //flashButton.LeftAnchor.ConstraintEqualTo(recordTimeLabel.RightAnchor).Active = true;

                View.AddSubview(videoButton);
                videoButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
                videoButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -25).Active = true;

                View.AddSubview(photoGallaryButton);
                photoGallaryButton.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, 25).Active = true;
                photoGallaryButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -25).Active = true;

                View.AddSubview(rotateButton);
                rotateButton.RightAnchor.ConstraintEqualTo(View.RightAnchor, -25).Active = true;
                rotateButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -25).Active = true;

				View.AddSubview (activitySpinner);
				activitySpinner.WidthAnchor.ConstraintEqualTo (View.WidthAnchor).Active = true;
				activitySpinner.HeightAnchor.ConstraintEqualTo (View.HeightAnchor).Active = true;
            }


            //takePhotoButton = new UIPaintCodeButton(DrawTakePhotoButton)
            //{
            //    Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight)
            //};

            //cancelPhotoButton = new UIPaintCodeButton(DrawCancelPictureButton)
            //{
            //    Frame = new CGRect(topLeftX, topButtonY, 37, 37)
            //};



        }


        private void SetupEventHandlers()
        {

            cancelPhotoButton.TouchUpInside += (s, e) =>
            {
                (Element as CustomVideoCamera).Cancel();
            };

            videoButton.TouchUpInside += (s, e) =>
            {
                var element = (Element as CustomVideoCamera);
                //AssetsLibrary.ALAssetsLibrary li = new 
                // var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                // var library = System.IO.Path.Combine(documents, "..", "Library");
                var urlpath = System.IO.Path.Combine(Path.GetTempPath(), "sweetMovieFilm.mov");
                if (!weAreRecording)
                {
					
                    recordTimeTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.5), delegate {

                        recordTimeLabel.Text = TimeSpan.FromSeconds(output.RecordedDuration.Seconds).ToString(@"mm\:ss");
                        //Write Action Here
                    });

                    NSUrl url = new NSUrl(urlpath, false);

                    NSFileManager manager = new NSFileManager();
                    NSError error = new NSError();

                    if (manager.FileExists(urlpath))
                    {
                        Console.WriteLine("Deleting File");
                        manager.Remove(urlpath, out error);
                        Console.WriteLine("Deleted File");
                    }

                    //var dataOutput = new AVCaptureVideoDataOutput()
                    //{
                    //    AlwaysDiscardsLateVideoFrames = true,
                    //    WeakVideoSettings = new CVPixelBufferAttributes { PixelFormatType = CVPixelFormatType.CV32BGRA }.Dictionary
                    //};

                   
                    AVCaptureConnection connection = null;
                    if (output.Connections != null)
                    {
                        foreach (AVCaptureConnection connectionItem in output.Connections)
                        {
                            foreach (AVCaptureInputPort port in connectionItem.InputPorts)
                            {
                                if (port.MediaType == AVMediaType.Video)
                                {
                                    connection = connectionItem;
                                    break;
                                }
                            }
                        }
                    }

                    if (connection != null && connection.SupportsVideoOrientation)
                    {
                        connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
                    }
                    //(AVCaptureConnection)output.Connections [0];
                    if (connection != null)
                    {
						CustomAvCaptureFileOutPutRecordingDelegate avDel = new CustomAvCaptureFileOutPutRecordingDelegate();
						avDel.Element = element;
						avDel.activityIndicator = activitySpinner;
						//output.StartRecordingToOutputFile(url, avDel);
						output.StartRecordingToOutputFile(url, avDel);
                    }

                    Console.WriteLine(urlpath);
                    weAreRecording = true;

                    videoButton.SetImage(UIImage.FromFile(element.StopVideoImage), UIControlState.Normal);
                }
                //we were already recording.  Stop recording
                else
                {
					
					activitySpinner.StartAnimating();

                    output.StopRecording();

                    videoButton.SetImage(UIImage.FromFile(element.StartVideoImage), UIControlState.Normal);
					recordTimeLabel.Text = "";
                    Console.WriteLine("stopped recording");
                    weAreRecording = false;
                    recordTimeTimer.Invalidate();
                }
            };

            flashButton.TouchUpInside += (s, e) =>
            {
                var element = (Element as CustomVideoCamera);
                var device = captureDeviceInput.Device;

                var error = new NSError();
                if (device.HasFlash)
                {
                    if (device.FlashMode == AVCaptureFlashMode.On)
                    {
                        device.LockForConfiguration(out error);
                        device.FlashMode = AVCaptureFlashMode.Off;
                        device.UnlockForConfiguration();

                        flashButton.SetBackgroundImage(UIImage.FromBundle(element.FlashLightOnImage), UIControlState.Normal);
                    }
                    else
                    {
                        device.LockForConfiguration(out error);
                        device.FlashMode = AVCaptureFlashMode.On;
                        device.UnlockForConfiguration();

                        flashButton.SetBackgroundImage(UIImage.FromBundle(element.FlashLightOffImage), UIControlState.Normal);
                    }
                }

                flashOn = !flashOn;

            };


            photoGallaryButton.TouchUpInside += (s, e) =>
            {
                var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.PhotoLibrary, MediaTypes = new string[] { "public.movie" } };
                imagePicker.AllowsEditing = false;

                //imagePicker.ShowsCameraControls = false;
                // imagePicker.ShowsCameraControls = false;
                //Make sure we have the root view controller which will launch the photo gallery 
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                //Show the image gallery
                vc.PresentViewController(imagePicker, true, null);

                //call back for when a picture is selected and finished editing
                imagePicker.FinishedPickingMedia += (sender, e2) =>
                {
                    if (e2.Info[UIImagePickerController.MediaType].ToString() == "public.movie")
                    {
                        NSUrl mediaURL = e2.Info[UIImagePickerController.MediaURL] as NSUrl;
                        if (mediaURL != null)
                        {
                            Console.WriteLine(mediaURL.ToString());
                            NSData data = NSData.FromUrl(mediaURL);
                            byte[] dataBytes = new byte[data.Length];
                            System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
                            (Element as CustomVideoCamera).SetPhotoResult(mediaURL.ToString(), dataBytes, 0, 0);
                        }
                    }


                    //UIImage originalImage = e2.Info[UIImagePickerController.OriginalImage] as UIImage;
                    //if (originalImage != null)
                    //{
                    //    //Got the image now, convert it to byte array to send back up to the forms project
                    //    var pngImage = originalImage.AsPNG();
                    //    //  UIImage imageInfo = new UIImage(pngImage);
                    //    byte[] myByteArray = new byte[pngImage.Length];
                    //    System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));

                    //    (Element as CustomVideoCamera).SetPhotoResult(originalImage.pmyByteArray,
                    //                                      (int)originalImage.Size.Width,
                    //                                      (int)originalImage.Size.Height);

                    //    //System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));

                    //    //MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");
                    //}

                    //Close the image gallery on the UI thread
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        vc.DismissViewController(true, null);
                    });
                };

                //Cancel button callback from the image gallery
                imagePicker.Canceled += (sender, e1) =>
                {
                    vc.DismissViewController(true, null);
                    //(Element as CustomCamera).Cancel();
                };

                //(Element as CustomCamera).Cancel();
            };

            rotateButton.TouchUpInside += (s, e) =>
            {
                var devicePosition = captureDeviceInput.Device.Position;
                if (devicePosition == AVCaptureDevicePosition.Front)
                {
                    devicePosition = AVCaptureDevicePosition.Back;
                }
                else
                {
                    devicePosition = AVCaptureDevicePosition.Front;
                }

                var device = GetCameraForOrientation(devicePosition);
                ConfigureCameraForDevice(device);

                captureSession.BeginConfiguration();
                captureSession.RemoveInput(captureDeviceInput);
                captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
                captureSession.AddInput(captureDeviceInput);
                captureSession.CommitConfiguration();
            };
        }

        public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (var device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }
            return null;
        }

        public async Task<NSData> CapturePhoto()
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            return jpegImageAsNsData;
        }

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			videoPreviewLayer.Frame = liveCameraStream.Bounds;
		}
		AVCaptureVideoPreviewLayer videoPreviewLayer;
        public void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = liveCameraStream.Bounds
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            if (captureDevice != null)
            {
                ConfigureCameraForDevice(captureDevice);
                captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

                var dictionary = new NSMutableDictionary();
                dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
                stillImageOutput = new AVCaptureStillImageOutput()
                {
                    OutputSettings = new NSDictionary()
                };

                captureSession.AddOutput(stillImageOutput);
                captureSession.AddInput(captureDeviceInput);
                var captureAudioDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Audio);
                if (captureAudioDevice != null)
                {
                    audioInput = AVCaptureDeviceInput.FromDevice(captureAudioDevice);
                    captureSession.AddInput(audioInput);
                }

                if (captureSession.CanAddOutput(output))
                {
                    captureSession.AddOutput(output);
                }

                captureSession.StartRunning();
            }


        }


        public void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device != null)
            {
                if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
                {
                    device.LockForConfiguration(out error);
                    device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                    device.UnlockForConfiguration();
                }
                else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
                {
                    device.LockForConfiguration(out error);
                    device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                    device.UnlockForConfiguration();
                }
                else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
                {
                    device.LockForConfiguration(out error);
                    device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                    device.UnlockForConfiguration();
                }
            }

        }

        public async Task AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }
    }

    public class CustomAvCaptureFileOutPutRecordingDelegate : AVCaptureFileOutputRecordingDelegate
    {
        public VisualElement Element;
		public UIActivityIndicatorView activityIndicator;
        public override void FinishedRecording(AVCaptureFileOutput captureOutput, Foundation.NSUrl outputFileUrl, Foundation.NSObject[] connections, Foundation.NSError error)
        {
            NSUrl urlCompressed = new NSUrl(System.IO.Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".mov"), false);
            compressVideo(outputFileUrl, urlCompressed);

			/*
			NSData data = NSData.FromUrl(outputFileUrl);
			byte[] dataBytes = new byte[data.Length];
			System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
			UIApplication.SharedApplication.InvokeOnMainThread(delegate
				{
					(Element as CustomVideoCamera).SetPhotoResult(outputFileUrl.ToString(), dataBytes, 0, 0);
					activityIndicator.StopAnimating ();
				});
*/
            //You can use captureOutput and outputFileUrl here.. 
            //throw new NotImplementedException();
        }

        private void compressVideo(NSUrl inputURL, NSUrl outputURL)
        {
            NSUrl url = inputURL;
            var urlAsset = new AVUrlAsset(inputURL);
            AVAssetExportSession exportSession = new AVAssetExportSession(urlAsset, AVAssetExportSessionPreset.MediumQuality);
            exportSession.OutputUrl = outputURL;
            exportSession.OutputFileType = AVFileType.QuickTimeMovie;
            exportSession.ShouldOptimizeForNetworkUse = true;

            exportSession.ExportAsynchronously(() =>
            {
                NSData data = NSData.FromUrl(outputURL);
                byte[] dataBytes = new byte[data.Length];
                System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
					UIApplication.SharedApplication.InvokeOnMainThread(delegate
						{
							(Element as CustomVideoCamera).SetPhotoResult(outputURL.ToString(), dataBytes, 0, 0);
							activityIndicator.StopAnimating ();
						});
                
            });
        }
    }
}