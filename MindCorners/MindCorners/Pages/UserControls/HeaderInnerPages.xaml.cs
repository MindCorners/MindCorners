using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.Helpers;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.UserControls
{
	public partial class HeaderInnerPages : Grid
	{
		public static readonly BindableProperty PageTitleProperty = BindableProperty.Create("PageTitle", typeof(string), typeof(HeaderInnerPages), "", BindingMode.TwoWay);
		public string PageTitle
		{
			get { return (string)GetValue(PageTitleProperty); }
			set { SetValue(PageTitleProperty, value); }
		}

		public static readonly BindableProperty AnimationDirectionProperty = BindableProperty.Create("AnimationDirection", typeof(TransitionTypes), typeof(HeaderInnerPages), TransitionTypes.RightToLeft, BindingMode.TwoWay);
		public TransitionTypes AnimationDirection
		{

			get { return (TransitionTypes)GetValue(AnimationDirectionProperty); }

			set { SetValue(AnimationDirectionProperty, value); }

		}

		public static readonly BindableProperty ShowSearchButtonProperty = BindableProperty.Create("ShowSearchButton", typeof(bool), typeof(HeaderInnerPages), false, BindingMode.TwoWay);
		public bool ShowSearchButton
		{

			get { return (bool)GetValue(ShowSearchButtonProperty); }

			set { SetValue(ShowSearchButtonProperty, value); }

		}
		public static readonly BindableProperty CanSendProperty = BindableProperty.Create("CanSend", typeof(bool), typeof(HeaderInnerPages), false, BindingMode.TwoWay);
		public bool CanSend
		{

			get { return (bool)GetValue(CanSendProperty); }

			set { SetValue(CanSendProperty, value); }

		}


		public static readonly BindableProperty ShowSendButtonProperty = BindableProperty.Create("ShowSendButton", typeof(bool), typeof(HeaderInnerPages), false, BindingMode.TwoWay);
		public bool ShowSendButton
		{

			get { return (bool)GetValue(ShowSendButtonProperty); }

			set { SetValue(ShowSendButtonProperty, value); }

		}

		public static readonly BindableProperty SendButtonTextProperty = BindableProperty.Create("SendButtonText", typeof(string), typeof(HeaderInnerPages), null, BindingMode.TwoWay);
		public string SendButtonText
		{

			get { return (string)GetValue(SendButtonTextProperty); }

			set { SetValue(SendButtonTextProperty, value); }

		}

		public static readonly BindableProperty SendButtonCommandProperty = BindableProperty.Create("SendButtonCommand", typeof(Command), typeof(HeaderInnerPages), null, BindingMode.TwoWay);
		public Command SendButtonCommand
		{

			get { return (Command)GetValue(SendButtonCommandProperty); }

			set { SetValue(SendButtonCommandProperty, value); }

		}
		public HeaderInnerPages()
		{
			InitializeComponent();
			SearchButton.IsVisible = ShowSearchButton;
			SendButton.IsVisible = ShowSendButton && CanSend;
			SendButton.Text = "Send";
			SendButton.SetBinding(Button.CommandProperty, "SendPostCommand");
		}

		private async void OnBackButtonClick(object sender, EventArgs e)
		{
			if (AnimationDirection == TransitionTypes.LeftToRight)
			{
				await App.NavigationPage.PopCustomAsync(AnimationDirection);
			}
			else
			{
				await App.NavigationPage.PopAsync();
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == PageTitleProperty.PropertyName)
			{
				PageTitleLabel.Text = PageTitle;
			}
			if (propertyName == ShowSearchButtonProperty.PropertyName)
			{
				SearchButton.IsVisible= ShowSearchButton;
			}
			if (propertyName == ShowSendButtonProperty.PropertyName || propertyName == CanSendProperty.PropertyName)
			{   
				SendButton.IsVisible = ShowSendButton && CanSend;
			}
			if (propertyName == SendButtonTextProperty.PropertyName)
			{
				SendButton.Text = SendButtonText;
			}
			if (propertyName == SendButtonCommandProperty.PropertyName)
			{
				SendButton.Command = SendButtonCommand;
			}
		}
	}
}
