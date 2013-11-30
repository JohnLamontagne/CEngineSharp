using SharpNetty;

namespace CEngineSharp_Server.Net.Packets
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
            get { return 4; }
        }
    }
}