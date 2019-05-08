using Common.Data;
using CommonLibs.Data;
using System.Collections.Generic;
using System.Threading;

namespace ClientLibs.Core.ConnectionToServer
{
    public class ClientCommandChain : BaseCommandChain<Command>
    {

        #region Private Members


        #endregion

        #region Constructor

        public ClientCommandChain()
        {
            commandsChain = new Queue<Command>();
            answersChain = new Queue<Command>();

            //Start new thread for handling commands
            Thread commandHandler = new Thread(new ThreadStart(sendCommand));
            commandHandler.Name = "Command Handler";
            commandHandler.Start();

            //Start new thread for handling answers
            Thread answersHandler = new Thread(new ThreadStart(sendCommand));
            commandHandler.Name = "Command Handler";
            commandHandler.Start();


            AsynchronousServerConnection.OnAnswerDataReady((sender, args) => addAnswer(sender, args));
        }

        #endregion

        #region Public Methods

        public Command MakeRequest(CommandType comType, object data, User user)
        {
            //Adding command to the queue 
            addCommand(new Command(comType, data, user));

            do
            {

                foreach (var answer in answersChain)
                {

                }

            } while ();

            return;
        }

        #endregion

        #region Private Methods

        protected override void answerHandler()
        {
            Command command = null;

            while (true)
            {
                //If there isn't any answers, then wait for a signal from addAnswer method
                if (answersChain.Count == 0)
                    newAnswer.WaitOne();

                while (commandsChain.Count != 0)
                {

                }
            }
        }

        protected override void sendCommand()
        {
            Command command = null;

            while (true)
            {
                //If there isn't any command, then wait for a signal from addCommand method
                if (commandsChain.Count == 0)
                    newCommand.WaitOne();

                while (commandsChain.Count != 0)
                {
                    //TODO handle if SendData method return false

                    //Send first command
                    command = getCommand();
                    AsynchronousServerConnection.SendData(command);
                }
            }
        }

        protected override void addCommand(Command com)
        {
            commandsChain.Enqueue(com);

            //Signal SendCommand to continue handle answers
            newCommand.ReleaseMutex();
        }

        protected override void addAnswer(object sender, byte[] args)
        {
            //Get bytes from args and deserialize them
            answersChain.Enqueue((Command)DataConverter.DeserializeData(args));

            //Signal AnswersHandle to continue handle answers
            newAnswer.ReleaseMutex();
        }

        protected override Command getCommand()
        {
            return commandsChain.Dequeue();
        }

        protected override Command getAnswer()
        {
            return answersChain.Dequeue();
        }

        #endregion

    }
}
