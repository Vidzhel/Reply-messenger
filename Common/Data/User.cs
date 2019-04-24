using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    /// <summary>
    /// Store user info
    /// </summary>
    public class User
    {
        #region public Members

        public ulong Id { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public string Bio { get; private set; }

        public List<ulong> ChatsId { get; private set; }

        public string TimePreferences { get; private set; }

        public string Image { get; private set; }

        #endregion

        #region Constructor

        public User(string userName, string password, string email, string bio, string timePreferences, string image, List<ulong> chatsId = null, ulong id = 0)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
            Bio = bio;
            TimePreferences = timePreferences;
            ChatsId = chatsId;
            Image = image;
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
