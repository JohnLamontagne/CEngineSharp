using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class PickupItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
        }

        public void WriteData(Map.MapItem mapItem)
        {
            this.DataBuffer.WriteInteger((int)mapItem.Item.Sprite.Position.X / 32);
            this.DataBuffer.WriteInteger((int)mapItem.Item.Sprite.Position.Y / 32);
        }

        public override int PacketID
        {
            get { return 11; }
        }
    }
}