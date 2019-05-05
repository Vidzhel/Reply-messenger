using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Connections.ClientServer
{
    public interface IConnectionProvider
    {

        bool Connect(int port, string server);
        void Disconect(bool reuseSocket);

    }
}
