using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void Execute(Netty netty)
        {
            // Server->Client packet only.
        }

        public override int PacketID
        {
            get { return 8; }
        }
    }
}