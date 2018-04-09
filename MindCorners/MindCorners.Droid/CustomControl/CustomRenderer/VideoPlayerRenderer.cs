using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{

    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, VideoView>, ISurfaceHolderCallback
    {
        VideoView videoview;
        MediaPlayer player;
        MediaController mediaController;
        Handler handler = new Handler();

        public VideoPlayerRenderer()
        {
        }

        public void SurfaceChanged(ISurfaceHolder holder, global::Android.Graphics.Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
           // throw new NotImplementedException();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            //base.OnElementChanged(e);
            //e.NewElement.StopAction = () => {
            //    this.player.Stop();
            //    this.Control.StopPlayback();
            //};

            //videoview = new VideoView(Forms.Context);

            //base.SetNativeControl(videoview);
            //Control.Holder.AddCallback(this);
            //player = new MediaPlayer();
            //play(e.NewElement.FileSource);


            base.OnElementChanged(e);
            e.NewElement.StopAction = () =>
            {
                mediaController.Hide();
                player.Stop();
                player.Release();
            };

            e.NewElement.StartAction = () =>
            {

                this.player.Start();
            };

            e.NewElement.PauseAction = () =>
            {

                this.player.Pause();
                mediaController.Hide();
            };

            e.NewElement.HideAction = () =>
            {
                mediaController.Hide();
            };


            videoview = new VideoView(Context);
            videoview.SetMediaController(mediaController);

            base.SetNativeControl(videoview);
            //Control.Holder.AddCallback(this);
            //Control.SetZOrderMediaOverlay(true);
            //player = new MediaPlayer();

            //player.SetOnPreparedListener(this);
            //mediaController = new MediaController(this.Context);
            //play(e.NewElement.FileSource);


        }
      //  VideoView videoview;
       // MediaPlayer player;
    
    }




    /* public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, Android.Views.View>, ISurfaceHolderCallback
     {

         private Android.Views.View mainView = null;
         private VideoView videoView = null;
         private MediaPlayer player;
         protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
         {
             base.OnElementChanged(e);
             if (Control == null)
             {
                 if (Control == null)
                 {
                     videoView = new VideoView(Context)
                     {   
                         //Background = new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()),
                        // Visibility = ViewStates.Gone,
                         LayoutParameters = new LayoutParams(
                             ViewGroup.LayoutParams.MatchParent,
                             ViewGroup.LayoutParams.MatchParent),
                     };

                     var uri = Android.Net.Uri.Parse(Element.Source);
                     videoView.SetVideoURI(uri);

                     this.AddView(videoView);
                    // videoView.Start();






                     ISurfaceHolder holder = videoView.Holder;
                     holder.SetType(SurfaceType.PushBuffers);
                     holder.AddCallback(this);
                     player = new MediaPlayer();
                     // Android.Content.Res.AssetFileDescriptor afd = this.Assets.OpenFd(fullPath);
                     //if (afd != null)
                     //{


                     //}


                     player.SetDataSource(Context, Android.Net.Uri.Parse(Element.Source));
                     //.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                     player.Prepare();
                     player.Start();


                     //_mainFrameLayout.AddView(_mainVideoView);
                     //_mainFrameLayout.AddView(_placeholder);

                     //SetNativeControl(_mainFrameLayout);

                     //PlayVideo(Element.Source);
                 }
                 //if (e.OldElement != null)
                 //{
                 //    // Unsubscribe
                 //    if (_videoPlayer != null && _isCompletionSubscribed)
                 //    {
                 //        _isCompletionSubscribed = false;
                 //        _videoPlayer.Completion -= Player_Completion;
                 //    }
                 //}
                 //if (e.NewElement != null)
                 //{
                 //    // Subscribe
                 //    if (_videoPlayer != null && !_isCompletionSubscribed)
                 //    {
                 //        _isCompletionSubscribed = true;
                 //        _videoPlayer.Completion += Player_Completion;
                 //    }
                 //}


             }
         }

         public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
         {
             Console.WriteLine("SurfaceCreated");
             player.SetDisplay(holder);
         }

         public void SurfaceCreated(ISurfaceHolder holder)
         {
             Console.WriteLine("SurfaceDestroyed");
         }

         public void SurfaceDestroyed(ISurfaceHolder holder)
         {
             Console.WriteLine("SurfaceChanged");
         }
     }



     */



    /*
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, FrameLayout>,
                 TextureView.ISurfaceTextureListener,
                 ISurfaceHolderCallback
    {
        private bool _isCompletionSubscribed = false;

        private FrameLayout _mainFrameLayout = null;

        private Android.Views.View _mainVideoView = null;
        private Android.Views.View _placeholder = null;


        private MediaPlayer _videoPlayer = null;
        internal MediaPlayer CustomVideoPlayer
        {
            get
            {
                if (_videoPlayer == null)
                {
                    _videoPlayer = new MediaPlayer();

                    if (!_isCompletionSubscribed)
                    {
                        _isCompletionSubscribed = true;
                        _videoPlayer.Completion += Player_Completion;
                    }

                    _videoPlayer.VideoSizeChanged += (sender, args) =>
                    {
                        AdjustTextureViewAspect(args.Width, args.Height);
                    };

                    _videoPlayer.Info += (sender, args) =>
                    {
                        Console.WriteLine("onInfo what={0}, extra={1}", args.What, args.Extra);
                        if (args.What == MediaInfo.VideoRenderingStart)
                        {
                            Console.WriteLine("[MEDIA_INFO_VIDEO_RENDERING_START] placeholder GONE");
                            _placeholder.Visibility = ViewStates.Gone;
                        }
                    };

                    _videoPlayer.Prepared += (sender, args) =>
                    {
                        _mainVideoView.Visibility = ViewStates.Visible;
                        _videoPlayer.Start();
                        if (Element != null)
                            _videoPlayer.Looping = Element.Loop;
                    };
                }

                return _videoPlayer;
            }
        }

        private void Player_Completion(object sender, EventArgs e)
        {
            Element?.OnFinishedPlaying?.Invoke();
        }

        private void PlayVideo(string fullPath)
        {
            Android.Content.Res.AssetFileDescriptor afd = null;

            try
            {
                afd = Context.Assets.OpenFd(fullPath);
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine("Play video: " + Element.Source + " not found because " + ex);
                _mainVideoView.Visibility = ViewStates.Gone;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error openfd: " + ex);
                _mainVideoView.Visibility = ViewStates.Gone;
            }

            if (afd != null)
            {
                Console.WriteLine("Lenght " + afd.Length);
                CustomVideoPlayer.Reset();
               // CustomVideoPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                CustomVideoPlayer.SetDataSource(Context, Android.Net.Uri.Parse(Element.Source));
                // CustomVideoPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                CustomVideoPlayer.PrepareAsync();
            }
        }

        private void AdjustTextureViewAspect(int videoWidth, int videoHeight)
        {
            if (!(_mainVideoView is TextureView))
                return;

            if (Control == null)
                return;

            var control = Control;

            var textureView = _mainVideoView as TextureView;

            var controlWidth = control.Width;
            var controlHeight = control.Height;
            var aspectRatio = (double)videoHeight / videoWidth;

            int newWidth, newHeight;

            if (controlHeight <= (int)(controlWidth * aspectRatio))
            {
                // limited by narrow width; restrict height
                newWidth = controlWidth;
                newHeight = (int)(controlWidth * aspectRatio);
            }
            else
            {
                // limited by short height; restrict width
                newWidth = (int)(controlHeight / aspectRatio);
                newHeight = controlHeight;
            }

            int xoff = (controlWidth - newWidth) / 2;
            int yoff = (controlHeight - newHeight) / 2;

            Console.WriteLine("video=" + videoWidth + "x" + videoHeight +
                " view=" + controlWidth + "x" + controlHeight +
                " newView=" + newWidth + "x" + newHeight +
                " off=" + xoff + "," + yoff);

            var txform = new Matrix();
            textureView.GetTransform(txform);
            txform.SetScale((float)newWidth / controlWidth, (float)newHeight / controlHeight);
            txform.PostTranslate(xoff, yoff);
            textureView.SetTransform(txform);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _mainFrameLayout = new FrameLayout(Context);

                _placeholder = new Android.Views.View(Context)
                {
                    Background = new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()),
                    LayoutParameters = new LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.MatchParent),
                };

                if (Build.VERSION.SdkInt < BuildVersionCodes.IceCreamSandwich)
                {
                    Console.WriteLine("Using VideoView");

                    var videoView = new VideoView(Context)
                    {
                        Background = new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()),
                        Visibility = ViewStates.Gone,
                        LayoutParameters = new LayoutParams(
                        ViewGroup.LayoutParams.MatchParent,
                        ViewGroup.LayoutParams.MatchParent),
                    };

                    ISurfaceHolder holder = videoView.Holder;
                    if (Build.VERSION.SdkInt < BuildVersionCodes.Honeycomb)
                    {
                        holder.SetType(SurfaceType.PushBuffers);
                    }
                    holder.AddCallback(this);

                    _mainVideoView = videoView;
                }
                else
                {
                    Console.WriteLine("Using TextureView");

                    var textureView = new TextureView(Context)
                    {
                        Background = new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()),
                        Visibility = ViewStates.Gone,
                        LayoutParameters = new LayoutParams(
                        ViewGroup.LayoutParams.MatchParent,
                        ViewGroup.LayoutParams.MatchParent),
                    };

                    textureView.SurfaceTextureListener = this;

                    _mainVideoView = textureView;
                }

                _mainFrameLayout.AddView(_mainVideoView);
                _mainFrameLayout.AddView(_placeholder);

                SetNativeControl(_mainFrameLayout);

                PlayVideo(Element.Source);
            }
            if (e.OldElement != null)
            {
                // Unsubscribe
                if (_videoPlayer != null && _isCompletionSubscribed)
                {
                    _isCompletionSubscribed = false;
                    _videoPlayer.Completion -= Player_Completion;
                }
            }
            if (e.NewElement != null)
            {
                // Subscribe
                if (_videoPlayer != null && !_isCompletionSubscribed)
                {
                    _isCompletionSubscribed = true;
                    _videoPlayer.Completion += Player_Completion;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Element == null || Control == null)
                return;

            if (e.PropertyName == VideoPlayer.SourceProperty.PropertyName)
            {
                Console.WriteLine("Play video: " + Element.Source);
                PlayVideo(Element.Source);
            }
            else if (e.PropertyName == VideoPlayer.LoopProperty.PropertyName)
            {
                Console.WriteLine("Is Looping? " + Element.Loop);
                CustomVideoPlayer.Looping = Element.Loop;
            }
        }

        private void RemoveVideo()
        {
            _placeholder.Visibility = ViewStates.Visible;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            Console.WriteLine("Surface.TextureAvailable");
            CustomVideoPlayer.SetSurface(new Surface(surface));
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            Console.WriteLine("Surface.TextureDestroyed");
            RemoveVideo();
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            Console.WriteLine("Surface.TextureSizeChanged");
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            Console.WriteLine("Surface.TextureUpdated");
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
            Console.WriteLine("Surface.Changed");
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            Console.WriteLine("Surface.Created");
            CustomVideoPlayer.SetDisplay(holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            Console.WriteLine("Surface.Destroyed");
            RemoveVideo();
        }
    }

    */
}