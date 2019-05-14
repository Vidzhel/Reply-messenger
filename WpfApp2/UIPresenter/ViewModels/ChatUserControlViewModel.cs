using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;

namespace UI.UIPresenter.ViewModels
{
    public class ChatUserControlViewModel
    {

        #region Private Members

        Group groupData;

        #endregion

        #region Public Members

        Contact contact;

        public MessageListViewModel MessageList { get; set; } = new MessageListViewModel();

        /// <summary>
        /// Gets name of the group
        /// </summary>
        public string ChatName => groupData.Name;

        /// <summary>
        /// Gets count of online users except you
        /// </summary>
        public int UsersOnline => groupData.UsersOnline - 1;
        //public string UsersOnline => groupData.IsChat ? contact.LastTimeOnline : (groupData.UsersOnline - 1).ToString();

        public bool IsChat => groupData.IsChat;

        #endregion

        #region Constructor

        public ChatUserControlViewModel(Group group)
        {
            groupData = group;

            //TODO delete
            var user = new Contact("VidzhelNeSuka", "myemail.com", "something there");
            var user1 = new Contact("Oleg", "myemail.com", "somthing there", null, "false");

            var message = new Message(10, 20, DataType.Text, new DateTime(2018, 2, 25), "Hi, how do you do?", MessageStatus.Sended);
            var message1 = new Message(10, 20, DataType.Image, DateTime.Now, "file source", MessageStatus.SendingInProgress);
            var message2 = new Message(10, 20, DataType.Text, DateTime.Now, "Ohh, thanks for the pressent, i very appreciated", MessageStatus.IsRead);


            MessageList.Items = new List<MessageListItemViewModel> {

                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, true, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, true, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user1, message1, true, true),
                new MessageListItemViewModel(user, message2, false, true)

            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all messages from the data base
        /// </summary>
        void loadMessages()
        {
            //Get all messages whick match to the croup Id
            var messages = UnitOfWork.MessagesTableRepo.Find(MessagesTableFields.ReceiverId.ToString(), groupData.Id.ToString());

            //Get all users data from server
            var users = UnitOfWork.GetUsersInfo(groupData.MembersIdList);


            foreach (var message in messages)
                foreach (var user in users)
                    //if user Id match to sender Id, than add message to chat
                    if (user.Id == message.SenderId)
                    {
                        //If it's chat and it's not you, than add to contact
                        if (groupData.IsChat)
                            if (UnitOfWork.User.Email != user.Email)
                                contact = user;

                        MessageList.Items.Add(new MessageListItemViewModel(user, message, user.Email == UnitOfWork.User.Email, groupData.IsChat));
                    }
        }

        #endregion
    }
}
