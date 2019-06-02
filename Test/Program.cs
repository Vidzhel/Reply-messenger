using System;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using CommonLibs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLibs.ConnectionToClient;
using ClientLibs.Core.ConnectionToServer;
using System.Threading;
using ClientLibs.Core.DataAccess;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                DataConverter.FromInt(521125, out var byte1, out var byte2, out var byte3, out var byte4);

                Console.WriteLine(DataConverter.ToInt(byte1, byte2, byte3, byte4));
                Console.ReadKey();
            }

        }
    }
}
