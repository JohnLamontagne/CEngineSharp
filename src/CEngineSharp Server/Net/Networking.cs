using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;
using System.Net;

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
            // Check to make sure that the remote ip isn't already connected.
            foreach (var player in PlayerManager.GetPlayers())
            {
                if ((player.Connection.Socket.RemoteEndPoint as IPEndPoint).Address == (_nettyServer.GetConnection(socketIndex).Socket.RemoteEndPoint as IPEndPoint).Address)
                {
                    _nettyServer.RemoveConnection(socketIndex);
                    return;
                }
            }

            PlayerManager.AddPlayer(socketIndex, new Player(_nettyServer.GetConnection(socketIndex), socketIndex));
        }

        private static void Handle_LostConnection(int socketIndex)
        {
            if (PlayerManager.PlayerCount > socketIndex)
            {
                if (PlayerManager.GetPlayer(socketIndex).LoggedIn)
                {
                    PlayerManager.GetPlayer(socketIndex).LeaveGame();
                    Server.ServerWindow.RemovePlayerFromGrid(socketIndex);
                }
            }
        }

        public static void KickPlayer(int playerIndex)
        {
            var alertMessagePacket = new AlertMessagePacket();
            alertMessagePacket.WriteData("Alert!", "You have been kicked from the server!", 300, 300, Color.Red);
            PlayerManager.GetPlayer(playerIndex).SendPacket(alertMessagePacket);

            Networking.RemoveConnection(playerIndex);
        }

        public static void RemoveConnection(int playerIndex)
        {
            _nettyServer.RemoveConnection(playerIndex);
        }
    }
}