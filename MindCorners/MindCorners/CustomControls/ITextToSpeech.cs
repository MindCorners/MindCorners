using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.CustomControls
{
    public interface ITextToSpeech
    {
        void Speak(string text); //note that interface members are public by default
    }
}
