using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetsLibrary;
using AVFoundation;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using Plugin.Media;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CustomCamera), typeof(CameraPageRenderer))]

namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class CameraPageRenderer : PageRenderer
    {
        bool flashOn = false;
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        UIView liveCameraStream;


        UIButton takePhotoButton;
        UIButton cancelPhotoButton;
        UIButton flashButton;
        UIButton rotateButton;
        UIButton photoGallaryButton;
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
                Frame = new CGRect(0f, 0f, View.Bounds.Width, View.Bounds.Height)
            };

            View.AddSubview(liveCameraStream);

            var element = (Element as CustomCamera);
            if (element != null)
            {
                takePhotoButton = UIButton.FromType(UIButtonType.Custom);
                takePhotoButton.Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight);
                takePhotoButton.SetImage(UIImage.FromFile(element.TakePhotoImage), UIControlState.Normal);


                cancelPhotoButton = UIButton.FromType(UIButtonType.Custom);
                cancelPhotoButton.Frame = new CGRect(topLeftX, topButtonY, 37, 37);
                cancelPhotoButton.SetImage(UIImage.FromFile(element.CancelImage), UIControlState.Normal);
                cancelPhotoButton.SizeToFit();


                flashButton = UIButton.FromType(UIButtonType.Custom);
                flashButton.Frame = new CGRect(topRightX, topButtonY, 37, 37);
                flashButton.SetImage(UIImage.FromFile(element.FlashLightOnImage), UIControlState.Normal);
                flashButton.SizeToFit();


                photoGallaryButton = UIButton.FromType(UIButtonType.Custom);
                photoGallaryButton.Frame = new CGRect(topLeftX, bottomButtonY+23, 37, 37);
                photoGallaryButton.SetImage(UIImage.FromFile(element.PictureGallaryImage), UIControlState.Normal);
                photoGallaryButton.SizeToFit();
                
                rotateButton = UIButton.FromType(UIButtonType.Custom);
                rotateButton.Frame = new CGRect(topRightX, bottomButtonY + 23, 37, 37);
                rotateButton.SetImage(UIImage.FromFile(element.CameraRotateImage), UIControlState.Normal);
                rotateButton.SizeToFit();

                View.AddSubview(takePhotoButton);
                View.AddSubview(cancelPhotoButton);
                View.AddSubview(flashButton);
                View.AddSubview(photoGallaryButton);
                View.AddSubview(rotateButton);
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
                (Element as CustomCamera).Cancel();
            };

            takePhotoButton.TouchUpInside += async (s, e) =>
            {
                var data = await CapturePhoto();
                if (data != null)
                {
                    UIImage imageInfo = new UIImage(data);

					byte[] myByteArray = RotateImage(imageInfo);

					(Element as CustomCamera).SetPhotoResult(myByteArray,
                                                                (int)imageInfo.Size.Width,
                                                                (int)imageInfo.Size.Height);
                }
            };

            flashButton.TouchUpInside += (s, e) =>
            {
                var element = (Element as CustomCamera);
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
                var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.PhotoLibrary, MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) };
                imagePicker.AllowsEditing = false;

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
                    UIImage originalImage = e2.Info[UIImagePickerController.OriginalImage] as UIImage;
                    if (originalImage != null)
                    {
                        //UIImage img = ScaleAndRotateImage(originalImage, originalImage.Orientation);

                        //Got the image now, convert it to byte array to send back up to the forms project
                        //var pngImage = img.AsPNG();

                        //Got the image now, convert it to byte array to send back up to the forms project
                        //var pngImage = img != null ? img.AsPNG() : originalImage.AsPNG();
                        //var pngImage = originalImage.AsPNG();



                        //Got the image now, convert it to byte array to send back up to the forms project
                        //  var pngImage = originalImage.AsPNG();
                        //  UIImage imageInfo = new UIImage(pngImage);
                        // byte[] myByteArray = new byte[pngImage.Length];
                        // System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));

                        byte[] myByteArray = RotateImage(originalImage);
                        (Element as CustomCamera).SetPhotoResult(myByteArray,
                                                          (int)originalImage.Size.Width,
                                                          (int)originalImage.Size.Height);

                        //System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));

                        //MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");
                    }

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

		private byte[] RotateImageTaken(UIImage image)
		{
			UIImage imageToReturn = null;
			if (image.Size.Height > image.Size.Width)
			{
				imageToReturn = image;
			}
			else
			{
				CGAffineTransform transform = CGAffineTransform.MakeIdentity();
				transform.Rotate(-(float)Math.PI / 2);
				transform.Translate(0, image.Size.Width);
				//now draw image
				using (var context = new CGBitmapContext(IntPtr.Zero,
														(int)image.Size.Height,
														(int)image.Size.Width,
														image.CGImage.BitsPerComponent,
														image.CGImage.BytesPerRow,
														image.CGImage.ColorSpace,
														image.CGImage.BitmapInfo))
				{
					context.ConcatCTM(transform);
					context.DrawImage(new RectangleF(PointF.Empty, new SizeF((float)image.Size.Width, (float)image.Size.Height)), image.CGImage);

					using (var imageRef = context.ToImage())
					{
						imageToReturn = new UIImage(imageRef);
					}
				}
			}

			using (NSData imageData = imageToReturn.AsPNG())
			{
				Byte[] byteArray = new Byte[imageData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, byteArray, 0, Convert.ToInt32(imageData.Length));
				return byteArray;
			}
		}

        private byte[] RotateImage(UIImage image)
        {
            UIImage imageToReturn = null;
            if (image.Orientation == UIImageOrientation.Up)
            {
                imageToReturn = image;
            }
            else
            {
                CGAffineTransform transform = CGAffineTransform.MakeIdentity();

                switch (image.Orientation)
                {
                    case UIImageOrientation.Down:
                    case UIImageOrientation.DownMirrored:
                        transform.Rotate((float)Math.PI);
                        transform.Translate(image.Size.Width, image.Size.Height);
                        break;

                    case UIImageOrientation.Left:
                    case UIImageOrientation.LeftMirrored:
                        transform.Rotate((float)Math.PI / 2);
                        transform.Translate(image.Size.Width, 0);
                        break;

                    case UIImageOrientation.Right:
                    case UIImageOrientation.RightMirrored:
                        transform.Rotate(-(float)Math.PI / 2);
                        transform.Translate(0, image.Size.Height);
                        break;
                    case UIImageOrientation.Up:
                    case UIImageOrientation.UpMirrored:
                        break;
                }

                switch (image.Orientation)
                {
                    case UIImageOrientation.UpMirrored:
                    case UIImageOrientation.DownMirrored:
                        transform.Translate(image.Size.Width, 0);
                        transform.Scale(-1, 1);
                        break;

                    case UIImageOrientation.LeftMirrored:
                    case UIImageOrientation.RightMirrored:
                        transform.Translate(image.Size.Height, 0);
                        transform.Scale(-1, 1);
                        break;
                    case UIImageOrientation.Up:
                    case UIImageOrientation.Down:
                    case UIImageOrientation.Left:
                    case UIImageOrientation.Right:
                        break;
                }

                //now draw image
                using (var context = new CGBitmapContext(IntPtr.Zero,
                                                        (int)image.Size.Width,
                                                        (int)image.Size.Height,
                                                        image.CGImage.BitsPerComponent,
                                                        image.CGImage.BytesPerRow,
                                                        image.CGImage.ColorSpace,
                                                        image.CGImage.BitmapInfo))
                {
                    context.ConcatCTM(transform);
                    switch (image.Orientation)
                    {
                        case UIImageOrientation.Left:
                        case UIImageOrientation.LeftMirrored:
                        case UIImageOrientation.Right:
                        case UIImageOrientation.RightMirrored:
                            // Grr...
                            context.DrawImage(new RectangleF(PointF.Empty, new SizeF((float)image.Size.Height, (float)image.Size.Width)), image.CGImage);
                            break;
                        default:
                            context.DrawImage(new RectangleF(PointF.Empty, new SizeF((float)image.Size.Width, (float)image.Size.Height)), image.CGImage);
                            break;
                    }

                    using (var imageRef = context.ToImage())
                    {
                        imageToReturn = new UIImage(imageRef);
                    }
                }
            }

            using (NSData imageData = imageToReturn.AsPNG())
            {
                Byte[] byteArray = new Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, byteArray, 0, Convert.ToInt32(imageData.Length));
                return byteArray;
            }
        }

        private UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
        {
            CGImage imgRef = imageIn.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();
            RectangleF bounds = new RectangleF(0, 0, width, height);

            SizeF imageSize = new SizeF(width, height);
            UIImageOrientation orient = orIn;
            float boundHeight;

            switch (orient)
            {
                case UIImageOrientation.Up:                                        //EXIF = 1
                    transform = CGAffineTransform.MakeIdentity();
                    break;

                case UIImageOrientation.UpMirrored:                                //EXIF = 2
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    break;

                case UIImageOrientation.Down:                                      //EXIF = 3
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
                    break;

                case UIImageOrientation.DownMirrored:                              //EXIF = 4
                    transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
                    transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
                    break;

                case UIImageOrientation.LeftMirrored:                              //EXIF = 5
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Left:                                      //EXIF = 6
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.RightMirrored:                             //EXIF = 7
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Right:                                     //EXIF = 8
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                default:
                    throw new Exception("Invalid image orientation");
                    break;
            }

            UIGraphics.BeginImageContext(bounds.Size);

            CGContext context = UIGraphics.GetCurrentContext();

            if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
            {
                context.TranslateCTM(-height, 0);
            }
            else
            {
                context.TranslateCTM(0, -height);
            }

            context.ConcatCTM(transform);
            context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }
        public void RotateImage(UIImage imageToRotate, bool isCCW){
            UIImage imageToReturn = null;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();

           // var imageRotation = isCCW ? UIImageOrientation.Down : UIImageOrientation.Up;
			//imageToRotate = UIImage.FromImage (imageToRotate.CGImage, imageToRotate.CurrentScale, imageRotation);
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

        public void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
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


        #region Drawings

        private void DrawTakePhotoButton(CGRect frame)
        {
            var color = UIColor.White;

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.27302f * frame.Width, frame.GetMinY() + 0.15053f * frame.Height), new CGPoint(frame.GetMinX() + 0.41628f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height), new CGPoint(frame.GetMinX() + 0.33832f * frame.Width, frame.GetMinY() + 0.10803f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.08333f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.15883f * frame.Width, frame.GetMinY() + 0.22484f * frame.Height), new CGPoint(frame.GetMinX() + 0.08333f * frame.Width, frame.GetMinY() + 0.35360f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.91667f * frame.Height), new CGPoint(frame.GetMinX() + 0.08333f * frame.Width, frame.GetMinY() + 0.73012f * frame.Height), new CGPoint(frame.GetMinX() + 0.26988f * frame.Width, frame.GetMinY() + 0.91667f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.91667f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.73012f * frame.Width, frame.GetMinY() + 0.91667f * frame.Height), new CGPoint(frame.GetMinX() + 0.91667f * frame.Width, frame.GetMinY() + 0.73012f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height), new CGPoint(frame.GetMinX() + 0.91667f * frame.Width, frame.GetMinY() + 0.26988f * frame.Height), new CGPoint(frame.GetMinX() + 0.73012f * frame.Width, frame.GetMinY() + 0.08333f * frame.Height));
            bezierPath.ClosePath();
            bezierPath.MoveTo(new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 1.00000f * frame.Height), new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.77614f * frame.Height), new CGPoint(frame.GetMinX() + 0.77614f * frame.Width, frame.GetMinY() + 1.00000f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.00000f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.22386f * frame.Width, frame.GetMinY() + 1.00000f * frame.Height), new CGPoint(frame.GetMinX() + 0.00000f * frame.Width, frame.GetMinY() + 0.77614f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.19894f * frame.Width, frame.GetMinY() + 0.10076f * frame.Height), new CGPoint(frame.GetMinX() + 0.00000f * frame.Width, frame.GetMinY() + 0.33689f * frame.Height), new CGPoint(frame.GetMinX() + 0.07810f * frame.Width, frame.GetMinY() + 0.19203f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.00000f * frame.Height), new CGPoint(frame.GetMinX() + 0.28269f * frame.Width, frame.GetMinY() + 0.03751f * frame.Height), new CGPoint(frame.GetMinX() + 0.38696f * frame.Width, frame.GetMinY() + 0.00000f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.77614f * frame.Width, frame.GetMinY() + 0.00000f * frame.Height), new CGPoint(frame.GetMinX() + 1.00000f * frame.Width, frame.GetMinY() + 0.22386f * frame.Height));
            bezierPath.ClosePath();
            color.SetFill();
            bezierPath.Fill();
            UIColor.Black.SetStroke();
            bezierPath.LineWidth = 1.0f;
            bezierPath.Stroke();

            var ovalPath = UIBezierPath.FromOval(new CGRect(frame.GetMinX() + NMath.Floor(frame.Width * 0.12500f + 0.5f), frame.GetMinY() + NMath.Floor(frame.Height * 0.12500f + 0.5f), NMath.Floor(frame.Width * 0.87500f + 0.5f) - NMath.Floor(frame.Width * 0.12500f + 0.5f), NMath.Floor(frame.Height * 0.87500f + 0.5f) - NMath.Floor(frame.Height * 0.12500f + 0.5f)));
            color.SetFill();
            ovalPath.Fill();
            UIColor.Black.SetStroke();
            ovalPath.LineWidth = 1.0f;
            ovalPath.Stroke();
        }

        private void DrawCancelPictureButton(CGRect frame)
        {
            var color2 = UIColor.White;

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(frame.GetMinX() + 0.73928f * frame.Width, frame.GetMinY() + 0.14291f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.85711f * frame.Width, frame.GetMinY() + 0.26074f * frame.Height), new CGPoint(frame.GetMinX() + 0.73926f * frame.Width, frame.GetMinY() + 0.14289f * frame.Height), new CGPoint(frame.GetMinX() + 0.85711f * frame.Width, frame.GetMinY() + 0.26074f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.61785f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.85711f * frame.Width, frame.GetMinY() + 0.26074f * frame.Height), new CGPoint(frame.GetMinX() + 0.74457f * frame.Width, frame.GetMinY() + 0.37328f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.85355f * frame.Width, frame.GetMinY() + 0.73570f * frame.Height), new CGPoint(frame.GetMinX() + 0.74311f * frame.Width, frame.GetMinY() + 0.62526f * frame.Height), new CGPoint(frame.GetMinX() + 0.85355f * frame.Width, frame.GetMinY() + 0.73570f * frame.Height));
            bezierPath.AddLineTo(new CGPoint(frame.GetMinX() + 0.73570f * frame.Width, frame.GetMinY() + 0.85355f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.61785f * frame.Height), new CGPoint(frame.GetMinX() + 0.73570f * frame.Width, frame.GetMinY() + 0.85355f * frame.Height), new CGPoint(frame.GetMinX() + 0.62526f * frame.Width, frame.GetMinY() + 0.74311f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.26785f * frame.Width, frame.GetMinY() + 0.85000f * frame.Height), new CGPoint(frame.GetMinX() + 0.37621f * frame.Width, frame.GetMinY() + 0.74164f * frame.Height), new CGPoint(frame.GetMinX() + 0.26785f * frame.Width, frame.GetMinY() + 0.85000f * frame.Height));
            bezierPath.AddLineTo(new CGPoint(frame.GetMinX() + 0.15000f * frame.Width, frame.GetMinY() + 0.73215f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.38215f * frame.Width, frame.GetMinY() + 0.50000f * frame.Height), new CGPoint(frame.GetMinX() + 0.15000f * frame.Width, frame.GetMinY() + 0.73215f * frame.Height), new CGPoint(frame.GetMinX() + 0.25836f * frame.Width, frame.GetMinY() + 0.62379f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.14645f * frame.Width, frame.GetMinY() + 0.26430f * frame.Height), new CGPoint(frame.GetMinX() + 0.25689f * frame.Width, frame.GetMinY() + 0.37474f * frame.Height), new CGPoint(frame.GetMinX() + 0.14645f * frame.Width, frame.GetMinY() + 0.26430f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.22060f * frame.Width, frame.GetMinY() + 0.19014f * frame.Height), new CGPoint(frame.GetMinX() + 0.14645f * frame.Width, frame.GetMinY() + 0.26430f * frame.Height), new CGPoint(frame.GetMinX() + 0.18706f * frame.Width, frame.GetMinY() + 0.22369f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.26430f * frame.Width, frame.GetMinY() + 0.14645f * frame.Height), new CGPoint(frame.GetMinX() + 0.24420f * frame.Width, frame.GetMinY() + 0.16655f * frame.Height), new CGPoint(frame.GetMinX() + 0.26430f * frame.Width, frame.GetMinY() + 0.14645f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.50000f * frame.Width, frame.GetMinY() + 0.38215f * frame.Height), new CGPoint(frame.GetMinX() + 0.26430f * frame.Width, frame.GetMinY() + 0.14645f * frame.Height), new CGPoint(frame.GetMinX() + 0.37474f * frame.Width, frame.GetMinY() + 0.25689f * frame.Height));
            bezierPath.AddCurveToPoint(new CGPoint(frame.GetMinX() + 0.73926f * frame.Width, frame.GetMinY() + 0.14289f * frame.Height), new CGPoint(frame.GetMinX() + 0.62672f * frame.Width, frame.GetMinY() + 0.25543f * frame.Height), new CGPoint(frame.GetMinX() + 0.73926f * frame.Width, frame.GetMinY() + 0.14289f * frame.Height));
            bezierPath.AddLineTo(new CGPoint(frame.GetMinX() + 0.73928f * frame.Width, frame.GetMinY() + 0.14291f * frame.Height));
            bezierPath.ClosePath();
            color2.SetFill();
            bezierPath.Fill();
            UIColor.Black.SetStroke();
            bezierPath.LineWidth = 1.0f;
            bezierPath.Stroke();
        }

        #endregion

    }

    internal class UIPaintCodeButton : UIButton
    {
        Action<CGRect> _drawing;
        public UIPaintCodeButton(Action<CGRect> drawing)
        {
            _drawing = drawing;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            _drawing(rect);
        }

    }

    //public class CustomCameraRenderer : ViewRenderer<MindCorners.CustomControls.CustomCamera, UICameraPreview>
    //{
    //    UICameraPreview uiCameraPreview;

    //    protected override void OnElementChanged(ElementChangedEventArgs<CustomCamera> e)
    //    {
    //        base.OnElementChanged(e);

    //        if (Element != null)
    //        {
    //            // var view = new MindCorners.CustomControls.CustomCameraTest();
    //            var renderer = Platform.GetRenderer(Element.Content);

    //            renderer.NativeView.Frame = UIScreen.MainScreen.Bounds;

    //            // renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
    //            // renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

    //            renderer.Element.Layout(UIScreen.MainScreen.Bounds.ToRectangle());

    //            var nativeView = renderer.NativeView;

    //            // nativeView.SetNeedsLayout();

    //            if (Control == null)
    //            {
    //                uiCameraPreview = new UICameraPreview(e.NewElement.Camera);
    //                SetNativeControl(uiCameraPreview);
    //            }
    //            if (e.OldElement != null)
    //            {
    //                // Unsubscribe
    //                uiCameraPreview.Tapped -= OnCameraPreviewTapped;
    //            }
    //            if (e.NewElement != null)
    //            {
    //                //e.NewElement.TakePhotoCommand.Execute(null);
    //                // Subscribe
    //                uiCameraPreview.Tapped += OnCameraPreviewTapped;
    //            }

    //            //uiCameraPreview.InsertSubviewAbove(nativeView, uiCameraPreview);
    //        }

    //    }

    //    void OnCameraPreviewTapped(object sender, EventArgs e)
    //    {
    //        if (uiCameraPreview.IsPreviewing)
    //        {
    //            uiCameraPreview.CaptureSession.StopRunning();
    //            uiCameraPreview.IsPreviewing = false;
    //        }
    //        else
    //        {
    //            uiCameraPreview.CaptureSession.StartRunning();
    //            uiCameraPreview.IsPreviewing = true;
    //        }
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            Control.CaptureSession.Dispose();
    //            Control.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}
    public class UICameraPreview : UIView
    {
        AVCaptureVideoPreviewLayer previewLayer;
        CameraOptionsNino cameraOptions;

        public event EventHandler<EventArgs> Tapped;

        public AVCaptureSession CaptureSession { get; private set; }

        public bool IsPreviewing { get; set; }

        public UICameraPreview(CameraOptionsNino options)
        {
            cameraOptions = options;
            IsPreviewing = false;
            Initialize();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            previewLayer.Frame = rect;

        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            OnTapped();
        }

        protected virtual void OnTapped()
        {
            var eventHandler = Tapped;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        void Initialize()
        {
            CaptureSession = new AVCaptureSession();
            previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
            {
                Frame = Bounds,
                VideoGravity = AVLayerVideoGravity.ResizeAspectFill
            };

            var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
            var cameraPosition = (cameraOptions == CameraOptionsNino.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
            var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition);

            if (device == null)
            {
                return;
            }

            NSError error;
            var input = new AVCaptureDeviceInput(device, out error);
            CaptureSession.AddInput(input);
            Layer.AddSublayer(previewLayer);
            
            UIButton button = new UIButton();
            CGRect ScreenBounds = UIScreen.MainScreen.Bounds;
            float buttonWidth = (float)ScreenBounds.X / 4;
            button.Frame = new CGRect(0f, 0f, buttonWidth, 50f);
            button.SetTitle("Title", UIControlState.Normal);
            button.BackgroundColor = UIColor.Green;

            

            // this.AddSubview(button);
            //this.InsertSubviewAbove(button, this);
            //Layer.AddSublayer(button);

            CaptureSession.StartRunning();
            IsPreviewing = true;
        }

        
    }



    /*


        UIImagePickerController imagePicker;
        UIViewController viewController;
        UIWindow window;
        //UICameraPreview uiCameraPreview;


        protected override async  void OnElementChanged(ElementChangedEventArgs<CustomCamera> e)
        {
            base.OnElementChanged(e);

            // create a new picker controller
            imagePicker = new UIImagePickerController();
            //imagePicker.RotatingFooterView = 
            // set our source to the photo library
            //imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;

            // set what media types
           // imagePicker.MediaTypes = new string[] { "public.image" };

            imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
            //imagePicker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
            //imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary | UIImagePickerControllerSourceType.Camera); 
            //imagePicker.CameraDevice = UIImagePickerControllerCameraDevice.Rear;
            //imagePicker.ShowsCameraControls = false;
            //imagePicker.NavigationBarHidden = true;
            //imagePicker.ToolbarHidden = true;
            //imagePicker.WantsFullScreenLayout = true;

           
            //imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
            //imagePicker.Canceled += Handle_Canceled;

            // show the picker

            window = UIApplication.SharedApplication.KeyWindow;
            viewController = window.RootViewController;

            if (viewController == null)
            {
                while (viewController.PresentedViewController != null)
                    viewController = viewController.PresentedViewController;
                await viewController.PresentViewControllerAsync(imagePicker, true);
            }
            else
                await viewController.PresentViewControllerAsync(imagePicker, true);

            

            //imagePicker.CameraOverlayView = self.overlay.view;
            //imagePicker.Delegate = self.overlay;

            //if (Control == null)
            //{
            //    imagePicker = new UIImagePickerController();

            //    uiCameraPreview = new UICameraPreview(e.NewElement.Camera);
            //    SetNativeControl(imagePicker);
            //}
            //if (e.OldElement != null)
            //{
            //    // Unsubscribe
            //    uiCameraPreview.Tapped -= OnCameraPreviewTapped;
            //}
            //if (e.NewElement != null)
            //{
            //    // Subscribe
            //    uiCameraPreview.Tapped += OnCameraPreviewTapped;
            //}

            //imagePicker.CameraOverlayView =;


        }
        */
    //void OnCameraPreviewTapped(object sender, EventArgs e)
    //{
    //    if (uiCameraPreview.IsPreviewing)
    //    {
    //        uiCameraPreview.CaptureSession.StopRunning();
    //        uiCameraPreview.IsPreviewing = false;
    //    }
    //    else
    //    {
    //        uiCameraPreview.CaptureSession.StartRunning();
    //        uiCameraPreview.IsPreviewing = true;
    //    }
    //}

    //protected override void Dispose(bool disposing)
    //{
    //    if (disposing)
    //    {
    //        Control.CaptureSession.Dispose();
    //        Control.Dispose();
    //    }
    //    base.Dispose(disposing);
    //}

    //}

    //public class CameraOverlayView : UIView
    //{

    //    // Transform values for full screen support
    //    public static nfloat CAMERA_ASPECT_RATIO = 1.333333f;   // 4:3 is the aspect ratio for taking photos

    //    public UIImagePickerController imagePickerController;
    //    public CameraOptions overlaySettings = new CameraOptions(); // default values

    //    public CameraOverlayView(UIImagePickerController controller, CameraOptions _settings, CGRect frame) : base(frame)
    //    {
    //        log("Intializing Camera Overlay from frame...");
    //        if (_settings != null)
    //            overlaySettings = _settings;

    //        // Clear the background of the overlay:
    //        this.Opaque = false;
    //        this.BackgroundColor = UIColor.Clear;  // transparent

    //        UIImage overlayGraphic;
    //        string OverlayImage = _settings.Overlay;

    //        log("Overlay Image: " + OverlayImage);
    //        if (OverlayImage != null && File.Exists("./" + OverlayImage + ".png"))
    //        {
    //            log("Overlay Image: " + OverlayImage + " found!");
    //            overlayGraphic = UIImage.FromBundle(OverlayImage + ".png");

    //        }
    //        else
    //        {
    //            log("Overlay Image not found");
    //            // Load the image to show in the overlay:
    //            overlayGraphic = UIImage.FromBundle(@"overlaygraphic.png");
    //        }
    //        overlayGraphic = overlayGraphic.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);  // convert image to template

    //        UIImageView overlayGraphicView = new UIImageView(overlayGraphic);
    //        overlayGraphicView.Frame = new CGRect(
    //            overlaySettings.GuidelinesMargins,
    //            overlaySettings.GuidelinesMargins,
    //            frame.Width - overlaySettings.GuidelinesMargins * 2,
    //            frame.Height - overlaySettings.GuidelinesMargins * 2);

    //        log("guidelines tint color: " + overlaySettings.GuidelinesColorHexadecimal);
    //        UIColor color = GetUIColorfromHex(overlaySettings.GuidelinesColorHexadecimal);
    //        if (color != null)
    //            overlayGraphicView.TintColor = color;

    //        this.AddSubview(overlayGraphicView);

    //        ScanButton scanButton = new ScanButton(
    //            new CGRect((frame.Width / 2) - (overlaySettings.ScanButtonWidth / 2), frame.Height - overlaySettings.ScanButtonHeight - overlaySettings.ScanButtonMarginBottom,
    //                overlaySettings.ScanButtonWidth, overlaySettings.ScanButtonHeight), overlaySettings);
    //        scanButton.TouchUpInside += delegate (object sender, EventArgs e) {

    //            log("Scan Button TouchUpInside... ");

    //            controller.TakePicture();
    //        };

    //        this.AddSubview(scanButton);

    //        if (overlaySettings.DescriptionLabelText != null)
    //        {
    //            UILabel label = new UILabel(new CGRect(overlaySettings.DescriptionLabelMarginLeftRight,
    //                                frame.Height - overlaySettings.DescriptionLabelHeight - overlaySettings.DescriptionLabelMarginBottom,
    //                frame.Width - overlaySettings.DescriptionLabelMarginLeftRight * 2, overlaySettings.DescriptionLabelHeight));  // applying "DescriptionLabelMarginLeftRight" margins to width and x position
    //            label.Text = overlaySettings.DescriptionLabelText;

    //            color = GetUIColorfromHex(overlaySettings.DescriptionLabelColorHexadecimal);
    //            if (color != null)
    //                label.TextColor = color;
    //            label.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
    //            label.TextAlignment = UITextAlignment.Center; // centered aligned
    //            label.LineBreakMode = UILineBreakMode.WordWrap; // wrap text by words
    //            label.Lines = 2;

    //            if (overlaySettings.DescriptionLabelFontFamilyName != null)
    //            {
    //                UIFont labelFont = UIFont.FromName(overlaySettings.DescriptionLabelFontFamilyName, overlaySettings.DescriptionLabelFontSize);
    //                if (labelFont != null)
    //                {
    //                    label.Font = labelFont;
    //                }
    //                else
    //                {
    //                    log("Font family [" + overlaySettings.DescriptionLabelFontFamilyName + "] for 'DescriptionLabelFontFamilyName' not found");
    //                }
    //            }

    //            this.AddSubview(label);
    //        }

    //        UILabel cancelLabel = new UILabel(new CGRect((frame.Width / 4) - (overlaySettings.CancelButtonWidth / 2),
    //            frame.Height - overlaySettings.CancelButtonHeight - overlaySettings.ScanButtonMarginBottom,
    //            overlaySettings.CancelButtonWidth, overlaySettings.CancelButtonHeight));
    //        cancelLabel.Text = overlaySettings.CancelButtonText;
    //        color = GetUIColorfromHex(overlaySettings.CancelButtonColorHexadecimal);
    //        if (color != null)
    //            cancelLabel.TextColor = color;
    //        cancelLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
    //        cancelLabel.TextAlignment = UITextAlignment.Center; // centered aligned

    //        // list of available ios fonts: https://developer.xamarin.com/recipes/ios/standard_controls/fonts/enumerate_fonts/
    //        if (overlaySettings.CancelButtonFontFamilyName != null)
    //        {
    //            UIFont cancelLabelFont = UIFont.FromName(overlaySettings.CancelButtonFontFamilyName, overlaySettings.CancelButtonFontSize);
    //            if (cancelLabelFont != null)
    //            {
    //                cancelLabel.Font = cancelLabelFont;
    //            }
    //            else
    //            {
    //                log("Font family [" + overlaySettings.CancelButtonFontFamilyName + "] for 'CancelButtonFontFamilyName' not found");
    //            }
    //        }

    //        UITapGestureRecognizer cancelLabelTap = new UITapGestureRecognizer(() => {
    //            log("Cancel Button TouchesEnded... ");
    //            UIApplication.SharedApplication.InvokeOnMainThread(delegate {
    //                SystemLogger.Log(SystemLogger.Module.PLATFORM, "Canceled picking image ");
    //                IPhoneUtils.GetInstance().FireUnityJavascriptEvent("Appverse.Media.onFinishedPickingImage", null);
    //                IPhoneServiceLocator.CurrentDelegate.MainUIViewController().DismissModalViewController(true);
    //            });
    //        });

    //        cancelLabel.UserInteractionEnabled = true;
    //        cancelLabel.AddGestureRecognizer(cancelLabelTap);

    //        this.AddSubview(cancelLabel);

    //    }

    //    public UIColor GetUIColorfromRGB(int red, int green, int blue)
    //    {
    //        return UIColor.FromRGBA(red, green, blue, 255);
    //    }

    //    public static UIColor GetUIColorfromHex(string hexValue)
    //    {
    //        if (hexValue == null)
    //            return null;
    //        float alpha = 1.0f; // opacity hardcoded


    //        var colorString = hexValue.Replace("#", "");

    //        if (alpha > 1.0f)
    //        {
    //            alpha = 1.0f;
    //        }
    //        else if (alpha < 0.0f)
    //        {
    //            alpha = 0.0f;
    //        }

    //        float red, green, blue;

    //        switch (colorString.Length)
    //        {
    //            case 3: // #RGB
    //                {
    //                    red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
    //                    green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
    //                    blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
    //                    return UIColor.FromRGBA(red, green, blue, alpha);
    //                }
    //            case 6: // #RRGGBB
    //                {
    //                    red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
    //                    green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
    //                    blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
    //                    return UIColor.FromRGBA(red, green, blue, alpha);
    //                }

    //            default:
    //                throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));

    //        }
    //    }

    //    protected void log(string message)
    //    {
    //        //SystemLogger.Log(SystemLogger.Module.PLATFORM, "CameraOverlayView : " + message);

    //    }
    //}

    //public class ScanButton : UIButton
    //{

    //    public ScanButton(CGRect frame, CameraOptionsNino options) : base(frame)
    //    {
    //        UIImage buttonImage = UIImage.FromBundle(@"scanbutton");
    //        buttonImage = buttonImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);  // convert image to template

    //        this.SetImage(buttonImage, UIControlState.Normal);
    //        this.SetImage(buttonImage, UIControlState.Highlighted);

    //        UIImage buttonIconImage = UIImage.FromBundle(@"scaniconbutton");
    //        buttonIconImage = buttonIconImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);  // convert image to template

    //        UIImageView buttonIconImageView = new UIImageView(buttonIconImage);

    //        if (options != null)
    //        {
    //            buttonIconImageView.Frame = new CGRect(
    //                (frame.Width / 2) - (options.ScanButtonIconWidth / 2),
    //                (frame.Height / 2) - (options.ScanButtonIconHeight / 2),
    //                options.ScanButtonIconWidth,
    //                options.ScanButtonIconHeight);  // we will place the icon in the middle of the button image

    //            SystemLogger.Log(SystemLogger.Module.PLATFORM, "ScanButton : tint color to be used " + options.ScanButtonColorHexadecimal);
    //            UIColor color = CameraOverlayView.GetUIColorfromHex(options.ScanButtonColorHexadecimal);
    //            if (color != null)
    //                this.TintColor = color;
    //            SystemLogger.Log(SystemLogger.Module.PLATFORM, "ScanButton : tint color to be used for icon " + options.ScanButtonIconColorHexadecimal);
    //            color = CameraOverlayView.GetUIColorfromHex(options.ScanButtonIconColorHexadecimal);
    //            if (color != null)
    //                buttonIconImageView.TintColor = color;
    //        }

    //        this.TouchUpInside += delegate {

    //            if (options != null)
    //            {
    //                SystemLogger.Log(SystemLogger.Module.PLATFORM, "ScanButton : TouchUpInside... changing color to:  " + options.ScanButtonPressedColorHexadecimal);
    //                UIColor color = CameraOverlayView.GetUIColorfromHex(options.ScanButtonPressedColorHexadecimal);
    //                if (color != null)
    //                    this.TintColor = color;
    //            }
    //        };

    //        this.AddSubview(buttonIconImageView);  // adding inside image as a new subview

    //    }


    //}

    //public partial class ViewController : UIViewController
    //{
    //    bool flashOn = false;

    //    AVCaptureSession captureSession;
    //    AVCaptureDeviceInput captureDeviceInput;
    //    AVCaptureStillImageOutput stillImageOutput;
    //    AVCaptureVideoPreviewLayer videoPreviewLayer;

    //    public ViewController(IntPtr handle) : base(handle)
    //    {
    //    }

    //    public override async void ViewDidLoad()
    //    {
    //        base.ViewDidLoad();

    //        await AuthorizeCameraUse();
    //        SetupLiveCameraStream();
    //    }

    //    public override void DidReceiveMemoryWarning()
    //    {
    //        base.DidReceiveMemoryWarning();
    //    }

    //    async partial void TakePhotoButtonTapped(UIButton sender)
    //    {
    //        var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
    //        var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

    //        var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
    //        var jpegAsByteArray = jpegImageAsNsData.ToArray();

    //        // TODO: Send this to local storage or cloud storage such as Azure Storage.
    //    }

    //    partial void SwitchCameraButtonTapped(UIButton sender)
    //    {

    //    }

    //    partial void FlashButtonTapped(UIButton sender)
    //    {

    //    }

    //    async Task AuthorizeCameraUse()
    //    {
    //        var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

    //        if (authorizationStatus != AVAuthorizationStatus.Authorized)
    //        {
    //            await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
    //        }
    //    }

    //    public void SetupLiveCameraStream()
    //    {
    //        captureSession = new AVCaptureSession();

    //        var viewLayer = liveCameraStream.Layer;
    //        videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
    //        {
    //            Frame = this.View.Frame
    //        };
    //        liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

    //        var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
    //        ConfigureCameraForDevice(captureDevice);
    //        captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
    //        captureSession.AddInput(captureDeviceInput);

    //        var dictionary = new NSMutableDictionary();
    //        dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
    //        stillImageOutput = new AVCaptureStillImageOutput()
    //        {
    //            OutputSettings = new NSDictionary()
    //        };

    //        captureSession.AddOutput(stillImageOutput);
    //        captureSession.StartRunning();
    //    }

    //    void ConfigureCameraForDevice(AVCaptureDevice device)
    //    {
    //        var error = new NSError();
    //        if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
    //        {
    //            device.LockForConfiguration(out error);
    //            device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
    //            device.UnlockForConfiguration();
    //        }
    //        else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
    //        {
    //            device.LockForConfiguration(out error);
    //            device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
    //            device.UnlockForConfiguration();
    //        }
    //        else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
    //        {
    //            device.LockForConfiguration(out error);
    //            device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
    //            device.UnlockForConfiguration();
    //        }
    //    }
    //}



    public class CameraOverlayView : UIView
    {
        #region -= constructors =-

        public CameraOverlayView() : base() { Initialize(); }
        public CameraOverlayView(CGRect frame) : base(frame) { Initialize(); }

        protected void Initialize()
        {
            this.BackgroundColor = UIColor.Clear;
        }

        #endregion

        // rect changes depending on if the whole view is being redrawn, or just a section
        public override void Draw(CGRect rect)
        {
            Console.WriteLine("Draw() Called");
            base.Draw(rect);

            // get a reference to the context
            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                // convert to View space
                CGAffineTransform affineTransform = CGAffineTransform.MakeIdentity();
                // invert the y axis
                affineTransform.Scale(1, -1);
                // move the y axis up
                affineTransform.Translate(0, Frame.Height);
                context.ConcatCTM(affineTransform);

                // draw some stars
                DrawStars(context);
            }
        }

        protected void DrawStars(CGContext context)
        {
            // HACK: Change SetRGBColor to SetFillColor
            context.SetFillColor(1f, 0f, 0f, 1f);

            // save state so that as we translate (move the origin around,
            // it goes back to normal when we restore)
            context.SetFillColor(0f, 0f, 0.329f, 1.0f);
            context.SaveState();
            context.TranslateCTM(30, 300);
            DrawStar(context, 30);
            context.RestoreState();

            context.SetFillColor(1f, 0f, 0f, 1f);
            context.SaveState();
            context.TranslateCTM(120, 200);
            DrawStar(context, 30);
            context.RestoreState();

        }

        /// <summary>
        /// Draws a star at the bottom left of the context of the specified diameter
        /// </summary>
        protected void DrawStar(CGContext context, float starDiameter)
        {
            // declare vars
            // 144º
            float theta = 2 * (float)Math.PI * (2f / 5f);
            float radius = starDiameter / 2;

            // move up and over
            context.TranslateCTM(starDiameter / 2, starDiameter / 2);

            context.MoveTo(0, radius);
            for (int i = 1; i < 5; i++)
            {
                context.AddLineToPoint(radius * (float)Math.Sin(i * theta), radius * (float)Math.Cos(i * theta));
            }
            //context.SetRGBFillColor (1, 1, 1, 1);
            context.ClosePath();
            context.FillPath();
        }

    }
}