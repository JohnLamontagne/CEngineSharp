using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class RegisterationPacket : Packet
    {
        private void WriteData(bool registrationOkay, string response, int playerIndex)
        {
            this.PacketBuffer.Flush();

            this.PacketBuffer.WriteByte((byte)(registrationOkay ? 1 : 0));
            if (registrationOkay)
            {
                this.PacketBuffer.WriteInteger(playerIndex);
                this.PacketBuffer.WriteString(response);
            }
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            try
            {
                var chatMessagePacket = new ChatMessagePacket();
                string username = this.PacketBuffer.ReadString();
                string password = this.PacketBuffer.ReadString();
                bool registrationOkay = PlayerManager.RegisterPlayer(PlayerManager.GetPlayer(socketIndex), username, password);

                this.WriteData(registrationOkay, registrationOkay ? "Your account has been registered, logging in now..." : "Your account has failed to register...", socketIndex);
                PlayerManager.GetPlayer(socketIndex).SendPacket(this);

                if (registrationOkay)
                {
                    Console.WriteLine("A new account has been registered: " + username);
                    chatMessagePacket.WriteData(username + " has logged in!");
                    PlayerManager.BroadcastPacket(chatMessagePacket);
                    Server.ServerWindow.AddPlayerToGrid(PlayerManager.GetPlayer(socketIndex));
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override string PacketID
        {
            get { return "Registeration"; }
        }
    }
}