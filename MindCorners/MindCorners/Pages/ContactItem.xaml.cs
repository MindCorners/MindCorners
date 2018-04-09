using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class ContactItem : ContentPage
    {
        //ContactViewModel viewModel;

        //public ContactItem(ContactViewModel viewModel)
        //{
        //    InitializeComponent();

        //    BindingContext = this.viewModel = viewModel;
        //}

        //public ContactViewModel ViewModel { get; private set; }
        public ContactItem(ContactViewModel vm)
        {
            InitializeComponent();
           // ViewModel = vm;
            BindingContext = vm;
        }
    }
}
