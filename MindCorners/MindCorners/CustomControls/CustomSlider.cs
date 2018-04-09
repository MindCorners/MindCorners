using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomSlider : Slider
    {   
        public IAudioPlayerService AudioService { get; set; }
        public Action OnProgressChanged { get; set; }
        public Action OnStartTrackingTouch { get; set; }
        public Action OnStopTrackingTouch { get; set; }

        // public EventTrigger OnButtonClick { get; set; }
        public Action StopPlayingAction;
        public Action ClickAction;
        public static readonly BindableProperty FileDurationProperty =
   BindableProperty.Create("FileDuration", typeof(double?), typeof(CustomSlider), defaultBindingMode: BindingMode.OneWay);

        public double? FileDuration
        {
            get { return (double?)GetValue(FileDurationProperty); }
            set
            {
                SetValue(FileDurationProperty, value);
                TimeLeftString = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(value ?? 0));
            }
        }

        //public event EventHandler ClickedButton;
        //public virtual void OnClickedButton()
        //{
        //    ClickAction();
        //    if (ClickedButton != null)
        //    {
        //        this.ClickedButton(this, EventArgs.Empty);
        //    }
        //}

        private double holeFile;
        public double HoleFile
        {
            get { return holeFile; }
            set
            {
                holeFile = value;
                OnPropertyChanged();
            }
        }
        private double currentPosition;
        public double CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged();
            }
        }
        //private string fileUrl;
        //public string FileUrl
        //{
        //    get { return fileUrl; }
        //    set
        //    {
        //        fileUrl = value;
        //        OnPropertyChanged();
        //    }
        //}

       public static readonly BindableProperty FileUrlProperty =
        BindableProperty.Create("FileUrl", typeof(string), typeof(CustomSlider), defaultBindingMode: BindingMode.OneWay);

        public string FileUrl
        {
            get { return (string)GetValue(FileUrlProperty); }
            set { SetValue(FileUrlProperty, value); }
        }


       public static readonly BindableProperty TimeLeftStringProperty =
       BindableProperty.Create("TimeLeftString", typeof(string), typeof(CustomSlider), defaultBindingMode: BindingMode.OneWay);

        public string TimeLeftString
        {
            get { return (string)GetValue(TimeLeftStringProperty); }
            set { SetValue(TimeLeftStringProperty, value); }
        }

       public static readonly BindableProperty CommandImageFileNameProperty =
       BindableProperty.Create("CommandImageFileName", typeof(string), typeof(CustomSlider));

        public string CommandImageFileName
        {
            get { return (string)GetValue(CommandImageFileNameProperty); }
            set { SetValue(CommandImageFileNameProperty, value); }
        }


		public static readonly BindableProperty PlayImageFileNameProperty =
		BindableProperty.Create("PlayImageFileName", typeof(string), typeof(CustomSlider), "audioPlay.png");

		public string PlayImageFileName
		{
			get { return (string)GetValue(PlayImageFileNameProperty); }
			set { SetValue(PlayImageFileNameProperty, value); } 
		}


		public static readonly BindableProperty PauseImageFileNameProperty =
		BindableProperty.Create("PauseImageFileName", typeof(string), typeof(CustomSlider), "audioPause.png");

		public string PauseImageFileName
		{
			get { return (string)GetValue(PauseImageFileNameProperty); }
			set { SetValue(PauseImageFileNameProperty, value); }
		}




		public static readonly BindableProperty TintImageFileNameProperty =
		BindableProperty.Create("TintImageFileName", typeof(string), typeof(CustomSlider), "audioTint.png");

		public string TintImageFileName
		{
			get { return (string)GetValue(TintImageFileNameProperty); }
			set { SetValue(TintImageFileNameProperty, value); }
		}



		public static readonly BindableProperty MaximumTrackTintColorProperty =
		BindableProperty.Create("MaximumTrackTintColor", typeof(string), typeof(CustomSlider), "#D3D1E5");

		public string MaximumTrackTintColor
		{
			get { return (string)GetValue(MaximumTrackTintColorProperty); }
			set { SetValue(MaximumTrackTintColorProperty, value); }
		}



		public static readonly BindableProperty MinimumTrackTintColorProperty =
		BindableProperty.Create("MinimumTrackTintColor", typeof(string), typeof(CustomSlider), "#D3D1E5");

		public string MinimumTrackTintColor
		{
			get { return (string)GetValue(MinimumTrackTintColorProperty); }
			set { SetValue(MinimumTrackTintColorProperty, value); }
		}



        //private string timeLeftString;
        //public string TimeLeftString
        //{
        //    get { return timeLeftString; }
        //    set
        //    {
        //        timeLeftString = value;
        //        OnPropertyChanged();
        //    }
        //}
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
