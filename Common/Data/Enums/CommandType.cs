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
    [Serializable]
    public enum CommandType
    {
        SignUp,
        SignIn,
        SignOut,

        GetUsersInfo,

        CreateGroup,
        ConnectToGroup,
        InviteToGroup,
        AddAdminToGroup,
        DisconectFromGroup,
        DeleteMemberFromGroup,

        UpdateGroup,
        UpdateMessage,
        UpdateUserInfo,
        SendMesssage,
        ReceiveMesssage,

        DeleteUser,

        Answer
    }
}
