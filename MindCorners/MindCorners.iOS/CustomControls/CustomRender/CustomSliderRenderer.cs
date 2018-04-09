
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{
    public class CustomSliderRenderer : ViewRenderer<CustomSlider, UISlider>
    {
        public CustomSliderRenderer()
        {
            AutoPackage = false;
            isPlaying = false;
        }

        AVPlayer player;
        AVAsset asset;
        AVPlayerItem playerItem;
        bool isPlaying;
        double? valueToSeekIfNotPlaing;
        UISlider seekBar;
        CustomSlider slider;
        CGSize _fitSize;

        public override CGSize SizeThatFits(CGSize size)
        {
            return _fitSize;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomSlider> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                seekBar = new UISlider();
                SetNativeControl(seekBar);
            }

            if (e.OldElement != null)
            {
				if (player != null) {
					player.Dispose();
					playerItem.Dispose();
					player = null;
					playerItem = null;
				}               
                
                /*
                player.RemoveObserver(this, "status");

                player.CurrentItem?.RemoveObserver(this, (NSString)"playbackBufferEmpty");
                player.CurrentItem?.RemoveObserver(this, (NSString)"playbackLikelyToKeepUp");
                */
            }

            if (e.NewElement != null)
            {
                slider = e.NewElement;

				seekBar.MaximumTrackTintColor = Color.FromHex(slider.MaximumTrackTintColor).ToUIColor();
                seekBar.MinimumTrackTintColor = Color.FromHex(slider.MinimumTrackTintColor).ToUIColor();
                //seekBar.ThumbTintColor = Color.FromHex("#827DCE").ToUIColor();
				seekBar.SetThumbImage(UIImage.FromFile(slider.TintImageFileName), UIControlState.Normal);
                seekBar.Continuous = false;

                seekBar.SizeToFit();

                _fitSize = Control.Bounds.Size;
                // except if your not running iOS 7... then it fails...
                if (_fitSize.Width <= 0 || _fitSize.Height <= 0)
                    _fitSize = new SizeF(22, 22); // Per the glorious documentation known as the SDK docs

                //if(e.OldElement != null)
                //	slider.ClickedButton -= Slider_ClickedButton;
                slider.ClickAction = UpdatePlaying;
				slider.StopPlayingAction = StopPlayingAction;
                // Control.ScaleY = 5;
				slider.CommandImageFileName = slider.PlayImageFileName;
                slider.CommandText = "Play";
                //slider.AudioService.IsStoped = true;
                slider.TimeLeftString = string.Format("{0:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));
            }

            seekBar.TouchDown += Control_TouchDown;
            seekBar.TouchUpInside += Control_TouchUpInside;
        }

		private void StopPlayingAction()
		{
			Console.WriteLine("Video Finished, will now restart");
			if (player != null) {
				player.Pause();
				player.Seek(new CMTime(0, 1));
			}
			if(slider != null){
				slider.CommandImageFileName = slider.PlayImageFileName;
				slider.CommandText = "Play";
			}


			isPlaying = false;
		}
        private void VideoDidFinishPlaying(NSNotification obj)
        {
			StopPlayingAction ();
        }

        private void Seek(CMTime seekTime, double totalSeconds, double value, bool canPlay = true)
        {
            player.Seek(seekTime, finished =>
            {
                if (finished)
                {
                    if (isPlaying && canPlay)
                        player.Play();
                    UpdateLabels(totalSeconds, value);
                }
                else
                {
                    playerItem.CancelPendingSeeks();
                    slider.TimeLeftString = "00:00";
                }
            });
        }

        private void Control_TouchUpInside(object sender, EventArgs e)
        {
            var duration = slider.FileDuration;
            if (duration.HasValue)
            {
                var totalSeconds = TimeSpan.FromMilliseconds(duration.Value).TotalSeconds;
                var value = ((UISlider)sender).Value * totalSeconds;
                var seekTime = new CMTime((long)value, 1);

                if (player != null)
                {
                    Seek(seekTime, totalSeconds, value);
                    //var obj = playerItem.SeekableTimeRanges;


                    // player.Play();
                }
                else
                {
                    //set valueToSeek
                    valueToSeekIfNotPlaing = value;
                    UpdateLabels(totalSeconds, value);
                }

            }
        }

        private void UpdateLabels(double totalSeconds, double value)
        {
            var leftTime = totalSeconds - value;
            slider.TimeLeftString = TimeSpan.FromSeconds(leftTime).ToString(@"mm\:ss");
        }

        private void Control_TouchDown(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Pause();
            }
        }
        private void UpdatePlaying()
        {
            if (!isPlaying)
            {
                if (player == null)
                {
                    playerItem = AVPlayerItem.FromUrl(NSUrl.FromString(slider.FileUrl));// (asset);

                    player = new AVPlayer(playerItem);

					slider.TimeLeftString = "loading...";
					AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                    var interval = new CMTime(1, 1);
                    player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
                    var videoEndNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, VideoDidFinishPlaying, playerItem);
                    if (valueToSeekIfNotPlaing.HasValue)
                    {
                        var duration = slider.FileDuration;
                        if (duration.HasValue)
                        {
                            var totalSeconds = TimeSpan.FromMilliseconds(duration.Value).TotalSeconds;
                            var value = valueToSeekIfNotPlaing.Value * totalSeconds;
                            var seekTime = new CMTime((long)value, 1);

                            Seek(seekTime, totalSeconds, value, false);
                        }
                    }
                   /* do
					{
						await Task.Delay(5);
					} while (player.Status != AVPlayerStatus.ReadyToPlay);
					*/
                    player.AddPeriodicTimeObserver(interval, DispatchQueue.MainQueue, delegate (CMTime time)
                    {
                                                var duration = slider.FileDuration;
                        if (duration.HasValue)
                        {
                            var totalSeconds = TimeSpan.FromMilliseconds(duration.Value).TotalSeconds;
                            var seconds = time.Seconds;
                            var leftTime = totalSeconds - seconds;
                            slider.Value = (float)(seconds / totalSeconds);
                            slider.TimeLeftString = TimeSpan.FromSeconds(leftTime).ToString(@"mm\:ss");
								if(Control != null){
									Control.Value = (float)(seconds / totalSeconds);
								}
                        }
                    });
                }

                slider.CommandText = "Pause";
                //audioSlider.BackgroundColor = Color.GreenYellow;
                slider.CommandImageFileName = slider.PauseImageFileName;
                isPlaying = true;
                player.Play();
            }
            else
            {
				slider.CommandImageFileName = slider.PlayImageFileName;
                slider.CommandText = "Play";

                //button.SetTitle("paused", UIControlState.Normal);
                player.Pause();
                isPlaying = false;
            }
        }
    }
}