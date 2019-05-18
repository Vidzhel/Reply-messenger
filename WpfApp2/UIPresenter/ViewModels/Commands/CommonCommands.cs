using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UI.InversionOfControl;
using UI.Pages;

namespace UI.UIPresenter.ViewModels.Commands
{
    public class CommonCommands
    {

        #region Public Members

        /// <summary>
        /// Adds new contact to the contact list
        /// </summary>
        public static ICommand AddToContactList { get; set; }

        /// <summary>
        /// Deletes contact from the contact list
        /// </summary>
        public static ICommand DeleteFromContactList { get; set; }

        /// <summary>
        /// Creates new chat with user
        /// </summary>
        public static ICommand StartChat { get; set; }

        /// <summary>
        /// Opens users info page
        /// </summary>
        public static ICommand OpenUserInfo { get; set; }

        /// <summary>
        /// Opens chat page
        /// </summary>
        public static ICommand OpenChat { get; set; }
        
        /// <summary>
        /// Opens Group info
        /// </summary>
        public static ICommand OpenGroupInfo { get; set; }

        

        #endregion

        #region Constructor

        static CommonCommands()
        {

            //Setup commands
            AddToContactList = new RelayCommandParametrized((args) => addToContactList(args));
            DeleteFromContactList = new RelayCommandParametrized((args) => deleteFromContactsList(args));
            StartChat = new RelayCommandParametrized((args) => startChat(args));
            OpenUserInfo = new RelayCommandParametrized((args) => openUserInfo(args));
            OpenChat = new RelayCommandParametrized((args) => openChat(args));
            OpenGroupInfo = new RelayCommandParametrized((args) => openGroupInfo(args));

        }

        #endregion

        #region Private Methods

        static void openGroupInfo(object group)
        {

        }

        static void openChat(object group)
        {
            ApplicationService.ChangeCurrentChat((Group)group);
            ApplicationService.ChangeCurrentChatPage(ChatPages.Chat);
        }

        static void addToContactList(object contact)
        {
            //Add to contact list
            UnitOfWork.ContactsTableRepo.Add((Contact)contact);
        }

        static void deleteFromContactsList(object contact)
        {
            //Ckeck data base for the contact
            if (UnitOfWork.ContactsTableRepo.IsExists(ContactsTableFields.Id.ToString(), ((Contact)contact).Id.ToString()))

                //Delete contact
                UnitOfWork.ContactsTableRepo.Remove(ContactsTableFields.Id.ToString(), (contact as Contact).Id.ToString());
        }

        static void startChat(object contact)
        {
            var user = (Contact)contact;

            //Get all private groups
            var groups = UnitOfWork.GroupsTableRepo.Find(GroupsTableFields.IsPrivate.ToString(), "True");

            //Find group with 2 users
            foreach (var group in groups)
            {
                //If there already is the group, then open it
                if (group.MembersIdList.Count == 2)
                    if (group.MembersIdList.Contains(user.Id))
                    {
                        ApplicationService.ChangeCurrentChat(group);
                        ApplicationService.ChangeCurrentChatPage(ChatPages.Chat);
                        return;
                    }
            }

            //Create chat and open it
            UnitOfWork.GroupsTableRepo.Add(new Group(true, user.UserName, false, user.ProfilePhoto, -1, null, new List<int>() { UnitOfWork.User.Id, user.Id }));
            ApplicationService.ChangeCurrentChatPage(ChatPages.Chat);
            ApplicationService.ChangeCurrentChat(UnitOfWork.GroupsTableRepo.FindLast(GroupsTableFields.Id.ToString(), "-1"));
        }

        static void openUserInfo(object contact)
        {
            ApplicationService.ChangeCurrentContact((Contact)contact);
            ApplicationService.ChangeCurrentChatPage(ChatPages.UserInfo);
        }
       
        #endregion

    }
}
