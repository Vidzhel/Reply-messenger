using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Connections.Repositories
{

    /// <summary>
    /// Interface of basic Repository commands
    /// </summary>
    /// <typeparam name="T">type of return values, user, person eth.</typeparam>
    class Repository<T> : IRepository<T>, INotifyDataChanged where T : class
    {

        public event EventHandler<DataChangedArgs> DataChanged;


        protected readonly string connectionString;

        public string LoadConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }

        /// <summary>
        /// Create 
        /// </summary>
        /// <param name="connectionString">The name of ellement in App.config</param>
        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }


        #region Main Funct

        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data"></param>
        public bool Add(T data)
        {
            DataChanged?.Invoke(this, new DataChangedArgs(data, "add"));
        }

        public bool AddRange(IEnumerable<T> dataEntity)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Finding all the mathes in table
        /// </summary>
        /// <param name="column">Header of table</param>
        /// <param name="value">A value of the desired row</param>
        /// <returns></returns>
        public IEnumerable<T> Find(IEnumerable<T> column, string value)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Return a data from row 
        /// </summary>
        /// <param name="id">an id of row</param>
        /// <returns></returns>
        public T Get(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all notes from table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = con.Query<T>("select * from ", new DynamicParameters( );

                return query.ToList();
            }
        }


        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data"></param>
        public bool Remove(T data)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRange(IEnumerable<T> dataEntity)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
