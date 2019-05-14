using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.ViewModels
{
    /// <summary>
    /// The design version of <see cref="MessageListViewModel"/>
    /// </summary>
    public class MessageListDesignViewModel : MessageListViewModel
    {
        #region Singleton

        public static MessageListDesignViewModel Instance => new MessageListDesignViewModel();

        #endregion

        public MessageListDesignViewModel()
        {

            var user = new Contact("VidzhelNeSuka", "myemail.com", "something there");
            var user1 = new Contact("Oleg", "myemail.com", "somthing there", null, "false");

            var message = new Message(10, 20, DataType.Text, new DateTime(2018, 2, 25), "Hi, how do you do?", MessageStatus.Sended);
            var message1 = new Message(10, 20, DataType.Image, DateTime.Now, "file source", MessageStatus.SendingInProgress);
            var message2 = new Message(10, 20, DataType.Text, DateTime.Now, "Ohh, thanks for the pressent, i very appreciated", MessageStatus.IsRead);

            Items = new List<MessageListItemViewModel> {

                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, true, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, true, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user, message, false, true),
                new MessageListItemViewModel(user1, message1, true, true),
                new MessageListItemViewModel(user, message2, false, true)

            };
        }
    }
}
