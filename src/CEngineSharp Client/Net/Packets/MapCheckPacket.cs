using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets
{
    public class MapCheckPacket : Packet
    {
        public void WriteData(bool mapExists)
        {
            this.DataBuffer.Flush();
            this.DataBuffer.WriteBool(mapExists);
        }

        public override void Execute(Netty netty)
        {
            string mapName = this.DataBuffer.ReadString();
            int mapVersion = this.DataBuffer.ReadInteger();

            bool mapExistence = MapManager.CheckMapExistence(mapName);

            bool mapVersionMatch = false;

            if (mapExistence == true)
            {
                MapManager.Map = new World.Map();
                MapManager.Map.LoadCache(mapName);
                mapVersionMatch = (MapManager.Map.Version == mapVersion);
            }

            this.WriteData((mapExistence & mapVersionMatch));

            Networking.SendPacket(this);
        }

        public override int PacketID
        {
            get { return 5; }
        }
    }
}