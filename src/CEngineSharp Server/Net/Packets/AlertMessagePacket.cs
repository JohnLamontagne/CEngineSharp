using CEngineSharp_Server.Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class AlertMessagePacket : Packet
    {
        public void WriteData(string alertTitle, string alertMessage, int alertX, int alertY, Color color)
        {
            this.PacketBuffer.WriteString(alertTitle);
            this.PacketBuffer.WriteString(alertMessage);
            this.PacketBuffer.WriteInteger(alertX);
            this.PacketBuffer.WriteInteger(alertY);
            this.PacketBuffer.WriteByte(color.R);
            this.PacketBuffer.WriteByte(color.G);
            this.PacketBuffer.WriteByte(color.B);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            // This should never be invoked.
        }

        public override string PacketID
        {
            get { return "AlertMessage"; }
        }
    }
}