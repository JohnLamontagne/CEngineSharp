using CEngineSharp_Client.Graphics;
using CEngineSharp_Utilities;
using SFML.Graphics;
using SharpNetty;
using System.Diagnostics;

namespace CEngineSharp_Client.Net.Packets.SocialPackets
{
    public class ChatMessagePacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var message = this.DataBuffer.ReadString();

            if (RenderManager.Instance.CurrentRenderer.GetType() != typeof(GameRenderer)) return;

            var gameRenderer = RenderManager.Instance.CurrentRenderer as GameRenderer;

            // This should NEVER be true if all is well.
            Debug.Assert(gameRenderer != null, "Graphical transition error!");
            gameRenderer.AddChatMessage(message, Color.White);

        }

        public void WriteData(string message)
        {
            this.DataBuffer.WriteString(message);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.ChatMessagePacket; }
        }
    }
}