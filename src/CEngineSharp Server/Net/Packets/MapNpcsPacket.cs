using CEngineSharp_Server.World.Maps;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MapNpcsPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            // Server->Client only.
        }

        public void WriteData(Map map)
        {
            var mapNpcs = map.GetMapNpcs();

            this.DataBuffer.WriteInteger(mapNpcs.Length);

            foreach (var npc in mapNpcs)
            {
                this.DataBuffer.WriteString(npc.Name);
                this.DataBuffer.WriteInteger(npc.Level);
                this.DataBuffer.WriteInteger(npc.TextureNumber);
                this.DataBuffer.WriteInteger(npc.Position.X);
                this.DataBuffer.WriteInteger(npc.Position.Y);
            }
        }

        public override int PacketID
        {
            get { return 18; }
        }
    }
}