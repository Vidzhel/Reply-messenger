using System.Security;
using System.Windows.Controls;
using UI.UIPresenter.ViewModels;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoUserControl : UserControl, IHaveThreePasswords
    {
        public InfoUserControl()
        {
            InitializeComponent();

            DataContext = new InfoUserControlViewModel();
        }

        public SecureString OldStringPassword => OldPassField.SecurePassword;

        public SecureString StringPassword => NewPassField.SecurePassword;

        public SecureString RepeatStringPassword => RepeatPassField.SecurePassword;
    }
}
