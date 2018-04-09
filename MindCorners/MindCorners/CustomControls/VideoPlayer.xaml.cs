using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using MindCorners.Pages;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public partial class VideoPlayer : View
    {
        public VideoPlayer()
        {
            InitializeComponent();
        }

        public Action StopAction;
        public Action StartAction;
        public Action PauseAction;
        public Action HideAction;

        public static readonly BindableProperty FileUrlProperty =
       BindableProperty.Create("FileUrl", typeof(string), typeof(VideoPlayer), defaultBindingMode: BindingMode.OneWay);
        public string FileUrl
        {
            get { return (string)GetValue(FileUrlProperty); }
            set { SetValue(FileUrlProperty, value); }
        }

        //private static readonly BindableProperty fileSourceProperty = BindableProperty.Create<VideoPlayer, string>(p => p.FileSource, string.Empty);
        //public string FileSource
        //{
        //    get { return (string)GetValue(FileSourceProperty); }
        //    set { SetValue(FileSourceProperty, value); }
        //}

        //public static BindableProperty FileSourceProperty
        //{
        //    get
        //    {
        //        return fileSourceProperty;
        //    }
        //}

        public void Stop()
        {
            if (StopAction != null)
                StopAction();
        }

        public void Start()
        {
            if (StartAction != null)
                StartAction();
        }
        public void Pause()
        {
            if (PauseAction != null)
                PauseAction();
        }
        public void Hide()
        {
            if (HideAction != null)
                HideAction();
        }
        //public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(VideoPlayer), string.Empty, BindingMode.TwoWay);
        //public string Source
        //{
        //    get { return (string)GetValue(SourceProperty); }
        //    set { SetValue(SourceProperty, value); }
        //}


        //public static readonly BindableProperty LoopProperty = BindableProperty.Create(nameof(Loop), typeof(bool), typeof(VideoPlayer), true, BindingMode.TwoWay);

        //public bool Loop
        //{
        //    get { return (bool)GetValue(LoopProperty); }
        //    set { SetValue(LoopProperty, value); }
        //}

        //public Action OnFinishedPlaying { get; set; }
    }
}
