
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

        DateTime lastTimeUpdated;

        #endregion

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
                AdminsIdList = DataConverter.StringToIntList(value);
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
                MembersIdList = DataConverter.StringToIntList(value);
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

        public int UsersOnline { get; set; }

        /// <summary>
        /// If a chat is privat, only with 2 members and without admins 
        /// </summary>
        public bool IsPrivateChat => MembersIdList.Count > 2 ? false : true;

        public string Image { get; set; }


        /// <summary>
        /// Get and set local last time updated DateTime
        /// </summary>
        public DateTime LocalLastTimeUpdated
        {
            get
            {
                return lastTimeUpdated;
            }

            set
            {
                lastTimeUpdated = value;
            }
        }

        /// <summary>
        /// Converted binary representation of universal time
        /// </summary>
        public string LastTimeUpdated
        {
            get
            {
                return lastTimeUpdated.ToUniversalTime().ToBinary().ToString();
            }
            set
            {
                lastTimeUpdated = DateTime.FromBinary((long)Convert.ToUInt64(value)).ToLocalTime();
            }
        }


        #endregion

        #region Constructor

        public Group(bool isPrivate, string name, bool isChannel, string image = "", int id = 0, List<int> adminsId = null, List<int> members = null, int usersOnline = 0)
        {
            Id = id;
            AdminsIdList = adminsId ?? new List<int>();
            MembersIdList = members ?? new List<int>();
            this.isPrivate = isPrivate;
            this.isChannel = isChannel;
            LocalLastTimeUpdated = DateTime.Now;
            Name = name;
            UsersOnline = usersOnline;
            Image = image;
        }

        public Group(Group gr)
        {
            Id = gr.Id;
            AdminsIdList = gr.AdminsIdList;
            MembersIdList = gr.MembersIdList;
            this.isPrivate = gr.isPrivate;
            this.isChannel = gr.isChannel;
            LocalLastTimeUpdated = gr.LocalLastTimeUpdated;
            Name = gr.Name;
            UsersOnline = gr.UsersOnline;
            Image = gr.Image;
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
