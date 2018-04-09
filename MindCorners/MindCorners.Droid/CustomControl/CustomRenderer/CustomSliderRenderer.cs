using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    class CustomSliderRenderer : ViewRenderer<CustomSlider, SeekBar>, SeekBar.IOnSeekBarChangeListener
    {
        double _max, _min;
        bool _progressChangedOnce;
        bool isStoped;
        public CustomSliderRenderer()
        {
            AutoPackage = false;
            isStoped = true;
        }

        double Value
        {
            get { return Control.Progress; }
            set { Control.Progress = (int)((value)); }
        }

        void SeekBar.IOnSeekBarChangeListener.OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (!_progressChangedOnce)
            {

                _progressChangedOnce = true;
                return;
            }

            ((IElementController)Element).SetValueFromRenderer(Slider.ValueProperty, progress);

        }

        void SeekBar.IOnSeekBarChangeListener.OnStartTrackingTouch(SeekBar seekBar)
        {
            CustomSlider view = Element;
            view.AudioService.Pause();
        }

        void SeekBar.IOnSeekBarChangeListener.OnStopTrackingTouch(SeekBar seekBar)
        {
            // audioService.Pause();

            CustomSlider view = Element;

            //view.AudioService.SeekTo((int)(Value*view.AudioService.FileLength/1000), isStoped);
            view.AudioService.SeekTo((int)Value, isStoped);
           // view.Value = Value;
            PlayPauseFile();
        }
        
        protected SeekBar CreateNativeControl()
        {
            return new SeekBar(Context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomSlider> e)
        {
            base.OnElementChanged(e);

            CustomSlider slider = e.NewElement;

            if (e.OldElement == null)
            {
                var seekBar = CreateNativeControl();

                SetNativeControl(seekBar);

                seekBar.Max = (int)(slider.FileDuration ?? 1);
                seekBar.SetOnSeekBarChangeListener(this);
            }

           
            _min = slider.Minimum;
            _max = slider.Maximum;
            Value = slider.Value;
            slider.ClickAction = Slider_ClickedButton;
            slider.StopPlayingAction = StopPlayingAction;
            // Control.ScaleY = 5;
            slider.CommandImageFileName = "audioPlay.png";
            slider.CommandText = "Play";
            slider.AudioService.IsStoped = true;
            slider.TimeLeftString = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));
        }

        private void StopPlayingAction()
        {
            CustomSlider view = Element;
            Console.WriteLine("Video Finished, will now restart");
            view.AudioService.Stop();
        }
        private void Slider_ClickedButton()
        {
            CustomSlider slider = Element;
            if (slider.CommandText == "Play")
            {
                if (slider.AudioService != null && slider.AudioService.FileUrl != slider.FileUrl)
                {
                    slider.AudioService.Stop();
                }

                isStoped = false;
                var position = slider.AudioService.CurrentPosition();
                if (position<= 1)
                {
                    slider.AudioService.Play(slider.FileUrl);
                    slider.AudioService.FileUrl = slider.FileUrl;
                    //var fileInfo = _audioPlayer.GetInfo();

                    //holeFile = fileInfo.TotalMilliseconds;
                    //LabelPLay.Text = string.Format("{0:hh\\:mm\\:ss}", fileInfo);
                }
                else
                {
                    slider.AudioService.Play();
                    //await audioSlider.ProgressTo(1, 250, Easing.Linear);
                }

                slider.CommandText = "Pause";
                //audioSlider.BackgroundColor = Color.GreenYellow;
                slider.CommandImageFileName = "audioPause.png";
                PlayPauseFile();
            }
            else
            {
                slider.AudioService.Pause();
                slider.CommandText = "Play";
                // audioSlider.BackgroundColor = Color.Yellow;
                slider.CommandImageFileName = "audioPlay.png";
                isStoped = true;
            }
        }

        private void PlayPauseFile()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), CheckPositionAndUpdateSlider);
        }

        private bool CheckPositionAndUpdateSlider()
        {
            CustomSlider slider = Element;
            
            if (slider.HoleFile <= 0)
            {   
                if (slider.FileDuration.HasValue)
                {
                    slider.Maximum = slider.FileDuration.Value;
                    slider.Minimum = 0;
                    slider.HoleFile = slider.FileDuration.Value;
                    slider.AudioService.FileLength = slider.FileDuration.Value;
                }
                //var fileInfo = slider.AudioService.GetInfo();
                //if (fileInfo.TotalMilliseconds > 0 && slider.FileDuration.HasValue)
                //{
                //    slider.Maximum = TimeSpan.FromMilliseconds(slider.FileDuration.Value).TotalMilliseconds;
                //    slider.Minimum = 0;
                //    slider.HoleFile = fileInfo.TotalMilliseconds> 0 ? fileInfo.TotalMilliseconds : slider.FileDuration.Value;
                //    slider.AudioService.FileLength = fileInfo.TotalMilliseconds;
                //}
                //else
                //{ 
                //    slider.HoleFile = slider.FileDuration ?? 0 ;
                //}
            }
            var position = slider.AudioService.CurrentPosition();

            var holeLength = slider.HoleFile;
            var leftTime = holeLength - position;
            slider.TimeLeftString = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(leftTime));
            // await AudioPlayImageOnProgress.TranslateTo(1, 0);
            //slider.Value = slider.HoleFile > 0 ? (1000 * position) / slider.HoleFile : 0;
            slider.Value = slider.HoleFile > 0 ? position : 0;

            if (slider.AudioService.IsStoped)
            {
                if (slider.AudioService.IsCompleted)
                {
                    slider.Value = 0;   
                    slider.CommandText = "Play";
                    slider.CommandImageFileName = "audioPlay.png";
                    slider.TimeLeftString = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));
                }
                return false;
            }
            //AudioSlider.Value = position;
            return true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            CustomSlider view = Element;
            switch (e.PropertyName)
            {
                case "Maximum":
                    _max = view.Maximum;
                    break;
                case "Minimum":
                    _min = view.Minimum;
                    break;
                case "Value":
                    if (Value != view.Value)
                        Value = view.Value;
                    break;
            }
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
            // seekbar.Background = new ColorDrawable(Color.AliceBlue);
            //  seekbar.ProgressDrawable = new ColorDrawable(Color.Violet);
            // seekbar.Background.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
            //seekbar.ProgressBackgroundTintList = new ColorStateList();
            // seekbar.IndeterminateDrawable.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
            seekbar.ProgressDrawable.SetColorFilter(Color.ParseColor("#697780"), PorterDuff.Mode.SrcIn);
            seekbar.Thumb.SetColorFilter(Color.ParseColor("#8DB0EA"), PorterDuff.Mode.SrcIn);

            //Drawable thumb = seekbar.Thumb;
            //int thumbTop = seekbar.Height / 2 - thumb.IntrinsicHeight / 2;

            //thumb.SetBounds(thumb.Bounds.Left, thumbTop, thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);


            //seekbar.ProgressDrawable.SetTint(); = new ShapeDrawable();
           // var path = new Path();
            //path.AddRect(Width / 2, Height / 2, radius, Path.Direction.Ccw);
            //seekbar.SetThumb();
          //  Drawable thumb = new Rectangle(0,0,5,10);
           //  seekbar.SetThumb(Resources.GetDrawable(Resource.Drawable.audioPause));
            //seekbar.SetMinimumHeight(20);
            //Drawable thumb = seekbar.Thumb;
            //int thumbTop = seekbar.Height / 2 - thumb.IntrinsicHeight / 2;

            //thumb.SetBounds(thumb.Bounds.Left, thumbTop, thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);
        }
    }
}