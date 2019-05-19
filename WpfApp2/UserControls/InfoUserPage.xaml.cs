using System.Windows.Controls;
using UI.UIPresenter.ViewModels;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoUserControl : UserControl
    {
        public InfoUserControl()
        {
            InitializeComponent();

            DataContext = new InfoUserControlViewModel();
        }
    }
}
