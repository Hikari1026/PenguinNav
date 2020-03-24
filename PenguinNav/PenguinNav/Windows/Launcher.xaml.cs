using System.Windows;
using System.Net;

namespace PenguinNav
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        #region Variables

        private PenguinClient penguinClient = new PenguinClient();
        private PenguinVid navVid = new PenguinVid();

        //Tells if Launcher is being closed from X or because ConnectionSucceded
        private bool closingFromX = true;


        #endregion


        public Launcher()
        {
            InitializeComponent();
            CommandPrint("Penguin Nav Alpha 0.2");

            //Creates a new NavVid window. This way I can bind behaviour more easly and pass valid arguments efficiently
            navVid.Show();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            //Disable button during connection
            ConnectButton.IsEnabled = false;

            CommandPrint($"Connecting to {Iptext.Text} : {PortText.Text}\n");

            //Get IP address and port and bind it to socket
            penguinClient.ipAddress = IPAddress.Parse(Iptext.Text);
            penguinClient.port = int.Parse(PortText.Text);

            string result = penguinClient.SocketConnect();

            if (result == null)
            {
                ConnectionSucceded();
            }
            else
            {
                ConnectionFailed(result);
            }

        }


        /// <summary>
        /// Writes inside Prompt
        /// </summary>
        /// <param name="msg">Text to write inside Prompt</param>
        public void CommandPrint(string msg)
        {
            string newMessage = msg + "\n";
            Prompt.Text += newMessage;
        }


        #region Private Helpers


        /// <summary>
        /// Handles output if connection to rov fails
        /// </summary>
        private void ConnectionFailed(string msg)
        {
            CommandPrint($"An error has occurred: {msg}\n");
            ConnectButton.IsEnabled = true;
        }

        /// <summary>
        /// Handles output if connection to rov succeeds
        /// Close connection, pass IP and Port to NavVid and make NavVid connect to rov
        /// </summary>
        private void ConnectionSucceded()
        {
            //It's safe to assume that if rov replied correctly, then it's fully working
            //I'll close this connection and open a new one on the player
            penguinClient.SocketDisconnect();

            closingFromX = false;
            this.Close();
            NavVidSetup();
        }


        /// <summary>
        /// Handles transfering data and setup of NavVid
        /// </summary>
        private void NavVidSetup()
        {
            navVid.videoLayer.VlcStart();
            navVid.Ipaddress = Iptext.Text;
            navVid.Port = int.Parse(PortText.Text);
            navVid.NavVidSocketSetup();
            navVid.timer.Enabled = true;
        }

        #endregion

        /// <summary>
        /// Manages window closing and decides if NavVid should be closed too
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closingFromX)
            {
                navVid.Close();
            }

            try
            {
                penguinClient.SocketDisconnect();
            }
            catch { }

        }
    }
}
