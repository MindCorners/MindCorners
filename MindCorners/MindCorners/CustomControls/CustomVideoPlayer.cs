using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomVideoPlayer : View
    {

        public Action StopPlayingAction;

        public static readonly BindableProperty FileDurationProperty =
      BindableProperty.Create("FileDuration", typeof(double?), typeof(CustomVideoPlayer), defaultBindingMode: BindingMode.OneWay);

        public double? FileDuration
        {
            get { return (double?)GetValue(FileDurationProperty); }
            set
            {
                SetValue(FileDurationProperty, value);
            }
        }

        public static readonly BindableProperty FileUrlProperty =
          BindableProperty.Create("FileUrl", typeof(string), typeof(CustomVideoPlayer), defaultBindingMode: BindingMode.OneWay);

        public string FileUrl
        {
            get { return (string)GetValue(FileUrlProperty); }
            set { SetValue(FileUrlProperty, value); }
        }


		public static readonly BindableProperty AutoPlayProperty =
			BindableProperty.Create("AutoPlay", typeof(bool), typeof(CustomVideoPlayer), true, defaultBindingMode: BindingMode.OneWay);

		public bool AutoPlay
		{
			get { return (bool)GetValue(AutoPlayProperty); }
			set { SetValue(AutoPlayProperty, value); }
		}



        public static readonly BindableProperty TimeLeftStringProperty =
        BindableProperty.Create("TimeLeftString", typeof(string), typeof(CustomVideoPlayer), defaultBindingMode: BindingMode.OneWay);

        public string TimeLeftString
        {
            get { return (string)GetValue(TimeLeftStringProperty); }
            set { SetValue(TimeLeftStringProperty, value); }
        }

        public static readonly BindableProperty TimePassedStringProperty =
       BindableProperty.Create("TimePassedString", typeof(string), typeof(CustomVideoPlayer), defaultBindingMode: BindingMode.OneWay);

        public string TimePassedString
        {
            get { return (string)GetValue(TimePassedStringProperty); }
            set { SetValue(TimePassedStringProperty, value); }
        }

        public static readonly BindableProperty CommandImageFileNameProperty =
        BindableProperty.Create("CommandImageFileName", typeof(string), typeof(CustomVideoPlayer));

        public string CommandImageFileName
        {
            get { return (string)GetValue(CommandImageFileNameProperty); }
            set { SetValue(CommandImageFileNameProperty, value); }
        }

        private string commandText;
        public string CommandText
        {
            get { return commandText; }
            set
            {
                commandText = value;
                OnPropertyChanged();
            }
        }

    }
}
