using CEngineSharp_Server.World.Maps;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Server.Networking.Packets.MapUpdatePackets
{
    public class SpawnMapItemPacket : Packet
    {
        public void WriteData(MapItem mapItem)
        {
            this.DataBuffer.WriteString(mapItem.Item.Name);
            this.DataBuffer.WriteInteger(mapItem.Item.TextureNumber);
            this.DataBuffer.WriteVector(mapItem.Position);
            this.DataBuffer.WriteInteger(mapItem.SpawnDuration);
        }

        public override void Execute(Netty netty)
        {
            //Server->Client only packet.
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.SpawnMapItemPacket; }
        }
    }
}