using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class RegisterationPacket : Packet
    {
        private void WriteData(bool registrationOkay, string response)
        {
        }

        public override void Execute(Netty netty, int socketIndex)
        {
        }
    }
}