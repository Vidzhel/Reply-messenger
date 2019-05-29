using ServerLibs.ConnectionToClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ServerViewModel();

            AsynchronousClientListener.DisplayMessageOnScreenContext = displayMessageOnScreen;
        }

        /// <summary>
        /// Displays messages on <see cref="LogScreen"/>
        /// </summary>
        /// <param name="message"></param>
        private void displayMessageOnScreen(string message)
        {
            // Allow us change text from another thread
            LogScreen.Dispatcher.BeginInvoke((Action)(() => LogScreen.Text += message + Environment.NewLine));
        }
    }
}
