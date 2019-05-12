using CommonLibs.Data;
using System;
using System.Collections.Generic;
using UI.Pages;

namespace UI.UIPresenter.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {

        #region Public Members

        /// <summary>
        /// Current Choosen page
        /// </summary>
        public ChatPages ActiveChatContent { get; set; } = ChatPages.Chat;

        #endregion

        #region Constructor

        public ChatPageViewModel()
        {
        }

        #endregion
    }
}
