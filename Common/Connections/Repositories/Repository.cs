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
    public class Repository<T> : BaseRepository<T> where T: class
    {

        #region Public Members

        /// <summary>
        /// Notify data changed haandler
        /// </summary>
        public override event EventHandler<DataChangedArgs<IEnumerable<T>>> DataChanged;

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
        /// Adds new handler
        /// </summary>
        /// <param name="handler"></param>
        public override void AddDataChangedHandler(EventHandler<DataChangedArgs<IEnumerable<T>>> handler)
        {
            DataChanged += handler;
        }

        /// <summary>
        /// Add data to the table
        /// </summary>
        /// <param name="data">Info to add</param>
        public override bool Add(T data)
        {
            try
            {
                using (var cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Open();

                    string request = "INSERT INTO " + table.ToString() + " ( " + table.GetFields() + " ) VALUES ( " + table.GetFieldsForQuery() + " )";
                    cnn.Execute(request, data);
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
        public override bool AddRange(IEnumerable<T> dataRange)
        {
            try
            {
                using (var con = new SQLiteConnection(LoadConnectionString()))
                {
                    con.Open();

                    string request = "INSERT INTO " + table.ToString() + " ( " + table.GetFields() + " ) VALUES ( " + table.GetFieldsForQuery() + " )";
                    foreach (var data in dataRange)
                    {
                        con.Execute(request, data);
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
        /// Update data 
        /// </summary>
        /// <param name="column">Column to search and replace data</param>
        /// <param name="value">Value to serarch</param>
        /// <param name="data">New data</param>
        /// <returns></returns>
        public override bool Update(string column, string value, T data)
        {
            var dp = new DynamicParameters();

            dp.Add(column, value);

            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();

                var request = $"DELETE FROM {table.ToString()} WHERE {column} = @{column}";
                var query = con.Execute(request, dp);

                request = "INSERT INTO " + table.ToString() + " ( " + table.GetFields() + " ) VALUES ( " + table.GetFieldsForQuery() + " )";
                con.Execute(request, data);
            }

            var ls = new List<T>();
            ls.Add(data);

            //Fires INotifyDataChanged event
            DataChanged?.Invoke(this, new DataChangedArgs<IEnumerable<T>>(ls, table, "Update"));
            return true;
        }


        /// <summary>
        /// Finding all the matches in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public override IEnumerable<T> Find(string column, string value)
        {
            var dp = new DynamicParameters();

            dp.Add(column, value);

            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();

                string request = "select * from " + table.ToString() + " WHERE " + column + " =@" + column;
                var query = con.Query<T>(request, dp);

                return query.ToList<T>();
            }
        }
        
        /// <summary>
        /// Finds first match in table
        /// </summary>
        /// <param name="column">Columnt to search</param>
        /// <param name="value">Search parameter</param>
        /// <returns></returns>
        public override T FindFirst(string column, string value)
        {
            var dp = new DynamicParameters();

            dp.Add(column, value);

            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();

                string request = "select * from " + table.ToString() + " WHERE " + column + " =@" + column;
                var query = con.Query<T>(request, dp).First<T>();

                return query;
            }
        }


        /// <summary>
        /// Return a data from row 
        /// </summary>
        /// <param name="id">an id of row</param>
        /// <returns></returns>
        public override T Get(int id)
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();

                var query = con.Query<T>("select * from " + table.ToString() + " WHERE Id = " + id, new DynamicParameters()).First<T>();

                return query;
            }
        }

        /// <summary>
        /// Gets all notes from table
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<T> GetAll()
        {
            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();

                string request = "select * from " + table.ToString();
                var query = con.Query<T>(request);

                return query.ToList();
            }
        }


        /// <summary>
        /// Remove row from db
        /// </summary>
        /// <param name="column">Column to search and replace data</param>
        /// <param name="value">Value to search</param>
        /// <returns></returns>
        public override bool Remove(string column, string value)
        {
            IEnumerable<T> data;
            var dp = new DynamicParameters();

            dp.Add(column, value);

            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();
                string request = $"SELECT * FROM {table.ToString()} WHERE {column}=@{column}";
                data = con.Query<T>(request, dp);

                request = $"DELETE FROM {table.ToString()} WHERE {column} = @{column}";
                var query = con.Execute(request, dp);
            }

            //Fires INotifyDataChanged event
            DataChanged?.Invoke(this, new DataChangedArgs<IEnumerable<T>>(data, table, "Remove"));
            return true;
        }

        /// <summary>
        /// Remove row from db
        /// </summary>
        /// <param name="column">Column to search and replace data</param>
        /// <param name="values">Values to search</param>
        /// <returns></returns>
        public override bool RemoveRange(string column, IEnumerable<string> values)
        {

            foreach (var value in values)
            {
                Remove(column, value);
            }

            return true;
        }

        /// <summary>
        /// Check db, return true if data exists
        /// </summary>
        /// <param name="column">column to search where</param>
        /// <param name="value">value to search</param>
        /// <returns></returns>
        public override bool IsExists(string column, string value)
        {
            var dp = new DynamicParameters();

            dp.Add(column, value);

            using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
            {
                con.Open();
                return con.ExecuteScalar<bool>($"select count(1) from {table.ToString()} where {column}=@{column}", dp);
            }
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
