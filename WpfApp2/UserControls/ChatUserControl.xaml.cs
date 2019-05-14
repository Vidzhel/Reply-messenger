using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI.UIPresenter.ViewModels;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for ChatUserControl.xaml
    /// </summary>
    public partial class ChatUserControl : UserControl
    {
        public ChatUserControl()
        {
            InitializeComponent();

            //Bind data
            this.DataContext = new ChatUserControlViewModel(new CommonLibs.Data.Group(true, "sadsad", false));
        }
    }
}
