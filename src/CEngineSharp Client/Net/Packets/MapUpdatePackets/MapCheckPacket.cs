using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.MapUpdatePackets
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
            var mapName = this.DataBuffer.ReadString();
            var mapVersion = this.DataBuffer.ReadInteger();

            var mapExistence = MapManager.CheckMapExistence(mapName);

            var mapVersionMatch = false;

            if (mapExistence == true)
            {
                MapManager.Map = new World.Map();
                MapManager.Map.LoadCache(mapName);
                mapVersionMatch = (MapManager.Map.Version == mapVersion);
            }

            this.WriteData((mapExistence & mapVersionMatch));

            NetManager.Instance.SendPacket(this);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.MapCheckPacket; }
        }
    }
}