
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common.Connections.ClientServer
{

    /// <summary>
    /// Store info about client, handle recieved data
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
        /// Received from client Deserealized data
        /// </summary>
        public object ReceivedData { get; set; }

        /// <summary>
        /// Recieved from client Serealized data
        /// </summary>
        public List<byte> BinReceivedData { get; set; }

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
        /// Deserialize data from <see cref="BinReceivedData"/> and put into <see cref="ReceivedData"/>
        /// </summary>
        /// <returns>return true if object deserialized without problems</returns>
        public bool DeserializeData()
        {
            //Create binary formatter for deserialize data
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                using (MemoryStream ms = new MemoryStream(BinReceivedData.ToArray()))
                {
                    ReceivedData = formatter.Deserialize(ms);
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public byte[] CommandHandler()
        {

            DeserializeData();

            var command = (Command)ReceivedData;

            if (command.UserData == null)
                return null;

            //copy user data
            UserInfo = command.UserData;

            return null;

            //switch (command.CommandType)
            //{
            //    case CommandType.Messsage:

            //        break;
            //    case CommandType.SignUp:

            //        break;
            //    case CommandType.SignIn:

            //        break;
            //    case CommandType.UpdateMessage:

            //        break;
            //    case CommandType.UpdateUserInfo:

            //        break;
            //    default:
            //        return null;
            //}

        }

        #endregion

        #region private Methods





        #endregion


    }
}
