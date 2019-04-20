using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Connections.Repositories
{
    interface IRemoteRepository<T> : IRepository<T> where T : class
    {
        //Additional methods for server repository
        bool Connect();
        bool Disconnect();
    }
}
