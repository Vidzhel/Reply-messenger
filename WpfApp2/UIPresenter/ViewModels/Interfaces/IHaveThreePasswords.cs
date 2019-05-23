using System.Security;

namespace UI.UIPresenter.ViewModels
{

    /// <summary>
    /// Provide method to get secure string
    /// </summary>
    public interface IHaveThreePasswords
    {

        SecureString OldStringPassword { get; }

        SecureString StringPassword { get; }

        SecureString RepeatStringPassword { get; }

    }
}
