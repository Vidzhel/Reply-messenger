using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using ServerLibs.ConnectionToClient;
using System;
using System.Collections.Generic;

namespace ServerLibs.DataAccess
{

    /// <summary>
    /// Provide acces to server local repositories
    /// </summary>
    public static class UnitOfWork
    {

        public static BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new RemoteMessagesTableFields(), "Messages"), "LocalDB");
        public static BaseRepository<User> UsersTableRepo = new Repository<User>(new Table(new RemoteUsersTableFields(), "Users"), "LocalDB");
        public static BaseRepository<Group> GroupsTableRepo = new Repository<Group>(new Table(new RemoteGroupsTableFields(), "Groups"), "LocalDB");

        public static List<Client> OnlineClients;

        public static ServerCommandChain CommandChain = new ServerCommandChain();

        #region Constructor

        static UnitOfWork()
        {
            //Set up event data changed handler for tables
            MessagesTableRepo.AddDataChangedHandler( (sender, args) => OnMessagesTableChanged(sender, args));
            UsersTableRepo.AddDataChangedHandler( (sender, args) => OnUsersTableChanged(sender, args));
            GroupsTableRepo.AddDataChangedHandler( (sender, args) => OnGroupsTableChanged(sender, args));

            //Add handler on new server command received e.g. new message
            CommandChain.OnNewClientCommand((sender, args) => OnClientCommand(sender, args));
        }

        #endregion

        #region Event Hendlers

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
                    CommandChain.SendResponseCommand(new ClientCommand(signUp(com.Command), com.Client));
                    break;
                case CommandType.UpdateUserInfo:
                    CommandChain.SendResponseCommand(new ClientCommand(signUp(com.Command), com.Client));
                    break;
                case CommandType.SendMesssage:
                    CommandChain.SendResponseCommand(new ClientCommand( sendMessage(com.Command), com.Client));
                    break;
                case CommandType.DeleteUser:
                    CommandChain.SendResponseCommand(new ClientCommand( deleteUser(com.Command), com.Client));
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

        }

        /// <summary>
        /// Do some stuff on users table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnUsersTableChanged(object sender, DataChangedArgs<IEnumerable<User>> args)
        {

        }

        /// <summary>
        /// Do some stuff on groups table data changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnGroupsTableChanged(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {

        }

        #endregion

        #region Command Helpers

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
                var group = GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());

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
                var user = UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), id.ToString());

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
            var user = UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), contact.Id.ToString());

            var groups = new List<Group>();

            //If there isn't any chat 
            if(user.chatsIdList == null)
                return new Command(CommandType.Answer, groups, null);

            //Get all required groups
            foreach (var id in user.chatsIdList)
            {
                var group = GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), id.ToString());

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
            var groups = GroupsTableRepo.GetAll();
            var users = UsersTableRepo.GetAll();

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
            GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);

            //Notify online members
            NotifyOnlineMembers(group, CommandType.UpdateGroup);
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

            //Update group
            group.AddNewAdmin(userToAdd.Id);
            var updated = GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);

            if (updated)
            {

                //Notify online members
                NotifyOnlineMembers(group, CommandType.UpdateGroup);

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

            //Send to all online members command to delete group
            foreach (var client in OnlineClients)
            {
                NotifyOnlineMembers(group, CommandType.RemoveGroup);
            }

            //Delete all admins and members
            group.AdminsId = "";
            group.MembersId = "";

            //Update db
            GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);
        }

        /// <summary>
        /// Refister user and send response
        /// </summary>
        static Command signUp(Command com)
        {
            //Get a user info
            User user = com.UserData;

            //Check if the user with same email already exist
            if (!UsersTableRepo.IsExists(UsersTableFields.Email.ToString(), user.Email))
            {
                //Add to table
                var added = UsersTableRepo.Add(user);
                if(added)
                    return new Command(CommandType.Answer, true, UsersTableRepo.FindFirst(UsersTableFields.Email.ToString(), user.Email));
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
            var findUser = UsersTableRepo.FindFirst(UsersTableFields.Email.ToString(), user.Email);

            //Check Passwords
            if (user.Password == findUser.Password)
            {
                //Update user status to Online
                //TODO check update
                var updated = UsersTableRepo.Update(UsersTableFields.Online.ToString(), "true", user);

                if(updated)
                    return new Command(CommandType.Answer, true, findUser);
                else
                    return new Command(CommandType.Answer, false, null);
            }
            else
                return new Command(CommandType.Answer, false, null);
        }

        /// <summary>
        /// Creates group and return it
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command createGroup(Command com)
        {
            var group = (Group)com.RequestData;

            var added = GroupsTableRepo.Add(group);

            //If added, return command with group data to client(with group Id)
            if (added)
            {
                //Update user
                var user = com.UserData;
                user.AddNewChat(group);
                UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

                var newGroup = GroupsTableRepo.FindLast(GroupsTableFields.Name.ToString(), group.Name);
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

            //add new member
            group.AddNewMember(user.Id);

            //update table
            var updated = GroupsTableRepo.Update(GroupsTableFields.MembersId.ToString(), group.MembersId, group);

            if (updated)
            {
                //Update user
                user.AddNewChat(group);
                UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

                //Send update command to online members
                NotifyOnlineMembers(group, CommandType.UpdateGroup);

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

            //Update table
            var updated = GroupsTableRepo.Update(GroupsTableFields.MembersId.ToString(), group.MembersId, group);

            if (updated)
            {
                //Update user
                user.RemoveChat(group);
                UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

                //Notify online members
                NotifyOnlineMembers(group, CommandType.UpdateGroup);

                return new Command(CommandType.Answer, true, null);
            }

            return new Command(CommandType.Answer, false, null);
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

            //Remove user
            group.RemoveMember(userToDelate.Id);
            group.RemoveAdmin(userToDelate.Id);

            //Update group
            var updated = GroupsTableRepo.Update(UsersTableFields.Id.ToString(), group.Id.ToString(), group);


            if (updated)
            {
                //Get user info
                var user = UsersTableRepo.FindFirst(UsersTableFields.Id.ToString(), userToDelate.Id.ToString());
                user.RemoveChat(group);

                //Update user data
                UsersTableRepo.Update(UsersTableFields.Id.ToString(), user.Id.ToString(), user);

                //Notify online members
                NotifyOnlineMembers(group, CommandType.UpdateGroup);
            }
        }

        static Command sendMessage(Command com)
        {
            var message = (Message)com.RequestData;

            //Update message status
            message.Status = MessageStatus.Sended;

            var added = MessagesTableRepo.Add(message);

            if (added)
            {
                //Get group
                var group = GroupsTableRepo.FindFirst(GroupsTableFields.Id.ToString(), message.ReceiverId.ToString());

                //Send message to all online members of the group
                foreach (var client in OnlineClients)
                    if (group.MembersIdList.Contains(client.UserInfo.Id))
                        CommandChain.SendResponseCommand(new ClientCommand(new Command(CommandType.SendMesssage, message, null), client));

                return new Command(CommandType.Answer, message, null);
            }

            return new Command(CommandType.Answer, null, null);
        }

        static Command deleteUser(Command com)
        {
            return null;
        }
        #endregion

        #region Command Helpers

        /// <summary>
        /// Sends command to online group members
        /// </summary>
        /// <param name="group"></param>
        /// <param name="commandType"></param>
        static void NotifyOnlineMembers(Group group, CommandType commandType)
        {
            //Send command to online members
            foreach (var client in OnlineClients)
            {
                if (group.MembersIdList.Contains(client.UserInfo.Id))
                    CommandChain.SendResponseCommand(new ClientCommand(new Command(commandType, group, null), client));
            }
        }

        #endregion
    }
}
