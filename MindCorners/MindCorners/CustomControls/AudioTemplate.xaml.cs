using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages.PromptTemplates;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.CustomControls.ChatItemTemplates
{
    public partial class AudioTemplate : ViewCell, INotifyCollectionChanged 
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
        public AudioTemplate()
        {
            InitializeComponent();
            AudioPlayer = DependencyService.Get<IAudioPlayerService>();
            AudioPlayer.OnFinishedPlaying = () =>
            {
                AudioPlayer.Pause();
                _isStopped = true;
                CommandText = "Play";
            };
            CommandText = "Play";
            _isStopped = true;
            //audioSlider.BackgroundColor = Color.Yellow;

            // LabelPLay.Text = CommandText;

            //AudioSlider.AudioService = _audioPlayer;
            // AudioProgressBar.Progress = 0;
            // Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), TimerElapsed);
            // commandButton.Text = CommandText;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            if (Settings.CurrentAudioPlayer == null)
            {
                Settings.CurrentAudioPlayer = AudioSlider;
            }
            if (Settings.CurrentAudioPlayer != null && Settings.CurrentAudioPlayer.FileUrl != AudioSlider.FileUrl)
            {
                Settings.CurrentAudioPlayer.StopPlayingAction();
            }
            AudioSlider.ClickAction();
            //.OnClickedButton();

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

        private async void TellMeMoreButtonClick(object sender, EventArgs e)
        {
            var vm = ((Post) BindingContext);
            if (vm != null)
            {
                var post = new Post() { Type = (int)PostTypes.TellMeMore, ParentId =vm.Id, CircleId = vm.CircleId};
                var pageToOpen = new TextChatItem(new TextChatItemViewModel() { ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Text }, CanSend = false });
                await App.NavigationPage.PushCustomAsync(pageToOpen);
            }
        }
    }
}
