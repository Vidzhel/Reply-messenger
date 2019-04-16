using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.ViewModels
{
    /// <summary>
    /// View model for windows chrome
    /// </summary>
    class WindowsViewModel :  BaseViewModel
    {
        #region private Members

        private Window window;

        private int borderResize = 10;

        #endregion

        #region Public Members
        /// <summary>
        /// Set Outher margin for the window
        /// </summary>
        public int OutherMargin { get; set; } = 6;
        public Thickness OutherMarginThickness { get { return new Thickness(OutherMargin); } }

        /// <summary>
        /// Set Resize border thickness
        /// </summary>
        public int BorderResize { get { return borderResize + OutherMargin; } set { borderResize = value; } }
        public Thickness BorderResizeThickness { get { return new Thickness(BorderResize); } }

        /// <summary>
        /// Set Corner Radious
        /// </summary>
        public int CornerRadious { get; set; } = 10;
        public CornerRadius CornerRadiousThickness { get { return new CornerRadius(CornerRadious); } }
        #endregion

        public WindowsViewModel(Window window)
        {
            this.window = window;
        }
    }
}
