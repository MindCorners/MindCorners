using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace MindCorners.CustomControls
{
    public interface IAudioRecorder
    {
        void Start(string fileName, out string fileFullPath);
        void Stop(out byte[] fileData);
        void Pause();
        void Resume();

		bool HaveMicrophonePermissions();
        //void Play();
       // Action OnFinishedRecoring { get; set; }
    }
}
