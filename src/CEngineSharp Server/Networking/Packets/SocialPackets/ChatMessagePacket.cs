using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.SocialPackets
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
                var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);

                var chatMessage = player.Name + " says: " + this.DataBuffer.ReadString();

                this.WriteData(chatMessage);

                player.Map.SendPacket(this);
            }

            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.ChatMessagePacket; }
        }
    }
}