using CEngineSharp_Server.World;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class InventoryUpdatePacket : Packet
    {
        public void WriteData(Player player)
        {
            this.DataBuffer.WriteInteger(player.GetInventory().Length);

            foreach (var item in player.GetInventory())
            {
                this.DataBuffer.WriteString(item.Name);
                this.DataBuffer.WriteInteger(item.TextureNumber);
            }
        }

        public override void Execute(Netty netty, int socketIndex)
        {
        }

        public override string PacketID
        {
            get { return "InventoryUpdatePacket"; }
        }
    }
}