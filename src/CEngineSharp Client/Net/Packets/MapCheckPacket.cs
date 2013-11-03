using CEngineSharp_Client.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class MapCheckPacket : Packet
    {
        private void WriteData(bool mapExists)
        {
            this.PacketBuffer.Flush();
            this.PacketBuffer.WriteBool(mapExists);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            string mapName = this.PacketBuffer.ReadString();

            bool mapExistence = MapManager.CheckMapExistence(mapName);

            this.WriteData(mapExistence);

            Networking.SendPacket(this);

            if (mapExistence == true)
            {
                MapManager.Map = new World.Map();
                MapManager.Map.LoadCache(mapName);
            }
        }

        public override string PacketID
        {
            get { return "MapCheckPacket"; }
        }
    }
}