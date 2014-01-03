using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MapCheckPacket : Packet
    {
        public void WriteData(string mapName, int mapVersion)
        {
            try
            {
                this.DataBuffer.Flush();
                this.DataBuffer.WriteString(mapName);
                this.DataBuffer.WriteInteger(mapVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void Execute(Netty netty)
        {
            try
            {
                Player player = PlayerManager.GetPlayer(this.SocketIndex);

                bool response = this.DataBuffer.ReadBool();

                if (response == false)
                {
                    MapDataPacket mapDataPacket = new MapDataPacket();
                    mapDataPacket.WriteData(PlayerManager.GetPlayer(this.SocketIndex).Map);

                    PlayerManager.GetPlayer(this.SocketIndex).SendPacket(mapDataPacket);

                    return;
                }

                // The player is now in the map.
                // Set their inMap variable to true.
                // This is to make sure they're able to actually see the map before any map updates occur.
                player.SetInMap(true);

                player.SendPlayerData();

                // Send all of the players...
                foreach (var mapPlayer in PlayerManager.GetPlayer(this.SocketIndex).Map.GetPlayers())
                {
                    if (mapPlayer == PlayerManager.GetPlayer(this.SocketIndex)) continue;

                    var playerDataPacket = new PlayerDataPacket();
                    playerDataPacket.WriteData(mapPlayer);
                    player.SendPacket(playerDataPacket);
                }

                // Send all of the items currently spawned in the map.
                foreach (var mapItem in PlayerManager.GetPlayer(this.SocketIndex).Map.GetMapItems())
                {
                    var spawnMapItemPacket = new SpawnMapItemPacket();
                    spawnMapItemPacket.WriteData(mapItem);
                    player.SendPacket(spawnMapItemPacket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override int PacketID
        {
            get { return 5; }
        }
    }
}