using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.ViewModels
{
    public class MessageListItemDesignViewModel : MessageListItemViewModel
    {
        #region Singleton

        public static MessageListItemDesignViewModel Instance => new MessageListItemDesignViewModel();

        #endregion

        #region Constructor

        public MessageListItemDesignViewModel()
        {

            var user = new Contact("Vidzhel", "myemail.com", "somthing there");
            var message = new Message(10, 20, DataType.Text, DateTime.Now, "Hello there, thenks for the pressent, i very", MessageStatus.Sended);
            
            ContactData = user;
            Message = message;
            IsTwoMembersInTheChat = true;
            IsYourMessage = true;
        }



        #endregion
    }
}
