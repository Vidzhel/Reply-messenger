using ServerLibs.ConnectionToClient;
using ServerLibs.DataAccess;

namespace Server
{
    public class ServerViewModel : BaseViewModel
    {

        #region Public Members

        public string Online => UnitOfWork.OnlineClients.Count.ToString();

        #endregion

        public ServerViewModel()
        {

        }

    }
}
