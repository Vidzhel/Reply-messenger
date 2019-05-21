
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

        #region Public Members
        
        public bool isPrivate;

        public bool isChannel;

        public List<int> AdminsIdList = new List<int>();

        public List<int> MembersIdList = new List<int>();

        public int Id { get; private set; }

        public string Name { get; set; }

        /// <summary>
        /// Get string representation of Admin id list
        /// </summary>
        public string AdminsId {
            get
            {
                return DataConverter.ListToString(AdminsIdList);
            }

            set
            {
                AdminsIdList = DataConverter.StringToList(value);
            }
        }

        /// <summary>
        /// Get string representation of Members id list 
        /// </summary>
        public string MembersId
        {
            get
            {
                return DataConverter.ListToString(MembersIdList);
            }

            set
            {
                MembersIdList = DataConverter.StringToList(value);
            }
        }

        public string IsPrivate
        {
            get => isPrivate.ToString();
            set => isPrivate = Convert.ToBoolean(value);
        }

        public string IsChannel
        {
            get => isChannel.ToString();
            set => isChannel = Convert.ToBoolean(value);
        }

        public int UsersOnline { get; private set; }

        /// <summary>
        /// If a chat is privat, only with 2 members and without admins 
        /// </summary>
        public bool IsPrivateChat => MembersIdList.Count > 2 ? false : true;

        public string Image { get; set; }

        #endregion

        #region Constructor

        public Group(bool isPrivate, string name, bool isChannel, string image = "", int id = 0, List<int> adminsId = null, List<int> members = null, int usersOnline = 0)
        {
            Id = id;
            AdminsIdList = adminsId ?? new List<int>();
            MembersIdList = members ?? new List<int>();
            this.isPrivate = isPrivate;
            this.isChannel = isChannel;
            Name = name;
            UsersOnline = usersOnline;
            Image = image;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        private Group()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds new member to a chat
        /// </summary>
        /// <param name="user">new member </param>
        public void AddNewMember(int userId)
        {
            MembersIdList.Add(userId);
        }

        /// <summary>
        /// Add new admin to a chat
        /// </summary>
        /// <param name="user"></param>
        public void AddNewAdmin(int userId)
        {
            AdminsIdList.Add(userId);
        }

        /// <summary>
        /// Delete member from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveMember(int userId)
        {
            MembersIdList.Remove(userId);
        }

        /// <summary>
        /// Delete admin from a chat
        /// </summary>
        /// <param name="user"></param>
        public void RemoveAdmin(int userId)
        {
            AdminsIdList.Remove(userId);
        }

        #endregion
    }
}
