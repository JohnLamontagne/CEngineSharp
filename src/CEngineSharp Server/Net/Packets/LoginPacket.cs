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
            }

            this.PacketBuffer.WriteString(message);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            ChatMessagePacket chatMessagePacket = new ChatMessagePacket();
            string username = this.PacketBuffer.ReadString();
            string password = this.PacketBuffer.ReadString();
            bool loginOkay = PlayerManager.Authenticate(username, password);

            this.WriteData(loginOkay, loginOkay ? "Login success!" : "Login failure!", socketIndex);
            PlayerManager.GetPlayer(socketIndex).SendPacket(this);

            if (loginOkay)
            {
                PlayerManager.LoadPlayer(username, socketIndex);
                Console.WriteLine(username + " has logged in!");
                chatMessagePacket.WriteData(username + " has logged in!");
                PlayerManager.BroadcastPacket(chatMessagePacket);
                Server.ServerWindow.AddPlayerToGrid(PlayerManager.GetPlayer(socketIndex));
            }
        }

        public override string PacketID
        {
            get { return "Login"; }
        }
    }
}