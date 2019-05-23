using System.Security;

namespace UI.UIPresenter.ViewModels
{
    /// <summary>
    /// Provide method to get secure string
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// Secure string getter
        /// </summary>
        SecureString StringPassword { get; }

    }
}
