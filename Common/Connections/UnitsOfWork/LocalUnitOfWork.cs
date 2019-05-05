using CommonLibs.Data;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using System;

namespace Common.Connections.UnitsOfWork
{
    /// <summary>
    /// Gives ability to with local repositories
    /// </summary>
    public class LocalUnitOfWork
    {

        public IRepository<Message> MessagesTableRepo { get; private set; }
        public IRepository<Contact> ContactsTableRepo { get; private set; }

        #region Constructor
        public LocalUnitOfWork()
        {
            MessagesTableRepo = new Repository<Message>(new Table(new MessagesTableFields(), "Messanges"), "LocalDB");
            ContactsTableRepo = new Repository<Contact>(new Table(new ContactTableFields(), "Contacts"), "LocalDB");
        }
        #endregion
    }
}
