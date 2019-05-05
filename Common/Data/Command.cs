using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs.Data
{
    /// <summary>
    /// Class for exchange data between server and client
    /// </summary>
    [Serializable]
    public class Command
    {
        //command type
        public CommandType CommandType { get; private set; }

        public object Data { get; private set; }

        public User UserData { get; private set; }

        public Command(CommandType command, object data, User userData)
        {
            CommandType = command;
            Data = data;
            UserData = userData;
        }
    }
}
