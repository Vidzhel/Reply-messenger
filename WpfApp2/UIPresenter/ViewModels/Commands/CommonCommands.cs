using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
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

        }

        #endregion

        #region Private Methods

        static void inviteUserToChat(object data)
        {
            Contact user = (Contact)data;
            Group group = ApplicationService.GetCurrentChoosenChat;

            //TODO invite
        }

        static void removeUserFromChat(object data)
        {
            //Get contact to remove
            Contact user = (Contact)data;

            //Get current open chat to remove from
            var group = ApplicationService.GetCurrentChoosenChat;

            UnitOfWork.RemoveUserFromChat(group, user);

            ////Remove user from group
            //group.RemoveMember(user.Id);
            //group.RemoveAdmin(user.Id);

            ////Update db
            //UnitOfWork.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), group.Id.ToString(), group);
        }

        static void openGroupInfo(object group)
        {
            ApplicationService.ChangeCurrentChat((Group)group);
            ApplicationService.ChangeCurrentChatPage(ChatPages.ChatInfo);
            ApplicationService.ChangeCurrentContact(null);
        }

        static void leaveChat(object group)
        {
            var gr = (Group)group;

            //If user hasn't joinde to the chat
            if (!UnitOfWork.User.chatsIdList.Contains(gr.Id))
                return;

            UnitOfWork.LeaveGroup(gr);

            ////Remove chat
            //UnitOfWork.User.RemoveChat(gr);
            //UnitOfWork.OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<Group>() { gr }, UsersTableFields.ChatsId.ToString(), RepositoryActions.Remove));

            ////Update group
            //gr.MembersIdList.Remove(UnitOfWork.User.Id);
            //gr.AdminsIdList.Remove(UnitOfWork.User.Id);
            //UnitOfWork.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), gr.Id.ToString(), gr);

            ////Delete From db
            //UnitOfWork.GroupsTableRepo.Remove(GroupsTableFields.Id.ToString(), gr.Id.ToString());
        }

        static void joinChat(object group)
        {
            var gr = (Group)group;

            //If user has already joined the group
            if (UnitOfWork.User.chatsIdList.Contains(gr.Id))
                return;

            //Add chat
            UnitOfWork.User.AddNewChat(gr);

            //Add Chat to db
            UnitOfWork.GroupsTableRepo.Add(gr);


            //Update group
            gr.MembersIdList.Add(UnitOfWork.User.Id);
            UnitOfWork.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), gr.Id.ToString(), gr);
        }

        static void openChat(object group)
        {
            ApplicationService.ChangeCurrentChat((Group)group);
            ApplicationService.ChangeCurrentChatPage(ChatPages.Chat);
        }

        static void addToContactList(object contact)
        {
            //Add to contact list
            UnitOfWork.User.AddNewContact((Contact)contact);
            UnitOfWork.OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<Contact>() { (Contact)contact }, UsersTableFields.ContactsId.ToString(), RepositoryActions.Add));

        }

        static void deleteFromContactsList(object contact)
        {
            //Remove contact
            UnitOfWork.User.RemoveContact((Contact)contact);
            UnitOfWork.OnUserUpdated(null, new DataChangedArgs<IEnumerable<object>>(new List<Contact>() { (Contact)contact }, UsersTableFields.ContactsId.ToString(), RepositoryActions.Remove));

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
