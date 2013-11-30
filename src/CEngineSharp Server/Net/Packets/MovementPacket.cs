using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MovementPacket : Packet
    {
        public void WriteData(int playerIndex, Vector2i position, byte direction)
        {
            try
            {
                this.DataBuffer.Flush();
                this.DataBuffer.WriteInteger(playerIndex);
                this.DataBuffer.WriteInteger(position.X);
                this.DataBuffer.WriteInteger(position.Y);
                this.DataBuffer.WriteByte(direction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void Execute(Netty netty)
        {
            try
            {
                int x = this.DataBuffer.ReadInteger();
                int y = this.DataBuffer.ReadInteger();
                byte direction = this.DataBuffer.ReadByte();

                PlayerManager.GetPlayer(this.SocketIndex).MoveTo(new Vector2i(x, y), direction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override int PacketID
        {
            get { return 7; }
        }
    }
}