

using CommonLibs.Data;
using CommonLibs.Connections.Repositories.Tables;

namespace CommonLibs.Connections.Repositories
{
    public class DataChangedArgs<T> where T: class
    {
        public T Data { get; private set; }

        public string ExtraInfo { get; private set; }

        public RepositoryActions Action { get; private set; }

        #region Constructor


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">The data thet changed</param>
        /// <param name="dataAction">Data Action</param>
        public DataChangedArgs(T data, string extraInfo, RepositoryActions dataAction)
        {
            ExtraInfo = extraInfo;
            Data = data;
            Action = dataAction;
        }

        #endregion
    }
}
