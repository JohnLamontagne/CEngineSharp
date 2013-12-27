using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class DropItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int slotNum = this.DataBuffer.ReadInteger();

            PlayerManager.GetPlayer(this.SocketIndex).DropItem(slotNum);
        }

        public override int PacketID
        {
            get { return 13; }
        }
    }
}