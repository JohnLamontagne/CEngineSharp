using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Maps;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    internal class DespawnMapItemPacket : Packet
    {
        public void WriteData(MapItem mapItem)
        {
            this.DataBuffer.WriteInteger(mapItem.X);
            this.DataBuffer.WriteInteger(mapItem.Y);
        }

        public override void Execute(Netty netty)
        {
            // Server->Client only.
        }

        public override int PacketID
        {
            get { return 12; }
        }
    }
}