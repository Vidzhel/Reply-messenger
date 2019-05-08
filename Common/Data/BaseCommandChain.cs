
using System.Collections.Generic;
using System.Threading;

namespace Common.Data
{

    /// <summary>
    /// Privide base members and methods for chain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCommandChain<T> where T: class
    {

        protected Queue<T> commandsChain { get; set; }
        protected Queue<T> answersChain { get; set; }

        protected Mutex newCommand = new Mutex(false);
        protected Mutex newAnswer = new Mutex(false);

        protected abstract void sendCommand();
        protected abstract void answerHandler();

        protected abstract void addCommand(T com);
        protected abstract void addAnswer(object sender, byte[] args);

        protected abstract T getCommand();
        protected abstract T getAnswer();

    }
}
