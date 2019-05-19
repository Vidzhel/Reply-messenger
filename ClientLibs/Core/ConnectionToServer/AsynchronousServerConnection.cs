using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientLibs.Core.ConnectionToServer
{

    /// <summary>
    /// Provide methods for send and receive data
    /// </summary>
    public static class AsynchronousServerConnection
    {
        #region Private Members

        //Receive buffer size

        static int bufferSize = 1024;

        static byte[] buffer = new byte[bufferSize];

        static bool ServerConnected = false;

        static List<byte> binReceivedData = new List<byte>();

        static Socket Socket;

        /// <summary>
        /// Notify about <see cref="BinReceivedData"/> is ready to be handle
        /// </summary>
        static event EventHandler<byte[]> answerDataReady;

        static event EventHandler<bool> connectionChanged;

        #endregion


        #region Public Members

        /// <summary>
        /// Specifies time to wait before reconnect
        /// </summary>
        static public int ReconectionTimeSeconds { get; set; } = 5;

        /// <summary>
        /// Specifies ip address or server name to connect with
        /// </summary>
        static public string ServerName { get; set; } = "localhost";

        /// <summary>
        /// Specifies server port
        /// </summary>
        static public int Port { get; set; } = 11000;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds new handler on answer data ready to use
        /// </summary>
        /// <param name="handler"></param>
        static public void AddAnswerDataReadyHandler(EventHandler<byte[]> handler)
        {
            answerDataReady += handler;
        }

        /// <summary>
        /// Adds new handler on connetction to the server changed
        /// </summary>
        /// <param name="handler"></param>
        static public void AddConnectionChanged(EventHandler<bool> handler)
        {
            connectionChanged += handler;
        }

        /// <summary>
        /// Conects to server
        /// </summary>
        /// <param name="Port">port to listen</param>
        /// <param name="ServerName">server info(ip address or name)</param>
        static public void Start()
        {

            //Get local ip addresses
            //TODO change local IP addresses on server IP
            IPHostEntry ipHost = Dns.GetHostEntry(ServerName);
            IPAddress ipAddress = ipHost.AddressList[0];

            //Specify end remote point 
            IPEndPoint RemoteEndPoint = new IPEndPoint(ipAddress, Port);


            //Create new socket
            Socket socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket = socket;

            do
            {

                try
                {
                    //Start connecting
                    var asyncResult = Socket.BeginConnect(RemoteEndPoint, null, null);

                    Socket.EndConnect(asyncResult);

                    ServerConnected = true;
                    //Notify connection changed
                    connectionChanged(null, ServerConnected);

                    //Start receiving data
                    Thread receiveData = new Thread(new ThreadStart(ReceiveData));
                    receiveData.IsBackground = true;
                    receiveData.Name = "Receiver From Server";
                    receiveData.Start();
                    
                    //Start checking connection
                    Thread checkingConnection = new Thread(new ThreadStart(checkConnection));
                    checkingConnection.IsBackground = true;
                    checkingConnection.Name = "Check Connection";
                    checkingConnection.Start();
                }
                catch (Exception e)
                {
                    DisplayMessageOnScreen($"Error on starting connectiong, try again after {ReconectionTimeSeconds} seconds \n " + e.ToString());
                }

                //If we didn't connect to server, than try again 
                if (!ServerConnected)
                    Thread.Sleep(ReconectionTimeSeconds * 1000);
                else
                    break;

            } while (true);

        }

        /// <summary>
        /// Sends object to server
        /// </summary>
        /// <param name="obj">object to send</param>
        /// <returns>Response of server</returns>
        static public bool SendData(object obj)
        {
            while (!ServerConnected)
            {
                Thread.Sleep(ReconectionTimeSeconds * 1000);
            }

            var data = DataConverter.SerializeData(obj);

            // Add EOF label to the end
            data = DataConverter.MergeByteArrays(data, Encoding.Default.GetBytes("<EOF>"));
            try
            {

                //Send files
                var asyncResult = Socket.BeginSend(data, 0, data.Length, 0, null, null);
                Socket.EndSend(asyncResult);

            }
            catch (Exception)
            {
                SendData(obj);
            }

            return true;
        }

        static public void ReceiveData()
        {

            string content = String.Empty;

            int bytesRead;

            try
            {
                var asyncResult = Socket.BeginReceive(buffer, 0, bufferSize, 0, null, null);
                bytesRead = Socket.EndReceive(asyncResult);
            }
            catch (Exception)
            {
                DisplayMessageOnScreen("Error on receive data from server");
                return;
            }

            if (bytesRead > 0)
            {

                //Check the end of file, if there isn't <EOF> label, then continue reading
                content = Encoding.Default.GetString(buffer);

                //Start of <EOF> label
                var EOFIndex = content.IndexOf("<EOF>");
                if (EOFIndex > -1)
                {
                    //Create new temp buffer with size of EOFIndex
                    byte[] temp = new byte[EOFIndex];

                    //Delete part after <EOF> label
                    Array.Copy(buffer, temp, temp.Length);

                    //Copy buffer to bin recieved data
                    binReceivedData.AddRange(temp);

                    //Notify listeners about recieved data is ready to handle
                    answerDataReady.Invoke(binReceivedData, binReceivedData.ToArray());

                    //All the data has benn read from the client. Display it on the console
                    DisplayMessageOnScreen($"Read {content.Length} bytes from server");

                    //Clear binReceivedData to fill it with new data
                    binReceivedData = new List<byte>();

                    //Start again
                    ReceiveData();
                }
                else
                {

                    //Copy buffer to bin recieved data
                    binReceivedData.AddRange(buffer);

                    //Continue reading data
                    ReceiveData();
                }
            }

        }

        /// <summary>
        /// Disconects socket
        /// </summary>
        static public void Disconect()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();

            DisplayMessageOnScreen("Disconected from the server");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check connection, notify connection lost
        /// </summary>
        static void checkConnection()
        {
            do
            {
                //Check every ReconectionTimeSeconds seconds
                Thread.Sleep(ReconectionTimeSeconds * 1000);

                //If socket not connected
                if ((Socket.Poll(1, SelectMode.SelectRead) && Socket.Available == 0) || Socket == null)
                {
                    ServerConnected = false;
                    //Notify connection changed
                    connectionChanged(null, ServerConnected);


                    //Reconnect
                    Start();
                }
                
            } while (true);
        }

        static void DisplayMessageOnScreen(string message)
        {

        }

        #endregion
        
    }
}
