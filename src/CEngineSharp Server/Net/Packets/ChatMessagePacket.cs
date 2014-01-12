using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class ChatMessagePacket : Packet
    {
        public void WriteData(string message)
        {
            try
            {
                this.DataBuffer.Flush();

                this.DataBuffer.WriteString(message);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override void Execute(Netty netty)
        {
            try
            {
                string chatMessage = PlayerManager.GetPlayer(this.SocketIndex).Name + " says: " + this.DataBuffer.ReadString();

                this.WriteData(chatMessage);

                PlayerManager.GetPlayer(this.SocketIndex).Map.SendPacket(this);
            }

            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override int PacketID
        {
            get { return 1; }
        }
    }
}