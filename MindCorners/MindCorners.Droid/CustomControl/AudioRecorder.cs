using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioRecorder))]

namespace MindCorners.Droid.CustomControl
{

    public class AudioRecorder: IAudioRecorder
    {
        MediaRecorder _recorder;
        MediaPlayer _player;
        string path = string.Empty;
        public void Start(string fileName, out string fileFullName)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath,fileName);
            fileFullName = filePath;
            path = filePath;
            _recorder = new MediaRecorder();
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(path);
            _recorder.Prepare();
            _recorder.Start();
        }

      
        public void Stop(out byte[] fileData)
        {
            try
            {
                _recorder.Stop();

                //_recorder.Reset();
                _recorder.Release();
                using (var streamReader = new StreamReader(path))
                {
                    var bytes = default(byte[]);
                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        fileData = memstream.ToArray();
                    }
                }

                _recorder = null;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                Console.WriteLine(e);
                fileData = null;
            }

           

            //Bitmap bmThumbnail;
            //bmThumbnail = ThumbnailUtils.CreateVideoThumbnail(path, ThumbnailKind.MiniKind);
            //imageThumbnail.setImageBitmap(bmThumbnail);




            //_player.SetDataSource(path);
            //_player.Prepare();
            //_player.Start();
        }

        public void Pause()
        {
           // _recorder.Pause();
            //_recorder.Stop();
            //_recorder.Reset();
        }

        public void Resume()
        {
           // _recorder.Resume();
        }

        public bool HaveMicrophonePermissions()
        {
            var permissionStatus = Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Microphone).ContinueWith(
                task =>
                {
                    return task.Result == PermissionStatus.Granted;
                });

            return false;
        }

        //public Action OnFinishedRecoring { get; set; }
    }
}