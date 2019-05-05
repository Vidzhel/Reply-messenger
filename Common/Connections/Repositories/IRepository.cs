using CommonLibs.Connections.Repositories.Tables;
using System;
using System.Collections.Generic;

namespace CommonLibs.Connections.Repositories
{
    /// <summary>
    /// Provide basic commands for repo
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public interface IRepository<T> where T: class 
    {
        
        IEnumerable<T> GetAll();
        T Get(int id);
        
        IEnumerable<T> Find(string column, string value);
        T FindFirst(string column, string value);
        
        bool Add(T data);
        bool AddRange(IEnumerable<T> dataEntity);
        
        bool Remove(string column, string value);
        bool RemoveRange(string column, IEnumerable<string> values);

        bool Update(string column, string value, T data);

        bool IsExists(string column, string value);
    }
}
