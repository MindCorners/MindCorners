using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.Models;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class VideoPreview : ContentPage
    {

       // private VideoPlayerView player;

        public VideoPreview(PostAttachment vm)
        {
            InitializeComponent();
            BindingContext = vm;

           //// player = new VideoPlayerView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };

           // var btnController = new Button()
           // {
           //     Text = "Controller",
           //     Command = new Command(() =>
           //     {
           //         this.player.VideoPlayer.AddVideoController = !this.player.VideoPlayer.AddVideoController;
           //     })
           // };

           // var btnFullScreen = new Button()
           // {
           //     Text = "Full Screen",
           //     Command = new Command(() =>
           //     {

           //         // resize the Content for full screen mode
           //         this.player.VideoPlayer.FullScreen = !this.player.VideoPlayer.FullScreen;
           //         if (this.player.VideoPlayer.FullScreen)
           //         {
           //             // this.player.HeightRequest = -1;
           //             this.Content.VerticalOptions = LayoutOptions.FillAndExpand;
           //             player.VideoPlayer.FullScreen = true;
           //         }
           //         else
           //         {
           //             // this.player.HeightRequest = 200;
           //             this.Content.VerticalOptions = LayoutOptions.CenterAndExpand;
           //             player.VideoPlayer.FullScreen = false;
           //         }
           //     })
           // };

           // var btnPlay = new Button()
           // {
           //     Text = "Play",
           //     Command = new Command(() =>
           //     {
           //         this.player.VideoPlayer.PlayerAction = VideoState.PLAY;
           //     })
           // };

           // var btnStop = new Button()
           // {
           //     Text = "Stop",
           //     Command = new Command(() =>
           //     {
           //         this.player.VideoPlayer.PlayerAction = VideoState.STOP;
           //     })
           // };

           // var btnPause = new Button()
           // {
           //     Text = "Pause",
           //     Command = new Command(() =>
           //     {
           //         this.player.VideoPlayer.PlayerAction = VideoState.PAUSE;
           //     })
           // };

           // var btnRestart = new Button()
           // {
           //     Text = "Restart",
           //     Command = new Command(() =>
           //     {
           //         this.player.VideoPlayer.PlayerAction = VideoState.RESTART;
           //     })
           // };

           // player.VideoPlayer.FullScreen = true;

           // // heightRequest must be set it not full screen
           // // player.HeightRequest = 200;
           // player.VideoPlayer.AddVideoController = true;


           // // location in Raw folder. no extension
           // player.VideoPlayer.FileSource = vm.FileUrl;

           // // autoplay video
           // player.VideoPlayer.AutoPlay = true;

           // StackLayout buttons = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, Children = { btnFullScreen, btnRestart, btnPlay, btnPause, btnStop } };
           //// player.VerticalOptions = LayoutOptions.FillAndExpand;
           //// player.HorizontalOptions = LayoutOptions.CenterAndExpand;
           // var grid = new Grid()
           // {
           //     RowDefinitions = new RowDefinitionCollection() { new RowDefinition { Height = GridLength.Auto }, new RowDefinition() { Height = GridLength.Star }},

           // };
           // grid.Children.Add(buttons, 0, 0);
           // grid.Children.Add(player, 0, 1);
            
            //this.Content = grid;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            //player.StopPlayingAction();
            App.Current.MainPage.Navigation.PopModalAsync();
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    this.player.VideoPlayer.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
        //    {
        //        if (e.PropertyName == MyVideoPlayer.StateProperty.PropertyName)
        //        {
        //            var s = this.player.VideoPlayer.State;
        //            if (s == VideoState.ENDED)
        //            {
        //                System.Diagnostics.Debug.WriteLine("State: ENDED");
        //            }
        //            else if (s == VideoState.PAUSE)
        //            {
        //                System.Diagnostics.Debug.WriteLine("State: PAUSE");
        //            }
        //            else if (s == VideoState.PLAY)
        //            {
        //                System.Diagnostics.Debug.WriteLine("State: PLAY");
        //            }
        //            else if (s == VideoState.STOP)
        //            {
        //                System.Diagnostics.Debug.WriteLine("State: STOP");
        //            }
        //        }
        //        else if (e.PropertyName == MyVideoPlayer.InfoProperty.PropertyName)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Info:\r\n" + this.player.VideoPlayer.Info);
        //        }
        //    };
        //}

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    this.player.VideoPlayer.ContentHeight = height;
        //    this.player.VideoPlayer.ContentWidth = width;
        //    if (width < height)
        //    {

        //        this.player.VideoPlayer.Orientation = MyVideoPlayer.ScreenOrientation.PORTRAIT;
        //    }
        //    else
        //    {
        //        this.player.VideoPlayer.Orientation = MyVideoPlayer.ScreenOrientation.LANDSCAPE;
        //    }
        //    this.player.VideoPlayer.OrientationChanged();
        //    base.OnSizeAllocated(width, height);
        //}

    }
}
