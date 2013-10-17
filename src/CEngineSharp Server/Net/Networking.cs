using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net
{
    public static class Networking
    {
        private static NettyServer _nettyServer;

        public static void Start()
        {
            _nettyServer = new NettyServer(true);

            _nettyServer.Handle_LostConnection = Networking.Handle_LostConnection;
            _nettyServer.Handle_NewConnection = Networking.Handle_NewConnection;

            _nettyServer.BindSocket(ServerConfiguration.ServerIP, ServerConfiguration.ServerPort);

            _nettyServer.Listen();
        }

        private static void Handle_NewConnection(int socketIndex)
        {
            PlayerManager.AddPlayer(socketIndex, new Player(_nettyServer.GetConnection(socketIndex)));
        }

        private static void Handle_LostConnection(int socketIndex)
        {
            if (PlayerManager.GetPlayer(socketIndex).LoggedIn)
            {
                PlayerManager.SavePlayer(PlayerManager.GetPlayer(socketIndex));
                PlayerManager.GetPlayer(socketIndex).LeaveGame();
                Console.WriteLine(PlayerManager.GetPlayer(socketIndex).Name + " has left!");
            }

            PlayerManager.RemovePlayer(socketIndex);
        }

        public static void KickPlayer(int index)
        {
            var alertMessagePacket = new AlertMessagePacket();
            alertMessagePacket.WriteData("Alert!", "You have been kicked from the server!", 300, 300, Color.Red);
            PlayerManager.GetPlayer(index).SendPacket(alertMessagePacket);

            _nettyServer.RemoveConnection(index);
            PlayerManager.RemovePlayer(index);
        }
    }
}