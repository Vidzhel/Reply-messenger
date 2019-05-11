using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ClientLibs.Core.ConnectionToServer
{
    public class ClientCommandChain : BaseDoubleCommandChain<Command>
    {

        #region Private Members

        protected event EventHandler<Command> newClientCommand;

        //There will be storing answer on the request
        protected Command answer;


        #endregion

        #region Constructor

        public ClientCommandChain() : base()
        {
            incomingCommandsChain = new Queue<Command>();
            commandsChain = new Queue<Command>();

            //Connect Server
            AsynchronousServerConnection.Start();

            //Add event handler for server answer ready event
            AsynchronousServerConnection.OnAnswerDataReady((sender, args) => addIncomingCommand(sender, args));


            //Start new thread for handling answers
            Thread answersHandler = new Thread(new ThreadStart(incomingCommandsHanddler));
            answersHandler.Name = "Answer Handler";
            answersHandler.Start();

            //Start new thread for handling commands
            Thread commandHandler = new Thread(new ThreadStart(sendCommand));
            commandHandler.Name = "Command Handler";
            commandHandler.Start();
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Make request to the server and wait for the answer
        /// </summary>
        /// <param name="comType"></param>
        /// <param name="data"></param>
        /// <param name="user"></param>
        /// <returns>Answer command</returns>
        public override Command MakeRequest(CommandType comType, object data, User user)
        {
            //Adding command to the queue 
            addCommand(new Command(comType, data, user));

            //Wait for a signal
            answerReady.WaitOne();
            answerReady.Reset();

            return answer;
        }

        /// <summary>
        /// Adds handler to handle incoming commands
        /// </summary>
        /// <param name="handler"></param>
        public override void OnNewIncomingCommand(EventHandler<Command> handler)
        {
            newIncomingCommand += handler;
        }

        /// <summary>
        /// Send command without waiting for the answer
        /// </summary>
        /// <param name="comType"></param>
        /// <param name="data"></param>
        /// <param name="user"></param>
        public override void SendCommand(CommandType comType, object data, User user)
        {

            //Adding command to the queue 
            addCommand(new Command(comType, data, user));

        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Sends command to the server
        /// </summary>
        protected override void sendCommand()
        {
            Command command = null;

            while (true)
            {
                //If there isn't any command, then wait for a signal from addCommand method
                if (commandsChain.Count == 0)
                    newCommand.WaitOne();
                newCommand.Reset();

                while (commandsChain.Count != 0)
                {
                    //TODO handle if SendData method return false

                    //Send first command
                    command = getCommand();
                    AsynchronousServerConnection.SendData(command);
                }
            }
        }

        /// <summary>
        /// Addes new incoming command to the chain and signales <see cref="BaseDoubleCommandChain{T}.newAnswer"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void addIncomingCommand(object sender, byte[] args)
        {
            //Get bytes from args and deserialize them
            var data = (Command)DataConverter.DeserializeData(args);
            incomingCommandsChain.Enqueue(data);

            //Signal AnswersHandle to continue handle incoming commands
            newAnswer.Set();
        }

        /// <summary>
        /// Get first incomming command from <see cref="BaseDoubleCommandChain{T}.incomingCommandsChain"/>
        /// </summary>
        /// <returns></returns>
        protected override Command getIncomingCommand()
        {
            return incomingCommandsChain.Dequeue();
        }

        protected override void incomingCommandsHanddler()
        {
            while (true)
            {
                //If there isn't any command, then wait for a signal from addIncomingCommand method
                if (incomingCommandsChain.Count == 0)
                    newAnswer.WaitOne();
                newAnswer.Reset();

                //Check all the commands
                while (incomingCommandsChain.Count != 0)
                {

                    var incomingCommand = getIncomingCommand();

                    //if command has type answer, then save it
                    if (incomingCommand.CommandType == CommandType.Answer)
                    {
                        //copy command
                        this.answer = incomingCommand;

                        //Signal MakeRequest method about answer is ready
                        answerReady.Set();
                    }

                    //else raise an newCommandFromTheServer event to send it further
                    else
                    {
                        newIncomingCommand?.Invoke(this, incomingCommand);
                    }
                }
            }
        }


        /// <summary>
        /// Addes new incoming command to the chain and signales <see cref="BaseDoubleCommandChain{T}.newCommand"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void addCommand(Command com)
        {
            commandsChain.Enqueue(com);

            //Signal SendCommand to continue handle commands
            newCommand.Set();
        }

        /// <summary>
        /// Get first command from <see cref="BaseDoubleCommandChain{T}.commandsChain"/>
        /// </summary>
        /// <returns></returns>
        protected override Command getCommand()
        {
            return commandsChain.Dequeue();
        }

        #endregion
    }
}
