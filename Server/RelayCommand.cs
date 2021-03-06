﻿using System;
using System.Windows.Input;

namespace Server
{
    public class RelayCommand : ICommand
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

        //Always can execute
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
