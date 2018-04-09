using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void BtnHistory_OnClicked(object sender, EventArgs e)
        {
            
        }
    }
}
