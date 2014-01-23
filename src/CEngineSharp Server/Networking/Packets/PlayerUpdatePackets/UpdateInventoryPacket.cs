using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets
{
    public class UpdateInventoryPacket : Packet
    {
        public void WriteData(Player player)
        {
            try
            {
                this.DataBuffer.WriteInteger(player.InventoryCount);

                foreach (var item in player.GetInventory())
                {
                    this.DataBuffer.WriteString(item.Name);
                    this.DataBuffer.WriteInteger(item.TextureNumber);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
                NetManager.Instance.KickPlayer(player.PlayerIndex);
            }
        }

        public override void Execute(Netty netty)
        {
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.UpdateInventoryPacket; }
        }
    }
}