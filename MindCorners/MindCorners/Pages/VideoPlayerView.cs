using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public class VideoPlayerView : ContentView
    {
        private MyVideoPlayer _VideoPlayer;

        public MyVideoPlayer VideoPlayer
        {
            get
            {
                return this._VideoPlayer;
            }
        }

        public VideoPlayerView()
        {
            this._VideoPlayer = new MyVideoPlayer();
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.FillAndExpand;

            this.Content = this._VideoPlayer;
        }
    }
}
