using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class ReplyItem : ContentPage
    {
        public ReplyItem(ReplyItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
