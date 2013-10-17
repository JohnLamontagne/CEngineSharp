using CEngineSharp_Client.Graphics;
using SFML.Graphics;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class AlertMessagePacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            string alertTitle = this.PacketBuffer.ReadString();
            string alertMessage = this.PacketBuffer.ReadString();
            int alertX = this.PacketBuffer.ReadInteger();
            int alertY = this.PacketBuffer.ReadInteger();
            byte r = this.PacketBuffer.ReadByte();
            byte g = this.PacketBuffer.ReadByte();
            byte b = this.PacketBuffer.ReadByte();
            Color alertColor = new Color(r, g, b);

            if (RenderManager.CurrentRenderer.GetType() == typeof(GameRenderer))
            {
                GameRenderer gameRenderer = RenderManager.CurrentRenderer as GameRenderer;
                gameRenderer.DisplayAlert(alertTitle, alertMessage, alertX, alertY, alertColor);
            }
        }

        public override string PacketID
        {
            get { return "AlertMessage"; }
        }
    }
}