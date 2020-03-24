using System.Windows.Controls;
using AxAXVLC;

namespace PenguinNav.Windows.NavVid
{
    /// <summary>
    /// Logica di interazione per VideoLayer.xaml
    /// </summary>
    public partial class VideoLayer : UserControl
    {
        AxVLCPlugin2 vlc;

        public VideoLayer()
        {
            InitializeComponent();
            VlcSetup();
        }


        /// <summary>
        /// Create Vlc control and set properties to it
        /// </summary>
        private void VlcSetup()
        {
            vlc = new AxVLCPlugin2();
            WinFormHost1.Child = vlc;

            vlc.CreateControl();
            vlc.Toolbar = false;
            vlc.FullscreenEnabled = false;

        }

        /// <summary>
        /// Starts Vlc video
        /// </summary>
        public void VlcStart()
        {
            //Dummy file --- REPLACE WITH VIDEOSOURCE FROM ROV
            vlc.playlist.add(@"file:///C:\Users\marco\Downloads\CheekiBreeki.mp4", "", " ");
            vlc.playlist.play();
        }
    }
}
