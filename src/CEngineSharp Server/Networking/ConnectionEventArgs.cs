using Lidgren.Network;
using System;

namespace CEngineSharp_Server.Networking
{
    public class ConnectionEventArgs : EventArgs
    {
        private readonly NetConnection _connection;

        public NetConnection Connection { get { return _connection; } }

        public ConnectionEventArgs(NetConnection connection)
        {
            _connection = connection;
        }
    }
}