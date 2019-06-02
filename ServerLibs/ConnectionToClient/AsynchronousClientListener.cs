using CommonLibs.Data;
using ServerLibs.DataAccess;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerLibs.ConnectionToClient
{
    public static class AsynchronousClientListener
    {

        #region Public Members


        /// <summary>
        /// delegate that set context of <see cref="displayMessageOnScreen"/>
        /// </summary>
        /// <param name="message">message to display</param>
        public delegate void DisplayMessage(string message);

        /// <summary>
        /// Set context of <see cref="displayMessageOnScreen"/>
        /// </summary>
        static public DisplayMessage DisplayMessageOnScreenContext { get; set; }

        /// <summary>
        /// Specifies ip address or server name to connect with
        /// </summary>
        static public string ServerName { get; set; } = "4ac9042580da.sn.mynetname.net";

        /// <summary>
        /// Specifies server port
        /// </summary>
        static public int Port { get; set; } = 11000;

        //Occur on user connected and disconected
        static event Action<Client> UserConnected;
        static event Action<Client> UserDiscconected;

        #endregion

        #region Private Members

        static public List<Client> connectedClients = connectedClients = new List<Client>();

        //main socket
        static Socket socket;

        // Thread signal 
        static ManualResetEvent allDone = new ManualResetEvent(false);

        #endregion

        #region Public Methods

        /// <summary>
        /// Add handler on new user connected to the server
        /// </summary>
        /// <param name="handler"></param>
        static public void OnUserConnected(Action<Client> handler)
        {
            UserConnected += handler;
        }

        /// <summary>
        /// Add handler on user disconected from the server
        /// </summary>
        /// <param name="handler"></param>
        static public void OnUserDisconnected(Action<Client> handler)
        {
            UserDiscconected += handler;
        }

        /// <summary>
        /// Starts listening to clients
        /// </summary>
        /// <param name="port">port to listen</param>
        /// <param name="server">server info(ip address or name)</param>
        static public void Start()
        {

            //Get local ip addresses
            //IPHostEntry ipHost = Dns.GetHostEntry(ServerName);

            //Get first server ip address from list
            //IPAddress ipAddress = ipHost.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse("0.0.0.0");


            //Specify end local point 
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);

            //Create new socket
            Socket listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Bind socket to the end point
                listener.Bind(ipEndPoint);

                //Start listening
                listener.Listen(0);

                //copy main socket
                socket = listener;

                while (true)
                {

                    // Set the event to nonsignaled state
                    allDone.Reset();

                    //Start an asynchronous socket to listen for connections
                    displayMessageOnScreen("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);

                    //Wait until AsyncCallback function sends signal to continue
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                displayMessageOnScreen("Error on starting listening ");
            }

            displayMessageOnScreen("End listening");
        }

        /// <summary>
        /// Disconects socket
        /// </summary>
        /// <param name="reuseSocket">reuse socket in the future</param>
        static public void Disconect(bool reuseSocket)
        {
            if (!socket.Connected)
                return;

            socket.Shutdown(SocketShutdown.Both);

            foreach (var client in connectedClients)
                client.Disconnect();

            connectedClients = new List<Client>();

            socket.Close();
            socket.Dispose();
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Display message, uses <see cref="DisplayMessage"/> delegate as context
        /// </summary>
        /// <param name="message">message to display</param>
        static void displayMessageOnScreen(string message)
        {
            DisplayMessageOnScreenContext?.Invoke(message);
        }

        static private void AcceptCallBack(IAsyncResult ar)
        {
            // Signal the main thread to continue
            allDone.Set();

            displayMessageOnScreen("New connection");

            try
            {

                // Get socket that handles current client request
                Socket handler = socket.EndAccept(ar);
                
                // Create client object that will provide command handler
                Client client = new Client(handler, Client_ClientConnected, Client_ClientDisconnected);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Client_ClientDisconnected(Client sender)
        {
            if (connectedClients.Contains(sender))
            {
                connectedClients.Remove(sender);
                UserDiscconected?.Invoke(sender);
            }
        }

        private static void Client_ClientConnected(Client sender)
        {
            connectedClients.Add(sender);
            UserConnected?.Invoke(sender);
        }

        #endregion

    }
}

