using ServerLibs.ConnectionToClient;
using System.Collections.Generic;
using System.Threading;

namespace CommonLibs.Data
{

    /// <summary>
    /// Privide base members and methods for single chain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCommandChain<T> where T: class
    {

        protected Queue<T> commandsChain { get; set; }

        protected static ManualResetEvent newCommand = new ManualResetEvent(false);

        public abstract void AddCommand(ClientCommand command);
        public abstract void SendResponseCommand(ClientCommand command);

        protected abstract void incomingCommandsHanddler();

        protected abstract void addCommand(T com);

        protected abstract T getCommand();
    }
}
