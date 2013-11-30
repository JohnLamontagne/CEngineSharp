using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class DropItemPacket : Packet
    {
        public void WriteData(Item item)
        {
            int itemPosX = (int)item.Sprite.Position.X / 32;
            int itemPosY = (int)item.Sprite.Position.Y / 32;

            this.DataBuffer.WriteInteger(itemPosX);
            this.DataBuffer.WriteInteger(itemPosY);
        }

        public override void Execute(Netty netty)
        {
            // Client->Server only packet.
        }

        public override int PacketID
        {
            get { return 13; }
        }
    }
}