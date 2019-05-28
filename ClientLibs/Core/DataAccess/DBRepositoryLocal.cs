using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;

namespace ClientLibs.Core.DataAccess
{
    public class DBRepositoryLocal
    {
        public BaseRepository<Message> MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messages"), "LocalDB");
        public BaseRepository<Contact> ContactsTableRepo = new Repository<Contact>(new Table(new ContactsTableFields(), "Contacts"), "LocalDB");
        public BaseRepository<Group> GroupsTableRepo = new Repository<Group>(new Table(new GroupsTableFields(), "Groups"), "LocalDB");
    }
}
