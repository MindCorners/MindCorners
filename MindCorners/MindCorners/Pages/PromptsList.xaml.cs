using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class PromptsList : ContentPage
    {
        public PromptsList(PromptsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext != null && BindingContext is PromptsListViewModel)
            {

                var vm = ((PromptsListViewModel)(BindingContext));
                vm.SearchEntry = SearchEntry;
                if (vm.ShowSearchBar)
                {
                    SearchEntry.Focus();
                }
            }
        }
    }
}
