using CommonLibs.Connections.Repositories.Tables;
using System;
using System.Collections.Generic;

namespace CommonLibs.Connections.Repositories
{
    
    public interface IRepository<T> where T: class 
    {
        
        IEnumerable<T> GetAll();
        T Get(long id);
        
        IEnumerable<T> Find(Enum column, string value);
        T FindFirst(Enum column, string value);
        
        bool Add(T data);
        bool AddRange(IEnumerable<T> dataEntity);
        
        bool Remove(Enum column, T data);
        bool RemoveRange(Enum column, IEnumerable<T> dataEntity);


        
    }
}
