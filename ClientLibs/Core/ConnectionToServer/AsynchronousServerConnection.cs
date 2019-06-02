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

        static int dataSize;

        static bool ServerConnected = false;

        static List<byte> binReceivedData = new List<byte>();

        static Socket Socket;

        /// <summary>
        /// Notify about <see cref="BinReceivedData"/> is ready to be handle
        /// </summary>
        static event EventHandler<byte[]> answerDataReady;

        static event EventHandler<bool> connectionChanged;

        static IPEndPoint RemoteEndPoint;


        // Thread signals
        static ManualResetEvent AcceptDone = new ManualResetEvent(true);
        static ManualResetEvent SendDone = new ManualResetEvent(true);
        static ManualResetEvent ReceiveDone = new ManualResetEvent(true);
        static ManualResetEvent ServerReceiveDone = new ManualResetEvent(true);


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

            Connect();
        }

        static void Connect()
        {

            do
            {

                try
                {

                    if (!Socket.Connected)
                    {

                        //Start connecting
                        var asyncResult = Socket.BeginConnect(RemoteEndPoint, null, null);
                        Socket.EndConnect(asyncResult);

                    }

                    ServerConnected = true;
                    //Notify connection changed
                    connectionChanged?.Invoke(null, ServerConnected);

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

                    break;
                }
                catch (Exception e)
                {
                    Thread.Sleep(ReconectionTimeSeconds * 1000);
                }

            } while (true);
        }

        /// <summary>
        /// Sends object to server
        /// </summary>
        /// <param name="obj">object to send</param>
        /// <returns>Response of server</returns>
        static public bool SendData(object obj, bool dontWait = false)
        {
            while (!ServerConnected)
            {
                Thread.Sleep(ReconectionTimeSeconds * 1000);
            }
                //Wait signal
                SendDone.WaitOne();
                SendDone.Reset();


            var data = DataConverter.SerializeData(obj);

            //Add size to vegin of the array
            var dataSize = data.Length;
            DataConverter.FromInt(dataSize, out var byte1, out var byte2, out var byte3, out var byte4);

            // Add data size to the beginning
            data = DataConverter.MergeByteArrays(new byte[] { byte1, byte2, byte3, byte4}, data);
            try
            {

                if (!dontWait)
                {
                    //Send files
                    ServerReceiveDone.WaitOne(7000);
                    ServerReceiveDone.Reset();
                }

                var asyncResult = Socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendDataCallback), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                SendData(obj);
                SendDone.Set();
            }

            return true;
        }



        static public void ReceiveData()
        {

            //Wait signal
            ReceiveDone.WaitOne();
            ReceiveDone.Reset();

            string content = String.Empty;

            try
            {
                Socket.BeginReceive(buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveDataCallback), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                answerDataReady.Invoke(binReceivedData, binReceivedData.ToArray());
                ReceiveDone.Set();
                DisplayMessageOnScreen("Error on receive data from server");
                return;
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
        
        static void ReceiveDataCallback(IAsyncResult ar)
        { 
            int bytesRead = 0;
            try
            {
                bytesRead = Socket.EndReceive(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ReceiveDone.Set();
                ReceiveData();
                return;

            }

            if (bytesRead > 0)
            {
                //Check lable and resume sending
                if (Encoding.Default.GetString(buffer).Contains("<EndOfReceiving>"))
                {
                    ServerReceiveDone.Set();
                    ReceiveDone.Set();
                    ReceiveData();
                    return;
                }

                //If it's first portion of data
                if (binReceivedData.Count == 0)
                    //get data size
                    dataSize = DataConverter.ToInt(buffer[0], buffer[1], buffer[2], buffer[3]) + 4;

                //Delete size of received data
                dataSize -= buffer.Length;

                if (dataSize <= 0)
                {
                    //Separate data from dummy bytes
                    byte[] temp = new byte[dataSize + buffer.Length];
                    Array.Copy(buffer, 0, temp, 0, dataSize + buffer.Length);

                    //Copy buffer to bin recieved data
                    binReceivedData.AddRange(temp);


                    //Remove first 2 bytes(size of data)
                    binReceivedData.RemoveRange(0, 4);

                    //Send signal to resume receiving
                    SendData("<EndOfReceiving>", true);

                    //Notify listeners about recieved data is ready to handle
                    answerDataReady.Invoke(binReceivedData, binReceivedData.ToArray());

                    //Clear binReceivedData to fill it with new data
                    binReceivedData = new List<byte>();

                    ReceiveDone.Set();

                    //Start again
                    ReceiveData();
                }
                else
                {

                    //Copy buffer to bin recieved data
                    binReceivedData.AddRange(buffer);

                    ReceiveDone.Set();

                    //Continue reading data
                    ReceiveData();
                }
            }
        }


        static void SendDataCallback(IAsyncResult ar)
        {

            try
            {
                Socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Signal the main thread to con.Message
            SendDone.Set();
        }

        /// <summary>
        /// Check connection, notify connection lost
        /// </summary>
        static void checkConnection()
        {
            byte[] tmp = new byte[1];

            do
            {
                try
                {
                    //Try connection
                    if (Socket.Connected)
                    {
                        if (!(Socket.Poll(0, SelectMode.SelectWrite) && (!Socket.Poll(0, SelectMode.SelectError))))
                            throw new Exception("Server disconnected");
                        else
                        {
                            ServerConnected = true;

                            //Notify connection changed
                            connectionChanged(null, ServerConnected);
                        }
                    }
                    else
                    {
                        throw new Exception("Server disconnected");
                    }
                    

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);


                    ServerConnected = false;
                    //Notify connection changed
                    connectionChanged(null, ServerConnected);

                    Socket.Dispose();

                    //Reconnect
                    Start();

                    //Send command to sign in again
                    UnitOfWork.SignIn(UnitOfWork.User);
                }

                //Check every ReconectionTimeSeconds seconds
                Thread.Sleep(ReconectionTimeSeconds * 1000);

            } while (true);
        }

        static void DisplayMessageOnScreen(string message)
        {

        }

        #endregion
        
    }
}
