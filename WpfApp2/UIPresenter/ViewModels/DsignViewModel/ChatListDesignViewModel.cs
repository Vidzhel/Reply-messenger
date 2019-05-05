using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.ViewModels
{
    /// <summary>
    /// The design version of <see cref="ChatListViewModel"/>
    /// </summary>
    public class ChatListDesignViewModel : ChatListViewModel
    {
        #region Singleton

        public static ChatListDesignViewModel Instance => new ChatListDesignViewModel();

        #endregion

        public ChatListDesignViewModel()
        {

            var user = new Contact("VidzhelNeSuka", "myemail.com", "somthing there");
            var user1 = new Contact("Oleg", "myemail.com", "somthing there", null, "false");
            var user2 = new Contact("Vlad NeSuka", "myemail.com", "somthing there");

            var message = new Message(10, 20, DataType.Text, new DateTime(2018, 2, 25), "Hello there, thenks for the pressent, i very", MessageStatus.Sended);
            var message1 = new Message(10, 20, DataType.Image, DateTime.Now, "file source", MessageStatus.SendingInProgress);
            var message2 = new Message(10, 20, DataType.Text, DateTime.Now, "Ohh, thenks for the pressent, i very appreciated", MessageStatus.IsRead);

            Items = new List<ChatListItemViewModel> {

                new ChatListItemViewModel(user, message, false),
                new ChatListItemViewModel(user1, message1, true),
                new ChatListItemViewModel(user1, message1, false),
                new ChatListItemViewModel(user1, message1, false),
                new ChatListItemViewModel(user1, message1, false),
                new ChatListItemViewModel(user1, message1, false),
                new ChatListItemViewModel(user1, message1, false),
                new ChatListItemViewModel(user2, message2, false)

            };
        }
    }
}
