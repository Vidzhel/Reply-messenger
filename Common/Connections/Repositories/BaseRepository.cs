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

        /// <summary>
        /// Adds new handler
        /// </summary>
        /// <param name="handler"></param>
        public abstract void AddDataChangedHandler(EventHandler<DataChangedArgs<IEnumerable<T>>> handler);

        public abstract IEnumerable<T> GetAll();
        /// <summary>
        /// Return a data from row 
        /// </summary>
        /// <param name="id">an id of row</param>
        /// <returns></returns>
        public abstract T Get(int id);
        /// <summary>
        /// Return an Id of the row
        /// </summary>
        /// <param name="id">an id of row</param>
        /// <returns></returns>
        public abstract int GetId(string column, string value);
        public abstract T GetLast();
        public abstract T GetFirst();

        /// <summary>
        /// Finding all the matches in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public abstract IEnumerable<T> Find(string column, string value);
        /// <summary>
        /// Finds first match in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public abstract T FindFirst(string column, string value);
        /// <summary>
        /// Finds last match in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public abstract T FindLast(string column, string value);

        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data">Info to add</param>
        public abstract bool Add(T data);
        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data">Info to add</param>
        public abstract bool AddRange(IEnumerable<T> dataEntity);

        /// <summary>
        /// Remove row from db
        /// </summary>
        /// <param name="column">Column to search and replace data</param>
        /// <param name="values">Values to search</param>
        /// <returns></returns>
        public abstract bool Remove(string column, string value);

        /// <summary>
        /// Update data 
        /// </summary>
        /// <param name="column">Column to search and replace data</param>
        /// <param name="value">Value to serarch</param>
        /// <param name="data">New data</param>
        /// <returns></returns>
        public abstract bool Update(string column, string value, T data);
       
        /// <summary>
        /// Check db, return true if data exists
        /// </summary>
        /// <param name="column">column to search where</param>
        /// <param name="value">value to search</param>
        /// <returns></returns>
        public abstract bool IsExists(string column, string value);
    }
}
