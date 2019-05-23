using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UI.InversionOfControl;
using UI.UIPresenter.ViewModels.Commands;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class ChatUserControlViewModel : BaseViewModel
    {
        
        #region Public Members

        public Contact Contact { get; set; }

        public Group CurrentChat { get; set; }

        /// <summary>
        /// Command for send message button
        /// </summary>
        public ICommand SendMessage { set; get; }

        /// <summary>
        /// Toggle invite list visibility
        /// </summary>
        public ICommand InviteListButton { set; get; }

        /// <summary>
        /// If true shows invite list
        /// </summary>
        public bool ShowInviteList{ get; set; }

        /// <summary>
        /// Contain text of message from Typing Box
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// List of all messages in the chat
        /// </summary>
        public MessageListViewModel MessageList { get; set; } = new MessageListViewModel();

        /// <summary>
        /// Invite list contacts
        /// </summary>
        public ContactsListViewModel ContactsInviteList { get; set; } = new ContactsListViewModel();

        /// <summary>
        /// Gets name of the group
        /// </summary>
        public string ChatName => CurrentChat?.Name;

        /// <summary>
        /// Gets count of online users except you
        /// </summary>
        public int UsersOnline => CurrentChat != null ? CurrentChat.UsersOnline - 1: 0;

        /// <summary>
        /// If true all users can send messages in the group
        /// </summary>
        public bool IsChat => CurrentChat != null? !CurrentChat.isChannel: false;


        #endregion

        #region Public Methods

        /// <summary>
        /// Changes current chat
        /// </summary>
        /// <param name="group"></param>
        public void ChangeChat(Group group)
        {
            CurrentChat = group;

            //Cleat Message list and load messages
            MessageList = new MessageListViewModel();
            loadMessages();

            //Update UI
            OnPropertyChanged("ChatName");
            OnPropertyChanged("UsersOnline");
            OnPropertyChanged("IsChat");
        }

        #endregion

        #region Constructor

        public ChatUserControlViewModel()
        {
            //Set up handelrs
            ApplicationService.GetChatViewModel.OnCurrentChatChanged((sender, args) => ChangeChat(args));
            UnitOfWork.MessagesTableRepo.AddDataChangedHandler((sender, args) => OnMessagesTableRepoChanged(sender, args));
            UnitOfWork.GroupsTableRepo.AddDataChangedHandler((sender, args) => OnGroupsTableRepoChanged(sender, args));

            //Set up commands
            SendMessage = new RelayCommand(() => sendMessage());
            InviteListButton = new RelayCommand(() => inviteListButtonClick());

            //Set current chat
            ChangeChat(ApplicationService.GetCurrentChoosenChat);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Togle visibility of invite list
        /// </summary>
        void inviteListButtonClick()
        {

            ShowInviteList = !ShowInviteList;

            if(ShowInviteList)
                //Load contacts
                ContactsInviteList = new ContactsListViewModel(UnitOfWork.GetUsersInfo(new List<int>(UnitOfWork.User.contactsIdList)), false, true);
        }

        void OnGroupsTableRepoChanged(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Update:
                    UpdateGroup((List<Group>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    RemoveGroup((List<Group>)args.Data);
                    break;
                default:
                    break;
            }
        }

        private void RemoveGroup(List<Group> dataChanged)
        {
            foreach (var data in dataChanged)
            {
                //if deleted current chat
                if(CurrentChat.Id == data.Id)
                {
                    //Open user info page
                    CommonCommands.OpenUserInfo.Equals(UnitOfWork.User);
                    return;
                }

            }
        }

        private void UpdateGroup(List<Group> dataChanged)
        {
            foreach (var data in dataChanged)
                //If the group updated
                if (data.Id == CurrentChat.Id)
                {
                    //Set new chat
                    CurrentChat = data;

                    //Update UI
                    OnPropertyChanged("ChatName");
                    OnPropertyChanged("UsersOnline");
                    OnPropertyChanged("IsChat");
                }
        }

        void OnMessagesTableRepoChanged(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {
            if (CurrentChat == null)
                return;

            switch (args.Action)
            {
                case RepositoryActions.Add:
                    AddMessagesToMessagesList((List<Message>)args.Data);
                    break;
                case RepositoryActions.Update:
                    UpdateMessagesInMessagesList((List<Message>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    RemoveMessagesFromMessagesList((List<Message>)args.Data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Updates messages if they from the chat
        /// </summary>
        /// <param name="dataChanged"></param>
        private void UpdateMessagesInMessagesList(List<Message> dataChanged)
        {
            if (dataChanged == null)
                return;

            foreach (var data in dataChanged)
            {

                //If message from the chat
                if (data.ReceiverId == CurrentChat.Id)

                    //Find the message to update
                    for (int i = 0; i < MessageList.Items.Count; i++)
                    {

                        if (MessageList.Items[i].Message.Id == data.Id)
                        {
                            MessageList.Items[i].Message = data;
                            break;
                        }
                    }


            }
        }

        /// <summary>
        /// Removes messages if they from the chat
        /// </summary>
        /// <param name="dataChanged"></param>
        private void RemoveMessagesFromMessagesList(List<Message> dataChanged)
        {
            if (dataChanged == null)
                return;

            foreach (var data in dataChanged)
            {

                //If message from the chat
                if(data.ReceiverId == CurrentChat.Id)

                    //Find the message to delete
                    for(int i = 0; i < MessageList.Items.Count; i++)
                    {

                        if (MessageList.Items[i].Message.Id == data.Id)
                        {
                            MessageList.Items.RemoveAt(i);
                            break;
                        }
                    }


            }
        }

        /// <summary>
        /// Adds messages if they from the chat 
        /// </summary>
        /// <param name="dataChanged"></param>
        private void AddMessagesToMessagesList(List<Message> dataChanged)
        {
            if (dataChanged == null)
                return;

            foreach (var data in dataChanged)
            {
                if (CurrentChat.Id == data.ReceiverId)
                {
                    //get user info
                    var user = UnitOfWork.GetUsersInfo(new List<int>() { data.SenderId });

                    //Becouse Items is ObservableCollection we should update elements from the main thread
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        MessageList.Items.Add(new MessageListItemViewModel(user[0], data, UnitOfWork.User.Id == data.SenderId, CurrentChat.IsPrivateChat));
                    });

                }
            }

        }

        /// <summary>
        /// Loads all messages from the data base
        /// </summary>
        void loadMessages()
        {
            //Get all messages whick match to the group Id
            var messages = UnitOfWork.MessagesTableRepo.Find(MessagesTableFields.ReceiverId.ToString(), CurrentChat?.Id.ToString());

            //Get all users data from server
            var users = UnitOfWork.GetUsersInfo(new List<int>(CurrentChat.MembersIdList));


            foreach (var message in messages)
                foreach (var user in users)
                    if(user.Id == message.SenderId)
                    {
                        //If it's chat and it's not you, than add to contact
                        if (CurrentChat.IsPrivateChat)
                            if (UnitOfWork.User.Email != user.Email)
                                Contact = user;

                        //Becouse Items is ObservableCollection we should update elements from the main thread
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            MessageList.Items.Add(new MessageListItemViewModel(user, message, user.Email == UnitOfWork.User.Email, CurrentChat.IsPrivateChat));
                        });
                    }
        }

        /// <summary>
        /// Sends message
        /// </summary>
        void sendMessage()
        {
            if (CurrentChat == null || this.MessageContent == null)
                return;

            //Delete unnecessary spaces
            var text = System.Text.RegularExpressions.Regex.Replace(MessageContent, @"^(\s*)(\S*)(\s*)$", "$2");

            //Add message to repository
            UnitOfWork.SendMessage(new Message(UnitOfWork.User.Id, CurrentChat.Id, DataType.Text, DateTime.Now, text));

            MessageContent = "";
        }

        #endregion
    }
}
