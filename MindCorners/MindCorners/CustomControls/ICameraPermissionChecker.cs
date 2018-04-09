using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace MindCorners.CustomControls
{
    public interface ICameraPermissionChecker
    {
		bool HaveCameraPermissions();
        //void Play();
       // Action OnFinishedRecoring { get; set; }
    }
}
