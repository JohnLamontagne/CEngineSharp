using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net
{
    public class Networking
    {
        private NettyServer _nettyServer;

        public Networking()
        {
            _nettyServer = new NettyServer(true);

            _nettyServer.Handle_LostConnection = this.Handle_LostConnection;
            _nettyServer.Handle_NewConnection = this.Handle_NewConnection;

            _nettyServer.BindSocket(ServerConfiguration.ServerIP, ServerConfiguration.ServerPort);
            _nettyServer.Listen();
        }

        private void Handle_NewConnection(int socketIndex)
        {
            GameWorld.Players.Add(socketIndex, new Player());
        }

        private void Handle_LostConnection(int socketIndex)
        {
            if (GameWorld.Players[socketIndex].LoggedIn)
            {
                GameWorld.Players[socketIndex].Save();
                Console.WriteLine(GameWorld.Players[socketIndex].Name + " has left!");
            }

            GameWorld.Players.Remove(socketIndex);
        }

        public void KickPlayer(int index)
        {
            var alertMessagePacket = new AlertMessagePacket();
            alertMessagePacket.WriteData("Alert!", "You have been kicked from the server!", 300, 300, Color.Red);
            this.SendPacket(alertMessagePacket, index);

            _nettyServer.RemoveConnection(index);
            GameWorld.Players.Remove(index);
        }

        public void BroadcastPacket(Packet packet)
        {
            _nettyServer.BroadCastPacket(packet);
        }

        public void SendPacket(Packet packet, int index)
        {
            _nettyServer.SendPacket(packet, index);
        }
    }
}