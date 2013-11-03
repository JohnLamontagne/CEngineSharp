using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MapCheckPacket : Packet
    {
        public void WriteData(string mapName)
        {
            this.PacketBuffer.WriteString(mapName);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            bool response = this.PacketBuffer.ReadBool();

            if (response == false)
            {
                MapDataPacket mapDataPacket = new MapDataPacket();
                mapDataPacket.WriteData(PlayerManager.GetPlayer(socketIndex).Map);
                PlayerManager.GetPlayer(socketIndex).SendPacket(mapDataPacket);
            }
        }

        public override string PacketID
        {
            get { return "MapCheckPacket"; }
        }
    }
}