using CEngineSharp_Server.World;
using CEngineSharp_Utilities;
using CEngineSharp_Server;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets
{
    public class PlayerMovementPacket : Packet
    {
        public void WriteData(int playerIndex, Vector position, byte direction)
        {
            try
            {
                this.DataBuffer.Flush();
                this.DataBuffer.WriteInteger(playerIndex);
                this.DataBuffer.WriteVector(position);
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

                var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);

                player.MoveTo(new Vector(x, y), direction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.PlayerMovementPacket; }
        }
    }
}