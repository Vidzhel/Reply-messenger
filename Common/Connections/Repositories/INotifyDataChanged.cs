using System;
using System.Collections.Generic;

namespace CommonLibs.Connections.Repositories
{
    interface INotifyDataChanged<T> where T: class
    {
        event EventHandler<DataChangedArgs<IEnumerable<T>>> DataChanged;
    }
}
