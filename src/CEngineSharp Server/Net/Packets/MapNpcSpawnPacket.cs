using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MapNpcSpawnPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            //Server->Client only
        }

        public void WriteData(MapNpc mapNpc)
        {
            this.DataBuffer.WriteString(mapNpc.Name);
            this.DataBuffer.WriteInteger(mapNpc.Level);
            this.DataBuffer.WriteInteger(mapNpc.TextureNumber);
        }

        public override int PacketID
        {
            get { return 19; }
        }
    }
}