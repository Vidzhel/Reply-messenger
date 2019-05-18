using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.Animations;

namespace UI.UserControls
{
    /// <summary>
    /// Interaction logic for MessageListItem.xaml
    /// </summary>
    public partial class MessageListItem : UserControl
    {

        float SlideDurationSec = 0.5F;

        public MessageListItem()
        {
            InitializeComponent();

            //Starts all page animations
            startAnimationsAsync();
        }

        #region Animation Helpers

        private async Task startAnimationsAsync()
        {
            //Wait for Page animation done
            await Task.Delay(TimeSpan.FromMilliseconds((int)SlideDurationSec * 1000));
            //Element Animations
            AnimationPresets.SlideAndFadeFromDown(this, SlideDurationSec, 150);
        }

        #endregion
    }
}
