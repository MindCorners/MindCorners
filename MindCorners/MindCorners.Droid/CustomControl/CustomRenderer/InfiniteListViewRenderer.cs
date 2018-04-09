using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class InfiniteListViewRenderer : ListViewRenderer
    {
        private int _mPosition;

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (e.ActionMasked == MotionEventActions.Down)
            {
                // Record the position the list the touch landed on
                _mPosition = this.Control.PointToPosition((int)e.GetX(), (int)e.GetY());
                return base.DispatchTouchEvent(e);
            }

            if (e.ActionMasked == MotionEventActions.Move)
            {
                // Ignore move eents
                return true;
            }

            if (e.ActionMasked == MotionEventActions.Up)
            {
                // Check if we are still within the same view
                if (this.Control.PointToPosition((int)e.GetX(), (int)e.GetY()) == _mPosition)
                {
                    base.DispatchTouchEvent(e);
                }
                else
                {
                    // Clear pressed state, cancel the action
                    Pressed = false;
                    Invalidate();
                    return true;
                }
            }

            return base.DispatchTouchEvent(e);
        }
    }
}