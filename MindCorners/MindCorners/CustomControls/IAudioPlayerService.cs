using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Annotations;

namespace MindCorners.CustomControls
{
    public interface IAudioPlayerService
    {
        void Play(string pathToAudioFile);
        void Play();
        void Pause();
        void Stop();
        Action OnFinishedPlaying { get; set; }
        double CurrentPosition();
        double CurrentPositionMiliseconds { get; set; }
        double FileLength { get; set; }
        TimeSpan GetInfo();
        Task SeekTo(int s, bool isStoped);

        bool IsStoped { get; set; }
        bool IsCompleted { get; set; }

        string FileUrl { get; set; }
    }
}
