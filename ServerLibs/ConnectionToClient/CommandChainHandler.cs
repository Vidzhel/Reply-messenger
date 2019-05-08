using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using ServerLibs.DataAcces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ServerLibs.ConnectionToClient
{
    /// <summary>
    /// Handles client commands
    /// </summary>
    public static class CommandChainHandler
    {

        #region Private Members

        //Commands Queue
        static Queue<Command> CommandsQueue = new Queue<Command>();

        #endregion

        #region Public Members
        
        public static bool IsHendlingCommand { get; set; } = false;

        #endregion


        #region Public Methods

        /// <summary>
        /// Adds command to the queue
        /// </summary>
        /// <param name="com">new command</param>
        public static void AddCommand(Command com)
        {
            CommandsQueue.Enqueue(com);

            //If commands isn't handling, than create new thread and handle a commands
            if (!IsHendlingCommand)
            {

                Thread commandHandler = new Thread(new ThreadStart(Handler));
                commandHandler.Name = "Command Handler";
                commandHandler.Start();
            }
        }

        #endregion

        #region Private Methods

        static void Handler()
        {
            IsHendlingCommand = true;
            Command command = null;

            try
            {

                while(CommandsQueue.Count != 0)
                {
                    // Get next commandin te list
                    command = CommandsQueue.Dequeue();

                    switch (command.CommandType)
                    {
                        case CommandType.Messsage:

                            break;
                        case CommandType.SignIn:
                            signIn(command);
                            break;
                        case CommandType.SignOut:
                            signUp(command);
                            break;
                        case CommandType.SignUp:

                            break;
                        case CommandType.UpdateMessage:

                            break;
                        case CommandType.UpdateUserInfo:

                            break;
                    }
                }

            }
            finally
            {
                IsHendlingCommand = true;
            }
        }

        #region Helpers

        /// <summary>
        /// Registrate new user
        /// </summary>
        private static void signUp(Command com)
        {
            //Check if user with same Email address already exist
            if (UnitOfWork.UsersTableRepo.IsExists(UsersTableFields.Email.ToString(), com.UserData.Email))
            {
                //Registr new User
                UnitOfWork.UsersTableRepo.Add(com.UserData);

                //Response to client
                ((Client)com.AnswerData).ResponseData = true;
            }
            else
                //If user already reggistered response false
                ((Client)com.AnswerData).ResponseData = false;
        }

        private static void signIn(Command com)
        {
            //Get user with same email
            var user = UnitOfWork.UsersTableRepo.Get(UsersTableFields.Email.ToString(), com.UserData.Email);

            //Check if 
        }

        #endregion

        #endregion
    }
}
