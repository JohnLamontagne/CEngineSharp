using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class MovementPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            try
            {
                int playerIndex = this.DataBuffer.ReadInteger();
                int x = this.DataBuffer.ReadInteger();
                int y = this.DataBuffer.ReadInteger();
                Directions direction = (Directions)this.DataBuffer.ReadByte();

                PlayerManager.GetPlayer(playerIndex).Move(x, y, direction);
            }
            catch (Exception)
            {
                Console.WriteLine("Error: A player moved wrongly!");
            }
        }

        public void WriteData(int x, int y, Directions direction)
        {
            this.DataBuffer.WriteInteger(x);
            this.DataBuffer.WriteInteger(y);
            this.DataBuffer.WriteByte((byte)direction);
        }

        public override int PacketID
        {
            get { return 7; }
        }
    }
}