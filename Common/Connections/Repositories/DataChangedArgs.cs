using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Connections.Repositories
{
    class DataChangedArgs
    {
        public Data Data { get; private set; }

        public string Action { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">The data thet changed</param>
        /// <param name="dataAction">Data Action</param>
        public DataChanged(Data data, string dataAction)
        {
            Data = data;
            Action = dataAction;
        }
    }
}
