using CEngineSharp_Server.Networking.Packets.MapUpdatePackets;
using CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.MapUpdatePackets
{
    public class MapCheckPacket : Packet
    {
        public void WriteData(string mapName, int mapVersion)
        {
            this.DataBuffer.Flush();
            this.DataBuffer.WriteString(mapName);
            this.DataBuffer.WriteInteger(mapVersion);
        }

        public override void Execute(Netty netty)
        {
            try
            {
                var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);

                var response = this.DataBuffer.ReadBool();

                if (response == false)
                {
                    var mapDataPacket = new MapDataPacket();
                    mapDataPacket.WriteData(player.Map);
                    player.SendPacket(mapDataPacket);
                    return;
                }

                // The player is now in the map.
                // Set their inMap variable to true.
                // This is to make sure they're able to actually see the map before any map updates occur.
                player.InMap = true;

                player.SendPlayerData();

                // Send all of the players...
                foreach (var mapPlayer in player.Map.GetPlayers())
                {
                    if (mapPlayer == player) continue;

                    var playerDataPacket = new PlayerDataPacket();
                    playerDataPacket.WriteData(mapPlayer);
                    player.SendPacket(playerDataPacket);
                }

                // Send all of the items currently spawned in the map.
                foreach (var mapItem in player.Map.GetMapItems())
                {
                    var spawnMapItemPacket = new SpawnMapItemPacket();
                    spawnMapItemPacket.WriteData(mapItem);
                    player.SendPacket(spawnMapItemPacket);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.MapCheckPacket; }
        }
    }
}