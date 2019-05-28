﻿using Common.Data.Security;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using ServerLibs.ConnectionToClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ServerLibs.DataAccess
{

    /// <summary>
    /// Provide acces to server local repositories
    /// </summary>
    public static class UnitOfWork
    {
        public static DBRepositoryRemote Database = new DBRepositoryRemote();

        public static List<Client> OnlineClients = new List<Client>();

        public static ServerCommandChain CommandChain = new ServerCommandChain();

        #region Constructor

        static UnitOfWork()
        {
            //Set up event data changed handler for tables
            Database.MessagesTableRepo.AddDataChangedHandler( (sender, args) => OnMessagesTableChanged(sender, args));
            Database.UsersTableRepo.AddDataChangedHandler( (sender, args) => OnUsersTableChanged(sender, args));
            Database.GroupsTableRepo.AddDataChangedHandler( (sender, args) => OnGroupsTableChanged(sender, args));

            //Add handlers on client connected and disconnected from the server
            AsynchronousClientListener.OnUserConnected(OnUserConnected);
            AsynchronousClientListener.OnUserDisconnected(OnUserDisconnected);

            //Add handler on new server command received e.g. new message
            CommandChain.OnNewClientCommand((sender, args) => OnClientCommand(sender, args));
        }

        #endregion

        #region Event Hendlers

        static void OnUserDisconnected(Client client)
        {
            OnlineClients.Remove(client);
        }

        static void OnUserConnected(Client client)
        {
            OnlineClients.Add(client);
        }

        /// <summary>
        /// Hande commands from server e.g. new message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="com">command to handle</param>
        static void OnClientCommand(object sender, ClientCommand com)
        {
            switch (com.Command.CommandType)
            {
                case CommandType.SignUp:
                    CommandChain.SendResponseCommand(new ClientCommand( signUp(com.Command), com.Client ));
                    break;
                case CommandType.SignIn:
                    CommandChain.SendResponseCommand(new ClientCommand( signIn(com.Command), com.Client ));
                    break;
                case CommandType.CreateGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( createGroup(com.Command), com.Client));
                    break;
                case CommandType.RemoveGroup:
                    removeGroup(com.Command);
                    break;
                case CommandType.JoinGroup:
                    CommandChain.SendResponseCommand( new ClientCommand(joinGroup(com.Command), com.Client));
                    break;
                case CommandType.MakeAdmin:
                    CommandChain.SendResponseCommand( new ClientCommand(makeAdmin(com.Command), com.Client));
                    break;
                case CommandType.LeaveGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( leaveFromGroup(com.Command), com.Client) );
                    break;
                case CommandType.DeleteMemberFromGroup:
                    deleteMemberFromGroup(com.Command);
                    break;
                case CommandType.SearchRequest:
                    CommandChain.SendResponseCommand( new ClientCommand(searchRequest(com.Command), com.Client));
                    break;
                case CommandType.UpdateGroup:
                    updateGroup(com.Command);
                    break;
                case CommandType.GetGroupsInfo:
                    CommandChain.SendResponseCommand(new ClientCommand(getGroupsInfo(com.Command), com.Client));
                    break;
                case CommandType.GetUserGroupsInfo:
                    CommandChain.SendResponseCommand(new ClientCommand(getUserGroupsInfo(com.Command), com.Client));
                    break;
                case CommandType.GetUsersInfo:
                    CommandChain.SendResponseCommand(new ClientCommand(getUsersInfo(com.Command), com.Client));
                    break;
                case CommandType.UpdateMessage:
                    updateMessage(com.Command);
                    break;
                case CommandType.RemoveMessage:
                    deleteMessage(com.Command);
                    break;
                case CommandType.SendFile:
                    sendFile(com.Command);
                    break;
                case CommandType.UpdateUserInfo:
                    CommandChain.SendResponseCommand(new ClientCommand(updateUserInfo(com.Command), com.Client));
                    break;
                case CommandType.UpdateUserPassword:
                    CommandChain.SendResponseCommand(new ClientCommand(updateUserPassword(com.Command), com.Client));
                    break;
                case CommandType.SendMesssage:
                    CommandChain.SendResponseCommand(new ClientCommand( sendMessage(com.Command), com.Client));
                    break;
                case CommandType.DeleteUser:
                    CommandChain.SendResponseCommand(new ClientCommand( deleteUser(com.Command), com.Client));
                    break;
                case CommandType.SignOut:
                    signOut(com.Command);
                    break;
                case CommandType.Answer:
                    break;
                default:
                    break;
            }
        }

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
                    OnMessageAdded((List<Message>)args.Data);
                    break;
                case RepositoryActions.Update:
                    OnMessageUpdated((List<Message>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    break;
                default:
                    break;
            }
        }

        private static void OnMessageUpdated(List<Message> data)
        {
            //Notify online users
            foreach (var client in OnlineClients)
            {
                foreach (var message in data)
                {
                    //If user have the group
                    if (client.UserInfo.chatsIdList.Contains(message.ReceiverId))
                    {
                        //Send command
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.UpdateMessage, message, null), client));
                    }
                }
            }
        }

        private static void OnMessageAdded(List<Message> data)
        {
            //Notify online users
            foreach (var client in OnlineClients)
            {
                foreach (var message in data)
                {
                    //Don't send message to sender
                    if (client.UserInfo.Id == message.SenderId)
                        return;

                    //If user have the group
                    if (client.UserInfo.chatsIdList.Contains(message.ReceiverId))
                    {
                        //Send command
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.SendMesssage, message, null), client));
                    }
                }
            }
        }

        /// <summary>
        /// Do some stuff on users table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnUsersTableChanged(object sender, DataChangedArgs<IEnumerable<User>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Update:
                    OnUserUpdated((List<User>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    OnUserRemoved((List<User>)args.Data);
                    break;
                default:
                    break;
            }
        }

        private static void OnUserRemoved(List<User> data)
        {
            //Notify online users
            foreach (var client in OnlineClients)
            {
                foreach (var user in data)
                {
                    //If user have the contact
                    if (client.UserInfo.contactsIdList.Contains(user.Id))
                    {
                        //Send command
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.DeleteUser, user, null), client));
                    }
                }
            }
        }

        private static void OnUserUpdated(List<User> data)
        {

            //Notify online users
            foreach (var client in OnlineClients)
            {
                foreach (var user in data)
                {
                    //If user have the contact
                    if (client.UserInfo.contactsIdList.Contains(user.Id))
                    {
                        //Update client
                        client.UserInfo = user;

                        //Send command
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.UpdateUserInfo, user, null), client));
                    }
                }
            }
        }

        /// <summary>
        /// Do some stuff on groups table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnGroupsTableChanged(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Update:
                    OnGroupUpdated((List<Group>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    OnGroupRemoved((List<Group>)args.Data);
                    break;
                default:
                    break;
            }
        }

        private static void OnGroupRemoved(List<Group> data)
        {
            //Notify online users
            foreach (var client in OnlineClients)
            {
                foreach (var group in data)
                {
                    //If user have the group
                    if (client.UserInfo.chatsIdList.Contains(group.Id))
                    {
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.RemoveGroup, group, null), client));
                    }
                }
            }
        }

        private static void OnGroupUpdated(List<Group> data)
        {
            //Notify online users
            foreach (var client in OnlineClients)
            {
                foreach (var group in data)
                {
                    //If user have the group
                    if (client.UserInfo.chatsIdList.Contains(group.Id))
                    {
                        //Send command
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.UpdateGroup, group, null), client));
                    }
                }
            }
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Creates file Saved Files in folder and adds to database
        /// </summary>
        /// <param name="command"></param>
        static void sendFile(Command command)
        {
            var fileName = (string)((object[])command.RequestData)[0];
            var fileContent = (byte[])((object[])command.RequestData)[1];
            var fileChecksum = DataConverter.CalculateChecksum(fileContent);

            //Dont create file if it alrady exist
            if (Database.FilesTableRepo.IsExists(FilesTableFields.Checksum.ToString(), fileChecksum))
                Database.FilesTableRepo.Add(new CommonLibs.Data.File(fileName, fileChecksum));

            //Check folders
            FileManager.CheckServerRequiredFolders();

            //Create file
            var filePath = Directory.GetCurrentDirectory() + @"\Reply Messenger Server\Saved Files" + fileChecksum;
            System.IO.File.Create(filePath);

            //Write data into the file
            var fs = new FileInfo(filePath).OpenWrite();
            fs.Write(fileContent, 0, fileContent.Length);

            //Add to database
            Database.FilesTableRepo.Add(new CommonLibs.Data.File(fileName, fileChecksum));
        }

        /// <summary>
        /// Updates user and return request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command updateUserInfo(Command command)
        {
            var user = command.UserData;

            //Get old user info
            var oldUserInfo = Database.UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), user.Id.ToString());

            //Check if new email doesn't exist in the table
            if (oldUserInfo.Email != user.Email)
                if (Database.UsersTableRepo.IsExists(RemoteUsersTableFields.Email.ToString(), user.Email)) 
                    return new Command(CommandType.Answer, "A User with same email already exist", null);

            var updated = Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

            if (updated)
                return new Command(CommandType.Answer, null, null);

            return new Command(CommandType.Answer, "Something went wrong", null);
        }

        /// <summary>
        /// Updates user and return request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command updateUserPassword(Command command)
        {
            var user = command.UserData;
            var passwords = (string[])command.RequestData;

            var oldPass = passwords[0];
            var newPass = passwords[1];


            //Get old user info
            var oldUserInfo = Database.UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), user.Id.ToString());

            //If old password doesn't match
            if (oldUserInfo.Password != oldPass)
                return new Command(CommandType.Answer, "Wrong old password", null);

            user.Password = newPass;

            var updated = Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

            if (updated)
                return new Command(CommandType.Answer, null, null);

            return new Command(CommandType.Answer, "Something went wrong", null);
        }

        /// <summary>
        /// Get required groups from db
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command getGroupsInfo(Command command)
        {
            //Get users id
            var groupsId = (List<int>)command.RequestData;

            var groups = new List<Group>();
            
            //Get all required groups
            foreach (var id in groupsId)
            {
                var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());

                if (group != null)
                    groups.Add(group);
            }

            return new Command(CommandType.Answer, groups, null);
        }

        /// <summary>
        /// Get required users from db
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command getUsersInfo(Command command)
        {
            //Get users id
            var usersId = (List<int>)command.RequestData;

            var users = new List<Contact>();

            //Get all required groups
            foreach (var id in usersId)
            {
                var user = Database.UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), id.ToString());

                if (user != null)
                    users.Add(user);
            }

            return new Command(CommandType.Answer, users, null);
        }

        /// <summary>
        /// Get required user's groups from db
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command getUserGroupsInfo(Command command)
        {
            //Get users id
            var contact = (Contact)command.RequestData;

            //Find full user info
            var user = Database.UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), contact.Id.ToString());

            var groups = new List<Group>();

            //If there isn't any chat 
            if(user == null || user.chatsIdList == null)
                return new Command(CommandType.Answer, groups, null);

            //Get all required groups
            foreach (var id in user.chatsIdList)
            {
                var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());

                if (group != null)
                    groups.Add(group);
            }

            return new Command(CommandType.Answer, groups, null);
        }

        /// <summary>
        /// Find all groups and users which mutch a search request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command searchRequest(Command command)
        {
            var text = (string)command.RequestData;
            var matchUsers = new List<Contact>();
            var matchGroups = new List<Group>();

            //Get all groups and users
            var groups = Database.GroupsTableRepo.GetAll();
            var users = Database.UsersTableRepo.GetAll();

            //If name of group contains seaarch request string, than add to match groups
            foreach (var group in groups)
                if (group.Name.Contains(text))
                    matchGroups.Add(group);

            //If name of user or his email contains seaarch request string, than add to match users
            foreach (var user in users)
                if (user.UserName.Contains(text))
                    matchUsers.Add(user);

            return new Command(CommandType.Answer, new object[] { matchUsers, matchGroups }, null);
        }

        /// <summary>
        /// Updates group and notify members
        /// </summary>
        /// <param name="command"></param>
        private static void updateGroup(Command command)
        {
            var group = (Group)command.RequestData;

            //Update group
            Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);

        }

        /// <summary>
        /// Adds user to the group admins and send response
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static Command makeAdmin(Command command)
        {
            var group = (Group)((object[])command.RequestData)[0];
            var userToAdd = (Contact)((object[])command.RequestData)[1];


            //Set group last updete time
            group.LocalLastTimeUpdated = DateTime.Now;

            //Update group
            group.AddNewAdmin(userToAdd.Id);
            var updated = Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);

            if (updated)
            {

                return new Command(CommandType.Answer, true, null);
            }

            return new Command(CommandType.Answer, false, null);
        }

        /// <summary>
        /// Remove all members and update
        /// </summary>
        /// <param name="command"></param>
        private static void removeGroup(Command command)
        {
            //Get group
            var group = (Group)command.RequestData;

            //Make private
            group.isPrivate = true;

            //Delete all admins and members
            group.AdminsId = "";
            group.MembersId = "";

            //Update db
            Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);
        }

        /// <summary>
        /// Refister user and send response
        /// </summary>
        static Command signUp(Command com)
        {
            //Get a user info
            User user = com.UserData;

            //Check if the user with same email already exist
            if (!Database.UsersTableRepo.IsExists(UsersTableFields.Email.ToString(), user.Email))
            {
                //Add to table
                var added = Database.UsersTableRepo.Add(user);
                if(added)
                    return new Command(CommandType.Answer, true, Database.UsersTableRepo.FindFirst(UsersTableFields.Email.ToString(), user.Email));
            }

            //else return error answer
            return new Command(CommandType.Answer, false, null);
        }

        /// <summary>
        /// Helps check and sign in client
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command signIn(Command com)
        {
            //Get user data
            var user = com.UserData;

            //Find user with same email
            var findUser = Database.UsersTableRepo.FindFirst(UsersTableFields.Email.ToString(), user.Email);

            //If the user doesn't exist
            if (findUser == null)
                return new Command(CommandType.Answer, null, null);

            //Check Passwords
            if (user.Password == findUser.Password)
            {
                //Update user status to Online

                var syncData = synchronize(findUser);

                findUser.Online = "true";
                findUser.LocalLastTimeUpdated = DateTime.Now;
                var updated = Database.UsersTableRepo.Update(UsersTableFields.Email.ToString(), findUser.Email, findUser);

                var groupsId = findUser.chatsIdList;

                //update groups
                foreach (var id in groupsId)
                {
                    var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());
                    group.UsersOnline += 1;
                    group.LocalLastTimeUpdated = DateTime.Now;
                    Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), id.ToString(), group);
                }

                if (updated)
                    return new Command(CommandType.Answer, syncData, findUser);
                else
                    return new Command(CommandType.Answer, null, null);
            }
            else
                return new Command(CommandType.Answer, null, null);
        }

        static object[] synchronize(User user)
        {
            //Get all user groups and contacts
            var userGroups = user.chatsIdList;
            var userContacts = user.contactsIdList;

            var groupsInfo = new List<Group>();
            var contactsInfo = new List<Contact>();
            var messagesInfo = new List<Message>();

            //get groups info
            foreach (var groupId in userGroups)
            {
                var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), groupId.ToString());
                var messages = Database.MessagesTableRepo.Find(MessagesTableFields.ReceiverId.ToString(), groupId.ToString());

                if (group != null)
                    groupsInfo.Add(group);

                if (messages != null)
                    messagesInfo.AddRange(messages);
            }

            //get contacts info
            foreach (var userId in userContacts)
            {
                var contact = Database.UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), userId.ToString());

                if (contact != null)
                    contactsInfo.Add(contact);
            }

            return new object[]{groupsInfo, contactsInfo, messagesInfo};
        }

        /// <summary>
        /// Creates group and return it
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command createGroup(Command com)
        {
            var group = (Group)com.RequestData;

            var added = Database.GroupsTableRepo.Add(group);

            //If added, return command with group data to client(with group Id)
            if (added)
            {
                //Update user
                var user = com.UserData;

                //Get new id of the group
                group = Database.GroupsTableRepo.GetLast();
                user.AddNewChat(group);
                Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

                var newGroup = Database.GroupsTableRepo.FindLast(GroupsTableFields.Name.ToString(), group.Name);
                return new Command(CommandType.Answer, newGroup, user);
            }

            return new Command(CommandType.Answer, null, null);
        }

        /// <summary>
        /// Helps connect to group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command joinGroup(Command com)
        {
            //Get group
            var group = (Group)com.RequestData;

            //Get user info
            var user = com.UserData;

            //Get the latest version of the group and update
            group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), group.Id.ToString());
            group.AddNewMember(user.Id);
            group.UsersOnline += 1;


            //Set group last updete time
            group.LocalLastTimeUpdated = DateTime.Now;

            //Update user
            user.AddNewChat(group);
            Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);


            //update table
            var updated = Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);

            if (updated)
            {
                //return response
                return new Command(CommandType.Answer, true, user);
            }

            return new Command(CommandType.Answer, false, null);
        }
        
        /// <summary>
        /// Helps disconnect member from group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command leaveFromGroup(Command com)
        {
            //Get group
            var group = (Group)com.RequestData;

            //Get User
            var user = com.UserData;

            //Remove user
            group.RemoveMember(user.Id);
            group.RemoveAdmin(user.Id);
            group.UsersOnline -= 1;

            //Set group last updete time
            group.LocalLastTimeUpdated = DateTime.Now;
            
            //Update user
            user.RemoveChat(group);
            Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);


            //If in group no more users or this is a chat with no admins delete it, else update
            if (group.MembersIdList.Count != 0 && (group.isChannel && group.AdminsIdList.Count != 0))
            {

                //Update table
                var updated = Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);

                if (updated)
                    return new Command(CommandType.Answer, true, user);

                return new Command(CommandType.Answer, false, null);
            }
            else
            {
                var removed = Database.GroupsTableRepo.Remove(GroupsTableFields.Id.ToString(), group.Id.ToString());

                //Delete all messages
                Database.MessagesTableRepo.Remove(MessagesTableFields.ReceiverId.ToString(), group.Id.ToString());

                if(removed)

                    return new Command(CommandType.Answer, true, user);

                return new Command(CommandType.Answer, false, null);
            }
        }

        /// <summary>
        /// Helps delete user from group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static void deleteMemberFromGroup(Command com)
        {
            //Get info
            var group = (Group)((object[])com.RequestData)[0];
            var userToDelate = (Contact)((object[])com.RequestData)[1];


            //Set group last updete time
            group.LocalLastTimeUpdated = DateTime.Now;

            //Delete from online users
            if (userToDelate.Online == "true")
                group.UsersOnline -= 1;

            //Remove user
            group.RemoveMember(userToDelate.Id);
            group.RemoveAdmin(userToDelate.Id);


            //Set group last updete time
            group.LocalLastTimeUpdated = DateTime.Now;

            //Update group
            var updated = Database.GroupsTableRepo.Update(UsersTableFields.Id.ToString(), group.Id.ToString(), group);


            if (updated)
            {
                //Get user info
                var user = Database.UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), userToDelate.Id.ToString());
                user.RemoveChat(group);

                //Update user data
                Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);
            }
        }

        static Command sendMessage(Command com)
        {
            var message = (Message)com.RequestData;

            //Update message status
            message.Status = MessageStatus.Sended;

            var added = Database.MessagesTableRepo.Add(message);

            if (added)
            {
                //Get group
                var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), message.ReceiverId.ToString());


                return new Command(CommandType.Answer, message, null);
            }

            return new Command(CommandType.Answer, null, null);
        }

        static Command deleteUser(Command com)
        {
            var user = com.UserData;

            //Update groups
            var groupsId = user.chatsIdList;

            foreach (var id in groupsId)
            {
                var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());

                group.RemoveMember(user.Id);
                group.RemoveAdmin(user.Id);
                
                //If group is chat and there isn't any admin or in the group no one member, than delete group and all messages
                if((!group.isChannel && group.AdminsIdList.Count == 0) || group.MembersIdList.Count == 0)
                {
                    Database.GroupsTableRepo.Remove(GroupsTableFields.Id.ToString(), group.Id.ToString());
                    Database.MessagesTableRepo.Remove(MessagesTableFields.ReceiverId.ToString(), group.Id.ToString());
                }

                //Otherwise update
                else
                    Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), id.ToString(), group);
            }

            var res = Database.UsersTableRepo.Remove(UsersTableFields.Id.ToString(), user.Id.ToString());

            if (res)
                return new Command(CommandType.Answer, true, null);
            return new Command(CommandType.Answer, false, null);
        }

        static void updateMessage(Command com)
        {
            var mess = (Message)com.RequestData;

            mess.LocalLastTimeUpdated = DateTime.Now;

            Database.MessagesTableRepo.Update(MessagesTableFields.Id.ToString(), mess.Id.ToString(), mess);
        }

        static void deleteMessage(Command com)
        {
            var mess = (Message)com.RequestData;

            Database.MessagesTableRepo.Remove(MessagesTableFields.Id.ToString(), mess.Id.ToString());
        }
        
        static void signOut(Command com)
        {
            var user = com.UserData;

            if (user == null)
                return;

            //Update groups
            var groupsId = user.chatsIdList;

            foreach (var id in groupsId)
            {
                var group = Database.GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());
                group.UsersOnline -= 1;

                group.LocalLastTimeUpdated = DateTime.Now;
                Database.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), id.ToString(), group);
            }

            //Change user status to offline
            user.Online = "false";
            Database.UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);
        }
        #endregion
    }
}
