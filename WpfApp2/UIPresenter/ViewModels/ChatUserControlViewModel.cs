using CommonLibs.Data;
using System;
using System.Collections.Generic;

namespace UI.UIPresenter.ViewModels
{
    public class ChatUserControlViewModel
    {

        #region Private Members

        Group groupData;

        #endregion

        #region Public Members

        public List<MessageListItemViewModel> Messageges { get; set; }

        /// <summary>
        /// Gets name of the group
        /// </summary>
        public string ChatName => groupData?.Name;



        #endregion

        #region Constructor

        public ChatUserControlViewModel()
        {
            Contact user1 = new Contact("Vidzhel", "someEmail", "SomeBio");
            Contact user2 = new Contact("Vlad", "someEmail", "SomeBio");

            Message mes1 = new Message(1, 2, DataType.Text, DateTime.Now, "Hello my friend, how are you?");
            Message mes2 = new Message(1, 2, DataType.Text, DateTime.Now, "I'm nice, and you");
            Message mes3 = new Message(1, 2, DataType.Text, DateTime.Now, "So so, there should be very long text to check how all things work");

            //TODO delete checking code
            Messageges = new List<MessageListItemViewModel>(){
                new MessageListItemViewModel(user1, mes1, true, true),
                new MessageListItemViewModel(user2, mes2, false, true),
                new MessageListItemViewModel(user1, mes3, true, true)
            };
        }

        #endregion
    }
}
