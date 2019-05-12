
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

        public List<int> chatsIdList { get; private set; } = new List<int>();

        #endregion

        #region public Members

        public int Id { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public string Bio { get; private set; }

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

        public string ProfilePhoto { get; private set; }

        public string Online { get; private set; }

        #endregion

        #region Constructor

        public User(string userName, string password, string email, string bio, string profilePhoto = null, string online = "false", List<int> chatsId = null, int id = 0)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
            Bio = bio;
            chatsIdList = chatsId;
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
            chatsIdList.Remove(chat.Id);
        }

        #endregion
    }
}
