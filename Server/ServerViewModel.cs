using ServerLibs.ConnectionToClient;


namespace Server
{
    public class ServerViewModel : BaseViewModel
    {

        #region Public Members

        public string Online => AsynchronousClientListener.ConnectedClients.Count.ToString();

        #endregion

        public ServerViewModel()
        {

        }

    }
}
