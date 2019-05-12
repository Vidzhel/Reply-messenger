
using System;
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store chat info
    /// </summary>
    [Serializable]
    public class Group
    {

        #region Private Members

        List<int> adminsIdList = new List<int>();

        List<int> membersIdList = new List<int>();

        #endregion

        #region Public Members

        public int Id { get; private set; }

        public string Name { get; private set; }

        /// <summary>
        /// Get string representation of Admin id list
        /// </summary>
        public string AdminsId {
            get
            {
                return DataConverter.ListToString(adminsIdList);
            }

            set
            {
                adminsIdList = DataConverter.StringToList(value);
            }
        }

        /// <summary>
        /// Get string representation of Members id list 
        /// </summary>
        public string MembersId
        {
            get
            {
                return DataConverter.ListToString(membersIdList);
            }

            set
            {
                membersIdList = DataConverter.StringToList(value);
            }
        }

        public bool IsPrivate { get; private set; }

        public bool IsChannel { get; private set; }

        public string Image { get; private set; }

        #endregion

        #region Constructor

        public Group(bool isPrivate, string name, bool isChannel, string image, int id = 0, List<int> adminsId = null, List<int> members = null)
        {
            Id = id;
            adminsIdList = adminsId;
            membersIdList = members;
            IsPrivate = isPrivate;
            IsChannel = isChannel;
            Name = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds new member to a chat
        /// </summary>
        /// <param name="user">new member </param>
        public void AddNewMember(User user)
        {
            membersIdList.Add(user.Id);
        }

        /// <summary>
        /// Add new admin to a chat
        /// </summary>
        /// <param name="user"></param>
        public void AddNewAdmin(User user)
        {
            adminsIdList.Add(user.Id);
        }

        /// <summary>
        /// Delete member from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveMember(User user)
        {
            membersIdList.Remove(user.Id);
        }

        /// <summary>
        /// Delete admin from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveAdmin(User user)
        {
            adminsIdList.Remove(user.Id);
        }

        #endregion
    }
}
