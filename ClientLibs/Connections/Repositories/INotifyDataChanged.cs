using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Connections.Repositories
{
    interface INotifyDataChanged
    {
        event EventHandler<DataChangedArgs> DataChanged;
    }
}
