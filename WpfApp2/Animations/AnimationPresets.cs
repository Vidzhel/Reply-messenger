using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace UI.Animations
{
    public static class AnimationPresets
    {
        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeFromDown(FrameworkElement page, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideFromDown(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeInAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;
            
            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }
        
        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeFromLeft(FrameworkElement elementToanimate, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideFromLeft(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeInAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(elementToanimate);

            //make page visible
            elementToanimate.Visibility = Visibility.Visible;
            
            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }
        
        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeFromRight(FrameworkElement elementToanimate, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideFromRight(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeInAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(elementToanimate);

            //make page visible
            elementToanimate.Visibility = Visibility.Visible;
            
            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeFromUp(FrameworkElement page, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideFromUp(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeInAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToUp(FrameworkElement page, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideToUp(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeOutAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToDown(FrameworkElement page, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideToDown(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeOutAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(FrameworkElement page, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideToLeft(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeOutAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToRight(FrameworkElement page, float slideDurationSec, double offset, float decelerationRatio = 0.9f)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddSlideToRight(sb, slideDurationSec, offset, decelerationRatio);
            CreateStoryboardAnim.AddFadeOutAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task FadeIn(FrameworkElement page, float slideDurationSec)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddFadeInAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="slideDurationSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <returns></returns>
        public static async Task FadeOut(FrameworkElement page, float slideDurationSec)
        {
            var sb = new Storyboard();

            //Add anumations
            CreateStoryboardAnim.AddFadeOutAnim(sb, slideDurationSec);

            //Begin anim
            sb.Begin(page);

            //make page visible
            page.Visibility = Visibility.Visible;

            //Wait 
            await Task.Delay((int)(slideDurationSec * 1000));
        }
    }
}
