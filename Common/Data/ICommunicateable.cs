
using System;

namespace CommonLibs.Data
{

    /// <summary>
    /// Provide base methods for communication between server and client 
    /// </summary>
    public interface ICommunicateable
    {

        bool SendData(object obj);

        void ReceiveData();

        void Start();

        void Disconect();
    }
}
