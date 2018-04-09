using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomSearchBar : SearchBar
    {
        public static readonly BindableProperty CancelButtonCommandProperty = BindableProperty.Create("CancelButtonCommand", typeof(Command), typeof(CustomSearchBar));
        public Command CancelButtonCommand
        {
            get { return (Command)GetValue(CancelButtonCommandProperty); }
            set { SetValue(CancelButtonCommandProperty, value); }
        }

    }
}
