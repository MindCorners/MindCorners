using System.ComponentModel;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using MindCorners.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


//[assembly: ExportRenderer(typeof(AudioSlider), typeof(SliderRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class AudioSliderRenderer : ViewRenderer<AudioSlider, SeekBar>, SeekBar.IOnSeekBarChangeListener
    {
        public AudioSliderRenderer()
        {
            AutoPackage = false;
        }

        double _max, _min;
        bool _progressChangedOnce;

        double Value
        {
            get { return _min + (_max - _min) * (Control.Progress / 1000.0); }
            set { Control.Progress = (int)((value - _min) / (_max - _min) * 1000.0); }
        }

        void SeekBar.IOnSeekBarChangeListener.OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (!_progressChangedOnce)
            {
                _progressChangedOnce = true;
                return;
            }

            ((IElementController)Element).SetValueFromRenderer(Slider.ValueProperty, Value);
        }

        void SeekBar.IOnSeekBarChangeListener.OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        void SeekBar.IOnSeekBarChangeListener.OnStopTrackingTouch(SeekBar seekBar)
        {
        }

        protected SeekBar CreateNativeControl()
        {
            return new SeekBar(Context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AudioSlider> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var seekBar = CreateNativeControl();
                SetNativeControl(seekBar);

                seekBar.Max = 1000;

                seekBar.SetOnSeekBarChangeListener(this);
            }

            AudioSlider slider = e.NewElement;
            //_min = slider.Minimum;
            //_max = slider.Maximum;
            //Value = slider.Value;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //Slider view = Element;
            //switch (e.PropertyName)
            //{
            //    case "Maximum":
            //        _max = view.Maximum;
            //        break;
            //    case "Minimum":
            //        _min = view.Minimum;
            //        break;
            //    case "Value":
            //        if (Value != view.Value)
            //            Value = view.Value;
            //        break;
            //}
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            BuildVersionCodes androidVersion = Build.VERSION.SdkInt;
            if (androidVersion < BuildVersionCodes.JellyBean)
                return;

            // Thumb only supported JellyBean and higher

            if (Control == null)
                return;

            SeekBar seekbar = Control;

            Drawable thumb = seekbar.Thumb;
            int thumbTop = seekbar.Height / 2 - thumb.IntrinsicHeight / 2;

            thumb.SetBounds(thumb.Bounds.Left, thumbTop, thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);
        }

        //protected override void OnLayout(bool changed, int l, int t, int r, int b)
        //{
        //    base.OnLayout(changed, l, t, r, b);


        //    // Cast your element here
        //    var element = (AudioSlider)Element;
        //    if (Control != null)
        //    {
        //        //var seekBar = Control;
        //        //seekBar.Context;o
        //        //seekBar.StartTrackingTouch += (sender, args) =>
        //        //{
        //        //    element.TouchDownEvent(this, EventArgs.Empty);
        //        //};

        //        //seekBar.StopTrackingTouch += (sender, args) =>
        //        //{
        //        //    element.TouchUpEvent(this, EventArgs.Empty);
        //        //};
        //        //// On Android you need to check if ProgressChange by user
        //        //seekBar.ProgressChanged += delegate (object sender, SeekBar.ProgressChangedEventArgs args)
        //        //{
        //        //    if (args.FromUser)
        //        //        element.Value = (element.Minimum + ((element.Maximum - element.Minimum) * (args.Progress) / 1000.0));
        //        //};
        //    }
        //}
    }
}