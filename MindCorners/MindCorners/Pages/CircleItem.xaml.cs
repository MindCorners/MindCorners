using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class CircleItem : ContentPage
    {
        public CircleItem(CircleViewModel vm)
        {
            InitializeComponent();
            // ViewModel = vm;
            BindingContext = vm;
        }
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (BindingContext != null && BindingContext is CircleViewModel)
			{

				var vm = ((CircleViewModel)(BindingContext));
				//vm.TitleEntry = TitleEntry;
			}
		}
    }
}
