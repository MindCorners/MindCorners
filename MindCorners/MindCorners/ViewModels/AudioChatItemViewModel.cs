using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.CustomControls;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class AudioChatItemViewModel : ChatItemAttachmentViewModel
    {
        public ICommand StartRecordingCommand { get; set; }
        public ICommand StopRecordingCommand { get; set; }

        public ICommand ResumeRecordingCommand { get; set; }

        public ICommand PauseRecordingCommand { get; set; }

        private bool isAudioRecording;
        private List<byte[]> Records;
        private List<string> FileNames; 
        public bool IsAudioRecording
        {
            get { return isAudioRecording; }
            set
            {
                isAudioRecording= value; 
                OnPropertyChanged();
            }
        }

         
        private bool canRecord;
        public bool CanRecord
        {
            get { return canRecord; }
            set
            {
                canRecord = value;
                OnPropertyChanged();
            }
        }

        private bool canStop;
        public bool CanStop
        {
            get { return canStop; }
            set
            {
                canStop = value;
                OnPropertyChanged();
            }
        }

        private bool canResume;
        public bool CanResume
        {
            get { return canResume; }
            set
            {
                canResume = value;
                OnPropertyChanged();
            }
        }

        private bool canPause;
        public bool CanPause
        {
            get { return canPause; }
            set
            {
                canPause = value;
                OnPropertyChanged();
            }
        }

        private bool isPaused;
        public bool IsPaused
        {
            get { return isPaused; }
            set
            {
                isPaused = value; 
                OnPropertyChanged();
            }
        }

      
        public IAudioRecorder AudioRecorder { get; set; }

      



        //   public IAudioRecorder AudioRecorder { get; set; }
        public AudioChatItemViewModel()
        {
            StartRecordingCommand = new Command(StartRecording);
            StopRecordingCommand = new Command(StopRecording);
            ResumeRecordingCommand = new Command(ResumeRecording);
            PauseRecordingCommand = new Command(PauseRecording);
            AudioRecorder =  DependencyService.Get<IAudioRecorder>();
            IsFile = true;
            Records = new List<byte[]>();
            FileNames = new List<string>();
            CanRecord = true;
        }

        private void StartRecording()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 1000), UpdateRecordTime);
            FileName = string.Format("Audio_{0}.wav", DateTime.Now.Ticks);
            var tempFileName = string.Format("Audio_Temp_{0}.wav", DateTime.Now.Ticks);
            FileNames.Add(tempFileName);
            string filePath = null;
            AudioRecorder.Start(tempFileName, out filePath);
            IsAudioRecording = true;
            CanRecord = false;
            CanStop = true;
            CanPause = true;
            CanResume = false;
        }
        private void StopRecording()
        {
           
            byte[] fileData = null;
            AudioRecorder.Stop(out fileData);
            if (fileData != null)
            {
                Records.Add(fileData);

                var length = Records.Sum(a => a.Length);
                FileItemSourceArray = new byte[length];

                // FileItemSourceArray = fileData;
                IsAudioRecording = false;
                var recordLength = 0;
                foreach (var record in Records)
                {
                    Array.Copy(record, 0, FileItemSourceArray, recordLength, record.Length);
                    //Array.Copy(files[1], 0, a, files[0].Length, files[1].Length);
                    recordLength += record.Length;

                    //Array.Copy(fileDataMixed, 0, fileData, 0, fileData.Length);
                    //Array.Copy(files[1], 0, a, files[0].Length, files[1].Length);
                }
            }

            CanRecord = true;
            CanStop = false;
            CanPause = false;
            CanResume = false;

            CanSend = true;

            SendPostCommand.Execute(null);
        }

        private void PauseRecording()
        {
            AudioRecorder.Pause();
            /*
            byte[] fileData = null;
            AudioRecorder.Stop(out fileData);
            Records.Add(fileData);
            */
            //FileItemSourceArray = fileData;
            IsAudioRecording = true;
            IsPaused = true;

            CanRecord = false;
            CanStop = false;
            CanPause = false;
            CanResume = true;

        }
        private void ResumeRecording()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 1000), UpdateRecordTime);

            AudioRecorder.Resume();
            /*
            var tempFileName = string.Format("Audio_Temp_{0}.wav", DateTime.Now.Ticks);
            FileNames.Add(tempFileName);
            string filePath = null;
            AudioRecorder.Start(tempFileName, out filePath);
            */

            IsAudioRecording = true;
            IsPaused = false;

            CanRecord = false;
            CanStop = true;
            CanPause = true;
            CanResume = false;
        }



        private bool UpdateRecordTime()
        {
            if (!IsAudioRecording || IsPaused)
            {
                return false;
            }
            RecordTimeSeconds += 1;

            return true;
        }
        //public static void Concatenate(string outputFile, IEnumerable<string> sourceFiles)
        //{
        //    byte[] buffer = new byte[1024];
        //    WaveFileWriter waveFileWriter = null;

        //    try
        //    {
        //        foreach (string sourceFile in sourceFiles)
        //        {
        //            using (WaveFileReader reader = new WaveFileReader(sourceFile))
        //            {
        //                if (waveFileWriter == null)
        //                {
        //                    // first time in create new Writer
        //                    waveFileWriter = new WaveFileWriter(outputFile, reader.WaveFormat);
        //                }
        //                else
        //                {
        //                    if (!reader.WaveFormat.Equals(waveFileWriter.WaveFormat))
        //                    {
        //                        throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
        //                    }
        //                }

        //                int read;
        //                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
        //                {
        //                    waveFileWriter.WriteData(buffer, 0, read);
        //                }
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (waveFileWriter != null)
        //        {
        //            waveFileWriter.Dispose();
        //        }
        //    }

        //}
    }
}
