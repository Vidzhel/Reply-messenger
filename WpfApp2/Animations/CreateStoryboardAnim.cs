using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace UI.Animations
{
    public static class CreateStoryboardAnim
    {

        /// <summary>
        /// To 0% opacity animation
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time in seconds</param>
        public static void AddFadeOutAnim(Storyboard sb, float timeSec)
        {
            var anim = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = 1,
                To = 0,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Opacity"));
            
            //Append child to the storyboard
            sb.Children.Add(anim);
        }
        
        /// <summary>
        /// To 100% opacity animation
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time in seconds</param>
        public static void AddFadeInAnim(Storyboard sb, float timeSec)
        {
            var anim = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = 0,
                To = 1,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Opacity"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from start point to Up
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideToUp(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(0),
                To = new Thickness(0, -offset, 0, offset),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }
        
        /// <summary>
        /// Margin animation from Up to start point
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideFromUp(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(0, -offset, 0, offset),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from down to start point
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideFromDown(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(0, offset, 0, -offset),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from start to down
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideToDown(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(0),
                To = new Thickness(0, offset, 0, -offset),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from left to start point
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideFromLeft(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(-offset, 0, offset, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from start to left
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideToLeft(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, offset, 0),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from right to start point
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideFromRight(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(offset, 0, -offset, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

        /// <summary>
        /// Margin animation from start to right
        /// </summary>
        /// <param name="sb">storyboard</param>
        /// <param name="timeSec">animation time</param>
        /// <param name="offset">margin offset</param>
        /// <param name="durationRatio"></param>
        public static void AddSlideToRight(Storyboard sb, float timeSec, double offset, float decelerationRatio = 0.9F)
        {
            var anim = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(timeSec)),
                From = new Thickness(0),
                To = new Thickness(offset, 0, -offset, 0),
                DecelerationRatio = decelerationRatio,
            };

            //Add target property
            Storyboard.SetTargetProperty(anim, new PropertyPath("Margin"));

            //Append child to the storyboard
            sb.Children.Add(anim);
        }

    }
}

