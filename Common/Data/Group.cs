
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store chat info
    /// </summary>
    public class Group
    {
        #region Public Members

        public int Id { get; private set; }

        public List<int> AdminsId { get; private set; }

        public List<int> MembersId { get; private set; }

        public bool IsPrivate { get; private set; }

        public bool IsChannel { get; private set; }

        public string Image { get; private set; }

        #endregion

        #region Constructor

        public Group(bool isPrivate, bool isChannel, string image, int id = 0, List<int> adminsId = null, List<int> members = null)
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
            MembersId.Add(user.Id);
        }

        /// <summary>
        /// Add new admin to a chat
        /// </summary>
        /// <param name="user"></param>
        public void AddNewAdmin(User user)
        {
            AdminsId.Add(user.Id);
        }

        /// <summary>
        /// Delete member from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveMember(User user)
        {
            MembersId.Remove(user.Id);
        }

        /// <summary>
        /// Delete admin from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveAdmin(User user)
        {
            AdminsId.Remove(user.Id);
        }

        #endregion
    }
}
