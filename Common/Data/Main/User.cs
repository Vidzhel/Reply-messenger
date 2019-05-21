
using System;
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store user info
    /// </summary>
    [Serializable]
    public class User : Contact
    {
        #region Private Members

        public List<int> chatsIdList { get; private set; }

        public List<int> contactsIdList { get; private set; }

        #endregion

        #region public Members

        public string Password { get; set; }

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

        #endregion

        #region Constructor

        public User(string userName, string password, string email, string bio, string profilePhoto = null, string online = "false", List<int> chatsId = null, List<int> contactsId = null, int id = 0) : base(userName, email, bio, profilePhoto, online, id)
        {
            Password = password;
            chatsIdList = chatsId ?? new List<int>();
            contactsIdList = contactsId ?? new List<int>();
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
