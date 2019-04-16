using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Connections.Repositories
{
    
    /// <summary>
    /// Interface of basic Repository commands
    /// </summary>
    /// <typeparam name="T">type of return values, user, person eth.</typeparam>
    interface IRepository<T> where T: class 
    {
        /// <summary>
        /// Gets all notes from table
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Return a data from row 
        /// </summary>
        /// <param name="id">an id of row</param>
        /// <returns></returns>
        T Get(long id);

        /// <summary>
        /// Finding all the mathes in table
        /// </summary>
        /// <param name="column">Header of table</param>
        /// <param name="value">A value of the desired row</param>
        /// <returns></returns>
        IEnumerable<T> Find(IEnumerable<T> column, string value);
        

        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data"></param>
        bool Add(T data);
        bool AddRange(IEnumerable<T> dataEntity);


        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data"></param>
        bool Remove(T data);
        bool RemoveRange(IEnumerable<T> dataEntity);
        
    }
}
