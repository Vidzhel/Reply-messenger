using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Connections.Repositories
{
    
    interface IRepository<T> where T: class 
    {
        
        IEnumerable<T> GetAll();
        T Get(long id);
        
        IEnumerable<T> Find(IEnumerable<T> column, string value);
        
        bool Add(T data);
        bool AddRange(IEnumerable<T> dataEntity);
        
        bool Remove(T data);
        bool RemoveRange(IEnumerable<T> dataEntity);
        
    }
}
