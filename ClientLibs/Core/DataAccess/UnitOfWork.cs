using CommonLibs.Data;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using System.Collections.Generic;
using ClientLibs.Core.ConnectionToServer;

namespace ClientLibs.Core.DataAccess
{

    /// <summary>
    /// Provide acces to client loacal data repositories
    /// </summary>
    public static class UnitOfWork
    {

        public static BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messanges"), "LocalDB");
        public static BaseRepository<Contact> ContactsTableRepo = new Repository<Contact>(new Table(new ContactTableFields(), "Contacts"), "LocalDB");

        static ClientCommandChain commandChain = new ClientCommandChain();
        static User user = new User("Vidzhel", "MyPass", "MyEmail", "MyBio", "+2");

        #region Constructor

        static UnitOfWork()
        {

            //Set up event data changed handler for tables
            MessagesTableRepo.AddDataChangedHandler((sender, args) => OnMessagesTableChanged(sender, args));
            ContactsTableRepo.AddDataChangedHandler((sender, args) => OnContactsTableChanged(sender, args));

            //Add handler on new server command received e.g. new message
            commandChain.OnNewIncomingCommand((sender, args) => OnServerCommand(sender, args));
        }

        #endregion

        #region Public Methods

        public static bool SendMessage(Message mes)
        {
            //Make request to the server
            var response = commandChain.MakeRequest(CommandType.SendMesssage, mes, user);

            //Return bool response
            return (bool)response.RequestData;
        }

        #endregion

        #region Private Handlers

        /// <summary>
        /// Hande commands from server e.g. new message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="com">command to handle</param>
        static void OnServerCommand(object sender, Command com)
        {

        }

        /// <summary>
        /// Do some stuff on message table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnMessagesTableChanged(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {

        }

        /// <summary>
        /// Do some stuff on contacts table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnContactsTableChanged(object sender, DataChangedArgs<IEnumerable<Contact>> args)
        {

        }
        #endregion
    }
}
