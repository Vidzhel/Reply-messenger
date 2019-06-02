
using Common.Data.Security;
using System;
using System.Collections.Generic;
using System.Security;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store user info
    /// </summary>
    [Serializable]
    public class User : Contact
    {
        #region Private Members

        string password;

        #endregion

        #region public Members

        /// <summary>
        /// Contains list of user's chats id
        /// </summary>
        public List<int> chatsIdList { get; private set; }

        /// <summary>
        /// Contains list of user's contacts id
        /// </summary>
        public List<int> contactsIdList { get; private set; }

        /// <summary>
        /// Convert secured password to hash string
        /// </summary>
        public SecureString SetSecurePassword { set => password = value.GetHash(); }

        /// <summary>
        /// Store hashed version of secured password
        /// </summary>
        public string Password {

            get
            {
                if (password != null)
                    return password;

                return null;
            }

            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Return string representation of chats list
        /// </summary>
        public string ChatsId {
            get
            {
                return DataConverter.ListToString(chatsIdList);
            }

            set
            {
                chatsIdList = DataConverter.StringToIntList(value);
            }
        }

        /// <summary>
        /// Return string representation of contacts list
        /// </summary>
        public string ContactsId {
            get
            {
                return DataConverter.ListToString(contactsIdList);
            }

            set
            {
                contactsIdList = DataConverter.StringToIntList(value);
            }
        }



        #endregion

        #region Constructor

        public User(string userName, SecureString password, string email, string bio, string profilePhoto = null, string online = "false", List<int> chatsId = null, List<int> contactsId = null, int id = 0) : base(userName, email, bio, profilePhoto, online, id)
        {
            SetSecurePassword = password;
            chatsIdList = chatsId ?? new List<int>();
            contactsIdList = contactsId ?? new List<int>();
        }

        public User(string userName, string hashedPassword, string email, string bio, string profilePhoto = null, string online = "false", List<int> chatsId = null, List<int> contactsId = null, int id = 0) : base(userName, email, bio, profilePhoto, online, id)
        {
            Password = hashedPassword;
            chatsIdList = chatsId ?? new List<int>();
            contactsIdList = contactsId ?? new List<int>();
        }

        public User(User user) : base(user.UserName, user.Email, user.Bio, user.ProfilePhoto, user.Online, user.Id) 
        {
            Password = user.Password;
            chatsIdList = user.chatsIdList;
            contactsIdList = user.contactsIdList;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        private User() : base()
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
