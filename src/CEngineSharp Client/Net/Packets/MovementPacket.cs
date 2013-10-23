using CEngineSharp_Client.World;
using SFML.Window;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class MovementPacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            int x = this.PacketBuffer.ReadInteger();
            int y = this.PacketBuffer.ReadInteger();
            GameWorld.Players[socketIndex].MovePlayer(x, y);
        }

        public void WriteData(int x, int y)
        {
            this.PacketBuffer.WriteInteger(x);
            this.PacketBuffer.WriteInteger(y);
        }

        public override string PacketID
        {
            get { return "MovementPacket"; }
        }
    }
}