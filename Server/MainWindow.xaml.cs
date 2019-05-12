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
            StopServer.IsEnabled = false;
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            StartServer.IsEnabled = false;
            StopServer.IsEnabled = true;
            AsynchronousClientListener.DisplayMessageOnScreenContext = displayMessageOnScreen;
            Task.Run(() => AsynchronousClientListener.Start());
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

        private void StopServer_Click(object sender, RoutedEventArgs e)
        {
            StopServer.IsEnabled = false;
            StartServer.IsEnabled = true;
            AsynchronousClientListener.Disconect(true);
        }
    }
}
