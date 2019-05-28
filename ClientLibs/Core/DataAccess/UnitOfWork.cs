using CommonLibs.Data;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using System.Collections.Generic;
using ClientLibs.Core.ConnectionToServer;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ClientLibs.Core.DataAccess
{

    /// <summary>
    /// Provide acces to client loacal data repositories
    /// </summary>
    public static class UnitOfWork
    {

        #region Private Members

        static event EventHandler<DataChangedArgs<IEnumerable<object>>> UserInfoUpdated;

        static DBRepositoryLocal database;

        #endregion

        #region Public Members

        /// <summary>
        /// Provide fields to get access to tables of the database
        /// </summary>
        public static DBRepositoryLocal Database
        {
            get
            {
                if (database == null)
                {
                    database = new DBRepositoryLocal();

                    //Check database file integrity
                    FileManager.DatabaseIntegrityCheck(database.ContactsTableRepo.LoadConnectionString());
                }

                return database;
            }
        }

        public static User User { get; private set; } = new User("Me", "asdsa", "email", "kek", Directory.GetCurrentDirectory() + @"\Reply Messenger\User Files\" + "ProfilePhoto.png");
        public static Contact Contact => User;

        public static bool ServerConnected { get; set; }

        static ClientCommandChain commandChain = new ClientCommandChain();


        #endregion

        #region Constructor

        static UnitOfWork()
        {


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
            UserInfoUpdated?.Invoke(sender, args);
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
        /// Make request to the server to change group info
        /// </summary>
        /// <param name="group"></param>
        public static void ChangeGroupInfo(Group group)
        {
            if(ServerConnected)
                commandChain.SendCommand(CommandType.UpdateGroup, group, User);
        }

        /// <summary>
        /// Make request to the server to update user info and return error message
        /// or null if all ok
        /// </summary>
        /// <returns></returns>
        public static async Task<string> ChangePassword(string oldPassword, string newPassword)
        {
            if (ServerConnected)
            {

                var result = await Task.Run(() =>
                {

                    var res = commandChain.MakeRequest(CommandType.UpdateUserPassword, new string[] { oldPassword, newPassword }, User);


                    //If response time is out
                    if (res == null)
                        return "Response time is out";


                    //If user updated
                    if (res.RequestData == null)
                    {
                        User.Password = newPassword;
                    }

                    return (string)res.RequestData;

                });

                return result;
            }

            return "";
        }

        /// <summary>
        /// Make request to the server to update user info and return error message
        /// or null if all ok
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<string> ChangeUserInfo(User user)
        {
            if (ServerConnected)
            {
                var result = await Task.Run(() =>
                {

                    var res = commandChain.MakeRequest(CommandType.UpdateUserInfo, null, user);

                    //If response time is out
                    if (res == null)
                        return "Response time is out";


                    //If user updated
                    if (res.RequestData == null)
                    {
                        User = user;
                        OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<User>() { user }, null, RepositoryActions.Update));
                    }

                    return (string)res.RequestData;
                });

                return result;
            }

            return "";
        }

        /// <summary>
        /// Sends command to leave a group
        /// </summary>
        /// <param name="group"></param>
        public static async Task LeaveGroup(Group group)
        {
            if (ServerConnected)
            {
                await Task.Run(() =>
                {
                    var res = commandChain.MakeRequest(CommandType.LeaveGroup, group, User);

                    //If response time is out
                    if (res == null)
                        return;

                    if ((bool)res.RequestData)
                    {
                        User = res.UserData;
                        OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<object>() { group.Id }, UsersTableFields.ChatsId.ToString(), RepositoryActions.Remove));

                        //Delete group
                        Database.GroupsTableRepo.Remove(GroupsTableFields.Id.ToString(), group.Id.ToString());

                        //Find and delete all messages from the group
                        Database.MessagesTableRepo.Remove(MessagesTableFields.ReceiverId.ToString(), group.Id.ToString());
                    }
                });
            }
        }

        /// <summary>
        /// Sends command to make user an admin
        /// </summary>
        /// <param name="group"></param>
        /// <param name="userToAdd"></param>
        public static async Task MakeAdmin(Group group, Contact userToAdd)
        {
            if (ServerConnected)
            {
                await Task.Run(() =>
                {

                    var res = commandChain.MakeRequest(CommandType.MakeAdmin, new object[] { group, userToAdd }, User);


                    //If response time is out
                    if (res == null)
                        return;


                    if (!(bool)res.RequestData)
                        ;
                    //TODO popup message
                });
            }
                //TODO popup message
        }

        /// <summary>
        /// Sends command to join the group
        /// </summary>
        /// <param name="group"></param>
        public static async Task JoinGroup(Group group)
        {
            if (ServerConnected)
            {
                await Task.Run(() =>
                {

                    //send update group command
                    var res = commandChain.MakeRequest(CommandType.JoinGroup, group, User);


                    //If response time is out
                    if (res == null)
                        return;


                    if ((bool)res.RequestData)
                    {

                        User = res.UserData;
                        OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<object>() { group.Id }, UsersTableFields.ChatsId.ToString(), RepositoryActions.Add));

                    }
                });

            }
        }

        /// <summary>
        /// Sends command to create the group
        /// </summary>
        /// <param name="group"></param>
        public static async Task CreateGroup(Group group)
        {
            if (ServerConnected)
            {
                await Task.Run(() =>
                {

                    var res = commandChain.MakeRequest(CommandType.CreateGroup, group, User);

                    //If response time is out
                    if (res == null)
                        return;


                    //If group created, add to local repo
                    if (res.RequestData != null)
                    {
                        //Add group to db
                        Database.GroupsTableRepo.Add((Group)res.RequestData);

                        //Update user
                        User = res.UserData;
                        OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<object>() { ((Group)res.RequestData).Id }, UsersTableFields.ChatsId.ToString(), RepositoryActions.Add));
                    }
                });

            }
        }

        /// <summary>
        /// Sends command to the server
        /// </summary>
        /// <param name="group"></param>
        public static async Task RemoveGroup(Group group)
        {
            if (ServerConnected)
                await Task.Run(() =>
                {

                    commandChain.SendCommand(CommandType.RemoveGroup, group, User);
                });
        }

        /// <summary>
        /// Removes user from chat
        /// </summary>
        /// <param name="group"></param>
        /// <param name="user"></param>
        public static async Task RemoveUserFromChat(Group group, Contact user)
        {
            if (ServerConnected)
                await Task.Run(() =>
                {

                    commandChain.SendCommand(CommandType.DeleteMemberFromGroup, new object[] { group, user }, User);
                });
                
        }

        /// <summary>
        /// Search users and groups in db than make request to server and return all search mathes
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public static async Task<object> Search(string searchRequest)
        {

            var result = await Task.Run(() =>
            {

                var users = new List<Contact>();
                var groups = new List<Group>();

                //Get all contacts and search there
                foreach (var item in Database.ContactsTableRepo.GetAll())
                {
                    //If user name or email contains searchRequest, than add
                    if (item.UserName.Contains(searchRequest) || item.Email.Contains(searchRequest))
                        users.Add(item);
                }

                //Get all groups and search there
                foreach (var item in Database.GroupsTableRepo.GetAll())
                {
                    //If group name contanis searchRequest, than add
                    if (item.Name.Contains(searchRequest))
                        groups.Add(item);
                }


                //If server connected make request 
                if (ServerConnected)
                {
                    var res = commandChain.MakeRequest(CommandType.SearchRequest, searchRequest, User);


                    //If response time is out
                    if (res == null)
                        return new object[] { users, groups };


                    //Get data from response
                    var serverUsers = (List<Contact>)((object[])res.RequestData)[0];
                    var serverGroups = (List<Group>)((object[])res.RequestData)[1];

                    //Find diferences and add to main lists
                    users = (List<Contact>)serverUsers.Except(users).ToList();

                    groups = (List<Group>)serverGroups.Except(groups).ToList();
                }

                return new object[] { users, groups };
            });

            return result;
        }

        /// <summary>
        /// Gets users data from local db and server
        /// </summary>
        /// <param name="id">copy of list(items will be delated)</param>
        /// <returns></returns>
        public static async Task<List<Contact>> GetUsersInfo(List<int> id)
        {

            List<Contact> users = new List<Contact>();
            Contact temp;


            if (!ServerConnected)
                return users;

            var result = await Task.Run(() =>
            {

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
                    temp = Database.ContactsTableRepo.FindFirst(ContactsTableFields.Id.ToString(), id[i].ToString());


                    //If we found user, than remove from id list and add to user list
                    if (temp != null)
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

                    //If response time is out
                    if (res == null)
                        return users;

                    users.AddRange((List<Contact>)res.RequestData);

                }

                return users;

            });

            return result;
        }

        /// <summary>
        /// Gets groups data from local db and server
        /// </summary>
        /// <param name="id">copy of list(items will be delated)</param>
        /// <returns></returns>
        public static async Task<List<Group>> GetGroupsInfo(List<int> id)
        {
            List<Group> groups = new List<Group>();
            Group temp;

            if (!ServerConnected)
                return groups;

            var result = await Task.Run(() =>
            {


                //Check local data base for the contacts
                for (int i = 0; i < id.Count; i++)
                {
                    //Try to find users in local data base
                    temp = Database.GroupsTableRepo.FindFirst(ContactsTableFields.Id.ToString(), id[i].ToString());


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
                    
                    //If response time is out
                    if (res == null)
                        return groups;

                    groups.AddRange((List<Group>)res.RequestData);

                }

                return groups;
            });

            return result;
        }

        /// <summary>
        /// Gets groups data from server
        /// </summary>
        /// <param name="id">whose groups to search</param>
        /// <returns></returns>
        public static async Task<List<Group>> GetUserGroupsInfo(Contact user)
        {

            if (User.contactsIdList.Contains(user.Id))
            {
                //TODO end getting your contact group info
            }

            if (!ServerConnected)
                return null;

            var result = await Task.Run(() =>
            {

                var res = commandChain.MakeRequest(CommandType.GetUserGroupsInfo, user, User);

                //If response time is out
                if (res == null)
                    return new List<Group>();

                return (List<Group>)res.RequestData;

            });

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">name of file from Reply messenger\saved files</param>
        /// <returns></returns>
        public static async Task SendFile(string name)
        {
            if (!ServerConnected)
                return;

            //Return if file doesn't exist in saved files
            if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + @"\Reply Messenger\Saved Files\" + name))
                return;

            await Task.Run(() =>
            {
                // Load file meta data with FileInfo
                FileInfo fileInfo = new FileInfo(Directory.GetCurrentDirectory() + @"\Reply Messenger\Saved Files\" + name);

                // The byte[] to save the data in
                byte[] data = new byte[fileInfo.Length];

                // Load a filestream and put its content into the byte[]

                using (var fs = fileInfo.OpenRead())
                {

                    fs.Read(data, 0, data.Length);

                    commandChain.SendCommand(CommandType.SendFile, new object[] { name, data }, User);

                }


            });
        }

        /// <summary>
        /// Make Log In request to the server, return true if all ok
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static async Task<bool> SighIn(User userData)
        {
            if (!ServerConnected)
                return false;

            var result = await Task.Run(() =>
            {

                //Make response to the server
                var response = commandChain.MakeRequest(CommandType.SignIn, null, userData);

                //If response time is out
                if (response == null)
                    return false;


                // if all Ok save user data
                if (response.RequestData != null)
                {
                    User = response.UserData;

                    syncData((object[])response.RequestData);

                    return true;
                }

                return false;
            });

            return result;
        }

        /// <summary>
        /// Adds removes and updates groups
        /// </summary>
        /// <param name="syncData"></param>
        static void syncData(object[] syncData)
        {
            var groupsInfo = (List<Group>)syncData[0];
            var contactsInfo = (List<Contact>)syncData[1];
            var messagesInfo = (List<Message>)syncData[2];

            //Find differences in Last Time Update and update if necessary
            foreach (var group in groupsInfo)
            {
                var foundGroup = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), group.Id.ToString());

                if(foundGroup == null)
                {
                    Database.GroupsTableRepo.Add(group);
                    continue;
                }


                if (foundGroup.LastTimeUpdated != group.LastTimeUpdated)
                    Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);
            }

            foreach (var contact in contactsInfo)
            {
                var foundContact = Database.ContactsTableRepo.FindFirst(ContactsTableFields.Id.ToString(), contact.Id.ToString());

                if (foundContact == null)
                {
                    Database.ContactsTableRepo.Add(contact);
                    continue;
                }

                if (foundContact.LastTimeUpdated != contact.LastTimeUpdated)
                    Database.ContactsTableRepo.Update(ContactsTableFields.Id.ToString(), contact.Id.ToString(), contact);
            }

            foreach (var message in messagesInfo)
            {
                var foundMessage = Database.MessagesTableRepo.FindFirst(MessagesTableFields.Id.ToString(), message.Id.ToString());

                if (foundMessage == null)
                {
                    Database.MessagesTableRepo.Add(message);
                    continue;
                }

                if (foundMessage.LastTimeUpdated != message.LastTimeUpdated)
                    Database.MessagesTableRepo.Update(MessagesTableFields.Id.ToString(), message.Id.ToString(), message);
            }

            #region Create custom comparers

            //Create comparers
            var groupsComparer = EqualityComparerFactory.Create<Group>(
                    (Group a) => a.Id.GetHashCode(),
                    (Group a, Group b) => a.Id == b.Id
                );

            var contactsComparer = EqualityComparerFactory.Create<Contact>(
                    (Contact a) => a.Id.GetHashCode(),
                    (Contact a, Contact b) => a.Id == b.Id
                );
            var messagesComparer = EqualityComparerFactory.Create<Message>(
                    (Message a) => a.Id.GetHashCode(),
                    (Message a, Message b) => a.Id == b.Id
                );

            #endregion

            //Delete 
            var deletedGroups = Database.GroupsTableRepo.GetAll().Except(groupsInfo, groupsComparer);
            var deletedContacts = Database.ContactsTableRepo.GetAll().ToList().Except(contactsInfo, contactsComparer);
            var deletedMessages = Database.MessagesTableRepo.GetAll().ToList().Except(messagesInfo, messagesComparer);

            foreach (var group in deletedGroups)
            {
                Database.GroupsTableRepo.Remove(GroupsTableFields.Id.ToString(), group.Id.ToString());
            }

            foreach (var contact in deletedContacts)
            {
                Database.ContactsTableRepo.Remove(GroupsTableFields.Id.ToString(), contact.Id.ToString());
            }

            foreach (var message in deletedMessages)
            {
                Database.MessagesTableRepo.Remove(GroupsTableFields.Id.ToString(), message.Id.ToString());
            }
        }

        /// <summary>
        /// Registrate user, return true if all ok
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static async Task<bool> SignUp(User userData)
        {
            if (!ServerConnected)
                return false;

            var result = await Task.Run(() =>
            {

                //Make SignUp request to the server
                var res = commandChain.MakeRequest(CommandType.SignUp, null, userData);

                //If response time is out
                if (res == null)
                    return false;

                //If registered
                if ((bool)res.RequestData)
                {
                    //Update user
                    User = res.UserData;
                    OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(null, null, RepositoryActions.Add));

                    return true;
                }

                return false;
            });

            return result;

        }

        public static async Task SendMessage(Message mess)
        {
            //Add message
            //Database.MessagesTableRepo.Add(mess);
            await Task.Run(async () =>
            {

                if (!ServerConnected)
                    return;

                var res = commandChain.MakeRequest(CommandType.SendMesssage, mess, User);


                //If response time is out
                if (res == null)
                    return;

                if (res.RequestData != null)
                {

                    if (mess.AttachmentsList.Count != 0)
                    {
                        //Load all attachments to saved files
                        FileManager.CheckClientRequiredFolders();

                        var savedFiles = Directory.GetCurrentDirectory() + @"\Reply Messenger\Saved Files";

                        foreach (var item in mess.AttachmentsList)
                        {
                            System.IO.File.Copy(item, savedFiles + @"\" + Path.GetFileName(item));
                            await SendFile(Path.GetFileName(item));
                        }
                    }

                    //Update the last message we sent
                    //Database.MessagesTableRepo.Update(MessagesTableFields.Status.ToString(), MessageStatus.SendingInProgress.ToString(), (Message)res.RequestData);
                    Database.MessagesTableRepo.Add((Message)res.RequestData);
                }
            });
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
            switch (com.CommandType)
            {
                case CommandType.UpdateMessage:
                    updateMessages((Message)com.RequestData);
                    break;
                case CommandType.SendMesssage:
                    getMessages((Message)com.RequestData);
                    break;
                case CommandType.RemoveMessage:
                    removeMessages((Message)com.RequestData);
                    break;
                case CommandType.RemoveGroup:
                    removeGroups((Group)com.RequestData);
                    break;
                case CommandType.UpdateUserInfo:
                    updateContactsList((Contact)com.RequestData);
                    break;
                case CommandType.DeleteUser:
                    removeContact((Contact)com.RequestData);
                    break;
                case CommandType.UpdateGroup:
                    updateGroupsList((Group)com.RequestData);
                    break;
                default:
                    break;
            }
        }

        private static void removeContact(Contact data)
        {
            
        }

        private static async void removeGroups(Group data)
        {
            //Remove from your accaunt
            await LeaveGroup(data);
        }

        private static void removeMessages(Message data)
        {
                //Delete from db
                Database.MessagesTableRepo.Remove(MessagesTableFields.Id.ToString(), data.Id.ToString());
        }

        private static void getMessages(Message data)
        {
                //Add to db
                Database.MessagesTableRepo.Add(data);
        }

        private static void updateMessages(Message data)
        {
                //Update db
                Database.MessagesTableRepo.Update(MessagesTableFields.Id.ToString(), data.Id.ToString(), data);
        }

        private static void updateGroupsList(Group data)
        {
            //If the group doesn't exist, than add to db
            if (!Database.GroupsTableRepo.IsExists(GroupsTableFields.Id.ToString(), data.Id.ToString()))
            {
                Database.GroupsTableRepo.Add(data);
                return;
            }

            //Update db
            Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), data.Id.ToString(), data);
        }

        private static void updateContactsList(Contact data)
        {
                //If it's you
                if (data.Id == User.Id)
                    return;

                //Update contact
                Database.ContactsTableRepo.Update(ContactsTableFields.Id.ToString(), data.Id.ToString(), data);
        }

        #endregion
    }
}
