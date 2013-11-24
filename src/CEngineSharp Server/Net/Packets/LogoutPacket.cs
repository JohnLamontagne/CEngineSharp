using SharpNetty;

namespace CEngineSharp_Server.Net.Packets
{
    public class LogoutPacket : Packet
    {
        public void WriteData(int playerIndex)
        {
            this.DataBuffer.WriteInteger(playerIndex);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            // Server->Client only.
        }

        public override string PacketID
        {
            get { return "LogoutPacket"; }
        }
    }
}