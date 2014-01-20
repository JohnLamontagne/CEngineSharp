using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Server.Networking.Packets.MapUpdatePackets
{
    public class SpawnMapNpcPacket : Packet
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
            get { return (int)PacketTypes.SpawnMapNpcPacket; }
        }
    }
}