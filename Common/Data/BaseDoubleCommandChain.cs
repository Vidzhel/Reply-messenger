
using System;
using System.Collections.Generic;
using System.Threading;

namespace CommonLibs.Data
{

    /// <summary>
    /// Privide base members and methods for double ( command - answer ) chain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDoubleCommandChain<T> where T: class
    {
        protected Queue<T> commandsChain { get; set; }
        protected Queue<T> incomingCommandsChain { get; set; }

        protected static ManualResetEvent newCommand = new ManualResetEvent(false);
        protected static ManualResetEvent newAnswer = new ManualResetEvent(false);
        protected static ManualResetEvent answerReady = new ManualResetEvent(false);

        public abstract void SendCommand(CommandType comType, object data, User user);
        public abstract Command MakeRequest(CommandType comType, object data, User user);

        protected abstract void sendCommand();
        protected abstract void incomingCommandsHanddler();

        protected abstract void addCommand(T com);
        protected abstract void addIncomingCommand(object sender, byte[] args);

        protected abstract T getIncomingCommand();
        protected abstract T getCommand();
    }
}
