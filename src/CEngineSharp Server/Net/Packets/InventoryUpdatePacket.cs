﻿using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class InventoryUpdatePacket : Packet
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
                Networking.KickPlayer(player.PlayerIndex);
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