using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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

        /// <summary>
        /// Removes chat from the user chat list
        /// </summary>
        public static ICommand LeaveChat { get; set; }

        /// <summary>
        /// Adds chat to the user chat list
        /// </summary>
        public static ICommand JoinChat { get; set; }

        /// <summary>
        /// Deletes user from chat
        /// </summary>
        public static ICommand RemoveUserFromChat { get; set; }

        /// <summary>
        /// Invites user to the chat
        /// </summary>
        public static ICommand InviteUserToChat { get; set; }

        /// <summary>
        /// Opens file using full path
        /// </summary>
        public static ICommand OpenFile { get; set; }

        

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
            JoinChat = new RelayCommandParametrized((args) => joinChat(args));
            LeaveChat = new RelayCommandParametrized((args) => leaveChat(args));
            RemoveUserFromChat = new RelayCommandParametrized((args) => removeUserFromChat(args));
            InviteUserToChat = new RelayCommandParametrized((args) => inviteUserToChat(args));
            OpenFile = new RelayCommandParametrized((args) => openFile(args));

        }

        #endregion

        #region Private Methods

        static void openFile(object data)
        {
            Process.Start((string)data);
        }

        static void inviteUserToChat(object data)
        {
            Contact user = (Contact)data;
            Group group = ApplicationService.GetCurrentChoosenChat;

            //TODO invite
        }

        static async void removeUserFromChat(object data)
        {
            //Get contact to remove
            Contact user = (Contact)data;

            //Get current open chat to remove from
            var group = ApplicationService.GetCurrentChoosenChat;

            await UnitOfWork.RemoveUserFromChat(group, user);

        }

        static void openGroupInfo(object group)
        {
            ApplicationService.ChangeCurrentChat((Group)group);
            ApplicationService.ChangeCurrentChatPage(ChatPages.ChatInfo);
            ApplicationService.ChangeCurrentContact(null);
        }

        static async void leaveChat(object group)
        {
            var gr = (Group)group;

            await UnitOfWork.LeaveGroup(gr);

        }

        static async void joinChat(object group)
        {
            var gr = (Group)group;

            await UnitOfWork.JoinGroup(gr);
        }

        static void openChat(object group)
        {
            ApplicationService.ChangeCurrentChat((Group)group);
            ApplicationService.ChangeCurrentChatPage(ChatPages.Chat);
        }

        static async void addToContactList(object contact)
        {
            var error = await UnitOfWork.ChangeUserInfo(UnitOfWork.User);

            //If there is no error
            if(error == null)
            {

            UnitOfWork.User.AddNewContact((Contact)contact);
            UnitOfWork.OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<Contact>() { (Contact)contact }, UsersTableFields.ContactsId.ToString(), RepositoryActions.Add));

            //Save contact to db
            UnitOfWork.Database.ContactsTableRepo.Add((Contact)contact);

            }

        }

        static async void deleteFromContactsList(object contact)
        {

            var error = await UnitOfWork.ChangeUserInfo(UnitOfWork.User);

            //If there is no error
            if (error == null)
            {

                //Remove contact
                UnitOfWork.User.RemoveContact((Contact)contact);
                UnitOfWork.OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<Contact>() { (Contact)contact }, UsersTableFields.ContactsId.ToString(), RepositoryActions.Remove));

                //Delete from db
                UnitOfWork.Database.ContactsTableRepo.Remove(ContactsTableFields.Id.ToString(), (contact as Contact).Id.ToString());
            }
        }

        static async void startChat(object contact)
        {
            var user = (Contact)contact;

            //Get all private groups
            var groups = UnitOfWork.Database.GroupsTableRepo.Find(GroupsTableFields.IsPrivate.ToString(), "true");

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
            await UnitOfWork.CreateGroup(new Group(true, user.UserName + " " + UnitOfWork.User.UserName + " Chat", false, user.ProfilePhoto, 0, new List<int>(), new List<int>() { user.Id, UnitOfWork.User.Id }, user.Online == "true" ? 2 : 1));
            ApplicationService.ChangeCurrentChat(UnitOfWork.Database.GroupsTableRepo.GetLast());
            ApplicationService.ChangeCurrentChatPage(ChatPages.Chat);
        }

        static void openUserInfo(object contact)
        {
            ApplicationService.ChangeCurrentContact((Contact)contact);
            ApplicationService.ChangeCurrentChatPage(ChatPages.UserInfo);
        }

        #endregion

    }
}
