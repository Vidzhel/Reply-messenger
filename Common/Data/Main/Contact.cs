using System;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store contact info
    /// </summary>
    [Serializable]
    public class Contact
    {
        #region public Members

        public int Id { get; private set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string ProfilePhoto { get; set; }

        public string Online { get; set; }

        //Return DateTime.Now if user is online, else return last time online
        public DateTime LastTimeOnline { get {
                if (Online.Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    return DateTime.Now;
                else
                    return DateTime.FromBinary(Convert.ToInt64(Online)).ToLocalTime();
            } }

        #endregion

        #region Constructor

        public Contact(string userName, string email, string bio, string profilePhoto = null, string online = "false", int id = 0)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Bio = bio;
            ProfilePhoto = profilePhoto;
            Online = online;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        private Contact()
        {

        }

        #endregion

    }
}
