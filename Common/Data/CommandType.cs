using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs.Data
{
    /// <summary>
    /// Represenst all commans which server can send to client and vice versa
    /// </summary>
    public enum CommandType
    {
        SignUp,
        AnswerSignUp,
        SignIn,
        AnswerSignIn,

        CreateGroup,
        AnswerCreateGroup,
        ConnectToGroup,
        AnswerConnectToGroup,
        DisconectFromGroup,
        AnswerDisconectFromGroup,

        UpdateGroup,
        AnswerUpdateGroup,
        UpdateMessage,
        AnswerUpdateMessage,
        UpdateUserInfo,
        AnswerUpdateUserInfo,
        SendMesssage,
        AnswerSendMesssage,
        ReceiveMesssage,
        AnswerReceiveMesssage,

        DeleteUser,
        AnswerDeleteUser
    }
}
