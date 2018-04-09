using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages.UserControls
{
    public partial class CustomAlertDialog : PopupPage
    {
		private bool showCancel = false;
		private Action OkAction;
		private Action CancelAction;
		public CustomAlertDialog(string result, string messageText, string buttonText)
		{
			InitializeComponent();
			MessageText.Text = messageText;
			OkButton.Text = buttonText;
			CancelButton.IsVisible = false;
		}

		public CustomAlertDialog(string result, string messageText, string buttonOkText, string buttonCancelText, Action okAction, Action cancelAction)
		{
			InitializeComponent();
			MessageText.Text = messageText;
			OkButton.Text = buttonOkText;
			CancelButton.IsVisible = true;
			CancelButton.Text = buttonCancelText;
			showCancel = true;
			OkAction = okAction;
			CancelAction = cancelAction;
		}


		private void Button_OnClicked(object sender, EventArgs e)
		{
			if (OkAction != null)
			{
				OkAction.Invoke();
			}
			Navigation.PopPopupAsync();
		}

		protected override bool OnBackButtonPressed()
		{
			return false;
		}

		private void ButtonCancle_OnClicked(object sender, EventArgs e)
		{
			if (showCancel && CancelAction != null)
			{
				CancelAction.Invoke();
			}
			Navigation.PopPopupAsync();
		}
    }
}