using ClientLibs.Core.DataAccess;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UI.InversionOfControl;

namespace UI.UIPresenter.ViewModels
{
    public class ChatListItemViewModel : BaseViewModel
    {

        #region Public Members

        //Contact info, user name, bio, eth..
        public Group GroupData { get; set; }

        //Last message info
        public Message LastMessage { get; set; }

        /// <summary>
        /// Return last message info
        /// </summary>
        public string LastMessageText {
            get { 
                switch (LastMessage?.DataType)
                {
                    // TODO change "File" ont File name

                    //Return different info depene on data type
                    case DataType.File:
                            return "File";

                    case DataType.Image:
                            return "Photo";

                    case DataType.Text:
                        {
                            if (LastMessage.Data != String.Empty)
                                return LastMessage.Data;
                            else
                                return LastMessage.AttachmentsList[LastMessage.AttachmentsList.Count - 1];
                        }

                    default:
                            return "";
                }
            }
        }


        public string GroupPhoto
        {
            get
            {
                var list = UnitOfWork.GetFilesByName(new List<string>() { GroupData.Image});
                return (list.Result)[0];
            }
        }


        public string UserName => GroupData?.Name;
        
        public DateTime LastMessageTime { get {
                if (LastMessage != null)
                    return LastMessage.LocalDate;

                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// If the message sent by the user
        /// </summary>
        public bool IsYourMessage => UnitOfWork.User.Id == LastMessage?.SenderId;

        /// <summary>
        /// If group has 2 memebers, then will show is user online
        /// </summary>
        public bool IsOnline {
            get {
                if (GroupData.IsPrivateChat && GroupData.UsersOnline == 2)
                    return true;
                
               return false;
            }
        }

        public bool IsSelected { get; set; }

        /// <summary>
        /// Returns Font Awesome icon depence on <see cref="CommonLibs.Data.MessageStatus"/> 
        /// </summary>
        public MessageStatus MessageStatus{
            get
            {
                if (!IsYourMessage)
                    return MessageStatus.Null;

                if (LastMessage != null)
                    return LastMessage.Status;
                else
                    return MessageStatus.Null;
            }
        }

        //public string UserPhotoSource => ContactData.ProfilePhoto;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact">Contact data</param>
        /// <param name="message">Last message in the chat</param>
        public ChatListItemViewModel(Group group, Message message)
        {
            //Set up hadlers
            ApplicationService.GetChatViewModel.OnCurrentChatChanged((sender, args) => OnCurrentChatChanged(args));

            GroupData = group;
            LastMessage = message;
        }

        public ChatListItemViewModel()
        {

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Selects the chat list item if it's the right chat
        /// </summary>
        /// <param name="group"></param>
        void OnCurrentChatChanged(Group group)
        {
            // If no one group is selected
            if (group == null)
            {
                IsSelected = false;
                return;
            }

            // If selected this group
            if (group.Id == GroupData.Id)
            {
                IsSelected = true;
                return;
            }

            IsSelected = false;
        }

        #endregion

    }
}
