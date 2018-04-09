using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.CustomControls
{
    public interface ICarouselView
    {
        /// <summary>The view is about to be shown</summary>
        /// Element created at 15/11/2014,3:36 PM by Charles
        void Showing();

        /// <summary>The view has been shown</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Shown();

        /// <summary>The view is about to be hiden</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Hiding();

        /// <summary>The view has been hiden</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Hiden();
    }
}
