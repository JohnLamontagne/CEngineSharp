using CEngineSharp_Client.Graphics;
using CEngineSharp_Utilities;
using SFML.Graphics;
using SharpNetty;
using System.Diagnostics;

namespace CEngineSharp_Client.Net.Packets.SocialPackets
{
    public class AlertMessagePacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var alertTitle = this.DataBuffer.ReadString();
            var alertMessage = this.DataBuffer.ReadString();
            var alertX = this.DataBuffer.ReadInteger();
            var alertY = this.DataBuffer.ReadInteger();
            var r = this.DataBuffer.ReadByte();
            var g = this.DataBuffer.ReadByte();
            var b = this.DataBuffer.ReadByte();
            var alertColor = new Color(r, g, b);

            if (RenderManager.CurrentRenderer.GetType() != typeof(GameRenderer)) return;

            var gameRenderer = RenderManager.CurrentRenderer as GameRenderer;

            // This should NEVER be true if all is well.
            Debug.Assert(gameRenderer != null, "Graphical transition error!");
            gameRenderer.DisplayAlert(alertTitle, alertMessage, alertX, alertY, alertColor);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.AlertMessagePacket; }
        }
    }
}