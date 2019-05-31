using ServerLibs.ConnectionToClient;
using ServerLibs.DataAccess;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server
{
    public class ServerViewModel : BaseViewModel
    {

        #region Public Members

        //Commands to start and stop listening
        public ICommand StartServer { get; set; }
        public ICommand StopServer { get; set; }

        //Trye if server listening to clients
        public bool ServerOnline { get; set; }
        public bool ServerOffline { get; set; }

        public string Online { get; set; }

        #endregion

        public ServerViewModel()
        {
            //Setup commands
            StartServer = new RelayCommand(startServer);
            StopServer = new RelayCommand(storServer);

            //Setup event handlers
            AsynchronousClientListener.OnUserConnected((client) => Online = (Convert.ToInt32(Online) + 1).ToString());
            AsynchronousClientListener.OnUserDisconnected((client) => Online = (Convert.ToInt32(Online) - 1).ToString());

            ServerOffline = true;
        }

        #region Private Methods

        void startServer()
        {
            Task.Run(() => AsynchronousClientListener.Start());
            var a = UnitOfWork.OnlineClients;
            ServerOnline = true;
            ServerOffline = false;
        }

        void storServer()
        {
            AsynchronousClientListener.Disconect(true);
            ServerOffline = true;
            ServerOnline = false;
        }

        #endregion

    }
}
