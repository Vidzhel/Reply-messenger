using CommonLibs.Data;
using PropertyChanged;
using System;

namespace UI.UIPresenter.ViewModels
{
    public class ChatListItemViewModel : BaseViewModel
    {

        #region Private Members

        Contact contactData;
        Message lastMessage;

        #endregion

        #region Public Members


        //Contact info, user name, bio, eth..
        public Contact ContactData { get { return contactData; }
            set
            {
                contactData = value;
            } }

        //Last message info
        public Message LastMessage { get { return lastMessage; }
            set {
                lastMessage = value;
            } }

        /// <summary>
        /// Get User Name
        /// </summary>
        
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
                            return LastMessage.Data;

                    default:
                            return "error";
                }
            }
        }
        
        public string UserName => ContactData?.UserName;
        
        public DateTime LastMessageTime { get {
                if (LastMessage != null)
                    return LastMessage.Date;
                else
                    return DateTime.Now;
            }
        }

        public bool IsOnline {
            get {
                if (contactData == null)
                    return false;


                if (ContactData.Online.Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                else
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
                if (LastMessage != null)
                    return LastMessage.Status;
                else
                    return MessageStatus.SendingInProgress;
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
        public ChatListItemViewModel(Contact contact, Message message, bool IsSelected)
        {
            ContactData = contact;
            LastMessage = message;
            this.IsSelected = IsSelected;
        }

        public ChatListItemViewModel()
        {

        }

        #endregion
    }
}
