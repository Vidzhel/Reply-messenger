using System;
using System.Threading.Tasks;
using UI.Animations;
using UI.UIPresenter.ViewModels;

namespace UI.Pages
{
    /// <summary>
    /// Interaction logic for MessengerPage.xaml
    /// </summary>
    public partial class ChatPage : BasePage<ChatPageViewModel>
    {
        public ChatPage()
        {
            InitializeComponent();
            
            //Start all page anmations
            startAnimationsAsync();
        }

        #region Animation Helpers

        private async Task startAnimationsAsync()
        {

            //Wait for Page animation done
            await Task.Delay(TimeSpan.FromMilliseconds((int)SlideDurationSec * 1000));

            //Element Animations
            AnimationPresets.SlideAndFadeFromLeft(LeftPannel, 1, 150);
        }


        #endregion
        
    }
}
