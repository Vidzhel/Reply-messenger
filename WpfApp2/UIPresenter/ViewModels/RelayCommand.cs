using System;
using System.Windows.Input;

namespace UI.UIPresenter.ViewModels
{
    class RelayCommand : ICommand
    {

        private Action action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #region Constructor

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        #endregion

        #region Functions

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }

        #endregion

    }
}
