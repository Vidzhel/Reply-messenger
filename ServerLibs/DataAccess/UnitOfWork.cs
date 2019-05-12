using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using ServerLibs.ConnectionToClient;
using System.Collections.Generic;

namespace ServerLibs.DataAccess
{

    /// <summary>
    /// Provide acces to server local repositories
    /// </summary>
    public static class UnitOfWork
    {

        public static BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new RemoteMessagesTableFields(), "Messages"), "LocalDB");
        public static BaseRepository<User> UsersTableRepo = new Repository<User>(new Table(new UsersTableFields(), "Users"), "LocalDB");
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
                    CommandChain.SendResponseCommand(new ClientCommand( createGroup(com.Command), com.Client) );
                    break;
                case CommandType.ConnectToGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( connectToGroup(com.Command), com.Client) );
                    break;
                case CommandType.InviteToGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( inviteToGroup(com.Command), com.Client) );
                    break;
                case CommandType.AddAdminToGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( connectToGroup(com.Command), com.Client) );
                    break;
                case CommandType.DisconectFromGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( disconnectFromGroup(com.Command), com.Client) );
                    break;
                case CommandType.DeleteMemberFromGroup:
                    CommandChain.SendResponseCommand(new ClientCommand( deleteMemberFromGroup(com.Command), com.Client));
                    break;
                case CommandType.UpdateGroup:
                    CommandChain.SendResponseCommand(new ClientCommand(signUp(com.Command), com.Client));
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
        /// Helps register users
        /// </summary>
        static Command signUp(Command com)
        {
            //Get a user info
            User user = com.UserData;

            //Check if the user with same email already exist
            if (!UsersTableRepo.IsExists(UsersTableFields.Email.ToString(), user.Email))
            {
                //Add to tible
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
        /// Helps create new group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command createGroup(Command com)
        {
            var group = (Group)com.RequestData;

            var added = GroupsTableRepo.Add(group);

            //TODO add code to get last element from the Group table
            if (added) ;
            return null;
                //return new Command(CommandType.Answer, GroupsTableRepo.FindFirst(GroupsTableFields.), null);
        }

        /// <summary>
        /// Helps connect to group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command connectToGroup(Command com)
        {
            //Get group
            var group = (Group)com.RequestData;

            //Get user info
            var user = com.UserData;

            //add new member
            group.AddNewMember(user);

            //update table
            var updated = GroupsTableRepo.Update(GroupsTableFields.MembersId.ToString(), group.MembersId, group);

            if (updated)
                return new Command(CommandType.Answer, true, null);
            else
                return new Command(CommandType.Answer, false, null);
        }

        /// <summary>
        /// Helps invite user to group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command inviteToGroup(Command com)
        {
            return null;
        }

        /// <summary>
        /// Helps disconnect member from group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command disconnectFromGroup(Command com)
        {
            //Get group
            var group = (Group)com.RequestData;

            //Get User
            var user = com.UserData;

            //Remove user
            group.RemoveMember(user);

            //Update table
            var updated = GroupsTableRepo.Update(GroupsTableFields.MembersId.ToString(), group.MembersId, group);

            if (updated)
                return new Command(CommandType.Answer, true, null);
            else
                return new Command(CommandType.Answer, false, null);
        }

        /// <summary>
        /// Helps delete user from group
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        static Command deleteMemberFromGroup(Command com)
        {
            return null;
        }

        static Command sendMessage(Command com)
        {
            var message = (Message)com.RequestData;

            var added = MessagesTableRepo.Add(message);

            if (added)
                return new Command(CommandType.Answer, true, null);
            return new Command(CommandType.Answer, false, null);
        }

        static Command deleteUser(Command com)
        {
            return null;
        }
        #endregion
    }
}
