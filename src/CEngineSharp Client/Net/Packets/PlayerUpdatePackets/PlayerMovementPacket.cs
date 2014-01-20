using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.PlayerUpdatePackets
{
    public class PlayerMovementPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var playerIndex = this.DataBuffer.ReadInteger();
            var x = this.DataBuffer.ReadInteger();
            var y = this.DataBuffer.ReadInteger();
            var direction = (Directions)this.DataBuffer.ReadByte();

            PlayerManager.GetPlayer(playerIndex).Move(x, y, direction);
        }

        public void WriteData(int x, int y, Directions direction)
        {
            this.DataBuffer.WriteInteger(x);
            this.DataBuffer.WriteInteger(y);
            this.DataBuffer.WriteByte((byte)direction);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.PlayerMovementPacket; }
        }
    }
}