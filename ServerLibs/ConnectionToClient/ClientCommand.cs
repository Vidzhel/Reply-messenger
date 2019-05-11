using CommonLibs.Data;

namespace ServerLibs.ConnectionToClient
{
    public class ClientCommand
    {

        public Command Command { get; set; }
        public Client Client{ get; set; }

        #region Constructor

        public ClientCommand(Command com, Client client)
        {
            Command = com;
            Client = client;
        }

        #endregion
    }
}
