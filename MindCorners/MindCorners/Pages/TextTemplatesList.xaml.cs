using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class TextTemplatesList : ContentPage
    {
        public TextTemplatesList(TextTemplateItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
