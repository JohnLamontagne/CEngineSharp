using CEngineSharp_Server.Networking.Packets.SocialPackets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.AuthenticationPackets
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
                    this.DataBuffer.WriteInteger(playerIndex);


                this.DataBuffer.WriteString(response);
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
                var username = this.DataBuffer.ReadString();
                var password = this.DataBuffer.ReadString();

                var player = ContentManager.Instance.PlayerManager.GetPlayer(this.SocketIndex);
                player.Name = username;
                player.Password = password;

                var registrationOkay = ContentManager.Instance.PlayerManager.RegisterPlayer(player);

                this.WriteData(registrationOkay, registrationOkay ? "Your account has been registered, logging in now..." : "Your account has failed to register...", this.SocketIndex);
                player.SendPacket(this);

                if (registrationOkay)
                {
                    player.EnterGame();
                }
                else
                {
                    NetworkManager.Instance.KickPlayer(this.SocketIndex);
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
            get { return (int)PacketTypes.RegistrationPacket; }
        }
    }
}