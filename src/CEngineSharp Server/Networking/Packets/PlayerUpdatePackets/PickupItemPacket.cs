using CEngineSharp_Server.World;
using CEngineSharp_Utilities;
using CEngineSharp_World;
using SharpNetty;

namespace CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets
{
    public class PickupItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var mapItemPosX = this.DataBuffer.ReadInteger();
            var mapItemPosY = this.DataBuffer.ReadInteger();

            var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);
            player.TryPickupItem(new Vector(mapItemPosX, mapItemPosY));
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.PickupItemPacket; }
        }
    }
}