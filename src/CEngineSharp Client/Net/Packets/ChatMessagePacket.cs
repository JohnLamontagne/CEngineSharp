using CEngineSharp_Client.Graphicss;
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
                if (Program.CurrentRenderer.GetType() == typeof(GameRenderer))
                {
                    string message = this.PacketBuffer.ReadString();
                    GameRenderer gameRenderer = Program.CurrentRenderer as GameRenderer;
                    gameRenderer.AddChatMessage(message, Color.White);
                }
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