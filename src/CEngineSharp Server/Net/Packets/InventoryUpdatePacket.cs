using CEngineSharp_Server.World;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class InventoryUpdatePacket : Packet
    {
        public void WriteData(Player player)
        {
            this.DataBuffer.WriteInteger(player.InventoryCount);

            foreach (var item in player.GetInventory())
            {
                this.DataBuffer.WriteString(item.Name);
                this.DataBuffer.WriteInteger(item.TextureNumber);
            }
        }

        public override void Execute(Netty netty)
        {
        }

        public override int PacketID
        {
            get { return 2; }
        }
    }
}