
using CommonLibs.Connections.Repositories;

namespace CommonLibs.Connections.Repositories
{
    public interface IRemoteRepository<T> : IRepository<T> where T : class
    {
        //Additional methods for server repository

        bool Connect();
        bool Disconnect();
    }
}
