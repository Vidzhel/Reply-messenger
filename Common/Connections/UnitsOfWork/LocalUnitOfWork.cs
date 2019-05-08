using CommonLibs.Data;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;

namespace Common.Connections.UnitsOfWork
{
    /// <summary>
    /// Provide acces to client loacal data repositories
    /// </summary>
    public class LocalUnitOfWork
    {

        public BaseRepository<Message> MessagesTableRepo { get; private set; }
        public BaseRepository<Contact> ContactsTableRepo { get; private set; }

        #region Constructor
        public LocalUnitOfWork()
        {
            MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messanges"), "LocalDB");
            ContactsTableRepo = new Repository<Contact>(new Table(new ContactTableFields(), "Contacts"), "LocalDB");
        }
        #endregion
    }
}
