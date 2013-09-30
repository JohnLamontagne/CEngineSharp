using SFML.Graphics;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class ChatMessagePacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            try
            {
                string message = this.PacketBuffer.ReadString();

                Program.GameGraphics.AddChatMessage(message, Color.White);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteData(string message)
        {
            this.PacketBuffer.WriteString(message);
        }

        public override string PacketID
        {
            get { return "ChatMessage"; }
        }
    }
}