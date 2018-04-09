using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AudioToolbox;
using AVFoundation;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioRecorder))]

namespace MindCorners.Droid.CustomControl
{

    public class AudioRecorder: IAudioRecorder
    {
        AVAudioRecorder recorder;
        NSError error;
        NSUrl url;
        NSDictionary settings;
        string path = string.Empty;

		public bool HaveMicrophonePermissions()
		{
			var audioSession = AVAudioSession.SharedInstance();
			return audioSession.RecordPermission != AVAudioSessionRecordPermission.Denied;
		}

        public void Start(string fileName, out string fileFullName)
        {
          //  var documentsPath = Path.GetTempPath();
          //  string libraryPath = Path.Combine(documentsPath, "MindCorners"); // Library folder
           // var filePath = Path.Combine(libraryPath, fileName);

            //var filePath = System.IO.Path.Combine(documentsPath,fileName);
          
          
            InitAudio();

            //Declare string for application temp path and tack on the file extension
            //string fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string audioFilePath = Path.Combine(Path.GetTempPath(), fileName);
            path = audioFilePath;
            fileFullName = audioFilePath;
            Console.WriteLine("Audio File Path: " + audioFilePath);

            url = NSUrl.FromFilename(audioFilePath);
            //set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
            NSObject[] values = new NSObject[]
            {
    NSNumber.FromFloat (44100.0f), //Sample Rate
    NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat
    NSNumber.FromInt32 (2), //Channels
    NSNumber.FromInt32 (16), //PCMBitDepth
    NSNumber.FromBoolean (false), //IsBigEndianKey
    NSNumber.FromBoolean (false) //IsFloatKey
            };

            //Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
            NSObject[] keys = new NSObject[]
            {
    AVAudioSettings.AVSampleRateKey,
    AVAudioSettings.AVFormatIDKey,
    AVAudioSettings.AVNumberOfChannelsKey,
    AVAudioSettings.AVLinearPCMBitDepthKey,
    AVAudioSettings.AVLinearPCMIsBigEndianKey,
    AVAudioSettings.AVLinearPCMIsFloatKey
            };

            //Set Settings with the Values and Keys to create the NSDictionary
            settings = NSDictionary.FromObjectsAndKeys(values, keys);

            //Set recorder parameters
            recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out error);
            //Set Recorder to Prepare To Record
            recorder.PrepareToRecord();
            
            recorder.Record();
        }

        private bool InitAudio()
        {
            var audioSession = AVAudioSession.SharedInstance();
            var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            if (err != null)
            {
                Console.WriteLine("audioSession: {0}", err);
                return false;
            }
            err = audioSession.SetActive(true);
            if (err != null)
            {
                Console.WriteLine("audioSession: {0}", err);
                return false;
            }
            return true;
        }

        public void Stop(out byte[] fileData)
        {
            try
            {
                recorder.Stop();
                recorder.Dispose();
                //_recorder.Reset();
                //recorder.Release();
                using (var streamReader = new StreamReader(path))
                {
                    var bytes = default(byte[]);
                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        fileData = memstream.ToArray();
                    }
                }

            }
            catch (Exception e)
            {
                string error = e.ToString();
                Console.WriteLine(e);
                fileData = null;
            }
            

            recorder = null;

            //Bitmap bmThumbnail;
            //bmThumbnail = ThumbnailUtils.CreateVideoThumbnail(path, ThumbnailKind.MiniKind);
            //imageThumbnail.setImageBitmap(bmThumbnail);




            //_player.SetDataSource(path);
            //_player.Prepare();
            //_player.Start();
        }

        public void Pause()
        {
            recorder.Pause();
            //_recorder.Stop();
            //_recorder.Reset();
        }

        public void Resume()
        {
            recorder.Record();
        }
        //public Action OnFinishedRecoring { get; set; }
    }
}