using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using CEngineSharp_World.Entities;
using SharpNetty;

namespace CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets
{
    public class UpdatePlayerStatsPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            // Server->Client only.
        }

        public void WriteData(Player player)
        {
            this.DataBuffer.WriteInteger(player.GetStat(Stats.Health));
            this.DataBuffer.WriteInteger(0);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.UpdatePlayerStatsPacket; }
        }
    }
}