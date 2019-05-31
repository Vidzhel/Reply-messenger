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
        SignUpAnswer,
        SignIn,
        SignInAnswer,
        SignOut,
        Synchronize,
        SynchronizeAnswer,

        SearchRequest,
        SearchRequestAnswer,

        GetUsersInfo,
        GetUsersInfoAnswer,
        GetGroupsInfo,
        GetGroupsInfoAnswer,
        GetUserGroupsInfo,
        GetUserGroupsInfoAnswer,

        CreateGroup,
        CreateGroupAnswer,
        UpdateGroup,
        RemoveGroup,
        JoinGroup,
        JoinGroupAnswer,
        MakeAdmin,
        MakeAdminAnswer,
        LeaveGroup,
        LeaveGroupAnswer,
        DeleteMemberFromGroup,


        UpdateMessage,
        RemoveMessage,
        SendMesssage,
        SendMesssageAnswer,
        SendFile,
        SendFileAnswer,
        GetFile,
        GetFileAnswer,

        UpdateUserInfo,
        UpdateUserInfoAnswer,
        UpdateUserPassword,
        UpdateUserPasswordAnswer,
        DeleteUser,
        DeleteUserAnswer,

        Answer
    }
}
