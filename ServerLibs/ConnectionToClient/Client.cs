using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerLibs.ConnectionToClient
{

    /// <summary>
    /// Store info about client, handle recieved, and response data
    /// </summary>
    public class Client
    {

        #region Public Members

        static event EventHandler<Command> incomingCommand;

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

        /// <summary>
        /// Adds new handler to handle incomming from client commands
        /// </summary>
        /// <param name="handler"></param>
        public void OnIncomingCommand(EventHandler<Command> handler)
        {
            incomingCommand += handler;
        }

        public Command HandleCommand()
        {
            //Deserealize data and get command
            var command = (Command) deserializeData();

            //copy user data
            if (UserInfo == null)
                UserInfo = command.UserData;
            else
                ;

            OnIncomingCommand

            return new Command(CommandType.Answer, true, UserInfo);
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
