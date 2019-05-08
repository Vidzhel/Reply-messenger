using System;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using CommonLibs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Connections.UnitsOfWork;
using ServerLibs.ConnectionToClient;
using ClientLibs.Core.ConnectionToServer;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var wu = new LocalUnitOfWork();

            //var user1 = new Contact("Friend", "friendsEmail", "friendsbio");
            //var user2 = new Contact("Friend2", "friendsEmail2", "friendsbio2");
            //var user3 = new Contact("Friend3", "friendsEmail3", "friendsbio3");

            //var ls = new List<string>();

            //ls.Add("friendsEmail3");
            //ls.Add("friendsEmail2");
            //ls.Add("someEmail131");

            //var arr = new string[2];
            //arr[0] = "someEmail13";
            //arr[1] = "someEmail13";

            //Console.WriteLine(wu.ContactsTableRepo.IsExists(ContactTableFields.Email.ToString(), "someEmail131"));
            //wu.ContactsTableRepo.RemoveRange(ContactTableFields.Email.ToString(), ls );
            //var user = wu.ContactsTableRepo.Find(ContactTableFields.Email.ToString(), "someEmail131");


            Thread thread = new Thread( new ThreadStart(AsynchronousServerConnection.ConnecToServer) );
            thread.Start();
            var response = AsynchronousServerConnection.SendData(new Command(CommandType.Messsage,
               new Message(1, 2, DataType.Text, DateTime.Now, "This is my message") , new User("Oleg", "OlegsPass", "myEmail@sdsa", "sadsao", "+2")));

            Console.WriteLine((string)response);

            Console.ReadKey();
        }
    }
}
