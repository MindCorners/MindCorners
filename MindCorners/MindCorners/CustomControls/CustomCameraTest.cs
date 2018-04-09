using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomCameraTest : ContentView
    {

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(propertyName: "Camera",returnType: typeof(CameraOptionsNino),declaringType: typeof(CustomCamera),defaultValue: CameraOptionsNino.Rear);
        public CameraOptionsNino Camera
        {
            get { return (CameraOptionsNino)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public CustomCameraTest()
        {
            BackgroundColor = Color.Black;
            var mainGrid = new Grid();
            mainGrid.RowDefinitions = new RowDefinitionCollection() {new RowDefinition(), new RowDefinition()};


            var headerGrid = new Grid() {BackgroundColor = Color.Red};
            Grid.SetRow(headerGrid, 0);

            var footerGrid = new Grid() { BackgroundColor = Color.Green };
            Grid.SetRow(footerGrid, 1);

            mainGrid.Children.Add(headerGrid);
            mainGrid.Children.Add(footerGrid);
            Content = mainGrid;

        }
        
    }
}
