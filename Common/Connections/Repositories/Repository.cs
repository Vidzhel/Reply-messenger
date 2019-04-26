using CommonLibs.Connections.Repositories.Tables;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace CommonLibs.Connections.Repositories
{

    /// <summary>
    /// Basic Repository commands
    /// </summary>
    /// <typeparam name="T">type of return values, user, person eth.</typeparam>
    public class Repository<T> : IRepository<T>, INotifyDataChanged<T> where T : class
    {  
        #region Public Members

        /// <summary>
        /// Notify data changed haandler
        /// </summary>
        public event EventHandler<DataChangedArgs<IEnumerable<T>>> DataChanged;


        #endregion

        #region Private Members


        /// <summary>
        /// Connection string of data base (App.config)
        /// </summary>
        protected readonly string connectionString;

        /// <summary>
        /// DB table
        /// </summary>
        public BaseTable table { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Create new repository
        /// </summary>
        /// <param name="connectionString">The name of ellement in App.config</param>
        /// <param name="table">The name DB table</param>
        public Repository(BaseTable table, string connectionString)
        {
            this.table = table;
            this.connectionString = connectionString;
        }

        #endregion 

        #region Main Funct

        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data">Info to add</param>
        public bool Add(T data)
        {
            try
            {
                using (var cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Open();
                    string request = "INSERT INTO " + table.ToString() + " ( " + table.GetFields() + " ) VALUES ( " + table.GetFieldsForQuery() + " )";
                    cnn.Execute(request, data);
                    //cnn.Execute(@"INSERT INTO Contacts ( UserName, UserId, Email, Bio, Online ) VALUES ( @UserName, @UserId,  @Email, @Bio, @Online )", data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            var ls = new List<T>();
            ls.Add(data);

            //Fires INotifyDataChanged event
            DataChanged?.Invoke(this, new DataChangedArgs<IEnumerable<T>>(ls, table, "add"));
            return true;
        }


        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data">Info to add</param>
        public bool AddRange(IEnumerable<T> dataRange)
        {
            try
            {
                using (var cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Open();
                    string request = "INSERT INTO " + table.ToString() + " ( " + table.GetFields() + " ) VALUES ( " + table.GetFieldsForQuery() + " )";
                    foreach (var data in dataRange)
                    {
                        cnn.Execute(request, data);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }


            //Fires INotifyDataChanged event
            DataChanged?.Invoke(this, new DataChangedArgs<IEnumerable<T>>(dataRange, table, "addRange"));
            return true;
        }


        /// <summary>
        /// Finding all the matches in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public IEnumerable<T> Find(Enum column, string value)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                string request = "select * from " + table.ToString() + "WHERE " + column + " = " + value;
                var query = con.Query<T>(request, new DynamicParameters());

                return query.ToList<T>();
            }
        }
        
        /// <summary>
        /// Finds first match in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public T FindFirst(Enum column, string value)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                string request = "select * from " + table.ToString() + "WHERE " + column + " = " + value;
                var query = con.Query<T>(request, new DynamicParameters()).First<T>();

                return query;
            }
        }


        /// <summary>
        /// Return a data from row 
        /// </summary>
        /// <param name="id">an id of row</param>
        /// <returns></returns>
        public T Get(long id)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                var query = con.Query<T>("select * from " + table.ToString() + "WHERE Id = " + id, new DynamicParameters()).First<T>();

                return query;
            }
        }

        /// <summary>
        /// Gets all notes from table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                string request = "select * from " + table.ToString();
                var query = con.Query<T>(request, new DynamicParameters());

                return query.ToList();
            }
        }


        /// <summary>
        /// Remove data from DB
        /// </summary>
        /// <param name="data"></param>
        public bool Remove(Enum column, T data)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                string request = $"DELETE FROM {table.ToString()} WHERE {column} = @{column}";
                var query = con.Execute(request, new DynamicParameters());
            }


            var ls = new List<T>();
            ls.Add(data);

            //Fires INotifyDataChanged event
            DataChanged?.Invoke(this, new DataChangedArgs<IEnumerable<T>>(ls, table, "Remove"));
            return true;
        }


        /// <summary>
        /// Remove data range from DB
        /// </summary>
        /// <param name="data"></param>
        public bool RemoveRange(Enum column, IEnumerable<T> dataRenge)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                string request = $"DELETE FROM {table.ToString()} WHERE {column} = @{column}";

                foreach (var data in dataRenge)
                {
                    var query = con.Execute(request, data);
                }
            }

            //Fires INotifyDataChanged event
            DataChanged?.Invoke(this, new DataChangedArgs<IEnumerable<T>>(dataRenge, table, "Remove"));
            return true;
        }


        #endregion

        #region Helpers

        /// <summary>
        /// Load connection string from App.config
        /// </summary>
        /// <returns></returns>
        public string LoadConnectionString()
        {
            //TODO delete comments
            //return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            return @"Data Source=..\..\SQLiteLocalDB.db; Version=3";
        }

        #endregion
    }
}
