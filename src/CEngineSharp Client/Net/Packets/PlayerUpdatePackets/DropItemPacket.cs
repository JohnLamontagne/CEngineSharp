using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.PlayerUpdatePackets
{
    public class DropItemPacket : Packet
    {
        public void WriteData(int slotNum)
        {
            this.DataBuffer.WriteInteger(slotNum);
        }

        public override void Execute(Netty netty)
        {
            // Client->Server only packet.
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.DropItemPacket; }
        }
    }
}