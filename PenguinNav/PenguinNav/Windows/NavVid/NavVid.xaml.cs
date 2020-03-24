using System;
using System.Net;
using System.Windows;
using Airhack;
using PenguinNav.Windows.NavVid;
using System.Timers;

namespace PenguinNav
{
    /// <summary>
    /// Logica di interazione per NavVid.xaml
    /// </summary>
    public partial class PenguinVid : Window
    {
        #region Variables

        public string Ipaddress;
        public int Port;

        public VideoLayer videoLayer = new VideoLayer();
        public Overlay overlay = new Overlay();
        private PenguinClient penguinClient = new PenguinClient();

        public System.Timers.Timer timer = new Timer(33);

        #endregion


        public PenguinVid()
        {
            InitializeComponent();

            //Merge VideoLayer and Overlay toghether and displays it
            myGrid.Children.Add(new Airhack.AirControl() { Back = videoLayer, Front = overlay });

            //Bind function to timer
            timer.Elapsed += new ElapsedEventHandler(DataExcange);
            

            
        }
        

        /// <summary>
        /// Load Ip and port into socket and connect to server. Note that it assumes connection succeeds, since launcher socket did
        /// </summary>
        public void NavVidSocketSetup()
        {
            penguinClient.ipAddress = IPAddress.Parse(Ipaddress);
            penguinClient.port = Port;
            penguinClient.SocketConnect();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                penguinClient.SocketDisconnect();
            }
            catch { }
        }


        #region Private Helpers

        private void DataExcange(object source, ElapsedEventArgs e)
        {
            //Send commands to rov --- TO DO: FIX THIS
            penguinClient.PenguinSend("Dummy text", penguinClient.clientSocket);

            //Recieve sensor data from rov
            string msg = penguinClient.PenguinRead(penguinClient.clientSocket);

            //Transform message into a series of variables (still string)
            string[] result = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Assign values to Members
            AssignValues(result);

        }

        private void AssignValues(string[] data)
        {
            overlay.GyroIndicator = double.Parse(data[0]);
            //INSERT OTHER VALUES HERE
        }

        #endregion

    }
}
