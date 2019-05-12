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
            int i = 0;
            while (i != 10)
            {
                i++;
                //var response = UnitOfWork.SignUp(new User("Vidzhel", "HardPass", "MyEmail", "MyBio", "+2"));
                var response = UnitOfWork.SendMessage(new Message(1, 2, DataType.Text, DateTime.Now , "Hello, I'm a new user", MessageStatus.SendingInProgress));


                if (response)
                    Console.WriteLine("Yeah, probably, that command means that all sh** work like have expected");

            }

            Console.ReadKey();
        }
    }
}
