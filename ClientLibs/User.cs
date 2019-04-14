using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibs
{
    public class User
    {
        string name;
        string password;
        string email;
        string datePreferences;

        public string Name
        {
            set;
            get;
        }

        public bool ChangePassword(string oldPass, string newPass)
        {
            if (oldPass == password)
                password = newPass;
            else
                return false;

            return true;
        }
    }
}
