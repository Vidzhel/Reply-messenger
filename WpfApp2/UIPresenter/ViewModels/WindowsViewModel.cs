using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using PropertyChanged;
using UI.Pages;

namespace UI.UIPresenter.ViewModels
{
    /// <summary>
    /// View model for windows chrome
    /// </summary>
    class WindowViewModel :  BaseViewModel
    {
        #region private Members

        private readonly Window window;

        private int borderResize = 5;


        #endregion

        #region Public Members


        //Commands for system buttons
        [DoNotNotify]
        public ICommand CloseWindow { get; set; }
        [DoNotNotify]
        public ICommand ColapseWindow { get; set; }
        [DoNotNotify]
        public ICommand MaximazeWindow { get; set; }

        /// <summary>
        /// Set Outher margin for the window
        /// </summary>
        public int OutherMargin { get; set; } = 6;
        public Thickness OutherMarginThickness {
            get {

                if (window.WindowState == WindowState.Maximized)
                    return new Thickness(0);
                    return new Thickness(OutherMargin);
            }
        }

        /// <summary>
        /// Set Resize border thickness
        /// </summary>
        public int BorderResize { get { return borderResize + OutherMargin; } set { borderResize = value; } }
        public Thickness BorderResizeThickness { get { return new Thickness(BorderResize); } }

        /// <summary>
        /// Set Resize border thickness
        /// </summary>
        public int WindowBorderSize { get; set; } = 4;
        public Thickness WindowBorderSizeThickness
        {
            get
            {

                if (window.WindowState == WindowState.Maximized)
                    return new Thickness(0);
                return new Thickness(WindowBorderSize);
            }
        }

        /// <summary>
        /// Set Corner Radious
        /// </summary>
        public int Radius { get; set; } = 10;
        public CornerRadius CornerRadius {
            get {
                if (window.WindowState == WindowState.Maximized)
                    return new CornerRadius(0);
                else
                    return new CornerRadius(Radius);
            }
        }


        /// <summary>
        /// Set Height of Caption
        /// </summary>
        public int CaptionHeight { get; set; } = 15;
        public GridLength CaptionHeightGridLeight { get { return new GridLength(CaptionHeight + BorderResize); } }

        /// <summary>
        /// A curent open page in the main window
        /// </summary>
        public ApplicationPage ApplicationCurrentPage { get; set; } = ApplicationPage.SignUpPage;

        #endregion

        #region Constructor
        public WindowViewModel(Window window)
        {
            this.window = window;

            //On window state change handdler
            window.StateChanged += (sender, e) =>{
                OnPropertyChanged(nameof(BorderResizeThickness));
                OnPropertyChanged(nameof(CornerRadius));
                OnPropertyChanged(nameof(OutherMarginThickness));
            };

            //commands for buttons
            CloseWindow = new RelayCommand(() => {
                window.Close();
            });
            ColapseWindow = new RelayCommand(() =>
            {
                window.WindowState = WindowState.Minimized;
            });
            MaximazeWindow = new RelayCommand(() =>
            {
                window.WindowState ^= WindowState.Maximized;
            });
        }

        #endregion

    }
}
