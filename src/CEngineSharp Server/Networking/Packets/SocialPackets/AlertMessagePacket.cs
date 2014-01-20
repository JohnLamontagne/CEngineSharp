using CEngineSharp_Server.Utilities;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets
{
    public class AlertMessagePacket : Packet
    {
        public void WriteData(string alertTitle, string alertMessage, int alertX, int alertY, Color color)
        {
            try
            {
                this.DataBuffer.WriteString(alertTitle);
                this.DataBuffer.WriteString(alertMessage);
                this.DataBuffer.WriteInteger(alertX);
                this.DataBuffer.WriteInteger(alertY);
                this.DataBuffer.WriteByte(color.R);
                this.DataBuffer.WriteByte(color.G);
                this.DataBuffer.WriteByte(color.B);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override void Execute(Netty netty)
        {
            // This should never be invoked.
            // Server->Client only.
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.AlertMessagePacket; }
        }
    }
}