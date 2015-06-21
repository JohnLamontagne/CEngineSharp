using CEngineSharp_Server.Utilities.ServiceLocators;
using CEngineSharp_Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server.Networking
{
    public class Packet
    {
        private NetOutgoingMessage _message;

        public NetOutgoingMessage Message { get { return _message; } }

        public Packet(PacketType packetType)
        {
            _message = NetServiceLocator.Singleton.GetService().ConstructMessage();
            _message.Write((short)packetType);
        }
    }
}