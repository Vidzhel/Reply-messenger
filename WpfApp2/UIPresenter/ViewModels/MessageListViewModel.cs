using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UI.UIPresenter.ViewModels
{
    public class MessageListViewModel : BaseViewModel
    {

        #region Public Members

        public ObservableCollection<MessageListItemViewModel> Items { get; set; } = new ObservableCollection<MessageListItemViewModel>();

        #endregion

        #region 

        public MessageListViewModel(ObservableCollection<MessageListItemViewModel> items)
        {
            Items = items;
        }

        public MessageListViewModel()
        {

        }

        #endregion

    }
}
