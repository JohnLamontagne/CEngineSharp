using Lidgren.Network;
using System;

namespace CEngineSharp_Server.Networking
{
    public class PacketReceivedEventArgs : EventArgs
    {
        private readonly NetIncomingMessage _message;
        private readonly NetConnection _connection;

        public NetIncomingMessage Message { get { return _message; } }

        public NetConnection Connection { get { return _connection; } }

        public PacketReceivedEventArgs(NetIncomingMessage message, NetConnection connection)
        {
            _message = message;
            _connection = connection;
        }
    }
}