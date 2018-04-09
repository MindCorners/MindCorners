using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages.UserControls
{
    public partial class PromptsTab : Grid
    {
        public static readonly BindableProperty IsLatestTabActiveProperty = BindableProperty.Create("IsLatestTabActive", typeof(bool), typeof(PromptsTab), true, BindingMode.OneWay );
        
        public bool IsLatestTabActive
        {

            get { return (bool)GetValue(IsLatestTabActiveProperty); }

            set
            {
                SetValue(IsLatestTabActiveProperty, value);
               
            }

        }

        public PromptsTab()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsLatestTabActiveProperty.PropertyName)
            {
                Grid.SetColumn(ActiveTabBoxView, 2);
            }
        }
    }
}