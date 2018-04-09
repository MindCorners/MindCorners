using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class PromptItem : ContentPage
    {
        public PromptItem(PromptItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (BindingContext != null && BindingContext is PromptItemViewModel)
			{

				var vm = ((PromptItemViewModel)(BindingContext));
				vm.TitleEntry = TitleEntry;
			}
		}
    }
}
