using System;
using Xamarin.Forms;
using MindCorners.iOS;
using MindCorners.CustomControls;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using UIKit;
using System.Reflection;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]

namespace MindCorners.iOS
{
	public class CustomViewCellRenderer: ViewCellRenderer
	{
		/*public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			CGRect rect = new CGRect(0, 0, 1, 1);
			CGSize size = rect.Size;
			UIGraphics.BeginImageContext(size);
			CGContext currentContext = UIGraphics.GetCurrentContext();

			currentContext.SetFillColor(Color.GreenYellow.ToCGColor());
			currentContext.FillRect(rect);
			var backgroundImage = UIGraphics.GetImageFromCurrentImageContext();
			currentContext.Dispose();


			// This is the assembly full name which may vary by the Xamarin.Forms version installed.
			// NullReferenceException is raised if the full name is not correct.
			var t = Type.GetType("Xamarin.Forms.Platform.iOS.ContextActionsCell, Xamarin.Forms.Platform.iOS, Version=1.3.1.0, Culture=neutral, PublicKeyToken=null");

			// Now change the static field value! "normalBackground" OR "destructiveBackground"
			var field = t.GetField("normalBackground", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			field.SetValue(null, backgroundImage);
			return base.GetCell (item, reusableCell, tv);
		}*/
		/*NativeiOSCell cell;
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var nativeCell = (CustomViewCell)item;
			//Console.WriteLine("\t\t" + nativeCell.Name);

			cell = reusableCell as NativeiOSCell;
			if (cell == null)
			{
				cell = new NativeiOSCell(item.GetType().FullName, nativeCell);
			}
			else
			{
				cell.NativeCell.PropertyChanged -= OnNativeCellPropertyChanged;
			}

			nativeCell.PropertyChanged += OnNativeCellPropertyChanged;

			cell.UpdateCell(nativeCell);
			return cell;
		}
*/
		/*public override UIKit.UITableViewCell GetCell (Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
		{
			CGRect rect = new CGRect(0, 0, 1, 1);
			CGSize size = rect.Size;
			UIGraphics.BeginImageContext(size);
			CGContext currentContext = UIGraphics.GetCurrentContext();

			currentContext.SetFillColor(Color.FromHex(((CustomViewCell)Element).DestructiveColor).ToCGColor());
			currentContext.FillRect(rect);
			var backgroundImage = UIGraphics.GetImageFromCurrentImageContext();
			currentContext.Dispose();


			// This is the assembly full name which may vary by the Xamarin.Forms version installed.
			// NullReferenceException is raised if the full name is not correct.
			var t = Type.GetType("Xamarin.Forms.Platform.iOS.ContextActionsCell, Xamarin.Forms.Platform.iOS, Version=1.3.1.0, Culture=neutral, PublicKeyToken=null");

			// Now change the static field value! "normalBackground" OR "destructiveBackground"
			var field = t.GetField("normalBackground", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			field.SetValue(null, backgroundImage);
			return 
		}
		*/
	}
}

