using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class PickupItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int mapItemPosX = this.DataBuffer.ReadInteger();
            int mapItemPosY = this.DataBuffer.ReadInteger();

            PlayerManager.GetPlayer(this.SocketIndex).TryPickupItem(new Vector2i(mapItemPosX, mapItemPosY));
        }

        public override int PacketID
        {
            get { return 11; }
        }
    }
}