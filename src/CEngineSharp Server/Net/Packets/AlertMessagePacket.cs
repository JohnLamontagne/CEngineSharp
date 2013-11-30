using CEngineSharp_Server.Utilities;
using SharpNetty;

namespace CEngineSharp_Server.Net.Packets
{
    public class AlertMessagePacket : Packet
    {
        public void WriteData(string alertTitle, string alertMessage, int alertX, int alertY, Color color)
        {
            this.DataBuffer.WriteString(alertTitle);
            this.DataBuffer.WriteString(alertMessage);
            this.DataBuffer.WriteInteger(alertX);
            this.DataBuffer.WriteInteger(alertY);
            this.DataBuffer.WriteByte(color.R);
            this.DataBuffer.WriteByte(color.G);
            this.DataBuffer.WriteByte(color.B);
        }

        public override void Execute(Netty netty)
        {
            // This should never be invoked.
        }

        public override int PacketID
        {
            get { return 0; }
        }
    }
}