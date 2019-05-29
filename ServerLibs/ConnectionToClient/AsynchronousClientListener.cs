using CommonLibs.Data;
using System;
using System.Collections.Generic;
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
        static public string ServerName { get; set; } = "localhost";

        /// <summary>
        /// Specifies server port
        /// </summary>
        static public int Port { get; set; } = 11000;

        //Occur on user connected and disconected
        static event Action<Client> UserConnected;
        static event Action<Client> UserDiscconected;

        #endregion

        #region Private Members

        static List<Client> connectedClients = new List<Client>();

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
            IPHostEntry ipHost = Dns.GetHostEntry(ServerName);

            //Get first server ip address from list
            IPAddress ipAddress = ipHost.AddressList[0];

            //Specify end local point 
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);

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
            if(socket.Connected)
                socket?.Shutdown(SocketShutdown.Both);

            foreach (var client in connectedClients)
            {
                client.HandledSocket.Shutdown(SocketShutdown.Both);
                socket.Close();
                UserDiscconected?.Invoke(client);
            }

            connectedClients = new List<Client>();

            socket?.Close();
        }
        
        static public void SendData(Client client, object dataToSend)
        {
            //Convert EOF label to byte array
            var eofLabel = Encoding.Default.GetBytes("<EOF>");

            //Serialize and add label to the end
            var data = DataConverter.MergeByteArrays( DataConverter.SerializeData(dataToSend), eofLabel);

            try
            {
                //Begin sending file
                client.HandledSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), client);
            }
            catch (Exception)
            {

            }
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

        static private void SendCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            try
            {
                // Get socket
                Socket handler = client.HandledSocket;

                //Complate sending data
                int bytesSent = handler.EndSend(ar);

                displayMessageOnScreen($"Complate sending data to {client.UserInfo.Email}");
            }
            catch (Exception e)
            {
                displayMessageOnScreen($"Error on sending data to {client.UserInfo.Email}" + e.ToString());
            }
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
                Client client = new Client(handler);

                connectedClients.Add(client);

                //Add new client to list of connected clients
                UserConnected?.Invoke(client);

                // Begin recieve data
                handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static private void ReadCallback(IAsyncResult ar)
        {
            string content = String.Empty;

            //Get state object, then Socket
            Client client = (Client)ar.AsyncState;
            Socket handler = client.HandledSocket;

            int bytesRead;

            // If user disconects than we will catch that
            try
            {
                //Read data from the client socket
                bytesRead = handler.EndReceive(ar);
            }
            catch (Exception)
            {
                displayMessageOnScreen($"User { client.UserInfo?.Email ?? "Unkown" } disconected");

                connectedClients.Remove(client);

                //Remove from online clients
                UserDiscconected?.Invoke(client);


                //Set user status to offline
                client.HandleCommand(new Command(CommandType.SignOut, null, client.UserInfo));

                return;
            }

            if (bytesRead > 0)
            {

                //Convert to text to find <EOF> label
                content = Encoding.Default.GetString(client.Buffer);


                //Find Start of <EOF> label
                var EOFIndex = content.IndexOf("<EOF>");
                if (EOFIndex > -1)
                {

                    //Create new temp buffer with size of EOFIndex
                    byte[] temp = new byte[EOFIndex];

                    //Delete part after <EOF> label, with label
                    Array.Copy(client.Buffer, temp, temp.Length);

                    //Copy buffer to bin recieved data
                    client.BinReceivedData.AddRange(temp);

                    //All the data has benn read from the client. Display it on the console
                    displayMessageOnScreen($"Read {content.Length} bytes from socket {client?.UserInfo?.UserName}");

                    client.HandleCommand();

                    try
                    {
                        // Begin recieve data
                        handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
                    }
                    catch (Exception)
                    {
                        displayMessageOnScreen($"User { client.UserInfo?.Email ?? "Unkown" } disconected");

                        connectedClients.Remove(client);

                        //Remove from online clients
                        UserDiscconected?.Invoke(client);

                        //Set user status to offline
                        client.HandleCommand(new Command(CommandType.SignOut, null, client.UserInfo));

                        return;
                    }
                }
                else
                {
                    //Copy buffer to bin recieved data
                    client.BinReceivedData.AddRange(client.Buffer);

                    //Continue reading data
                    handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
                }
            }
        }

        #endregion

    }
}
