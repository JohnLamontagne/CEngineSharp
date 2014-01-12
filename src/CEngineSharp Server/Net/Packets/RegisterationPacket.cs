using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class RegisterationPacket : Packet
    {
        private void WriteData(bool registrationOkay, string response, int playerIndex)
        {
            try
            {
                this.DataBuffer.Flush();

                this.DataBuffer.WriteByte((byte)(registrationOkay ? 1 : 0));
                if (registrationOkay)
                {
                    this.DataBuffer.WriteInteger(playerIndex);
                    this.DataBuffer.WriteString(response);
                }
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
                var chatMessagePacket = new ChatMessagePacket();
                string username = this.DataBuffer.ReadString();
                string password = this.DataBuffer.ReadString();
                bool registrationOkay = PlayerManager.RegisterPlayer(PlayerManager.GetPlayer(this.SocketIndex), username, password);

                this.WriteData(registrationOkay, registrationOkay ? "Your account has been registered, logging in now..." : "Your account has failed to register...", this.SocketIndex);
                PlayerManager.GetPlayer(this.SocketIndex).SendPacket(this);

                if (registrationOkay)
                {
                    Server.ServerWindow.AddPlayerToGrid(PlayerManager.GetPlayer(this.SocketIndex));

                    PlayerManager.GetPlayer(this.SocketIndex).EnterGame();
                }
                else
                {
                    Networking.KickPlayer(this.SocketIndex);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
                Networking.KickPlayer(this.SocketIndex);
            }
        }

        public override int PacketID
        {
            get { return 9; }
        }
    }
}