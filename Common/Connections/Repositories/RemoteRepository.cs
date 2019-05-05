using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs.Connections.Repositories
{
    class RemoteRepository<T> : IRemoteRepository<T> where T : class
    {
        #region Main Func


        public bool Add(T data)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<T> dataEntity)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Find(string column, string value)
        {
            throw new NotImplementedException();
        }

        public T FindFirst(string column, string value)
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsExists(string column, string value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string column, string value)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRange(string column, IEnumerable<string> values)
        {
            throw new NotImplementedException();
        }

        public bool Update(string column, string value, T data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
