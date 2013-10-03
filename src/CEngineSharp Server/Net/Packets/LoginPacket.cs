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

            WriteData(loginOkay, loginOkay ? "Login success!" : "Login failure!", socketIndex);
            Networking.SendPacket(this, socketIndex);

            if (loginOkay)
            {
                Console.WriteLine(username + " has logged in!");
                chatMessagePacket.WriteData(username + " has logged in!");
                Networking.BroadcastPacket(chatMessagePacket);
                Server.ServerWindow.AddPlayerToGrid(GameWorld.Players[socketIndex]);
            }
        }

        public override string PacketID
        {
            get { return "Login"; }
        }
    }
}