using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellItemSelectedCustomRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class ViewCellItemSelectedCustomRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }
    }
}