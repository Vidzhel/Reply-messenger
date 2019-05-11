using CommonLibs.Data;
using ServerLibs.ConnectionToClient;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ServerLibs.DataAccess
{
    public class ServerCommandChain : BaseCommandChain<ClientCommand>
    {

        #region Private Members

        //Invoke when a command starts handling
        protected event EventHandler<ClientCommand> newClientCommand;

        #endregion


        #region Constructor

        public ServerCommandChain()
        {
            commandsChain = new Queue<ClientCommand>();


            //Start new thread for handling commands
            Thread answersHandler = new Thread(new ThreadStart(incomingCommandsHanddler));
            answersHandler.Name = "Command Handler";
            answersHandler.Start();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds handler to handle incoming commands
        /// </summary>
        /// <param name="handler"></param>
        public void OnNewClientCommand(EventHandler<ClientCommand> handler)
        {
            newClientCommand += handler;
        }

        /// <summary>
        /// Adds new command to the chain
        /// </summary>
        public override void AddCommand(ClientCommand clientCommand)
        {
            //Send command
            AsynchronousClientListener.SendData(clientCommand.Client, clientCommand.Command);
        }

        /// <summary>
        /// Send response for the client
        /// </summary>
        /// <param name="comType"></param>
        /// <param name="data"></param>
        /// <param name="user"></param>
        public override void SendResponseCommand(ClientCommand command)
        {

            //Adding command to the queue 
            addCommand(command);

        }

        #endregion

        #region Private Methods
        
        protected override void incomingCommandsHanddler()
        {
            while (true)
            {
                //If there isn't any command, then wait for a signal from addIncomingCommand method
                if (commandsChain.Count == 0)
                    newCommand.WaitOne();
                newCommand.Reset();

                //Check all the commands
                while (commandsChain.Count != 0)
                {
                    newClientCommand?.Invoke(this, getCommand());
                }
            }
        }

        /// <summary>
        /// Addes new incoming command to the chain and signales <see cref="BaseDoubleCommandChain{T}.newCommand"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void addCommand(ClientCommand com)
        {
            commandsChain.Enqueue(com);

            //Signal SendCommand to continue handle commands
            newCommand.Set();
        }

        /// <summary>
        /// Get first command from <see cref="BaseDoubleCommandChain{T}.commandsChain"/>
        /// </summary>
        /// <returns></returns>
        protected override ClientCommand getCommand()
        {
            return commandsChain.Dequeue();
        }


        #endregion
    }
}
