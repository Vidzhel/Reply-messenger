using CommonLibs.Data;
using System;

namespace UI.UIPresenter.ViewModels
{
    public class MessageListItemViewModel : BaseViewModel
    {


        #region Private Members

        Contact contactData;
        Message message;

        #endregion

        #region Public Members


        //Contact info, user name, bio, eth..
        public Contact ContactData
        {
            get { return contactData; }
            set
            {
                contactData = value;
            }
        }

        //message info
        public Message Message
        {
            get { return message; }
            set
            {
                message = value;
            }
        }


        /// <summary>
        /// Return message text
        /// </summary>
        public string MessageText
        {
            get
            {
                switch (Message?.DataType)
                {
                    // TODO change "File" ont File name

                    //Return different info depene on data type
                    case DataType.File:
                        return "File";

                    case DataType.Image:
                        return "Photo";

                    case DataType.Text:
                        return Message.Data;

                    default:
                        return "error";
                }
            }
        }


        /// <summary>
        /// Get User Name
        /// </summary>
        public string UserName => ContactData?.UserName;

        public DateTime MessageTime
        {
            get
            {
                if (Message != null)
                    return Message.Date;
                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// Returns Font Awesome icon depence on <see cref="CommonLibs.Data.MessageStatus"/> 
        /// </summary>
        public MessageStatus MessageStatus
        {
            get
            {
                if (Message != null)
                    return Message.Status;
                else
                    return MessageStatus.SendingInProgress;
            }
        }

        /// <summary>
        /// Display User name if it isn't your message and in the chat more than two members
        /// </summary>
        public bool DisplayUserName => !IsYourMessage && !IsTwoMembersInTheChat;

        /// <summary>
        /// If true, than change background color of message and alignment
        /// </summary>
        public bool IsYourMessage{ get; set; }

        /// <summary>
        /// If true, than don't show Users name
        /// </summary>
        public bool IsTwoMembersInTheChat{ get; set; }

        //public string UserPhotoSource => ContactData.ProfilePhoto;

        #endregion

        #region Constructor
            
        public MessageListItemViewModel(Contact contact, Message message, bool isYourMessage, bool isTwoMembersInTheChat)
        {
            ContactData = contact;
            Message = message;
            IsYourMessage = isYourMessage;
            IsTwoMembersInTheChat = isTwoMembersInTheChat;
        }

        public MessageListItemViewModel()
        {

        }
        #endregion
    }
}
