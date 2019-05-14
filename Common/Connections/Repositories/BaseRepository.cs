using CommonLibs.Connections.Repositories.Tables;
using System;
using System.Collections.Generic;

namespace CommonLibs.Connections.Repositories
{
    /// <summary>
    /// Provide basic commands for repo
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public abstract class BaseRepository<T> : INotifyDataChanged<T> where T : class 
    {

        /// <summary>
        /// Notify data changed haandler
        /// </summary>
        public abstract event EventHandler<DataChangedArgs<IEnumerable<T>>> DataChanged;

        public abstract void AddDataChangedHandler(EventHandler<DataChangedArgs<IEnumerable<T>>> handler);

        public abstract IEnumerable<T> GetAll();
        public abstract T Get(int id);
        public abstract T GetLast();

        public abstract IEnumerable<T> Find(string column, string value);
        public abstract T FindFirst(string column, string value);
        public abstract T FindLast(string column, string value);

        public abstract bool Add(T data);
        public abstract bool AddRange(IEnumerable<T> dataEntity);

        public abstract bool Remove(string column, string value);
        public abstract bool RemoveRange(string column, IEnumerable<string> values);

        public abstract bool Update(string column, string value, T data);

        public abstract bool IsExists(string column, string value);
    }
}
