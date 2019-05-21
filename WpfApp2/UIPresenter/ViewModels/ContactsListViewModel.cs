using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class ContactsListViewModel : BaseViewModel
    {

        #region Public Member

        public ObservableCollection<ContactsListItemViewModel> Items { get; set; } = new ObservableCollection<ContactsListItemViewModel>();

        public bool IsGroupContactsList { get; set; }

        public bool IsInviteList { get; set; }

        public bool AreYouAdmin { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts">contacts to add</param>
        /// <param name="isGroupContactsList">If true provide button to remove user from a group</param>
        /// <param name="isInviteList">If true provide button to invite user to a group</param>
        public ContactsListViewModel(List<Contact> contacts, bool isGroupContactsList = false, bool isInviteList = false, bool areYouAdmin = false)
        {
            IsGroupContactsList = isGroupContactsList;
            IsInviteList = isInviteList;
            AreYouAdmin = areYouAdmin;

            //Add event handler
            UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnContactsListChanged(sender, args));

            foreach (var contact in contacts)
                Items.Add(new ContactsListItemViewModel(contact, isGroupContactsList, isInviteList, areYouAdmin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isGroupContactsList">If true provide button to remove user from a group</param>
        /// <param name="isInviteList">If true provide button to invite user to a group</param>
        public ContactsListViewModel(bool isGroupContactsList = false, bool isInviteList = false, bool areYouAdmin = false)
        {
            IsGroupContactsList = isGroupContactsList;
            IsInviteList = isInviteList;
            AreYouAdmin = areYouAdmin;

            //Add event handler
            UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnContactsListChanged(sender, args));

        }

        #endregion


        #region Private Methods

        void OnContactsListChanged(object sender, DataChangedArgs<IEnumerable<object>> args)
        {
            //Check specific action
            if(args.ExtraInfo == UsersTableFields.ContactsId.ToString())
                switch (args.Action)
                {
                    case RepositoryActions.Add:
                        AddContacts((List<Contact>)args.Data);
                        break;
                    case RepositoryActions.Update:
                        UpdateContacts((List<Contact>)args.Data);
                        break;
                    case RepositoryActions.Remove:
                        RemoveContacts((List<Contact>)args.Data);
                        break;
                    default:
                        break;
                }

            //Else update yourself
            else
            {
                UpdateContacts(new List<Contact>() { UnitOfWork.Contact });
            }

        }

        public void AddContacts(List<Contact> contacts)
        {
            bool add;

            foreach (var contact in contacts)
            {
                add = true;
                //Check if there isn't the contact
                foreach (var item in Items)
                {
                    if(contact.Id == item.UserInfo.Id)
                    {
                        item.UpdateUser(contact);
                        add = false;
                        break;
                    }
                }

                if (add)
                    //Becouse Items is ObservableCollection we should update elements from the main thread
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Items.Add(new ContactsListItemViewModel(contact, IsGroupContactsList, IsInviteList, AreYouAdmin));
                    });
            }
        }

        public void UpdateContacts(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                //Search for the contact
                foreach (var item in Items)
                {
                    
                    if (item.UserInfo.Id == contact.Id)
                    {
                        //Update user info
                        item.UpdateUser(contact);
                        break;
                    }

                }

            }
        }
        
        public void RemoveContacts(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                //Search for the contact
                foreach (var item in Items)
                {

                    if (item.UserInfo.Id == contact.Id)
                    {
                        //Delate contact
                        //Becouse Items is ObservableCollection we should update elements from the main thread
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            Items.Remove(item);
                        });
                        break;
                    }

                }

            }
        }

        #endregion
    }
}
