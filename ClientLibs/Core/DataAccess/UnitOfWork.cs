using CommonLibs.Data;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using System.Collections.Generic;
using ClientLibs.Core.ConnectionToServer;
using System;

namespace ClientLibs.Core.DataAccess
{

    /// <summary>
    /// Provide acces to client loacal data repositories
    /// </summary>
    public static class UnitOfWork
    {

        #region Private Members

        static event EventHandler<DataChangedArgs<IEnumerable<object>>> UserInfoUpdated;

        #endregion

        #region Public Members

        public static BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messages"), "LocalDB");
        public static BaseRepository<Contact> ContactsTableRepo = new Repository<Contact>(new Table(new ContactsTableFields(), "Contacts"), "LocalDB");
        public static BaseRepository<Group> GroupsTableRepo = new Repository<Group>(new Table(new GroupsTableFields(), "Groups"), "LocalDB");

        public static User User { get; private set; } = new User("Vidzhel", "MyPass", "MyEmail", "MyBio", "+2", "false", null, new List<int>(){1},0);
        public static Contact Contact => new Contact(User.UserName, User.Email, User.Bio, User.ProfilePhoto, User.Online, User.Id);

        public static bool ServerConnected { get; set; }

        static ClientCommandChain commandChain = new ClientCommandChain();


        #endregion

        #region Constructor

        static UnitOfWork()
        {

            //Setup event data changed handler for tables
            MessagesTableRepo.AddDataChangedHandler((sender, args) => OnMessagesTableChanged(sender, args));
            ContactsTableRepo.AddDataChangedHandler((sender, args) => OnContactsTableChanged(sender, args));

            //Setup handler on connection changed
            AsynchronousServerConnection.AddConnectionChanged((sender, args) => OnServerConnectionChanged(sender, args));

            //Add handler on new server command received e.g. new message
            commandChain.OnNewIncomingCommand((sender, args) => OnServerCommand(sender, args));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Notify about Connection to the server changed
        /// </summary>
        /// <param name="handler"></param>
        public static void AddConnectionChangedHandler(EventHandler<bool> handler)
        {
            AsynchronousServerConnection.AddConnectionChanged(handler);
        }

        public static void OnUserUpdated(object sender, DataChangedArgs<IEnumerable<object>> args)
        {
            UserInfoUpdated.Invoke(sender, args);
        }

        /// <summary>
        /// Notify about user data changed
        /// </summary>
        /// <param name="handler"></param>
        public static void AddUserInfoUpdatedHandler(EventHandler<DataChangedArgs<IEnumerable<object>>> handler)
        {
            UserInfoUpdated += handler;
        }

        /// <summary>
        /// Gets users data from local db and server
        /// </summary>
        /// <param name="id">copy of list(items will be delated)</param>
        /// <returns></returns>
        public static List<Contact> GetUsersInfo(List<int> id)
        {
            List<Contact> users = new List<Contact>();
            Contact temp;

            //Add yourself
            if (id.Contains(User.Id))
            {
                users.Add(new Contact(User.UserName, User.Email, User.Bio, User.ProfilePhoto, User.Online, User.Id));
                id.Remove(User.Id);
            }

            //Check local data base for the contacts
            for (int i = 0; i < id.Count; i++)
            {
                //Try to find users in local data base
                temp = ContactsTableRepo.FindFirst(ContactsTableFields.Id.ToString(), id[i].ToString());


                //If we found user, than remove from id list and add to user list
                if(temp != null)
                {
                    id.RemoveAt(i);
                    users.Add(temp);
                }
            }

            //If we didn't find all users
            if (id.Count != 0 && ServerConnected)
            {

                //Make request to server and get response command
                var res = commandChain.MakeRequest(CommandType.GetUsersInfo, id, User);

                users.AddRange((List<Contact>)res.RequestData);

            }

            return users;
        }

        /// <summary>
        /// Gets groups data from local db and server
        /// </summary>
        /// <param name="id">copy of list(items will be delated)</param>
        /// <returns></returns>
        public static List<Group> GetGroupsInfo(List<int> id)
        {
            List<Group> groups = new List<Group>();
            Group temp;

            //Check local data base for the contacts
            for (int i = 0; i < id.Count; i++)
            {
                //Try to find users in local data base
                temp = GroupsTableRepo.FindFirst(ContactsTableFields.Id.ToString(), id[i].ToString());


                //If we found user, than remove from id list and add to user list
                if (temp != null)
                {
                    id.RemoveAt(i);
                    groups.Add(temp);
                }

            }

            //If we didn't find all users
            if (id.Count != 0 && ServerConnected)
            {

                //Make request to server and get response command
                var res = commandChain.MakeRequest(CommandType.GetGroupsInfo, id, User);

                groups.AddRange((List<Group>)res.RequestData);

            }

            return groups;
        }

        /// <summary>
        /// Gets groups data from server
        /// </summary>
        /// <param name="id">whose groups to search</param>
        /// <returns></returns>
        public static List<Group> GetUserGroupsInfo(Contact user)
        {
            if (!ServerConnected)
                return null;


            var res = commandChain.MakeRequest(CommandType.GetUserGroupsInfo, user, User);

            return (List<Group>)res.RequestData;
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
        /// Update ServerConnected property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnServerConnectionChanged(object sender, bool args)
        {
            ServerConnected = args;
        }

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
