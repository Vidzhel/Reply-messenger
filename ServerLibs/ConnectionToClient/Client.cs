using CommonLibs.Data;
using ServerLibs.DataAccess;
using System.Collections.Generic;
using System.Net.Sockets;

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
        /// Converts from bin representation and adds command to the server command chain
        /// </summary>
        public void HandleCommand()
        {
            //Deserealize data and get command
            var command = (Command)DataConverter.DeserializeData(BinReceivedData.ToArray());

            //copy user data
            if (command.UserData != null)
                UserInfo = command.UserData;

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

    }
}
