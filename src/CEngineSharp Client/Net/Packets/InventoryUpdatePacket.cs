using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class InventoryUpdatePacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            int inventoryItemCount = this.DataBuffer.ReadInteger();

            for (int i = 0; i < inventoryItemCount; i++)
            {
                string itemName = this.DataBuffer.ReadString();
                int itemTextureNum = this.DataBuffer.ReadInteger();

                GameWorld.GetPlayer(Globals.MyIndex).AddInventoryItem(new Item(itemName, itemTextureNum));
            }
        }

        public override string PacketID
        {
            get { return "InventoryUpdatePacket"; }
        }
    }
}