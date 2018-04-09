using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using AVKit;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MediaPlayer;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(CustomVideoPlayer), typeof(CustomVideoPlayerRenderer))]

namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class CustomVideoPlayerRenderer : ViewRenderer<CustomVideoPlayer, UIView>
    {
        public CustomVideoPlayerRenderer()
        {
            AutoPackage = false;
            isPlaying = false;
        }

        UIView mainView;
        UIView bootomToolbarView;
        UIView videoView;
        AVPlayerViewController videoPlayerAV;
        AVPlayer player;
        AVPlayerLayer playerLayer;
        AVAsset asset;
        AVPlayerItem playerItem;
        UILabel labelTimePlayed;
        UILabel labelTimeLeft;
        UIButton button;
        CAGradientLayer gradientLayer;
        bool isPlaying;
        bool isVisiblePlayPauseButton;

        UISlider seekBar;
        CustomVideoPlayer slider;
        CGSize _fitSize;

        public override CGSize SizeThatFits(CGSize size)
        {
            return _fitSize;
        }
        /*
                MPMoviePlayerController _player;
                private List<NSObject> _observers = new List<NSObject>();
                protected override void OnElementChanged(ElementChangedEventArgs<CustomVideoSlider> e)
                {
                    base.OnElementChanged(e);
                    if (e.NewElement != null)
                    {
                        if (base.Control == null)
                        {
                            _player = new MPMoviePlayerController();
                            _player.ScalingMode = MPMovieScalingMode.AspectFit;

                            _player.ShouldAutoplay = true;
                            _player.ControlStyle = MPMovieControlStyle.None;
                            _player.MovieControlMode=MPMovieControlMode.Hidden;
                            _player.View.Frame = this.Bounds;
                            _player.BackgroundColor = UIColor.Clear;
                            base.SetNativeControl(_player.View);
                            var center = NSNotificationCenter.DefaultCenter;
                           // _observers.Add(center.AddObserver((NSString)"UIDeviceOrientationDidChangeNotification", DeviceRotated));
                            _observers.Add(center.AddObserver(MPMoviePlayerController.PlaybackStateDidChangeNotification, OnLoadStateChanged));
                            _observers.Add(center.AddObserver(MPMoviePlayerController.PlaybackDidFinishNotification, OnPlaybackComplete));
                            _observers.Add(center.AddObserver(MPMoviePlayerController.WillExitFullscreenNotification, OnExitFullscreen));
                            _observers.Add(center.AddObserver(MPMoviePlayerController.WillEnterFullscreenNotification, OnEnterFullscreen));



                            bootomToolbarView = new UIView()
                            {
                                TranslatesAutoresizingMaskIntoConstraints = false,
                            };

                            bootomToolbarView.AddObserver(this, (NSString)"bounds", NSKeyValueObservingOptions.New, IntPtr.Zero);

                            gradientLayer = new CAGradientLayer();
                            //gradientLayer.Frame = Bounds;
                            gradientLayer.Colors = new CGColor[] { UIColor.Clear.CGColor, UIColor.Orange.CGColor };
                            gradientLayer.Locations= new NSNumber[2] {new NSNumber(0.7),new NSNumber(1.2)};
                            bootomToolbarView.Layer.AddSublayer(gradientLayer);
                            this.InsertSubviewAbove(bootomToolbarView, _player.View);

                            labelTimeLeft = new UILabel()
                            {
                                TranslatesAutoresizingMaskIntoConstraints = false,
                                Text = string.Format("{0:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0)),
                                TextColor = UIColor.White
                            };
                            bootomToolbarView.AddSubview(labelTimeLeft);

                            labelTimePlayed = new UILabel()
                            {
                                TranslatesAutoresizingMaskIntoConstraints = false,
                                Text = "00:00",
                                TextColor = UIColor.White
                            };

                            bootomToolbarView.AddSubview(labelTimePlayed);

                            seekBar = new UISlider()
                            {
                                TranslatesAutoresizingMaskIntoConstraints = false,
                            MaximumTrackTintColor = Color.LightGray.ToUIColor(),
                            MinimumTrackTintColor = Color.White.ToUIColor(),
                            ThumbTintColor = Color.FromHex("#827DCE").ToUIColor()

                        };

                            seekBar.TouchDown += Control_TouchDown;
                            seekBar.TouchUpInside += Control_TouchUpInside;

                            bootomToolbarView.AddSubview(seekBar);

                            bootomToolbarView.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
                            bootomToolbarView.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
                            bootomToolbarView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
                            bootomToolbarView.HeightAnchor.ConstraintEqualTo(40).Active = true;


                            labelTimeLeft.RightAnchor.ConstraintEqualTo(bootomToolbarView.RightAnchor, -10).Active = true;
                            labelTimeLeft.BottomAnchor.ConstraintEqualTo(bootomToolbarView.BottomAnchor, -10).Active = true;
                            labelTimeLeft.WidthAnchor.ConstraintEqualTo(50).Active = true;
                            labelTimeLeft.HeightAnchor.ConstraintEqualTo(20).Active = true;

                            labelTimePlayed.LeftAnchor.ConstraintEqualTo(bootomToolbarView.LeftAnchor, 10).Active = true;
                            labelTimePlayed.BottomAnchor.ConstraintEqualTo(bootomToolbarView.BottomAnchor, -10).Active = true;
                            labelTimePlayed.WidthAnchor.ConstraintEqualTo(50).Active = true;
                            labelTimePlayed.HeightAnchor.ConstraintEqualTo(20).Active = true;

                            seekBar.LeftAnchor.ConstraintEqualTo(labelTimePlayed.RightAnchor, 10).Active = true;
                            seekBar.RightAnchor.ConstraintEqualTo(labelTimeLeft.LeftAnchor, -10).Active = true;
                            seekBar.BottomAnchor.ConstraintEqualTo(bootomToolbarView.BottomAnchor, -10).Active = true;
                            seekBar.HeightAnchor.ConstraintEqualTo(20).Active = true;

                            ToggleFullscreen();
                        }
                    }
                    updateVideoPath();
                    updateFullscreen();
                }
                private void DeviceRotated(NSNotification notification)
                {
                    ToggleFullscreen();
                }

                private void OnLoadStateChanged(NSNotification notification)
                {

                }

                private void OnPlaybackComplete(NSNotification notification) { }

                private void OnExitFullscreen(NSNotification notification) { }

                private void OnEnterFullscreen(NSNotification notification) { }

                private void ToggleFullscreen()
                {
                    _player.ScalingMode = MPMovieScalingMode.None;
                    switch (UIDevice.CurrentDevice.Orientation)
                    {
                        case UIDeviceOrientation.Portrait:
                            _player?.SetFullscreen(false, true);
                            break;
                        case UIDeviceOrientation.PortraitUpsideDown:
                            _player?.SetFullscreen(false, true);
                            break;
                        case UIDeviceOrientation.LandscapeLeft:
                            _player?.SetFullscreen(true, true);
                            break;
                        case UIDeviceOrientation.LandscapeRight:
                            _player?.SetFullscreen(true, true);
                            break;
                    }
                    _player.View.SetNeedsLayout();
                    _player.View.SetNeedsDisplay();
                }

                protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    base.OnElementPropertyChanged(sender, e);
                    if (e.PropertyName == CustomVideoSlider.FileUrlProperty.PropertyName) updateVideoPath();
                   // if (e.PropertyName == CustomVideoSlider.FullscreenProperty.PropertyName) updateFullscreen();
                }

                public override void MovedToSuperview()
                {
                    base.MovedToSuperview();
                }

                private void updateVideoPath()
                {
                    if (_player == null) return;
                   // _player.ControlStyle = MPMovieControlStyle.Embedded;
                    _player.ShouldAutoplay = true;
                    _player.ContentUrl = !string.IsNullOrWhiteSpace(this.Element.FileUrl) ? NSUrl.FromString(this.Element.FileUrl) : null;
                    _player.PrepareToPlay();
                }

                private void updateFullscreen()
                {
                    if (_player == null) return;
                    _player.SetFullscreen(true, true);
                    _player.View.SetNeedsLayout();
                    _player.View.SetNeedsDisplay();
                }

                protected override void Dispose(bool disposing)
                {
                    if (this._player != null)
                    {
                        this._player.Dispose();
                        this._player = null;
                    }
                    base.Dispose(disposing);
                }

               */
        protected override void OnElementChanged(ElementChangedEventArgs<CustomVideoPlayer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                //seekBar = new UISlider();
                //SetNativeControl(seekBar);
                mainView = new UIView() { TranslatesAutoresizingMaskIntoConstraints = false, };
                SetNativeControl(mainView);
            }

            if (e.OldElement != null)
            {

                videoPlayerAV.RemoveObserver(this, (NSString)"videoBounds");
				bootomToolbarView.RemoveObserver(this, (NSString)"bounds");
                UIApplication.SharedApplication.SetStatusBarHidden(false, true);
				StopPlayingAction (false);
				playerItem.RemoveObserver(this, (NSString)"loadedTimeRanges");
				playerItem.Dispose ();
				player.Dispose ();

				Console.WriteLine("Video Finished 1");
				//playerItem = null;
				//player = null;
            }

            if (e.NewElement != null)
            {
                slider = e.NewElement;

                _fitSize = Control.Bounds.Size;
                // except if your not running iOS 7... then it fails...
                if (_fitSize.Width <= 0 || _fitSize.Height <= 0)
                    _fitSize = new SizeF(22, 22); // Per the glorious documentation known as the SDK docs

                slider.CommandImageFileName = "audioPlay.png";
                slider.CommandText = "Play";
				slider.StopPlayingAction = StopPlayingAction;

                //seekBar = new UISlider();
                //seekBar.MaximumTrackTintColor = Color.LightGray.ToUIColor();
                //seekBar.MinimumTrackTintColor = Color.White.ToUIColor();
                //seekBar.ThumbTintColor = Color.FromHex("#827DCE").ToUIColor();
                //seekBar.Continuous = false;




                // Control.ScaleY = 5;

                if (slider.FileUrl != null)
                {
                    // playerItem = AVPlayerItem.FromUrl(NSUrl.FromString("https://s3.amazonaws.com/kargopolov/kukushka.mp3"));// (asset);
                    playerItem = AVPlayerItem.FromUrl(NSUrl.FromString(slider.FileUrl));// (asset);
					//playerItem = AVPlayerItem.FromUrl(NSUrl.FromString("https://mindcorners.blob.core.windows.net/post-attachment-videos/6412cb27-414a-435b-a480-66af5e935cb8.mov?sv=2017-04-17&sr=b&sig=gd3mn1Gai7QZHoSpx%2Fi8x%2FHfnOutACOV7WvsVblywk0%3D&se=2017-10-11T15%3A05%3A15Z&sp=r"));
					playerItem.AddObserver(this, (NSString)"loadedTimeRanges", NSKeyValueObservingOptions.New, IntPtr.Zero);

                    player = new AVPlayer(playerItem);


                    //player.AddObserver(this, "loadedTimeRanges", NSKeyValueObservingOptions.New, null);
                    //playerLayer = AVPlayerLayer.FromPlayer(player);
                    videoPlayerAV = new AVPlayerViewController();
                    videoPlayerAV.View.BackgroundColor = UIColor.Clear;
                    videoPlayerAV.ShowsPlaybackControls = false;


                    videoPlayerAV.AddObserver(this, (NSString)"videoBounds", NSKeyValueObservingOptions.New, IntPtr.Zero);
                    videoPlayerAV.View.Frame = Control.Frame;
                    videoPlayerAV.Player = player;

                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(OnVideoTap);
					videoView = new UIView() { BackgroundColor = UIColor.Clear};
                    // videoView.Bounds = new CGRect(0,0,100,100);
                    videoView.AddGestureRecognizer(tapGesture);
                    //videoPlayerAV.View.AddSubview(videoView);

                    
                    //button.CenterYAnchor.ConstraintEqualTo(videoPlayerAV.ContentOverlayView.CenterYAnchor).Active = true;
                    //button.WidthAnchor.ConstraintEqualTo(50).Active = true;
                    //button.HeightAnchor.ConstraintEqualTo(50).Active = true;




                    //videoPlayerAV.VideoGravity = AVLayerVideoGravity.ResizeAspect;

                    mainView.AddSubview(videoPlayerAV.View);
					mainView.AddSubview(videoView);

					videoView.TopAnchor.ConstraintEqualTo(mainView.TopAnchor).Active = true;
					videoView.BottomAnchor.ConstraintEqualTo(mainView.BottomAnchor).Active = true;
					videoView.LeftAnchor.ConstraintEqualTo(mainView.LeftAnchor).Active = true;
					videoView.RightAnchor.ConstraintEqualTo(mainView.RightAnchor).Active = true;
					videoView.WidthAnchor.ConstraintEqualTo(mainView.WidthAnchor).Active = true;
					videoView.HeightAnchor.ConstraintEqualTo(mainView.HeightAnchor).Active = true;


                    button = new UIButton() { BackgroundColor = UIColor.Clear, TitleLabel = { Text = slider.CommandText }, TranslatesAutoresizingMaskIntoConstraints = false, };
                    button.TouchUpInside += Button_TouchUpInside;

					videoView.AddSubview(button);
					button.CenterXAnchor.ConstraintEqualTo(videoView.CenterXAnchor).Active = true;
					button.CenterYAnchor.ConstraintEqualTo(videoView.CenterYAnchor).Active = true;
					//button.CenterXAnchor.ConstraintEqualTo(videoPlayerAV.View.CenterXAnchor).Active = true;
					//button.CenterYAnchor.ConstraintEqualTo(videoPlayerAV.View.CenterYAnchor).Active = true;
					button.WidthAnchor.ConstraintEqualTo(50).Active = true;
					button.HeightAnchor.ConstraintEqualTo(50).Active = true;




                    var center = NSNotificationCenter.DefaultCenter;
                    NSNotificationCenter.DefaultCenter.AddObserver((NSString)"UIDeviceOrientationDidChangeNotification", DeviceRotated);



                    //mainView.Layer.AddSublayer(playerLayer);
					if (slider.AutoPlay) {
						UpdatePlaying ();
					} else {
						{
							slider.CommandImageFileName = "audioPlay.png";
							slider.CommandText = "Play";

							button.Hidden = false;
							//button.SetTitle("paused", UIControlState.Normal);
							player.Pause();
							isPlaying = false;
						}

						button.SetImage(UIImage.FromFile(slider.CommandImageFileName), UIControlState.Normal);
					}
                }



                bootomToolbarView = new UIView()
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                };

                bootomToolbarView.AddObserver(this, (NSString)"bounds", NSKeyValueObservingOptions.New, IntPtr.Zero);

                gradientLayer = new CAGradientLayer();
                //gradientLayer.Frame = Bounds;
                gradientLayer.Colors = new CGColor[] { UIColor.Clear.CGColor, UIColor.Black.CGColor };
                gradientLayer.Locations = new NSNumber[2] { new NSNumber(0.7), new NSNumber(1.2) };
                bootomToolbarView.Layer.AddSublayer(gradientLayer);
                mainView.InsertSubviewAbove(bootomToolbarView, videoPlayerAV.View);

                labelTimeLeft = new UILabel()
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Text = string.Format("{0:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0)),
                    TextColor = UIColor.White
                };
                bootomToolbarView.AddSubview(labelTimeLeft);

                labelTimePlayed = new UILabel()
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Text = "00:00",
                    TextColor = UIColor.White
                };

                bootomToolbarView.AddSubview(labelTimePlayed);

                seekBar = new UISlider()
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    MaximumTrackTintColor = Color.LightGray.ToUIColor(),
                    MinimumTrackTintColor = Color.White.ToUIColor(),
                    ThumbTintColor = Color.FromHex("#827DCE").ToUIColor()

                };

                seekBar.SetThumbImage(UIImage.FromFile("audioTint.png"), UIControlState.Normal);

                seekBar.TouchDown += Control_TouchDown;
                seekBar.TouchUpInside += Control_TouchUpInside;

                bootomToolbarView.AddSubview(seekBar);

                //videoPlayerAV.View.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
                //videoPlayerAV.View.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
                //videoPlayerAV.View.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
                //videoPlayerAV.View.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;


                bootomToolbarView.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
                bootomToolbarView.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
                bootomToolbarView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
                bootomToolbarView.HeightAnchor.ConstraintEqualTo(40).Active = true;


                labelTimeLeft.RightAnchor.ConstraintEqualTo(bootomToolbarView.RightAnchor, -10).Active = true;
                labelTimeLeft.BottomAnchor.ConstraintEqualTo(bootomToolbarView.BottomAnchor, -10).Active = true;
                labelTimeLeft.WidthAnchor.ConstraintEqualTo(50).Active = true;
                labelTimeLeft.HeightAnchor.ConstraintEqualTo(20).Active = true;

                labelTimePlayed.LeftAnchor.ConstraintEqualTo(bootomToolbarView.LeftAnchor, 10).Active = true;
                labelTimePlayed.BottomAnchor.ConstraintEqualTo(bootomToolbarView.BottomAnchor, -10).Active = true;
                labelTimePlayed.WidthAnchor.ConstraintEqualTo(50).Active = true;
                labelTimePlayed.HeightAnchor.ConstraintEqualTo(20).Active = true;

                seekBar.LeftAnchor.ConstraintEqualTo(labelTimePlayed.RightAnchor, 10).Active = true;
                seekBar.RightAnchor.ConstraintEqualTo(labelTimeLeft.LeftAnchor, -10).Active = true;
                seekBar.BottomAnchor.ConstraintEqualTo(bootomToolbarView.BottomAnchor, -10).Active = true;
                seekBar.HeightAnchor.ConstraintEqualTo(20).Active = true;



                mainView.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
                mainView.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
                // mainView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
                mainView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;

                // bootomToolbarView = new UIView();
                // //bootomToolbarView.BackgroundColor =  UIColor.Orange;
                // CAGradientLayer gradientLayer = new CAGradientLayer();
                // gradientLayer.Frame = bootomToolbarView.Frame;
                // gradientLayer.Colors = new CGColor[] {UIColor.Clear.CGColor, UIColor.Orange.CGColor};
                // bootomToolbarView.Layer.AddSublayer(gradientLayer);
                // //bootomToolbarView.BackgroundColor = new CGGradient(CGColorSpace.CreateAcesCGLinear(), new CGColor[] {UIColor.Clear.CGColor, UIColor.Black.CGColor });
                // labelTimePlayed = new UILabel();
                //// labelTimePlayed.Frame = new CGRect(0,10,50,20);
                // labelTimePlayed.Text = "00:00";
                // labelTimePlayed.TextColor = UIColor.White;

                // //seekBar.Frame = new CGRect(50, 10, 100, 20);

                // labelTimeLeft = new UILabel();
                // //labelTimeLeft.Frame = new CGRect(150, 10, 50, 20);
                // labelTimeLeft.Text = "00:00";
                // labelTimeLeft.Text = string.Format("{0:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));
                // labelTimeLeft.TextColor = UIColor.White;


                // //bootomToolbarView.Frame = new CGRect(0, 10, 200, 20);
                // bootomToolbarView.AddSubview(labelTimePlayed);
                // bootomToolbarView.InsertSubviewAbove(labelTimeLeft, labelTimePlayed);
                // bootomToolbarView.InsertSubviewAbove(seekBar, labelTimeLeft);
                // mainView.InsertSubviewAbove(bootomToolbarView, videoPlayerAV.View);



                //playerLayer = AVPlayerLayer.FromPlayer(player);
                UIApplication.SharedApplication.SetStatusBarHidden(true, true);
            }

            //seekBar.TouchDown += Control_TouchDown;
            //seekBar.TouchUpInside += Control_TouchUpInside;


        }

        private void OnVideoTap()
        {
            if (isPlaying)
            {
                button.Hidden = !button.Hidden;

                Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                {
                    if (!button.Hidden && isPlaying)
                    {
                        UIView.Animate(0.3, () => { button.Hidden = true; });

                    }
                    return false;
                });
                //button.Animate(2, 5, UIViewAnimationOptions.f);
            }
            else
            {
                UpdatePlaying();
            }
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {
            UpdatePlaying();
        }

        private void DeviceRotated(NSNotification notification)
        {
            videoPlayerAV.VideoGravity = AVLayerVideoGravity.ResizeAspect;
            videoPlayerAV.View.SetNeedsLayout();
            videoPlayerAV.View.SetNeedsDisplay();
        }
		private void StopPlayingAction(bool canSeek)
		{
			if (player != null) {
				player.Pause();
			}   
			Console.WriteLine("Video Finished 2");
		}

        private void StopPlayingAction()
        {
			if (player != null) {
				Console.WriteLine("Video Finished");
				player.Pause();
				player.Seek (new CMTime (0, 1));
				slider.CommandImageFileName = "audioPlay.png";
				slider.CommandText = "Play";
				isPlaying = false;
			}           
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (Equals(ofObject, playerItem))
            {
                if (keyPath.Equals((NSString)"loadedTimeRanges"))
                {
                    slider.FileDuration = TimeSpan.FromSeconds(playerItem.Duration.Seconds).TotalMilliseconds;
                }
            }
            if (Equals(ofObject, bootomToolbarView))
            {
                if (keyPath.Equals((NSString)"bounds"))
                {
                    if (!bootomToolbarView.Bounds.IsEmpty)
                    {
                        gradientLayer.Frame = bootomToolbarView.Bounds;
                    }
                }
            }
            if (Equals(ofObject, videoPlayerAV))
            {
                if (keyPath.Equals((NSString)"videoBounds"))
                {
                    if (!videoPlayerAV.VideoBounds.IsEmpty && slider != null)
                    {
                        videoView.Frame = videoPlayerAV.VideoBounds;

                        //mainView.Frame = videoPlayerAV.VideoBounds;
                        // playerLayer.Frame = videoPlayerAV.VideoBounds;
                        //bootomToolbarView.TopAnchor.ConstraintEqualTo(TopAnchor, videoPlayerAV.VideoBounds.Bottom-40).Active=true;
                        //bootomToolbarView.Frame = new CGRect(videoPlayerAV.VideoBounds.Left + 10, videoPlayerAV.VideoBounds.Bottom - 10, Control.Bounds.Width - 20, 40);

                        //labelTimePlayed.Frame = new CGRect(bootomToolbarView.Frame.X, bootomToolbarView.Frame.Y, 50, 20);
                        //seekBar.Frame = new CGRect(bootomToolbarView.Frame.X + labelTimePlayed.Frame.Width, bootomToolbarView.Frame.Y, bootomToolbarView.Frame.Width - 100, 20);
                        //labelTimeLeft.Frame = new CGRect(bootomToolbarView.Frame.X + labelTimePlayed.Frame.Width + seekBar.Frame.Width, bootomToolbarView.Frame.Y, 50, 20);

                        //bootomToolbarView.Frame = new CGRect(videoPlayerAV.VideoBounds.Left, videoPlayerAV.VideoBounds.Bottom - 10, Control.Bounds.Width, 40);
                        //labelTimePlayed.Frame = new CGRect(10, 10, 50, 20);
                        //seekBar.Frame = new CGRect(60, 10, bootomToolbarView.Frame.Width-120, 20);
                        //labelTimeLeft.Frame = new CGRect(seekBar.Frame.Width+60, 10, 50, 20);
                        //labelTimePlayed = new UILabel();
                        //labelTimePlayed.Text = "00:00";
                        //labelTimePlayed.TextColor = UIColor.White;

                        //labelTimeLeft = new UILabel();
                        //labelTimeLeft.Text = string.Format("{0:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));
                        //labelTimeLeft.TextColor = UIColor.White;





                    }
                    //if (!playerLayer.VideoRect.IsEmpty && slider != null)
                    //{
                    //    slider.Frame = new CGRect(playerLayer.VideoRect.Left+20, playerLayer.VideoRect.Bottom-20, View.Bounds.Width - 20, 20);
                    //    //new CGRect(View.Bounds.Left + 20, View.Bounds.Bottom - 20, View.Bounds.Width - 20, 40);
                    //}
                }
            }

            //base.ObserveValue(keyPath, ofObject, change, context);
        }
        private void VideoDidFinishPlaying(NSNotification obj)
        {
            Console.WriteLine("Video Finished, will now restart");
			if (player != null) {
				player.Pause();
				player.Seek(new CMTime(0, 1));

				slider.CommandImageFileName = "audioPlay.png";
				slider.CommandText = "Play";
				isPlaying = false;
				button.SetImage(UIImage.FromFile(slider.CommandImageFileName), UIControlState.Normal);
				button.Hidden = false;
			}            
        }

        public async void Play()
        {
            do
            {
                await Task.Delay(5);
            } while (player.Status != AVPlayerStatus.ReadyToPlay);

            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            player.Play();
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

                    //var obj = playerItem.SeekableTimeRanges;
                    player.Seek(seekTime, finished =>
                    {
                        if (finished)
                        {
                            if (isPlaying)
                                player.Play();
                            UpdateLabels(totalSeconds, value);
                        }
                        else
                        {
                            playerItem.CancelPendingSeeks();
                            slider.TimeLeftString = "00:00";
                        }
                    });

                    // player.Play();
                }
                else
                {
                    //set valueToSeek
                    UpdateLabels(totalSeconds, value);
                }

            }
        }

        private void UpdateLabels(double totalSeconds, double value)
        {
            var leftTime = totalSeconds - value;
            slider.TimeLeftString = TimeSpan.FromSeconds(leftTime).ToString(@"mm\:ss");
            labelTimeLeft.Text = TimeSpan.FromSeconds(leftTime).ToString(@"mm\:ss");
            labelTimePlayed.Text = TimeSpan.FromSeconds(value).ToString(@"mm\:ss");
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
                if (player != null)
                {

                    var interval = new CMTime(1, 1);
                    player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
                    var videoEndNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, VideoDidFinishPlaying, playerItem);

                    player.AddPeriodicTimeObserver(interval, DispatchQueue.MainQueue, delegate (CMTime time)
                    {
                        var duration = slider.FileDuration;
                        if (duration.HasValue)
                        {
                            var totalSeconds = TimeSpan.FromMilliseconds(duration.Value).TotalSeconds;
                            var seconds = time.Seconds;
                            var leftTime = totalSeconds - seconds;
                            // slider.TimeLeftString = TimeSpan.FromSeconds(leftTime).ToString(@"mm\:ss");
                            UpdateLabels(totalSeconds, time.Seconds);
                            seekBar.Value = (float)(seconds / totalSeconds);
                        }
                    });
                }

                slider.CommandText = "Pause";
                //audioSlider.BackgroundColor = Color.GreenYellow;
                slider.CommandImageFileName = "audioPause.png";
                isPlaying = true;
                button.Hidden = true;
                Play();

            }
            else
            {
                slider.CommandImageFileName = "audioPlay.png";
                slider.CommandText = "Play";

                button.Hidden = false;
                //button.SetTitle("paused", UIControlState.Normal);
                player.Pause();
                isPlaying = false;
            }

            button.SetImage(UIImage.FromFile(slider.CommandImageFileName), UIControlState.Normal);
        }

        /*

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
            get { return Control.Value; }
            set { Control.Value = (int) ((value)); }
        }

        CGSize _fitSize;

        private AudioPlayerService player;
        public AudioPlayerService Player
        {
            get { return player; }
            set { player = value; }
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            return _fitSize;
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
                Control.ValueChanged -= OnControlValueChanged;

            base.Dispose(disposing);
        }

        UISlider seekBar;
        CustomSlider slider;
        protected override void OnElementChanged(ElementChangedEventArgs<CustomSlider> e)
        {

           if (Control == null)
            {
                // Create the native control and use SetNativeControl
                // Do not assign directly to the Control property unless you know what you are doing
                seekBar = CreateNativeControl();
                SetNativeControl(seekBar);

            }

            if (e.OldElement != null)
            {
                e.OldElement.ClickedButton -= Slider_ClickedButton;
                // Cleanup resources and remove event handlers for this element.
            }

            if (e.NewElement != null)
            {
                slider = e.NewElement;

                //seekBar.MaxValue = (int)(slider.FileDuration ?? 1);
                Player = (AudioPlayerService)slider.AudioService;
                slider.AudioService.FileUrl = slider.FileUrl;
               // Control.ValueChanged += OnControlValueChanged;
                Control.TouchDown += Control_TouchDown;
                Control.TouchUpInside += Control_TouchUpInside;
                //  slider.OnProgressChanged = (() => { this.OnProgressChanged(); });
                // slider.OnStartTrackingTouch = (() => { this.OnStartTrackingTouch(); });
                //  slider.OnStopTrackingTouch = (() => { this.OnStopTrackingTouch(); });

                // Control.SizeToFit();
                _fitSize = Control.Bounds.Size;

                // except if your not running iOS 7... then it fails...
                if (_fitSize.Width <= 0 || _fitSize.Height <= 0)
                    _fitSize = new SizeF(22, 22); // Per the glorious documentation known as the SDK docs

                //Control.SizeThatFits(new SizeF(22, 22));


                // UISlider seekbar = Control;
                // seekbar.Background = new ColorDrawable(Color.AliceBlue);
                //  seekbar.ProgressDrawable = new ColorDrawable(Color.Violet);
                // seekbar.Background.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                //seekbar.ProgressBackgroundTintList = new ColorStateList();

                // seekbar.IndeterminateDrawable.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                seekBar.MaximumTrackTintColor = Color.FromHex("#D3D1E5").ToUIColor();
                seekBar.MinimumTrackTintColor = Color.FromHex("#D3D1E5").ToUIColor();
                seekBar.ThumbTintColor = Color.FromHex("#827DCE").ToUIColor();
                seekBar.Continuous = false;


                _min = slider.Minimum;
                _max = slider.Maximum;
                Value = slider.Value;
                slider.ClickedButton += Slider_ClickedButton;
                // Control.ScaleY = 5;
                slider.CommandImageFileName = "audioPlay.png";
                slider.CommandText = "Play";
                slider.AudioService.IsStoped = true;
                slider.TimeLeftString = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));




                // seekBar.SetThumbImage(UIImage.FromFile("audioTint.png"), UIControlState.Normal);
                // seekBar.ThumbRectForBounds(new CGRect(0, 0, 2, 10), new CGRect(0, 0, 2, 10), 0);
                //seekBar.SetOnSeekBarChangeListener(this);



                // Use the properties of this element to assign to the native control, which is assigned to the base.Control property
            }



            //if (e.OldElement == null)
            //{





            //}
            //if (slider != null)
            //{

            //}



            base.OnElementChanged(e);
        }

        private void Control_TouchUpInside(object sender, EventArgs e)
        {
            var sliderCurrentPosition = slider.AudioService.CurrentPosition();
            // slider.AudioService.SeekTo((int)Control.Value, isStoped);
            if (sliderCurrentPosition > 0)
            {
              slider.AudioService.SeekTo((int)Control.Value, isStoped, () => { PlayPauseFile();
                  CheckIfFileIsPlayedAll(sliderCurrentPosition); });
            }
            else
            {
                slider.AudioService.SliderVaue = Control.Value;
                PlayPauseFile();
            }

            // view.Value = Value;

        }

        private void Control_TouchDown(object sender, EventArgs e)
        {
           slider.AudioService.Pause();
        }

        private void Slider_ClickedButton(object sender, EventArgs e)
        {
            //CustomSlider slider = Element;
            if (slider.CommandText == "Play")
            {



                //if (slider.AudioService != null && slider.AudioService.FileUrl != slider.FileUrl)
                //{
                //    slider.AudioService.Stop();
                //}
                isStoped = false;
               // slider.AudioService.Play();


                var position = slider.AudioService.CurrentPosition();
                if (position <= 1)
                {
                    slider.AudioService.Play(slider.FileUrl, isStoped, slider.AudioService.SliderVaue > 0? slider.AudioService.SliderVaue:(double?)null);

                    var interval = new CMTime(1, 2);
                    Player.Player.AddPeriodicTimeObserver(interval, DispatchQueue.MainQueue, delegate (CMTime time)
                    {
                        var duration = slider.FileDuration;
                        if (duration.HasValue)
                        {
                            var totalSeconds = TimeSpan.FromMilliseconds(duration.Value).TotalSeconds;
                            var seconds = time.Seconds;
                            Control.Value = (float)(seconds / totalSeconds);
                        }
                    });



                    //    //var fileInfo = _audioPlayer.GetInfo();

                    //    //holeFile = fileInfo.TotalMilliseconds;
                    //LabelPLay.Text = string.Format("{0:hh\\:mm\\:ss}", fileInfo);
                }
                else
                {
                    slider.AudioService.Play();
                //    //await audioSlider.ProgressTo(1, 250, Easing.Linear);
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

        void OnProgressChanged()
        {
            if (!_progressChangedOnce)
            {

                _progressChangedOnce = true;
                return;
            }

            ((IElementController) Element).SetValueFromRenderer(Slider.ValueProperty, Control.Value);

        }

        void OnStartTrackingTouch()
        {
            CustomSlider view = Element;
            view.AudioService.Pause();
        }

        void OnStopTrackingTouch()
        {
            // audioService.Pause();

            //CustomSlider view = Element;

            //view.AudioService.SeekTo((int)(Value*view.AudioService.FileLength/1000), isStoped);
           slider.AudioService.SeekTo((int) Value, isStoped, PlayPauseFile);
            // view.Value = Value;

        }

        private void PlayPauseFile()
        {
            //Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), CheckPositionAndUpdateSlider);
        }

        private bool CheckPositionAndUpdateSlider()
        {
            //CustomSlider slider = Element;

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
            var audioCurrentPosition = slider.AudioService.CurrentPosition();
            var position = audioCurrentPosition >0 ? audioCurrentPosition: Value;

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
                    slider.TimeLeftString = string.Format("{0:hh\\:mm\\:ss}",
                        TimeSpan.FromMilliseconds(slider.FileDuration ?? 0));
                }
                else
                {
                    slider.TimeLeftString = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(leftTime));
                }
                return false;
            }
            else
            {
                CheckIfFileIsPlayedAll(audioCurrentPosition);
            }
            //AudioSlider.Value = position;
            return true;
        }

        private void CheckIfFileIsPlayedAll(double sliderCurrentPosition)
        {
             if (sliderCurrentPosition >= slider.FileDuration.Value)
                {
                    slider.AudioService.Stop();
                }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        if (e.PropertyName == CustomSlider.FileDurationProperty.PropertyName)
                UpdateMaximum();
            if (e.PropertyName == Slider.MaximumProperty.PropertyName)
                UpdateMaximum();
            if (e.PropertyName == Slider.MaximumProperty.PropertyName)
                UpdateMaximum();
            else if (e.PropertyName == Slider.MinimumProperty.PropertyName)
                UpdateMinimum();
            else if (e.PropertyName == Slider.ValueProperty.PropertyName)
                UpdateValue();
        }


        void OnControlValueChanged(object sender, EventArgs eventArgs)
        {
            var sliderCurrentPosition = slider.AudioService.CurrentPosition();
            // slider.AudioService.SeekTo((int)Control.Value, isStoped);
            if (sliderCurrentPosition > 0)
            {
                slider.AudioService.SeekTo((int) Control.Value, isStoped, () => { PlayPauseFile(); CheckIfFileIsPlayedAll(sliderCurrentPosition); });

            }
            else
            {
                slider.AudioService.SliderVaue = Control.Value;
                PlayPauseFile();
            }

            // view.Value = Value;



            // ((IElementController) Element).SetValueFromRenderer(Slider.ValueProperty, Control.Value);
        }

        void UpdateMaximum()
        {
           // Control.MaxValue = (float) Element.FileDuration;
        }

        void UpdateMinimum()
        {
            //Control.MinValue = (float) Element.Minimum;
        }

        void UpdateValue()
        {
            if ((float) Element.Value != Control.Value)
                Control.Value = (float) Element.Value;
        }

        protected UISlider CreateNativeControl()
        {
            return new UISlider();
        }
        */
    }

}