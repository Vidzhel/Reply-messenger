using System;
using System.Collections.Generic;


namespace UI.UIPresenter.ViewModels
{
    public class ChatListViewModel : BaseViewModel
    {

        //List of chats
        public List<ChatListItemViewModel> Items { get; set; }

        #region Constructor

        public ChatListViewModel()
        {

        }

        public ChatListViewModel(List<ChatListItemViewModel> chats) 
        {
            Items = chats;
        }

        #endregion
    }
}
