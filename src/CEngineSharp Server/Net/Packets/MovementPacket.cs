using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MovementPacket : Packet
    {
        public void WriteData(int playerIndex, Vector2i position)
        {
            this.PacketBuffer.WriteInteger(position.X);
            this.PacketBuffer.WriteInteger(position.Y);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            int x = this.PacketBuffer.ReadInteger();
            int y = this.PacketBuffer.ReadInteger();

            PlayerManager.GetPlayer(socketIndex).MoveTo(new Vector2i(x, y));
        }

        public override string PacketID
        {
            get { return "MovementPacket"; }
        }
    }
}