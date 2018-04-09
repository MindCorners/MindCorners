using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using Xamarin.Forms;

namespace MindCorners.Behaviors
{
    public class GestureResult
    {
        /// <summary>
        /// The gesture type
        /// </summary>
        public GestureType GestureType { get; internal set; }
        /// <summary>
        /// The direction (if any) of the direction
        /// </summary>
        public Directionality Direction { get; internal set; }
        /// <summary>
        /// The point, relative to the start view where the 
        /// gesture started
        /// </summary>
        public Point Origin { get; internal set; }
        /// <summary>
        /// The point, relative to the start view where the second finger of the
        /// gesture is located (valid for GestureType.Pinch)
        /// </summary>
        public Point Origin2 { get; internal set; }
        /// <summary>
        /// The view that the gesture started in
        /// </summary>
        public View StartView { get; internal set; }
        /// <summary>
        /// The Vector Length of the gesture (if appropiate)
        /// </summary>
        public Double Length { get; set; }
        /// <summary>
        /// The Vertical distance the gesture travelled
        /// </summary>
        internal Double VerticalDistance { get; set; }
        /// <summary>
        /// The horizontal distance the gesture travelled
        /// </summary>
        internal Double HorizontalDistance { get; set; }

        /// <summary>Gets or sets the view stack.</summary>
        /// <value>A list of all view elements containing the origin point.</value>
        /// Element created at 07/11/2014,11:54 PM by Charles
        internal List<View> ViewStack { get; set; }

    }
}
