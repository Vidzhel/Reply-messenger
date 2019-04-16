using System;
using ClientLibs.Core.DataTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs.Core.Service
{
    abstract class Service
    {
        static ServiceType Type { set; get; }

        Response MakeRequest(Target target, Request request, Data data);
    }
}
