using System;
using System.Windows.Input;

namespace UI.UIPresenter.ViewModels
{
    public class RelayCommandParametrized : ICommand
    {

        private Action<object> action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #region Constructor

        public RelayCommandParametrized(Action<object> action)
        {
            this.action = action;
        }

        #endregion

        #region Functions

        //Always can execute
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        #endregion

    }
}
