using CEngineSharp_Server.World;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class LoginPacket : Packet
    {
        private void WriteData(bool loginOkay, string message, int playerIndex)
        {
            this.PacketBuffer.Flush();

            this.PacketBuffer.WriteByte((byte)(loginOkay == true ? 1 : 0));
            if (loginOkay)
            {
                this.PacketBuffer.WriteInteger(playerIndex);
                this.PacketBuffer.WriteString(message);
            }
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            ChatMessagePacket chatMessagePacket = new ChatMessagePacket();
            string username = this.PacketBuffer.ReadString();
            string password = this.PacketBuffer.ReadString();
            bool loginOkay = GameWorld.Players[socketIndex].Authenticate(username, password);

            if (loginOkay)
                Console.WriteLine(username + " has logged in!");

            WriteData(loginOkay, loginOkay ? "Login success!" : "Login failure!", socketIndex);
            Server.Networking.SendPacket(this, socketIndex);

            chatMessagePacket.WriteData(username + " has logged in!");
            Server.Networking.BroadcastPacket(chatMessagePacket);
        }

        public override string PacketID
        {
            get { return "Login"; }
        }
    }
}