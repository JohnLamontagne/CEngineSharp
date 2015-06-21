using Lidgren.Network;
using System;

namespace CEngineSharp_Client.Net
{
    public class PacketReceivedEventArgs : EventArgs
    {
        private readonly NetIncomingMessage _message;

        public NetIncomingMessage Message { get { return _message; } }

        public PacketReceivedEventArgs(NetIncomingMessage message)
        {
            _message = message;
        }
    }
}