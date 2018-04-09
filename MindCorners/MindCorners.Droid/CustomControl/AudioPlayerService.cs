using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Xamarin.Forms;
using Uri = Android.Net.Uri;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace MindCorners.Droid.CustomControl
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private MediaPlayer _mediaPlayer;
        public Action OnFinishedPlaying { get; set; }
        public double CurrentPosition()
        {
            if (_mediaPlayer != null)
            {
                CurrentPositionMiliseconds = _mediaPlayer.CurrentPosition;
                return _mediaPlayer.CurrentPosition;
            }
            else
            { return 0; }
        }

        public void Stop()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Reset();
                _mediaPlayer.Stop();
                _mediaPlayer.Completion -= MediaPlayer_Completion;

                IsStoped = true;
                IsCompleted = true;
            }
        }

        public double CurrentPositionMiliseconds { get; set; }
        public double FileLength { get; set; }
        public string FileUrl { get; set; }
        public bool IsStoped { get; set; }
        public bool IsCompleted { get; set; }

        public TimeSpan GetInfo()
        {
            int arr;
            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();

            }
            arr = _mediaPlayer.Duration;

            return TimeSpan.FromMilliseconds(arr);//TimeSpan.FromMilliseconds(arr).TotalMinutes;
        }

        public async Task SeekTo(int s, bool isStoped)
        {
            await seekTo(s, isStoped);
        }

        private async Task seekTo(int mseconds, bool isStoped)
        {
            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();
            }

            _mediaPlayer.SeekTo(mseconds);

            if (!isStoped)
            {
                _mediaPlayer.Start();
                IsStoped = false;
            }

            //_mediaPlayer.Start();
            //var p1 = _mediaPlayer.CurrentPosition;
            //IsStoped = true;
            IsCompleted = false;
        }
        public AudioPlayerService()
        {
        }

        public void Play(string pathToAudioFile)
        {
            if (_mediaPlayer != null)
            {   
                _mediaPlayer.Stop();
                _mediaPlayer.Completion -= MediaPlayer_Completion;
                IsStoped = true;
                IsCompleted = true;
            }

            var fullPath = pathToAudioFile;

            // Android.Content.Res.AssetFileDescriptor afd = null;

            //try
            //{
            //    afd = Forms.Context.Assets.OpenFd(fullPath);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error openfd: " + ex);
            //}
            // if (afd != null)
            {
                //  System.Diagnostics.Debug.WriteLine("Length " + afd.Length);
                if (_mediaPlayer == null)
                {
                    _mediaPlayer = new MediaPlayer();
                    _mediaPlayer.Prepared += (sender, args) =>
                    {
                        _mediaPlayer.Start();
                        _mediaPlayer.Completion += MediaPlayer_Completion;
                    };
                }


                _mediaPlayer.Reset();
                _mediaPlayer.SetVolume(1.0f, 1.0f);

                _mediaPlayer.SetDataSource(Forms.Context, Uri.Parse(fullPath));
                // _mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                _mediaPlayer.PrepareAsync();
            }

            IsStoped = false;
        }
        void MediaPlayer_Completion(object sender, EventArgs e)
        {
            OnFinishedPlaying?.Invoke();
            IsStoped = true;
            IsCompleted = true;
        }

        public void Pause()
        {
            _mediaPlayer?.Pause();
            IsStoped = true;
            IsCompleted = false;
        }

        public void Play()
        {
            _mediaPlayer?.Start();
            IsStoped = false;
        }

    }
}