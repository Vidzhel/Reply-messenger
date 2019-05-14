using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UI.UIPresenter.ViewModels
{
    public class ChatListViewModel : BaseViewModel
    {

        //List of chats
        public ObservableCollection<ChatListItemViewModel> Items { get; set; } = new ObservableCollection<ChatListItemViewModel>();

        #region Constructor

        public ChatListViewModel()
        {

        }

        public ChatListViewModel(ObservableCollection<ChatListItemViewModel> chats) 
        {
            Items = chats;
        }

        #endregion

        #region Public Methods

        public void SortChatList()
        {
            
            //Items.Sort(new ChatListItemViewModelComparer());
        }

        #endregion
    }
}
