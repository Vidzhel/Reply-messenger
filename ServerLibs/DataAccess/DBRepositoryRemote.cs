using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;

namespace ServerLibs.DataAccess
{
    public class DBRepositoryRemote
    {
        public BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new RemoteMessagesTableFields(), "Messages"), "LocalDB");
        public BaseRepository<User> UsersTableRepo = new Repository<User>(new Table(new RemoteUsersTableFields(), "Users"), "LocalDB");
        public BaseRepository<Group> GroupsTableRepo = new Repository<Group>(new Table(new RemoteGroupsTableFields(), "Groups"), "LocalDB");
        public BaseRepository<File> FilesTableRepo = new Repository<File>(new Table(new FilesTableFields(), "Files"), "LocalDB");

    }
}
