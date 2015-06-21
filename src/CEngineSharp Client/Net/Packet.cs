using CEngineSharp_Client.Utilities;
using CEngineSharp_Utilities;
using Lidgren.Network;

namespace CEngineSharp_Client.Networking
{
    public class Packet
    {
        private NetOutgoingMessage _message;

        public NetOutgoingMessage Message { get { return _message; } }

        public Packet(PacketType packetType)
        {
            _message = ServiceLocator.NetManager.ConstructMessage();
            _message.Write((short)packetType);
        }
    }
}