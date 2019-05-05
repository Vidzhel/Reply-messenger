
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store user info
    /// </summary>
    public class User
    {
        #region public Members

        public int Id { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public string Bio { get; private set; }

        public List<int> ChatsId { get; private set; }

        public string TimePreferences { get; private set; }

        public byte[] ProfilePhoto { get; private set; }

        public string Online { get; private set; }

        #endregion

        #region Constructor

        public User(string userName, string password, string email, string bio, string timePreferences, byte[] profilePhoto = null, string online = "false", List<int> chatsId = null, int id = 0)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
            Bio = bio;
            TimePreferences = timePreferences;
            ChatsId = chatsId;
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

        public void AddNewChat(Chat chat)
        {
            ChatsId.Add(chat.Id);
        }
        public void RemoveChat(Chat chat)
        {
            ChatsId.Remove(chat.Id);
        }

        #endregion
    }
}
