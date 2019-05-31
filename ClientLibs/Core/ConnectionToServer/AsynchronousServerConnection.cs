using ClientLibs.Core.DataAccess;
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

        static short dataSize;

        static bool ServerConnected = false;

        static List<byte> binReceivedData = new List<byte>();

        static Socket Socket;

        /// <summary>
        /// Notify about <see cref="BinReceivedData"/> is ready to be handle
        /// </summary>
        static event EventHandler<byte[]> answerDataReady;

        static event EventHandler<bool> connectionChanged;

        static IPEndPoint RemoteEndPoint;

        #endregion


        #region Public Members

        /// <summary>
        /// Specifies time to wait before reconnect
        /// </summary>
        static public int ReconectionTimeSeconds { get; set; } = 5;

        /// <summary>
        /// Specifies ip address or server name to connect with
        /// </summary>
        static public string ServerName { get; set; } = "4ac9042580da.sn.mynetname.net";


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

            //IPHostEntry ipHost = Dns.GetHostEntry(ServerName);

            //Get local ip addresses
            //IPAddress ipAddress = IPAddress.Parse("192.168.137.1");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            //Specify end remote point 
            RemoteEndPoint = new IPEndPoint(ipAddress, Port);


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

            //Add size to vegin of the array
            var dataSize = (short)data.Length;
            DataConverter.FromShort(dataSize, out var byte1, out var byte2);

            // Add data size to the beginning
            data = DataConverter.MergeByteArrays(new byte[] { byte1, byte2}, data);
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

                //If it's first portion of data
                if(binReceivedData.Count == 0)
                    //get data size
                    dataSize = (short)(DataConverter.ToShort(buffer[0], buffer[1]) + 2);

                //Delete size of received data
                dataSize -= (short)buffer.Length;

                if (dataSize <= 0)
                {
                    //Separate data from dummy bytes
                    byte[] temp = new byte[dataSize + buffer.Length];
                    Array.Copy(buffer, 0, temp, 0, dataSize + buffer.Length);

                    //Copy buffer to bin recieved data
                    binReceivedData.AddRange(temp);


                    //Remove first 2 bytes(size of data)
                    binReceivedData.RemoveRange(0, 2);


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

                try
                {


                    //If socket not connected
                    if ((Socket.Poll(1, SelectMode.SelectRead) && Socket.Available == 0) || Socket == null)
                    {
                        ServerConnected = false;
                        //Notify connection changed
                        connectionChanged(null, ServerConnected);

                        Socket.Dispose();

                        //Reconnect
                        Start();

                        //Send command to sign in again
                        UnitOfWork.SignIn(UnitOfWork.User);
                    }
                }
                catch (Exception e) { }
                
            } while (true);
        }

        static void DisplayMessageOnScreen(string message)
        {

        }

        #endregion
        
    }
}
