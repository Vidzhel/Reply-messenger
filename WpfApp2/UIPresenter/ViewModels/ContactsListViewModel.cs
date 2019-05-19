using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UI.UIPresenter.ViewModels
{
    public class ContactsListViewModel : BaseViewModel
    {

        #region Public Member

        public ObservableCollection<ContactsListItemViewModel> Items { get; set; } = new ObservableCollection<ContactsListItemViewModel>();

        #endregion

        #region Constructor

        public ContactsListViewModel(List<Contact> contacts)
        {
            //Add event handler
            UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnContactsListChanged(sender, args));

            foreach (var contact in contacts)
                Items.Add(new ContactsListItemViewModel(contact));
        }

        public ContactsListViewModel()
        {

            //Add event handler
            UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnContactsListChanged(sender, args));

        }

        #endregion


        #region Private Methods

        void OnContactsListChanged(object sender, DataChangedArgs<IEnumerable<object>> args)
        {
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

                if(add)
                    Items.Add(new ContactsListItemViewModel(contact));
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
                        Items.Remove(item);
                        break;
                    }

                }

            }
        }

        #endregion
    }
}
