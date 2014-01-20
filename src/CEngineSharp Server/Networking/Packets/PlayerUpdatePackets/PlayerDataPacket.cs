using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets
{
    public class PlayerDataPacket : Packet
    {
        public void WriteData(Player player)
        {
            try
            {
                this.DataBuffer.Flush();
                this.DataBuffer.WriteInteger(player.PlayerIndex);
                this.DataBuffer.WriteString(player.Name);
                this.DataBuffer.WriteInteger(player.Level);
                this.DataBuffer.WriteInteger(player.Position.X);
                this.DataBuffer.WriteInteger(player.Position.Y);
                this.DataBuffer.WriteByte(player.Direction);
                this.DataBuffer.WriteInteger(player.TextureNumber);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override void Execute(Netty netty)
        {
            // Server->Client packet only.
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.PlayerDataPacket; }
        }
    }
}