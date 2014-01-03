using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Maps;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class SpawnMapItemPacket : Packet
    {
        public void WriteData(MapItem mapItem)
        {
            this.DataBuffer.WriteString(mapItem.Item.Name);
            this.DataBuffer.WriteInteger(mapItem.Item.TextureNumber);
            this.DataBuffer.WriteInteger(mapItem.X);
            this.DataBuffer.WriteInteger(mapItem.Y);
            this.DataBuffer.WriteInteger(mapItem.SpawnDuration);
        }

        public override void Execute(Netty netty)
        {
            //Server->Client only packet.
        }

        public override int PacketID
        {
            get { return 10; }
        }
    }
}