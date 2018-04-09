using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AVFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls;
using UIKit;
using Xamarin.Forms;


[assembly: Dependency(typeof(GetVideoThumbnailService))]
namespace MindCorners.iOS.CustomControls
{
    public class GetVideoThumbnailService : IGetVideoThumbnail
    {
        public byte[] GetVideoThumbnail(string path)
        {

            try
            {
                CMTime actualTime;
                NSError outError;
                using (var asset = AVAsset.FromUrl(NSUrl.FromString(path)))
                using (var imageGen = new AVAssetImageGenerator(asset))
                using (var imageRef = imageGen.CopyCGImageAtTime(new CMTime(1, 1), out actualTime, out outError))
                {
                    return RotateImage(UIImage.FromImage(imageRef));
                    //return UIImage.FromImage(imageRef).AsPNG().ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        private byte[] RotateImage(UIImage image)
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

        private UIImage GetVideoThumbnailIOS(string path)
        {
            try
            {
                CMTime actualTime;
                NSError outError;
                using (var asset = AVAsset.FromUrl(NSUrl.FromFilename(path)))
                using (var imageGen = new AVAssetImageGenerator(asset))
                using (var imageRef = imageGen.CopyCGImageAtTime(new CMTime(1, 1), out actualTime, out outError))
                {
                    return UIImage.FromImage(imageRef);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
