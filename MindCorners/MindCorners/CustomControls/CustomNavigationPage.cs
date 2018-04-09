using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models.Enums;
using MindCorners.UserControls;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
	public class CustomNavigationPage : NavigationPage
	{
		public static readonly BindableProperty AnimationDirectionProperty = BindableProperty.Create("AnimationDirection", typeof(TransitionTypes), typeof(HeaderInnerPages), TransitionTypes.RightToLeft, BindingMode.TwoWay);
		public MindCorners.Models.Enums.TransitionTypes AnimationDirection
		{

			get { return (TransitionTypes)GetValue(AnimationDirectionProperty); }

			set { SetValue(AnimationDirectionProperty, value); }

		}
		public CustomNavigationPage(Page root) : base(root)
		{
			SetHasNavigationBar(this, false);
		}
		// Analysis disable once MethodOverloadWithOptionalParameter
		public async Task PushCustomAsync(Page page, TransitionTypes transitionType = TransitionTypes.RightToLeft)
		{
			AnimationDirection = transitionType;
			await base.PushAsync(page);
		}

		// Analysis disable once MethodOverloadWithOptionalParameter
		public async Task<Page> PopCustomAsync(TransitionTypes transitionType = TransitionTypes.RightToLeft)
		{
			AnimationDirection = transitionType;
			var task = await base.PopAsync();
			return task;
		}

		public async Task PushAsync(Page page, bool animated = true)
		{
			AnimationDirection = TransitionTypes.RightToLeft;
			await base.PushAsync(page, true);
		}

		// Analysis disable once MethodOverloadWithOptionalParameter
		public async Task<Page> PopAsync(bool animated = true)
		{
			AnimationDirection = TransitionTypes.RightToLeft;
			var task = await base.PopAsync(animated);
			return task;
		}
	}
}
