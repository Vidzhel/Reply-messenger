using Common.Connections.ClientServer;
using CommonLibs.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ClientLibs.Core.ConnectionToServer
{
    public class AsynchronousServerConnection
    {
        #region Private Members

        //Receive buffer size
        static int bufferSize = 1024;

        byte[] buffer = new byte[bufferSize];

        #endregion

        static Socket Socket;

        #region Public Methods

        /// <summary>
        /// Conects to server
        /// </summary>
        /// <param name="port">port to listen</param>
        /// <param name="server">server info(ip address or name)</param>
        public static void ConnecToServer(int port = 11000, string server = "localhost")
        {

            //Get local ip addresses
            //TODO change local IP addresses on server IP
            IPHostEntry ipHost = Dns.GetHostEntry(server);
            IPAddress ipAddress = ipHost.AddressList[0];

            //Specify end remote point 
            IPEndPoint RemoteEndPoint = new IPEndPoint(ipAddress, port);


            //Create new socket
            Socket socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                Socket = socket;

                //Start connecting
                var asyncResult =socket.BeginConnect(RemoteEndPoint, null, null);

                Socket.EndConnect(asyncResult);
            }
            catch (Exception e)
            {
                DisplayMessageOnScreen("Error on starting listening " + e.ToString());
            }

            
        }

        /// <summary>
        /// Sends command to send info to server, and get response
        /// </summary>
        /// <param name="comand"></param>
        /// <returns>Response of server</returns>
        public object Send(Command comand)
        {
            //Create binary formatter for serializing
            BinaryFormatter bf = new BinaryFormatter();
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, comand);
                data = ms.ToArray();
                
            }


            var asyncResult = Socket.BeginSend(data, 0, data.Length, 0, null, null);
            Socket.EndSend(asyncResult);
            return Receive();
        }

        public object Receive()
        {
            try
            {
                var asyncResult = Socket.BeginReceive(buffer, 0, bufferSize, 0, null, null);
                Socket.EndReceive(asyncResult);
                return serializeData(buffer);
            }
            catch (Exception)
            {
                DisplayMessageOnScreen("Error on receive data from server");
                return null;
            }
        }

        /// <summary>
        /// Disconects socket
        /// </summary>
        public void Disconect()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();

            DisplayMessageOnScreen("Disconected from the server");
        }


        #endregion

        #region Private Methods

        static object deserializeData(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using(MemoryStream ms = new MemoryStream(data))
            {
                return bf.Deserialize(ms);
            }
        }

        static object serializeData(object data)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using(MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, data);

                return ms.ToArray();
            }
        }


        private static void DisplayMessageOnScreen(string message)
        {
            throw new NotImplementedException();
        }

        //private static void ReceiveCallback(IAsyncResult ar)
        //{
        //    string content = String.Empty;

        //    //Get state object, then Socket
        //    Client client = (Client)ar.AsyncState;
        //    Socket handler = client.HandledSocket;

        //    //Read data from the client socket
        //    int bytesRead = handler.EndReceive(ar);

        //    if (bytesRead > 0)
        //    {
        //        //Copy buffer to bin recieved data
        //        client.BinReceivedData.AddRange(client.Buffer);

        //        //Check the end of file
        //        content = Encoding.Default.GetString(client.Buffer);
        //        if (content.IndexOf("<EOF>") > -1)
        //        {

        //            //Deserialize received data
        //            client.DeserializeData();

        //            //All the data has benn read from the client. Display it on the console
        //            DisplayMessageOnScreen($"Read {content.Length} bytes from socket. \n Data : {content}");

        //            // Echo the data back to the client
        //            //Send(handler, content);

        //            //Send response command
        //            var response = client.CommandHandler();
        //            if (response != null)
        //                Send(client, response);

        //            // Begin recieve data
        //            handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReceiveCallback), client);
        //        }
        //        else
        //        {

        //            //Continue reading data
        //            handler.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReceiveCallback), client);
        //        }
        //    }
        //}

        #endregion

    }
}
