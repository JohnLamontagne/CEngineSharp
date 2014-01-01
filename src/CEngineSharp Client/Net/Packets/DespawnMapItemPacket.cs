using CEngineSharp_Client.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class DespawnMapItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int mapItemX = this.DataBuffer.ReadInteger();
            int mapItemY = this.DataBuffer.ReadInteger();

            if (Client.InGame)
            {
                MapManager.Map.RemoveMapItem(mapItemX, mapItemY);
            }
        }

        public override int PacketID
        {
            get { return 12; }
        }
    }
}