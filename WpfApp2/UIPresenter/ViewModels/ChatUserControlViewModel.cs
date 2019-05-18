using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UI.InversionOfControl;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class ChatUserControlViewModel : BaseViewModel
    {

        #region Private Members

        Group currentChat;

        Contact contact;

        #endregion

        #region Public Members

        /// <summary>
        /// Command for send message button
        /// </summary>
        public ICommand SendMessage { set; get; }

        /// <summary>
        /// Contain text of message from Typing Box
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// List of all messages in the chat
        /// </summary>
        public MessageListViewModel MessageList { get; set; } = new MessageListViewModel();

        /// <summary>
        /// Gets name of the group
        /// </summary>
        public string ChatName => currentChat?.Name;

        /// <summary>
        /// Gets count of online users except you
        /// </summary>
        public int UsersOnline => currentChat != null ? currentChat.UsersOnline - 1: 0;

        /// <summary>
        /// Return true if in the chat are less or equal than 3 users
        /// </summary>
        public bool IsChat => currentChat != null? currentChat.IsChat : false;

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Changes current chat
        /// </summary>
        /// <param name="group"></param>
        public void ChangeChat(Group group)
        {
            currentChat = group;

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

            //Set current chat
            ChangeChat(ApplicationService.GetCurrentChoosenChat);
        }

        #endregion

        #region Private Methods

        void OnGroupsTableRepoChanged(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Add:
                    break;
                case RepositoryActions.Update:
                    UpdateGroup((List<Message>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    RemoveGroup((List<Message>)args.Data);
                    break;
                default:
                    break;
            }
        }

        private void RemoveGroup(List<Message> dataChanged)
        {
            throw new NotImplementedException();
        }

        private void UpdateGroup(List<Message> dataChanged)
        {
            throw new NotImplementedException();
        }

        void OnMessagesTableRepoChanged(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {
            if (currentChat == null)
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
                if (data.ReceiverId == currentChat.Id)

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
                if(data.ReceiverId == currentChat.Id)

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
                if (currentChat.Id == data.ReceiverId)
                {
                    //get user info
                    var user = UnitOfWork.GetUsersInfo(new List<int>() { data.SenderId });

                    //Becouse Items is ObservableCollection we should update elements from the main thread
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        MessageList.Items.Add(new MessageListItemViewModel(user[0], data, UnitOfWork.User.Id == data.SenderId, currentChat.IsChat));
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
            var messages = UnitOfWork.MessagesTableRepo.Find(MessagesTableFields.ReceiverId.ToString(), currentChat.Id.ToString());

            //Get all users data from server
            var users = UnitOfWork.GetUsersInfo(new List<int>(currentChat.MembersIdList));


            foreach (var message in messages)
                foreach (var user in users)
                    if(user.Id == message.SenderId)
                    {
                        //If it's chat and it's not you, than add to contact
                        if (currentChat.IsChat)
                            if (UnitOfWork.User.Email != user.Email)
                                contact = user;

                        //Becouse Items is ObservableCollection we should update elements from the main thread
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            MessageList.Items.Add(new MessageListItemViewModel(user, message, user.Email == UnitOfWork.User.Email, currentChat.IsChat));
                        });
                    }
        }

        void sendMessage()
        {
            if (currentChat == null || this.MessageContent == null)
                return;

            //Delete unnecessary spaces
            var text = System.Text.RegularExpressions.Regex.Replace(MessageContent, @"^(\s*)(\S*)(\s*)$", "$2");

            //Add message to repository
            UnitOfWork.MessagesTableRepo.Add(new Message(UnitOfWork.User.Id, currentChat.Id, DataType.Text, DateTime.Now, text));

            MessageContent = "";
        }

        #endregion
    }
}
