using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;

namespace UI.UIPresenter.ViewModels
{
    public class GroupsListItemViewModel : BaseViewModel
    {

        #region Public Members
        
        /// <summary>
        /// Group info to display
        /// </summary>
        public Group GroupInfo { get; set; }

        /// <summary>
        /// True if the user is in your contact list
        /// </summary>
        public bool IsYourGroup => UnitOfWork.User.chatsIdList.Contains(GroupInfo.Id);
        
        /// <summary>
        /// Gets user name
        /// </summary>
        public string GroupName => GroupInfo.Name;

        #endregion

        #region Constructor

        public GroupsListItemViewModel(Group group)
        {
            UpdateUser(group);
        }

        #endregion

        #region Public Methods

        public void UpdateUser(Group group)
        {
            GroupInfo = group;

            //Check data base

            //update UI
            OnPropertyChanged("GroupName");
            OnPropertyChanged("IsYourGroup");
        }

        #endregion

    }
}
