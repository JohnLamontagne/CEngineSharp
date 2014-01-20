using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets
{
    public class DropItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            try
            {
                var slotNum = this.DataBuffer.ReadInteger();

                var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);
                player.DropItem(slotNum);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.DropItemPacket; }
        }
    }
}