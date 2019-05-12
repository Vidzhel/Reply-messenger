

using System.Collections.Generic;

namespace UI.UIPresenter.ViewModels
{
    public class MessageListViewModel : BaseViewModel
    {

        #region Public Members

        public List<MessageListItemViewModel> Items { get; set; } = new List<MessageListItemViewModel>();

        #endregion

        #region 

        public MessageListViewModel(List<MessageListItemViewModel> items)
        {
            Items = items;
        }

        public MessageListViewModel()
        {

        }

        #endregion

    }
}
