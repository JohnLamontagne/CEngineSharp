using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class ChatMessagePacket : Packet
    {
        public void WriteData(string message)
        {
            this.DataBuffer.Flush();

            this.DataBuffer.WriteString(message);
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
                Console.WriteLine("Chat Message Error: " + ex.StackTrace);
            }
        }

        public override int PacketID
        {
            get { return 1; }
        }
    }
}