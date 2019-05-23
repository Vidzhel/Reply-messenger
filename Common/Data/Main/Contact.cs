using System;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store contact info
    /// </summary>
    [Serializable]
    public class Contact
    {
        #region Private Members

        DateTime online;

        DateTime lastTimeUpdated;

        #endregion

        #region public Members

        public int Id { get; private set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string ProfilePhoto { get; set; }


        /// <summary>
        /// Get and set local DateTime online
        /// </summary>
        public DateTime LocalLastTimeOnline
        {
            get
            {
                return online;
            }

            set
            {
                online = value;
            }
        }

        /// <summary>
        /// Converted binary representation of universal time
        /// </summary>
        public string Online
        {
            get
            {
                //If oline equals to zero, that means user online
                if (online == DateTime.MaxValue)
                    return "true";

                else if (online == DateTime.MinValue)
                    return "false";

                return online.ToUniversalTime().ToBinary().ToString();
            }
            set
            {
                if (value.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    online = DateTime.MaxValue;
                    return;
                }

                else if (value.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                {
                    online = DateTime.MinValue;
                    return;
                }

                online = DateTime.FromBinary((long)Convert.ToUInt64(value)).ToLocalTime();
            }
        }


        /// <summary>
        /// Get and set local last time updated DateTime
        /// </summary>
        public DateTime LocalLastTimeUpdated
        {
            get
            {
                return lastTimeUpdated;
            }

            set
            {
                lastTimeUpdated = value;
            }
        }

        /// <summary>
        /// Converted binary representation of universal time
        /// </summary>
        public string LastTimeUpdated
        {
            get
            {
                return lastTimeUpdated.ToUniversalTime().ToBinary().ToString();
            }
            set
            {
                lastTimeUpdated = DateTime.FromBinary((long)Convert.ToUInt64(value)).ToLocalTime();
            }
        }


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
            LocalLastTimeUpdated = DateTime.Now;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        protected Contact()
        {

        }

        #endregion

    }
}
