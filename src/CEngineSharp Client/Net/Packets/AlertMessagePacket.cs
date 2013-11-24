using CEngineSharp_Client.Graphics;
using SFML.Graphics;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets
{
    public class AlertMessagePacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            string alertTitle = this.DataBuffer.ReadString();
            string alertMessage = this.DataBuffer.ReadString();
            int alertX = this.DataBuffer.ReadInteger();
            int alertY = this.DataBuffer.ReadInteger();
            byte r = this.DataBuffer.ReadByte();
            byte g = this.DataBuffer.ReadByte();
            byte b = this.DataBuffer.ReadByte();
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