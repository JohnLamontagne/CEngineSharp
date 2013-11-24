using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class LoginPacket : Packet
    {
        private void WriteData(bool loginOkay, string message, int playerIndex)
        {
            try
            {
                this.DataBuffer.Flush();

                this.DataBuffer.WriteByte((byte)(loginOkay == true ? 1 : 0));
                if (loginOkay)
                {
                    this.DataBuffer.WriteInteger(playerIndex);
                }

                this.DataBuffer.WriteString(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            string username = this.DataBuffer.ReadString();
            string password = this.DataBuffer.ReadString();
            bool loginOkay = PlayerManager.Authenticate(username, password);

            this.WriteData(loginOkay, loginOkay ? "Login success!" : "Login failure!", socketIndex);
            PlayerManager.GetPlayer(socketIndex).SendPacket(this);

            if (loginOkay)
            {
                PlayerManager.LoadPlayer(username, socketIndex);

                Server.ServerWindow.AddPlayerToGrid(PlayerManager.GetPlayer(socketIndex));

                PlayerManager.GetPlayer(socketIndex).EnterGame();
            }
            else
            {
                Networking.RemoveConnection(socketIndex);
            }
        }

        public override string PacketID
        {
            get { return "Login"; }
        }
    }
}