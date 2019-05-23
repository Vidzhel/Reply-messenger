using Common.Data.Security;
using System;
using System.Security;
using System.Text.RegularExpressions;

namespace ClientLibs.Core
{
    public static class ValidateUserData
    {

        #region Private Members

        //Regexp for validation
        private static Regex userNameRegex = new Regex(@"^.{3,15}$");
        private static Regex emailRegex = new Regex(@"^\w+[._-]?\w+@\w+.\w+$");
        private static Regex passwordRegex = new Regex(@"^[\w.\-,/]{8,}$");

        #endregion

        #region Public Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">user name to validate</param>
        /// <param name="checkEmptiness">check on emtiness</param>
        /// <returns></returns>
        public static string ValidateUserName(string name, bool checkEmptiness)
        {
            if(checkEmptiness)
                if (name == String.Empty)
                    return "User name field is empty";

            //Check data 
            if (!userNameRegex.IsMatch(name))
                return "User name should be 3-15 symbols";

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">user email to validate</param>
        /// <param name="checkEmptiness">check on emtiness</param>
        /// <returns></returns>
        public static string ValidateEmail(string email, bool checkEmptiness)
        {
            if(checkEmptiness)
                if (email == String.Empty)
                    return "Email field is empty";

            //Check data
            if (!emailRegex.IsMatch(email))
                return "Wrong email";

            return null;
        }

        public static string ValidatePassword(SecureString pass, bool checkEmptiness)
        {
            if (checkEmptiness)
            {
                //TODO check password on emtiness
                if (pass.Unsecure() == String.Empty)
                    return "Password field is empty";
            }


            //Check data
            if (!passwordRegex.IsMatch(pass.Unsecure()))
                return "Password should have at less 8 symbols";

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass">user passwords to validate</param>
        /// <param name="checkEmptiness">check on emtiness</param>
        /// <returns></returns>
        public static string ValidatePassword(SecureString[] pass, bool checkEmptiness)
        {
            if (checkEmptiness)
            {
                if (pass[0].Unsecure() == String.Empty)
                    return "Password field is empty";
                if (pass[1].Unsecure() == String.Empty)
                    return "Repeat password field is empty";
            }


            //Check data
            if (!passwordRegex.IsMatch(pass[0].Unsecure()))
                return "Password should have at less 8 symbols";


            if (pass[0].Unsecure() != pass[1].Unsecure())
                return "Paswords don't match";

            return null;
        }

        #endregion
    }
}
