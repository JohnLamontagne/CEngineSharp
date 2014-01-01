using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class InventoryUpdatePacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int inventoryItemCount = this.DataBuffer.ReadInteger();

            Player player = PlayerManager.GetPlayer(PlayerManager.MyIndex);

            player.ClearInventory();

            for (int i = 0; i < inventoryItemCount; i++)
            {
                string itemName = this.DataBuffer.ReadString();
                int itemTextureNum = this.DataBuffer.ReadInteger();

                player.AddInventoryItem(new Item(itemName, itemTextureNum));
            }
        }

        public override int PacketID
        {
            get { return 2; }
        }
    }
}