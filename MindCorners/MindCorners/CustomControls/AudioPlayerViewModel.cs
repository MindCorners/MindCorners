using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Models;
using Xamarin.Forms;

namespace MindCorners.CustomControls.ChatItemTemplates
{
    public class AudioPlayerViewModel: PostAttachment
    {

        private IAudioPlayerService _audioPlayer;
        private bool _isStopped;
       
        private Post mainPost;

        public Post MainPost
        {
            get { return mainPost; }
            set
            {
                mainPost = value; 
                OnPropertyChanged();
            }
        }

        public AudioPlayerViewModel(IAudioPlayerService audioPlayer)
        {  
            _audioPlayer = audioPlayer;
            _audioPlayer.OnFinishedPlaying = () => {
                _isStopped = true;
                CommandText = "Play";
            };
            CommandText = "Play";
            _isStopped = true;
        }
        
        private string _commandText;
        public string CommandText
        {
            get { return _commandText; }
            set
            {
                _commandText = value;
              OnPropertyChanged();
            }
        }

        private ICommand _playPauseCommand;
        public ICommand PlayPauseCommand
        {
            get
            {
                return _playPauseCommand ?? (_playPauseCommand = new Command(
                  (obj) =>
                  {
                      if (CommandText == "Play")
                      {
                          if (_isStopped)
                          {
                              _isStopped = false;
                              _audioPlayer.Play(FileUrl);
                          }
                          else
                          {
                              _audioPlayer.Play();
                          }
                          CommandText = "Pause";
                      }
                      else
                      {
                          _audioPlayer.Pause();
                          CommandText = "Play";
                      }
                  }));
            }
        }
    }

}
