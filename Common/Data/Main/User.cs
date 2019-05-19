
using System;
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store user info
    /// </summary>
    [Serializable]
    public class User
    {
        #region Private Members

        public List<int> chatsIdList { get; private set; }

        public List<int> contactsIdList { get; private set; }

        DateTime online;

        #endregion

        #region public Members

        public int Id { get; private set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string ChatsId {
            get
            {
                return DataConverter.ListToString(chatsIdList);
            }

            set
            {
                chatsIdList = DataConverter.StringToList(value);
            }
        }

        public string ContactsId {
            get
            {
                return DataConverter.ListToString(contactsIdList);
            }

            set
            {
                contactsIdList = DataConverter.StringToList(value);
            }
        }

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

        #endregion

        #region Constructor

        public User(string userName, string password, string email, string bio, string profilePhoto = null, string online = "false", List<int> chatsId = null, List<int> contactsId = null, int id = 0)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
            Bio = bio;
            chatsIdList = chatsId ?? new List<int>();
            contactsIdList = contactsId ?? new List<int>();
            ProfilePhoto = profilePhoto;
            Online = online;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        private User()
        {
        }
        #endregion

        #region Public Methods

        public void AddNewChat(Group chat)
        {
            chatsIdList.Add(chat.Id);
        }

        public void RemoveChat(Group chat)
        {
            if (chatsIdList.Contains(chat.Id))
                chatsIdList.Remove(chat.Id);
        }
        

        public void AddNewContact(Contact contact)
        {
            contactsIdList.Add(contact.Id);
        }

        public void RemoveContact(Contact contact)
        {
            if(contactsIdList.Contains(contact.Id))
                contactsIdList.Remove(contact.Id);
        }

        #endregion
    }
}
