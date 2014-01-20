using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.AuthenticationPackets
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
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
                NetworkManager.Instance.KickPlayer(playerIndex);
            }
        }

        public override void Execute(Netty netty)
        {
            try
            {
                string username = this.DataBuffer.ReadString();
                string password = this.DataBuffer.ReadString();

                var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);
                player.Name = username;
                player.Password = password;

                bool loginOkay = ContentManager.Instance.PlayerManager.LoginPlayer(this.SocketIndex);

                this.WriteData(loginOkay, loginOkay ? "Login success!" : "Login failure!", this.SocketIndex);

                player.SendPacket(this);

                if (loginOkay)
                {
                    player.EnterGame();
                }

                else
                {
                    NetworkManager.Instance.RemoveConnection(player.PlayerIndex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
                NetworkManager.Instance.KickPlayer(this.SocketIndex);
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.LoginPacket; }
        }
    }
}