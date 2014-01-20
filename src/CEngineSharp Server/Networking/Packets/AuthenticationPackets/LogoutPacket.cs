using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Server.Networking.Packets.AuthenticationPackets
{
    public class LogoutPacket : Packet
    {
        public void WriteData(int playerIndex)
        {
            this.DataBuffer.WriteInteger(playerIndex);
        }

        public override void Execute(Netty netty)
        {
            // Server->Client only.
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.LogoutPacket; }
        }
    }
}