using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.ViewModels
{
    public class ChatListItemViewModelComparer : IComparer<ChatListItemViewModel>
    {
        public int Compare(ChatListItemViewModel x, ChatListItemViewModel y)
        {
            return x.LastMessageTime.CompareTo(y.LastMessageTime);
        }
    }
}
