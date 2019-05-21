using CommonLibs.Data;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using System.Collections.Generic;
using ClientLibs.Core.ConnectionToServer;
using System;
using System.Linq;

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

        public static User User { get; private set; } = new User("Vidzhel", "MyPass", "MyEmail", "MyBio", "+2", "false", new List<int>() { 4, 2, 3, -1}, new List<int>(){1},0);
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

        #endregion

        #region Server Requests

        /// <summary>
        /// Sends command to leave a group
        /// </summary>
        /// <param name="group"></param>
        public static void LeaveGroup(Group group)
        {

            var res = commandChain.MakeRequest(CommandType.LeaveGroup, group, User);

            if ((bool)res.RequestData)
            {
                //Delete group
                GroupsTableRepo.Remove(GroupsTableFields.Id.ToString(), group.Id.ToString());
            }
        }

        /// <summary>
        /// Sends command to make user an admin
        /// </summary>
        /// <param name="group"></param>
        /// <param name="userToAdd"></param>
        public static void MakeAdmin(Group group, Contact userToAdd)
        {
            if (ServerConnected)
            {
                var res = commandChain.MakeRequest(CommandType.MakeAdmin, new object[] { group, userToAdd }, User);

                if (!(bool)res.RequestData) ;
                //TODO popup message
            }
            else;
                //TODO popup message
        }

        /// <summary>
        /// Sends command to join the group
        /// </summary>
        /// <param name="group"></param>
        public static void JoinGroup(Group group)
        {
            if (ServerConnected)
            {
                //Join the group
                group.AddNewMember(User.Id);

                //send update group command
                var res = commandChain.MakeRequest(CommandType.UpdateGroup, group, User);

                if((bool)res.RequestData == true)
                {
                    //Add group to db
                    GroupsTableRepo.Add(group);

                }

            }
        }

        /// <summary>
        /// Sends command to create the group
        /// </summary>
        /// <param name="group"></param>
        public static void CreateGroup(Group group)
        {
            if (ServerConnected)
            {
                var res = commandChain.MakeRequest(CommandType.CreateGroup, group, User);

                //If group created, add to local repo
                if (res.RequestData != null)
                {
                    GroupsTableRepo.Add((Group)res.RequestData);

                }

            }
        }

        /// <summary>
        /// Sends command to the server
        /// </summary>
        /// <param name="group"></param>
        public static void RemoveGroup(Group group)
        {
            if(ServerConnected)
                commandChain.SendCommand(CommandType.RemoveGroup, group, User);
        }

        public static void RemoveUserFromChat(Group group, Contact user)
        {
            if (ServerConnected)
                commandChain.SendCommand(CommandType.DeleteMemberFromGroup, new object[] { group, user }, User);
                
        }

        /// <summary>
        /// Search users and groups in db than make request to server and return all search mathes
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public static object Search(string searchRequest)
        {
            var users = new List<Contact>();
            var groups = new List<Group>();

            //Get all contacts and search there
            foreach (var item in ContactsTableRepo.GetAll())
            {
                //If user name or email contains searchRequest, than add
                if (item.UserName.Contains(searchRequest) || item.Email.Contains(searchRequest))
                    users.Add(item);
            }

            //Get all groups and search there
            foreach (var item in GroupsTableRepo.GetAll())
            {
                //If group name contanis searchRequest, than add
                if (item.Name.Contains(searchRequest))
                    groups.Add(item);
            }


            //If server connected make request 
            if (ServerConnected)
            {
                var res = commandChain.MakeRequest(CommandType.SearchRequest, searchRequest, User);

                //Get data from response
                var serverUsers = (List<Contact>)((object[])res.RequestData)[0];
                var serverGroups = (List<Group>)((object[])res.RequestData)[1];

                //Find diferences and add to main lists
                users = (List<Contact>)serverUsers.Except(users);
                groups = (List<Group>)serverGroups.Except(groups);
            }

            return new object[] { users, groups };
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
                    i--;
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
            var res = commandChain.MakeRequest(CommandType.SignUp, null, userData);

            //If registered
            if ((bool)res.RequestData)
            {
                //Update user
                User = res.UserData;
                OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(null, null, RepositoryActions.Add));

                return true;
            }

            return false;

        }
        
        public static bool SyncData()
        {
            return false;
        }

        public static void SendMessage(Message mess)
        {
            //Add message
            MessagesTableRepo.Add(mess);

            var res = commandChain.MakeRequest(CommandType.SendMesssage, mess, User);

            if(res.RequestData != null)
            {

                //Update message
                MessagesTableRepo.Update(MessagesTableFields.Date.ToString(), mess.Date, (Message)res.RequestData);

            }

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

        #region Messages table Changed

        /// <summary>
        /// Do some stuff on message table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnMessagesTableChanged(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Add:
                    OnNewMessageAdded((List<Message>)args.Data);
                    break;
                case RepositoryActions.Update:
                    OnMessageUpdated((List<Message>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    OnMessageRemoved((List<Message>)args.Data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sends remove message commad to the server
        /// </summary>
        /// <param name="data"></param>
        private static void OnMessageRemoved(List<Message> data)
        {
            commandChain.SendCommand(CommandType.RemoveMessage, data, User);
        }

        /// <summary>
        /// Sends update message command to the server
        /// </summary>
        /// <param name="data"></param>
        private static void OnMessageUpdated(List<Message> data)
        {
            commandChain.SendCommand(CommandType.UpdateMessage, data, User);
        }

        /// <summary>
        /// Sends mesages to the server
        /// </summary>
        /// <param name="data"></param>
        private static void OnNewMessageAdded(List<Message> data)
        {
            commandChain.SendCommand(CommandType.SendMesssage, data, User);
        }

        #endregion

        #region Groups table Changed

        /// <summary>
        /// Do some stuff on groups table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnGroupTableChanged(object sender, DataChangedArgs<IEnumerable<Contact>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Update:
                    OnGroupUpdated((List<Group>)args.Data);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Sends command to update the group
        /// </summary>
        /// <param name="data"></param>
        private static void OnGroupUpdated(List<Group> data)
        {
            commandChain.SendCommand(CommandType.UpdateGroup, data, User);
        }
        

        #endregion

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
