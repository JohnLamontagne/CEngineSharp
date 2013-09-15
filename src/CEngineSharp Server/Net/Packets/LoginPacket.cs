using CEngineSharp_Server.World;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class LoginPacket : Packet
    {
        private void WriteData(bool loginOkay, string message)
        {
            this.GetPacketBuffer().Flush();

            this.GetPacketBuffer().WriteByte((byte)(loginOkay == true ? 1 : 0));
            this.GetPacketBuffer().WriteString(message);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            bool loginOkay = GameWorld.Players[socketIndex].Authenticate(this.GetPacketBuffer().ReadString(), this.GetPacketBuffer().ReadString());

            if (loginOkay)
                WriteData(loginOkay, "Logging in now...");
            else
                WriteData(loginOkay, "Authentication failed...");
        }
    }
}