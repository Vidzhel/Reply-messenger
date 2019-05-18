using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.InversionOfControl;
using UI.Pages;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {

        #region Public Members

        /// <summary>
        /// store сurrent Choosen page and eth.
        /// </summary>
        public ChatViewModel ChatViewModel { get; set; } = ApplicationService.GetChatViewModel;

        public ChatListViewModel ChatList { get; set; } = new ChatListViewModel();

        #endregion

        #region Constructor

        public ChatPageViewModel()
        {
            //TODO delete
            //UnitOfWork.MessagesTableRepo.Add(new Message(1, 2, DataType.Text, DateTime.Now, "Hello world", MessageStatus.Sended));
            //UnitOfWork.MessagesTableRepo.Add(new Message(0, 2, DataType.Text, DateTime.Now, "It's me", MessageStatus.Sended, 1));
            //UnitOfWork.MessagesTableRepo.Add(new Message(0, 3, DataType.Text, DateTime.Now, "It's me", MessageStatus.Sended, 2));
            //UnitOfWork.MessagesTableRepo.Add(new Message(0, 4, DataType.Text, DateTime.Now, "It's me", MessageStatus.Sended, 3));
            //UnitOfWork.MessagesTableRepo.Add(new Message(0, 2, DataType.Text, DateTime.Now, "This is very and very long message that I use to check sddjhagj dhsaj dklsa jdlksa jdkls ajdlk sakdlj sadlka jsdlks ajdkl sajdlk sajdl ksajd lkas jd", MessageStatus.Sended, 7));
            //UnitOfWork.GroupsTableRepo.Add(new Group(false, "V Chat", false, "", 2, new List<int>() { 1 }, new List<int>() { 1, 0 }));
            //UnitOfWork.GroupsTableRepo.Add(new Group(false, "It's Mark", false, "", 3, new List<int>() { 1 }, new List<int>() { 1, 0 }));
            //UnitOfWork.GroupsTableRepo.Add(new Group(false, "Vlad", false, "", 4, new List<int>() { 1 }, new List<int>() { 1, 0 }));

            //Add all groups from the Groups repository and add them to chat list
            AddGroupToChatList((List<Group>)UnitOfWork.GroupsTableRepo.GetAll());

            //Add Handlers
            UnitOfWork.GroupsTableRepo.DataChanged += GroupRepoChangedHandler;
            UnitOfWork.MessagesTableRepo.DataChanged += MessageRepoChangedHandler;

            Task.Run(() => Check());
        }

        #endregion

        #region private Methods

        //TODO Delate
        async void Check()
        {
            await Task.Delay(10000);

            //UnitOfWork.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), "3", new Group(false, "It's not Mark", false, "", 3, new List<int>() { 1 }));
            //UnitOfWork.MessagesTableRepo.Add(new Message(0, 4, DataType.Text, DateTime.Now, "Hi Mark, how do you do, i haven't seen you for ages", MessageStatus.Sended, 4));
            //UnitOfWork.MessagesTableRepo.Add(new Message(1, 2, DataType.Text, DateTime.Now, "Hi new message", MessageStatus.Sended, 9));
            //UnitOfWork.MessagesTableRepo.Remove(MessagesTableFields.Id.ToString(), "4");
        }

        /// <summary>
        /// Handles changed messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void MessageRepoChangedHandler(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Add:
                    AddMessageToChatList((List<Message>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    RemoveMesageFromChatList((List<Message>)args.Data);
                    break;
                case RepositoryActions.Update:
                    RemoveMesageFromChatList((List<Message>)args.Data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Remove message and set new one (if it's the last message of the group)
        /// </summary>
        /// <param name="data"></param>
        private void RemoveMesageFromChatList(List<Message> dataChanged)
        {
            if (dataChanged == null)
                return;

            foreach (var data in dataChanged)
            {

                //Find the group to update last message
                foreach (var group in ChatList.Items)
                {
                    //If message from the group
                    if (group.GroupData.Id == data.Id)

                        //Check if it was the last message of the group
                        if (group.LastMessage.Id == data.Id)
                        {
                            //Get last message
                            group.LastMessage = UnitOfWork.MessagesTableRepo.FindLast(MessagesTableFields.ReceiverId.ToString(), data.ReceiverId.ToString()); ;
                        }

                }


            }

        }

        /// <summary>
        /// Update message (if it's the last message of the group)
        /// </summary>
        /// <param name="data"></param>
        private void AddMessageToChatList(List<Message> dataChanged)
        {
            if (dataChanged == null)
                return;

            foreach (var data in dataChanged)
            {
                //Check if it's the last message of the group
                var lastMess = UnitOfWork.MessagesTableRepo.FindLast(MessagesTableFields.ReceiverId.ToString(), data.ReceiverId.ToString());
                if(lastMess.Id == data.Id)
                {

                    //Find the group to update last message
                    foreach (var group in ChatList.Items)
                    {
                        //If message from the group, then update
                        if (group.GroupData.Id == data.ReceiverId)
                            group.LastMessage = data;

                    }


                }


            }

        }


        /// <summary>
        /// Handle groups repository changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GroupRepoChangedHandler(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
            switch (args.Action)
            {
                case RepositoryActions.Add:
                    AddGroupToChatList((List<Group>)args.Data);
                    break;
                case RepositoryActions.Remove:
                    RemoveGroupFromChatList((List<Group>)args.Data);
                    break;
                case RepositoryActions.Update:
                    UpdateGroupInChatList((List<Group>)args.Data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Updates groups
        /// </summary>
        /// <param name="dataChanged"></param>
        private void UpdateGroupInChatList(List<Group> dataChanged)
        {
            if (dataChanged == null)
                return;

            //For each updated group
            foreach (var data in dataChanged)
            {
                //Find the group with same Id
                foreach (var item in ChatList.Items)
                {
                    //And Replace
                    if (item.GroupData.Id == data.Id)
                    {
                        //Becouse Items is ObservableCollection we should update elements from the main thread
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            item.GroupData = data;
                        });

                        break;
                    }
                }

            }
        }

        /// <summary>
        /// Deletes groups
        /// </summary>
        /// <param name="dataChanged"></param>
        void RemoveGroupFromChatList(List<Group> dataChanged)
        {
            if (dataChanged == null)
                return;

            //For each updated group
            foreach (var data in dataChanged)
            {

                //Find the group with same Id
                foreach (var item in ChatList.Items)
                {
                    //And Delete
                    if (item.GroupData.Id == data.Id)
                    {

                        //Becouse Items is ObservableCollection we should remove elements from the main thread
                        App.Current.Dispatcher.Invoke(() => 
                        {
                            ChatList.Items.Remove(item);
                        });
                        break;
                    }

                }
            }
        }

        /// <summary>
        /// Adds new element to chat list
        /// </summary>
        /// <param name="dataChanged"></param>
        void AddGroupToChatList(List<Group> dataChanged)
        {
            if (dataChanged == null)
                return;

            foreach (var group in dataChanged)
            {
                //Get last message from dataBase
                var lastMess = UnitOfWork.MessagesTableRepo.FindLast(MessagesTableFields.ReceiverId.ToString(), group.Id.ToString());

                //Becouse Items is ObservableCollection we should remove elements from the main thread
                App.Current.Dispatcher.Invoke(() =>
                {
                    ChatList.Items.Add(new ChatListItemViewModel(group, lastMess));
                });

                //Sort list by the last message time 
                ChatList.SortChatList();
            }
        }

        #endregion
    }
}
