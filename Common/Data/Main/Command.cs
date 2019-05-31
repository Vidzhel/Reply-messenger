using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs.Data
{
    /// <summary>
    /// Class for command server what to do
    /// </summary>
    [Serializable]
    public class Command
    {
        //command type
        public CommandType CommandType { get; private set; }

        //command data
        public object RequestData { get; private set; }

        //user data
        public User UserData { get; private set; }


        public Command(CommandType command, object data, User userData)
        {
            CommandType = command;
            RequestData = data;
            UserData = userData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="com"></param>
        public Command(Command com)
        {
            CommandType = com.CommandType;
            RequestData = com.RequestData;
            UserData = com.UserData;
        }
    }
}
