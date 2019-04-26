using System;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using CommonLibs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Repository<User> repo = new Repository<User>(new Table(new ContactTableFields(), "Contacts"), "LocalDB");
            var user = new User("Oleg", "HardPass", "someEmail3", "MyBio", "+2", "photo.img","false",null, 1);
            repo.Add(user);

            repo.

            Console.ReadKey();
        }
    }
}
