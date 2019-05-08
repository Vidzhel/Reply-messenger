using CommonLibs.Core.ConnectionToServer;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerLibs.ConnectionToClient
{

    /// <summary>
    /// Store info about client, handle recieved, and response data
    /// </summary>
    public class Client
    {

        #region Private Members

        /// <summary>
        /// Signales <see cref="HandleCommand"/> about response data is ready
        /// </summary>
        ManualResetEvent responseDataIsReady = new ManualResetEvent(false);

        object responseData = new object();

        #endregion

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

        /// <summary>
        /// Data for cient
        /// </summary>
        public object ResponseData {
            get { return responseData}
            set
            {
                responseData = value;

                //Signal CommandHandler to continue
                responseDataIsReady.Set();
            }
        }

        //Recieve buffer
        public byte[] Buffer = new byte[BufferSize];

        #endregion

        #region Constructor

        public Client(Socket handler)
        {
            HandledSocket = handler;
        }

        #endregion

        #region Public Methods

        public byte[] HandleCommand()
        {
            //Reset signal to use again
            responseDataIsReady.Reset();

            //Deserealize data and get command
            var command = (Command) deserializeData();

            //Set extra data to this object
            command.AnswerData = this;

            //Pull command to the queue
            CommandChainHandler.AddCommand(command);

            //copy user data5
            UserInfo = command.UserData;

            //Wait for response data ready signal from another thread
            responseDataIsReady.WaitOne();

            return serializeData(ResponseData);
        }

        #endregion

        #region Private Methods



        /// <summary>
        /// Serealize data
        /// </summary>
        /// <returns>return true if object deserialized without problems</returns>
        byte[] serializeData(object dataToSerealize)
        {
            //Create binary formatter for deserialize data
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, dataToSerealize);

                    return ms.ToArray();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Deserialize data from <see cref="BinReceivedData"/> and put into <see cref="ReceivedData"/>
        /// </summary>
        /// <returns>return true if object deserialized without problems</returns>
        object deserializeData()
        {
            //Create binary formatter for deserialize data
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                using (MemoryStream ms = new MemoryStream(BinReceivedData.ToArray()))
                {
                    return formatter.Deserialize(ms);
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
