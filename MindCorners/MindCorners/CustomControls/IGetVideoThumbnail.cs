using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.CustomControls
{
    public interface IGetVideoThumbnail
    {
        byte[] GetVideoThumbnail(string path);
    }
}
