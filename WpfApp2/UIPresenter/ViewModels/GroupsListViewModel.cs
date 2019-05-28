using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UI.UIPresenter.ViewModels
{
    public class GroupsListViewModel : BaseViewModel
    {

        #region Public Member

        public ObservableCollection<GroupsListItemViewModel> Items { get; set; } = new ObservableCollection<GroupsListItemViewModel>();

        #endregion

        #region Constructor

        public GroupsListViewModel(List<Group> contacts)
        {
            //Add event handler
            UnitOfWork.Database.GroupsTableRepo.AddDataChangedHandler((sender, args) => OnGroupsListChanged(sender, args));

            if(contacts != null)
                foreach (var contact in contacts)
                    Items.Add(new GroupsListItemViewModel(contact));
        }

        public GroupsListViewModel()
        {

            //Add event handler
            UnitOfWork.Database.GroupsTableRepo.AddDataChangedHandler((sender, args) => OnGroupsListChanged(sender, args));

        }

        #endregion


        #region Private Methods

        void OnGroupsListChanged(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Update:
                    UpdateGroups((List<Group>)args.Data);
                    break;
                default:
                    break;
            }
        }
        
        public void UpdateGroups(List<Group> groups)
        {
            foreach (var group in groups)
            {
                //Search for the contact
                foreach (var item in Items)
                {
                    
                    if (item.GroupInfo.Id == group.Id)
                    {
                        //Update user info
                        item.UpdateUser(group);
                        break;
                    }

                }

            }
        }
        
        #endregion
    }
}
