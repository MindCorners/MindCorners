using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellItemSelectedCustomRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class ViewCellItemSelectedCustomRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);

            //cell.SetBackgroundResource(Resource.Drawable.ViewCellBackground);

            return cell;
        }
    }
}