using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages
{
   public partial class NotificationsList : ContentPage
    {
        public NotificationsList(NotificationsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}