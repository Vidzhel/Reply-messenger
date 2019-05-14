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

        public static BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messages"), "LocalDB");
        public static BaseRepository<Contact> ContactsTableRepo = new Repository<Contact>(new Table(new ContactTableFields(), "Contacts"), "LocalDB");
        public static BaseRepository<Group> GroupsTableRepo = new Repository<Group>(new Table(new GroupsTableFields(), "Groups"), "LocalDB");

        public static User User { get; private set; } = new User("Vidzhel", "MyPass", "MyEmail", "MyBio", "+2");

        static ClientCommandChain commandChain = new ClientCommandChain();

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

        public static List<Contact> GetUsersInfo(List<int> id)
        {
            //Make request to server and get response command
            var res = commandChain.MakeRequest(CommandType.GetUsersInfo, id, User);

            return (List<Contact>)res.RequestData;
        }

        /// <summary>
        /// Sends message to the server
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static bool SendMessage(Message mes)
        {
            //Send command to the server
            commandChain.SendCommand(CommandType.SendMesssage, mes, User);

            //Add message to local rep
            return MessagesTableRepo.Add(mes);
        }

        /// <summary>
        /// Make Log In request to the server, return true if all ok
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static bool SighIn(User userData)
        {
            //Make response to the server
            var response =  commandChain.MakeRequest(CommandType.SignIn, null, userData);

            // if all Ok save user data
            if ((bool)response.RequestData)
            {
                User = response.UserData;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Registrate user, return true if all ok
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static bool SignUp(User userData)
        {
            //Make SignUp request to the server
            var response = commandChain.MakeRequest(CommandType.SignUp, null, userData);

            //If registered
            if ((bool)response.RequestData)
            {
                User = response.UserData;
                return true;
            }

            return false;

        }
        
        public static bool SyncData()
        {
            return false;
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
