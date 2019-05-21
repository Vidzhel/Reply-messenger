using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.InversionOfControl;

namespace UI.UIPresenter.ViewModels
{
    public class SearchResultsViewModel : BaseViewModel
    {

        public ContactsListViewModel UsersSearchResults { get; set; } = new ContactsListViewModel();

        public GroupsListViewModel GroupsSearchResults { get; set; } = new GroupsListViewModel();

        #region Constructor

        public SearchResultsViewModel()
        {
            //Setup handlers
            ApplicationService.GetChatViewModel.OnUsersSearchResultChanged((sender, args) => onUsersSearchResultChanged(args));
            ApplicationService.GetChatViewModel.OnGroupSearchResultChanged((sender, args) => onGroupSearchResultChanged(args));

            //Set results
            var users = ApplicationService.UsersSearchResult;
            var groups = ApplicationService.GroupsSearchResult;

            if (users != null)
                UsersSearchResults = new ContactsListViewModel(users);

            if (groups != null)
                GroupsSearchResults = new GroupsListViewModel(groups);

        }

        #endregion

        #region Private Methods

        void onUsersSearchResultChanged(List<Contact> dataChanged)
        {
            UsersSearchResults = new ContactsListViewModel(dataChanged);
        }

        void onGroupSearchResultChanged(List<Group> dataChanged)
        {
            GroupsSearchResults = new GroupsListViewModel(dataChanged);
        }
        #endregion
    }
}
