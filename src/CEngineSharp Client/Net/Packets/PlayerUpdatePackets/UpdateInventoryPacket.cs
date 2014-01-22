using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets.PlayerUpdatePackets
{
    public class UpdateInventoryPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var inventoryItemCount = this.DataBuffer.ReadInteger();

            var player = PlayerManager.GetPlayer(PlayerManager.MyIndex);

            var packets = NetManager.Instance;

            player.ClearInventory();

            for (int i = 0; i < inventoryItemCount; i++)
            {
                var itemName = this.DataBuffer.ReadString();
                var itemTextureNum = this.DataBuffer.ReadInteger();

                player.AddInventoryItem(new Item(itemName, itemTextureNum));
            }

        }

        public override int PacketID
        {
            get { return (int)PacketTypes.UpdateInventoryPacket; }
        }
    }
}