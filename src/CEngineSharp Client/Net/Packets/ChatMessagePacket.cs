using CEngineSharp_Client.Graphics;
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
                if (RenderManager.CurrentRenderer.GetType() == typeof(GameRenderer))
                {
                    string message = this.DataBuffer.ReadString();
                    GameRenderer gameRenderer = RenderManager.CurrentRenderer as GameRenderer;
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
            this.DataBuffer.WriteString(message);
        }

        public override string PacketID
        {
            get { return "ChatMessage"; }
        }
    }
}