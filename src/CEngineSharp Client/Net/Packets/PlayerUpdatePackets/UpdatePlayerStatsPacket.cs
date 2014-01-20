using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.PlayerUpdatePackets
{
    internal class UpdatePlayerStatsPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var playerHp = this.DataBuffer.ReadInteger();
            var playerMp = this.DataBuffer.ReadInteger();

            PlayerManager.GetPlayer(PlayerManager.MyIndex).Hp = playerHp;
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.UpdatePlayerStatsPacket; }
        }
    }
}