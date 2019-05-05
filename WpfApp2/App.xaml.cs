using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UI.InversionOfControl;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //Defoult actions
            base.OnStartup(e);

            //Bind all injections
            IoCController.SetUp();

            //Show main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
