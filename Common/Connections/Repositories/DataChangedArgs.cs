

using CommonLibs.Data;
using CommonLibs.Connections.Repositories.Tables;

namespace CommonLibs.Connections.Repositories
{
    public class DataChangedArgs<T> where T: class
    {
        public T Data { get; private set; }

        public BaseTable Table { get; private set; }

        public RepositoryActions Action { get; private set; }

        #region Constructor


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">The data thet changed</param>
        /// <param name="dataAction">Data Action</param>
        public DataChangedArgs(T data, BaseTable table, RepositoryActions dataAction)
        {
            Table = table;
            Data = data;
            Action = dataAction;
        }

        #endregion
    }
}
