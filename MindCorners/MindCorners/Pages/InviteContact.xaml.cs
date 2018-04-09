using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class InviteContact : ContentPage
    {
        public InviteContact(InviteContactViewModel vm)
        {
            InitializeComponent();
            // ViewModel = vm;
            BindingContext = vm;
        }
    }
}
