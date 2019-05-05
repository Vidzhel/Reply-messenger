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
            
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            AsynchronousClientListener.DisplayMessageOnScreenContext = displayMessageOnScreen;
            Task.Run(() => AsynchronousClientListener.SrtartListening());
        }

        /// <summary>
        /// Displays messages on <see cref="LogScreen"/>
        /// </summary>
        /// <param name="message"></param>
        private void displayMessageOnScreen(string message)
        {
            LogScreen.Text += message + Environment.NewLine;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AsynchronousClientListener.Disconect(true);
        }
    }
}
