using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace PenguinNav
{
    class PenguinClient
    {
        //Get IP address and port and combine them into an IPEndPont
        public IPAddress ipAddress;
        IPEndPoint ipEndpoint => new IPEndPoint(ipAddress, 8080);

        public Socket clientSocket;

        /// <summary>
        /// Tries to connect to rov (open connection)
        /// </summary>
        /// <returns>Message from rov or exception generated</returns>
        public string SocketConnect()
        {
            try
            {
                //Declaring SocketType and connecting it to EndPoint
                clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(ipEndpoint);

                //If connection doesent fail, socket is working correctly (at least i hope so)
                return null;
                
            }
            catch (Exception ex)
            {
                //Throw exception like an african kid
                return ex.ToString();
            }
        }

        public void SocketDisconnect()
        {
            clientSocket.Close();
        }


        /// <summary>
        /// Encode message in ASCII and send it to socket
        /// </summary>
        /// <param name="message"></param>
        /// <returns>New message encoded in bytes</returns>
        public void PenguinSend(string message, Socket clientSocket)
        {
            //Encode message to ASCII string
            byte[] EncodedMessage = Encoding.ASCII.GetBytes(message);

            //Send encoded message to socket
            clientSocket.Send(EncodedMessage);
        }


        /// <summary>
        /// Listen to socket stream and returns decoded message
        /// </summary>
        /// <param name="clientSocket">Socket to listen to</param>
        /// <returns>Message recieved</returns>
        public string PenguinRead(Socket clientSocket)
        {
            //Allocate buffer
            byte[] Buffer = new byte[1024];

            //Read message from stream
            clientSocket.Receive(Buffer);

            //Decode message and return it to caller
            string data = Encoding.ASCII.GetString(Buffer);
            return data;

        }
    }
}
