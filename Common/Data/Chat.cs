
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store chat info
    /// </summary>
    public class Chat
    {
        #region Public Members

        public ulong Id { get; private set; }

        public List<ulong> AdminsId { get; private set; }

        public List<ulong> MembersId { get; private set; }

        public bool IsPrivate { get; private set; }

        public bool IsChannel { get; private set; }

        public string Image { get; private set; }

        #endregion

        #region Constructor

        public Chat(bool isPrivate, bool isChannel, string image, ulong id = 0, List<ulong> adminsId = null, List<ulong> members = null)
        {
            Id = id;
            AdminsId = adminsId;
            MembersId = members;
            IsPrivate = isPrivate;
            IsChannel = isChannel;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds new member to a chat
        /// </summary>
        /// <param name="user">new member </param>
        public void AddNewMember(User user)
        {
            MembersId.Add(user.UserId);
        }

        /// <summary>
        /// Add new admin to a chat
        /// </summary>
        /// <param name="user"></param>
        public void AddNewAdmin(User user)
        {
            AdminsId.Add(user.UserId);
        }

        /// <summary>
        /// Delete member from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveMember(User user)
        {
            MembersId.Remove(user.UserId);
        }

        /// <summary>
        /// Delete admin from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveAdmin(User user)
        {
            AdminsId.Remove(user.UserId);
        }

        #endregion
    }
}
