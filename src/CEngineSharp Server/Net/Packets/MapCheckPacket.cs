using CEngineSharp_Server.World.Content_Managers;
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

        public override void Execute(Netty netty, int socketIndex)
        {
            try
            {
                bool response = this.DataBuffer.ReadBool();

                if (response == false)
                {
                    MapDataPacket mapDataPacket = new MapDataPacket();
                    mapDataPacket.WriteData(PlayerManager.GetPlayer(socketIndex).Map);
                    PlayerManager.GetPlayer(socketIndex).SendPacket(mapDataPacket);
                }

                // Send the map players.

                var playerDataPacket = new PlayerDataPacket();
                playerDataPacket.WriteData(PlayerManager.GetPlayer(socketIndex));
                PlayerManager.GetPlayer(socketIndex).Map.SendPacket(playerDataPacket);

                // Send all of the players...
                foreach (var player in PlayerManager.GetPlayer(socketIndex).Map.GetPlayers())
                {
                    if (player == PlayerManager.GetPlayer(socketIndex)) continue;

                    playerDataPacket = new PlayerDataPacket();
                    playerDataPacket.WriteData(player);
                    PlayerManager.GetPlayer(socketIndex).SendPacket(playerDataPacket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override string PacketID
        {
            get { return "MapCheckPacket"; }
        }
    }
}