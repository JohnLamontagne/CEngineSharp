using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.MapUpdatePackets
{
    public class DespawnMapItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var mapItemX = this.DataBuffer.ReadInteger();
            var mapItemY = this.DataBuffer.ReadInteger();

            if (Client.InGame)
            {
                MapManager.Map.RemoveMapItem(mapItemX, mapItemY);
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.DespawnMapItemPacket; }
        }
    }
}