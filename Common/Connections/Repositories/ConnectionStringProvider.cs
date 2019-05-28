using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs.Connections.Repositories
{
    public interface ConnectionStringProvider
    {
        /// <summary>
        /// Returns connection string to a database from App.config
        /// </summary>
        /// <returns></returns>
        string LoadConnectionString();
    }
}
