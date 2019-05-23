using System.Security;

namespace UI.UIPresenter.ViewModels
{
    /// <summary>
    /// Provide method to get secure string
    /// </summary>
    public interface IHavePasswords
    {
        /// <summary>
        /// Secure string getter
        /// </summary>
        SecureString StringPassword { get; }

        SecureString RepeatStringPassword { get; }

    }
}
