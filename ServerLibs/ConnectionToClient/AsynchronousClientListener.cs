using Common.Connections.ClientServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerLibs.ConnectionToClient
{
    public static class AsynchronousClientListener
    {

        #region Public Members


        /// <summary>
        /// delegate that set context of <see cref="DisplayMessageOnScreen"/>
        /// </summary>
        /// <param name="message">message to display</param>
        public delegate void DisplayMessage(string message);

        /// <summary>
        /// Set context of <see cref="DisplayMessageOnScreen"/>
        /// </summary>
        public static DisplayMessage DisplayMessageOnScreenContext { get; set; }

        // Thread signal 
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        #endregion

        #region Private Members

        //main socket
        static Socket socket;

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts listening to clients
        /// </summary>
        /// <param name="port">port to listen</param>
        /// <param name="server">server info(ip address or name)</param>
        public static void SrtartListening(int port = 11000, string server = "localhost")
        {

            //Get local ip addresses
            //TODO change local IP addresses on server IP
            IPHostEntry ipHost = Dns.GetHostEntry(server);

            //Get first server ip address from list
            IPAddress ipAddress = ipHost.AddressList[0];

            //Specify end local point 
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);


            //Create new socket
            Socket listener  = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Bind socket to the end point
                listener.Bind(ipEndPoint);

                //Start listening
                listener.Listen(10);

                //copy main socket
                socket = listener;

                while (true)
                {

                    // Set the event to nonsignaled state
                    allDone.Reset();

                    //Start an asynchronous socket to listen for connections
                    DisplayMessageOnScreen("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);

                    //Wait until AsyncCallback function sends signal to continue
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                DisplayMessageOnScreen("Error on starting listening " +e.ToString());
            }

            DisplayMessageOnScreen("End listening");
        }


        /// <summary>
        /// Disconects socket
        /// </summary>
        /// <param name="reuseSocket">reuse socket in the future</param>
        public static void Disconect(bool reuseSocket)
        {
            socket.Disconnect(reuseSocket);
        }

        /// <summary>
        /// Display message, uses <see cref="DisplayMessage"/> delegate as context
        /// </summary>
        /// <param name="message">message to display</param>
        private static void DisplayMessageOnScreen(string message)
        {
            DisplayMessageOnScreenContext?.Invoke(message);
        }

        private static void AcceptCallBack(IAsyncResult ar)
        {
            // Signal the main thread to continue
            allDone.Set();

            DisplayMessageOnScreen("New connection");

            // Get socket that handles current client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create client object that will provide Send Recieve data methods
            Client client = new Client(handler);

            // Begin recieve data
            handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            string content = String.Empty;

            //Get state object, then Socket
            Client client = (Client)ar.AsyncState;
            Socket handler = client.HandledSocket;

            //Read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if(bytesRead > 0)
            {
                //Copy buffer to bin recieved data
                client.BinReceivedData.AddRange(client.Buffer);

                //Check the end of file
                content = Encoding.Default.GetString(client.Buffer);
                if(content.IndexOf("<EOF>") > -1)
                {

                    //Deserialize received data
                    client.DeserializeData();

                    //All the data has benn read from the client. Display it on the console
                    DisplayMessageOnScreen($"Read {content.Length} bytes from socket. \n Data : {content}");

                    // Echo the data back to the client
                    //Send(handler, content);

                    //Send response command
                    var response = client.CommandHandler();
                    if (response != null)
                        Send(client, response);

                    // Begin recieve data
                    handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
                }
                else
                {

                    //Continue reading data
                    handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
                }
            }
        }

        private static void Send(Client client, byte[] dataToSend)
        {
            //Begin sending file
            client.HandledSocket.BeginSend(dataToSend, 0, dataToSend.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            try
            {
                // Get socket
                Socket handler = client.HandledSocket;

                //Complate sending data
                int bytesSent = handler.EndSend(ar);

                DisplayMessageOnScreen($"Complate sending data to {client.UserInfo.Email}");
            }
            catch (Exception e)
            {
                DisplayMessageOnScreen($"Error on sending data to {client.UserInfo.Email}" + e.ToString());
            }
        }


        #endregion
    }
}
