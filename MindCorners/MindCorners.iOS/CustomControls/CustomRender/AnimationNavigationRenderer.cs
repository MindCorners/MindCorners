using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MindCorners.Models.Enums;

//[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(AnimationNavigationRenderer))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(AnimationNavigationRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
	class AnimationNavigationRenderer : NavigationRenderer
	{
		private void CreateAnimation(NSString type, NSString direction)
		{
			CATransition transition = CATransition.CreateAnimation();
			transition.Duration = 0.5;
			transition.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
			transition.Type = type;
			transition.Subtype = direction;
			View.Layer.AddAnimation(transition, null);
		}

		protected override Task<bool> OnPushAsync(Page page, bool animated)
		{/*
			var element = ((CustomNavigationPage)Element);
			if (element.AnimationDirection == TransitionTypes.LeftToRight)
			{
				CreateAnimation(CAAnimation.TransitionMoveIn, CAAnimation.TransitionFromLeft);
			}
			else
			{
				if (animated)
					CreateAnimation(CAAnimation.TransitionMoveIn, CAAnimation.TransitionFromRight);
			}
*/
			return base.OnPushAsync(page, false);
		}

		//If poped from: Navigation.PopAsync()
		protected override Task<bool> OnPopViewAsync(Page page, bool animated) //Wrong page?
		{/*
			var element = ((CustomNavigationPage)Element);
			if (element.AnimationDirection == TransitionTypes.LeftToRight)
			{
				CreateAnimation(CAAnimation.TransitionMoveIn, CAAnimation.TransitionFromRight);
			}
			else
			{
				if (animated)
					CreateAnimation(CAAnimation.TransitionMoveIn, CAAnimation.TransitionFromLeft);
			}*/
			return base.OnPopViewAsync(page, false);
		}
	}
}