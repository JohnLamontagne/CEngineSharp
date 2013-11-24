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

        public override void Execute(Netty netty, int socketIndex)
        {
            try
            {
                var chatMessagePacket = new ChatMessagePacket();
                string username = this.DataBuffer.ReadString();
                string password = this.DataBuffer.ReadString();
                bool registrationOkay = PlayerManager.RegisterPlayer(PlayerManager.GetPlayer(socketIndex), username, password);

                this.WriteData(registrationOkay, registrationOkay ? "Your account has been registered, logging in now..." : "Your account has failed to register...", socketIndex);
                PlayerManager.GetPlayer(socketIndex).SendPacket(this);

                if (registrationOkay)
                {
                    Server.ServerWindow.AddPlayerToGrid(PlayerManager.GetPlayer(socketIndex));

                    PlayerManager.GetPlayer(socketIndex).EnterGame();
                }
                else
                {
                    Networking.RemoveConnection(socketIndex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override string PacketID
        {
            get { return "Registeration"; }
        }
    }
}