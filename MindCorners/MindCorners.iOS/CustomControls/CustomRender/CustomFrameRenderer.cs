using System.IO;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl.CustomRenderer;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{
   public  class CustomFrameRenderer : Xamarin.Forms.Platform.iOS.FrameRenderer
    {   
       
    }
}