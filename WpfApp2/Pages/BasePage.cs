using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using UI.Animations;
using System.Threading.Tasks;
using UI.UIPresenter.ViewModels;

namespace UI.Pages
{
    public class BasePage<VM> : Page where VM: BaseViewModel, new()
    {

        #region Private Members

        VM viewModel;

        #endregion

        #region Public Members

        /// <summary>
        /// ViewModel for the page
        /// </summary>
        public VM ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                if (viewModel == value)
                    return;
                else
                    viewModel = value;

                //Set data context to page view model
                this.DataContext = ViewModel;
            }
        }

        //Page Animations
        public AnimationTypes AppearanceAnimation { get; set; } = AnimationTypes.FadeIn;
        public AnimationTypes DasappearingAnimation { get; set; } = AnimationTypes.FadeOut;
        public double Offset { get; set; } = 100;

        public float SlideDurationSec { set; get; } = 0.9f;

        #endregion



        #region Constructors

        public BasePage()
        {
            //Hide before animate
            if (AppearanceAnimation != AnimationTypes.None)
                this.Visibility = Visibility.Collapsed;

            Loaded += BasePageViewModel_Loaded;
            Unloaded += BasePage_Unloaded;

            ViewModel = new VM();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start appearance animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePageViewModel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AnimateIn();
        }

        /// <summary>
        /// Start desappearance animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            AnimateOut();
        }

        private async Task AnimateIn()
        {
            if (AppearanceAnimation == AnimationTypes.None)
                return;

            switch (AppearanceAnimation)
            {
                case AnimationTypes.SlideAndFadeFromDown:
                    await Animations.AnimationPresets.SlideAndFadeFromDown(this, SlideDurationSec, Offset);
                    break;
                case AnimationTypes.SlideAndFadeFromUp:
                    await Animations.AnimationPresets.SlideAndFadeFromUp(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.SlideAndFadeOutToDown:
                    await Animations.AnimationPresets.SlideAndFadeOutToDown(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.SlideAndFadeOutToUp:
                    await Animations.AnimationPresets.SlideAndFadeOutToUp(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.FadeIn:
                    await Animations.AnimationPresets.FadeIn(this, this.SlideDurationSec);
                    break;
                case AnimationTypes.FadeOut:
                    await Animations.AnimationPresets.FadeOut(this, this.SlideDurationSec);
                    break;
            }
        }

        private async Task AnimateOut()
        {
            if (AppearanceAnimation == AnimationTypes.None)
                return;

            switch (AppearanceAnimation)
            {
                case AnimationTypes.SlideAndFadeFromDown:
                    await Animations.AnimationPresets.SlideAndFadeFromDown(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.SlideAndFadeFromUp:
                    await Animations.AnimationPresets.SlideAndFadeFromUp(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.SlideAndFadeOutToDown:
                    await Animations.AnimationPresets.SlideAndFadeOutToDown(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.SlideAndFadeOutToUp:
                    await Animations.AnimationPresets.SlideAndFadeOutToUp(this, this.SlideDurationSec, Offset);
                    break;
                case AnimationTypes.FadeIn:
                    await Animations.AnimationPresets.FadeIn(this, this.SlideDurationSec);
                    break;
                case AnimationTypes.FadeOut:
                    await Animations.AnimationPresets.FadeOut(this, this.SlideDurationSec);
                    break;
            }
        }

        #endregion

    }
}
