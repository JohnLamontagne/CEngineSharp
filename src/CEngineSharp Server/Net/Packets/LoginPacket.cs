using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using CEngineSharp_Server.World.Entities;
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

        public override void Execute(Netty netty)
        {
            try
            {
                string username = this.DataBuffer.ReadString();
                string password = this.DataBuffer.ReadString();
                bool loginOkay = PlayerManager.Authenticate(username, password);

                this.WriteData(loginOkay, loginOkay ? "Login success!" : "Login failure!", this.SocketIndex);

                Player player = PlayerManager.GetPlayer(this.SocketIndex);

                player.SendPacket(this);

                if (loginOkay)
                {
                    PlayerManager.LoadPlayer(username, this.SocketIndex);

                    Server.ServerWindow.AddPlayerToGrid(PlayerManager.GetPlayer(this.SocketIndex));

                    player.EnterGame();
                }

                else
                {
                    Networking.RemoveConnection(this.SocketIndex);
                }
            }

            catch (Exception)
            {
                Networking.RemoveConnection(this.SocketIndex);
            }
        }

        public override int PacketID
        {
            get { return 3; }
        }
    }
}