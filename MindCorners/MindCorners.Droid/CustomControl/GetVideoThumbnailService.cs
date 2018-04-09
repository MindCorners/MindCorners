using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Xamarin.Forms;
using Console = System.Console;

[assembly: Dependency(typeof(GetVideoThumbnailService))]

namespace MindCorners.Droid.CustomControl
{
    public class GetVideoThumbnailService : IGetVideoThumbnail  
    {
        public byte[] GetVideoThumbnail(string path)
        {
            try
            {
                Bitmap bmThumbnail = ThumbnailUtils.CreateVideoThumbnail(path,ThumbnailKind.FullScreenKind);

                byte[] bitmapData;
                using (var stream = new MemoryStream())
                {
                    bmThumbnail.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }

                //ByteArrayOutputStream stream = new ByteArrayOutputStream();
                //bmThumbnail.Compress(Bitmap.CompressFormat.Png, 100, stream.);
                //byte[] byteArray = stream.ToByteArray();
                return bitmapData;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
           // // MINI_KIND: 512 x 384 thumbnail 
           // bmThumbnail = ThumbnailUtils.createVideoThumbnail(filePath,
           //MediaStore.Video.Thumbnails.MINI_KIND);
           // thumbnail_mini.setImageBitmap(bmThumbnail);


            return null;
        }


    }
}