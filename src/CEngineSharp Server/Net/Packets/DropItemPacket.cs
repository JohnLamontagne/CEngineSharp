using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class DropItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            try
            {
                int slotNum = this.DataBuffer.ReadInteger();

                PlayerManager.GetPlayer(this.SocketIndex).DropItem(slotNum);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override int PacketID
        {
            get { return 13; }
        }
    }
}