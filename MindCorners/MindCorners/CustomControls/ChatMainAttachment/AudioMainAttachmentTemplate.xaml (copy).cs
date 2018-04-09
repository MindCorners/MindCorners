using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using Xamarin.Forms;

namespace MindCorners.CustomControls.ChatMainAttachment
{
    public partial class AudioMainAttachmentTemplateGrid : Grid, INotifyCollectionChanged 
    {
        private IAudioPlayerService _audioPlayer;

        public IAudioPlayerService AudioPlayer
        {
            get { return _audioPlayer; }
            set
            {
                _audioPlayer = value;
                OnPropertyChanged();
            }
        }

       // private IAudioPlayerService _audioPlayer;
        public bool _isStopped;
        private string CommandText;
        private double holeFile;
        public AudioMainAttachmentTemplateGrid()
        {
            InitializeComponent();
            AudioPlayer = DependencyService.Get<IAudioPlayerService>();
            AudioPlayer.OnFinishedPlaying = () =>
            {
                _isStopped = true;
                CommandText = "Play";
            };
            CommandText = "Play";
            _isStopped = true;
            //audioSlider.BackgroundColor = Color.Yellow;

            // LabelPLay.Text = CommandText;

            AudioSlider.AudioService = _audioPlayer;
            // AudioProgressBar.Progress = 0;
            // Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), TimerElapsed);
            // commandButton.Text = CommandText;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            AudioSlider.ClickAction();

            /*
            if (CommandText == "Play")
            {
                if (_isStopped)
                {
                    _isStopped = false;
                    AudioPlayer.Play(((Post)BindingContext).MainAttachment.FileUrl);

                    //var fileInfo = _audioPlayer.GetInfo();

                    //holeFile = fileInfo.TotalMilliseconds;
                    //LabelPLay.Text = string.Format("{0:hh\\:mm\\:ss}", fileInfo);
                }
                else
                {
                    AudioPlayer.Play();

                    //await audioSlider.ProgressTo(1, 250, Easing.Linear);
                }
                CommandText = "Pause";
                //audioSlider.BackgroundColor = Color.GreenYellow;
                AudioPlayImage.Source = "audioPause.png";
                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), CheckPositionAndUpdateSlider);

            }
            else
            {
                AudioPlayer.Pause();
                CommandText = "Play";
                // audioSlider.BackgroundColor = Color.Yellow;
                AudioPlayImage.Source = "audioPlay.png";
            }
            //commandButton.Text = CommandText;

            //LabelPLay.Text = CommandText;

            */
        }

        private bool CheckPositionAndUpdateSlider()
        {
            if (AudioPlayer.IsStoped)
            {
                return false;
            }
            if (holeFile <= 0)
            {
                var fileInfo = AudioPlayer.GetInfo();
                if (fileInfo.TotalMilliseconds > 0)
                {
                    AudioSlider.Maximum = 1000;
                    AudioSlider.Minimum = 0;
                    holeFile = fileInfo.TotalMilliseconds;
                    AudioSlider.AudioService.FileLength = holeFile;
                }
            }
            var position = AudioPlayer.CurrentPosition();

            LabelPLay.Text = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromMilliseconds(holeFile - position));
            // await AudioPlayImageOnProgress.TranslateTo(1, 0);
            AudioSlider.Value = holeFile > 0 ? (1000* position) / holeFile : 0;

            //AudioSlider.Value = position;
            return true;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
