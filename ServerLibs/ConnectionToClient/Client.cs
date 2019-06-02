using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using ServerLibs.DataAccess;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Threading;

namespace ServerLibs.ConnectionToClient
{

    /// <summary>
    /// Store info about client, handle recieved, and response data
    /// </summary>
    public class Client
    {

        #region Public Members

        // Client Socket
        public Socket HandledSocket { get; set; }

        //User info
        public User UserInfo { get; set; }

        //Size of recieve buffer
        public const int BufferSize = 1024;

        /// <summary>
        /// Recieved from client Serealized data
        /// </summary>
        public List<byte> BinReceivedData { get; set; } = new List<byte>();

        public delegate void ClientAction(Client sender);
        public event ClientAction ClientConnected;
        public event ClientAction ClientDisconnected;

        static ManualResetEvent ClientReceiveDone = new ManualResetEvent(true);
        public int dataSize;

        //Recieve buffer
        public byte[] Buffer = new byte[BufferSize];

        #endregion

        #region Constructor

        public Client(Socket handler, ClientAction clientConnected, ClientAction clientDisconnected)
        {

            ClientConnected = clientConnected;
            ClientDisconnected = clientDisconnected;

            HandledSocket = handler;
            ClientConnected?.Invoke(this);
            ReceiveData();
        }

        #endregion

        #region Public Methods

        public void Disconnect()
        {
            HandledSocket.Close();
            HandledSocket.Dispose();
            ClientDisconnected?.Invoke(this);

            //Set user status to offline
            if(UserInfo != null)
                HandleCommand(new Command(CommandType.SignOut, null, UserInfo));
        }

        public void ReceiveData()
        {
            try
            {
                HandledSocket.BeginReceive(Buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Disconnect();
            }
        }

        public void SendData(object dataToSend, bool dontWait = false)
        {
            var data = DataConverter.SerializeData(dataToSend);

            //Add size to vegin of the array
            var dataSize = (short)data.Length;
            DataConverter.FromInt(dataSize, out var byte1, out var byte2, out var byte3, out var byte4);

            // Add data size to the beginning
            data = DataConverter.MergeByteArrays(new byte[] { byte1, byte2, byte3, byte4 }, data);

            try
            {
                if (!dontWait)
                {

                //Begin sending file
                ClientReceiveDone.WaitOne(7000);
                ClientReceiveDone.Reset();

                }

                HandledSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Disconnect();
            }
        }

        /// <summary>
        /// Converts from bin representation and adds command to the server command chain
        /// </summary>
        public void HandleCommand()
        {
            //Deserealize data and get command
            var command = (Command)DataConverter.DeserializeData(BinReceivedData.ToArray());

            //Clear buffer
            BinReceivedData = new List<byte>();

            if (command == null)
                return;

            //copy user data
            if (command.UserData != null)
                UserInfo = UnitOfWork.Database.UsersTableRepo.FindFirst(UsersTableFields.Email.ToString(), command.UserData.Email.ToString());

            UnitOfWork.CommandChain.AddCommand(new ClientCommand(command, this));
        }

        /// <summary>
        /// Adds the command to the server command chain
        /// </summary>
        /// <param name="com"></param>
        public void HandleCommand(Command com)
        {

            UnitOfWork.CommandChain.AddCommand(new ClientCommand(com, this));
        }

        #endregion

        #region Private Methods

        void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead;
            // If user disconects than we will catch that
            try
            {
                //Read data from the client socket
                bytesRead = HandledSocket.EndReceive(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Disconnect();
                return;
            }


            if (bytesRead > 0)
            {
                //Check lable and resume sending
                if (Encoding.Default.GetString(Buffer).Contains("<EndOfReceiving>"))
                {
                    ClientReceiveDone.Set();
                    ReceiveData();
                    return;
                }

                //If it's first portion of data
                if (BinReceivedData.Count == 0)
                    //get data size
                    dataSize = DataConverter.ToInt(Buffer[0], Buffer[1], Buffer[2], Buffer[3]) + 4;

                //Delete size of received data
                dataSize -= Buffer.Length;

                if (dataSize <= 0)
                {
                    //Separate data from dummy bytes
                    byte[] temp = new byte[dataSize + Buffer.Length];
                    Array.Copy(Buffer, 0, temp, 0, dataSize + Buffer.Length);

                    //Copy buffer to bin recieved data
                    BinReceivedData.AddRange(temp);

                    //Remove first 2 bytes(size of data)
                    BinReceivedData.RemoveRange(0, 4);

                    //Send signal to resume receiving
                    SendData("<EndOfReceiving>", true);

                    HandleCommand();

                    ReceiveData();

                }
                else
                {
                    //Copy buffer to bin recieved data
                    BinReceivedData.AddRange(Buffer);

                    ReceiveData();
                }
            }
        }


        void SendCallback(IAsyncResult ar)
        {
            try
            {
                //Complate sending data
                int bytesSent = HandledSocket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Disconnect();
            }
        }

        #endregion

    }
}
