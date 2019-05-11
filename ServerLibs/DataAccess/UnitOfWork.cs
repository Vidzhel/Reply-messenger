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

        public static BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messanges"), "LocalDB");
        public static BaseRepository<User> UsersTableRepo = new Repository<User>(new Table(new UsersTableFields(), "Users"), "LocalDB");
        public static BaseRepository<Group> GroupsTableRepo = new Repository<Group>(new Table(new GroupsTableFields(), "Users"), "LocalDB");

        static ServerCommandChain commandChain = new ServerCommandChain();

        #region Constructor

        static UnitOfWork()
        {
            //Set up event data changed handler for tables
            MessagesTableRepo.AddDataChangedHandler( (sender, args) => OnMessagesTableChanged(sender, args));
            UsersTableRepo.AddDataChangedHandler( (sender, args) => OnUsersTableChanged(sender, args));
            GroupsTableRepo.AddDataChangedHandler( (sender, args) => OnGroupsTableChanged(sender, args));

            //Add handler on new server command received e.g. new message
            commandChain.OnNewClientCommand((sender, args) => OnClientCommand(sender, args));
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
    }
}
