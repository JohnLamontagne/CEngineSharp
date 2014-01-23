using CEngineSharp_Server.Networking.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;
using System.Net;

namespace CEngineSharp_Server.Networking
{
    public class NetManager
    {
        #region Singleton
        private readonly NettyServer _nettyServer;

        private static NetManager _networking;

        public static NetManager Instance
        {
            get { return _networking ?? (_networking = new NetManager()); }
        }

        #endregion

        public NetManager()
        {
            _nettyServer = new NettyServer(true)
            {
                Handle_LostConnection = this.Handle_LostConnection,
                Handle_NewConnection = this.Handle_NewConnection
            };
        }

        public void Start()
        {
            _nettyServer.BindSocket(ServerConfiguration.ServerIP, ServerConfiguration.ServerPort);

            _nettyServer.Listen();
        }

        private void Handle_NewConnection(int socketIndex)
        {
            try
            {
                var player = new Player(socketIndex);
                player.Connection = _nettyServer.GetConnection(socketIndex);
                ContentManager.Instance.PlayerManager.AddPlayer(player, socketIndex);
            }
            catch (Exception)
            {
                _nettyServer.RemoveConnection(socketIndex);
            }
        }

        private void Handle_LostConnection(int socketIndex)
        {
            var player = ContentManager.Instance.PlayerManager.GetPlayer(socketIndex);

            if (player != null && player.LoggedIn)
            {
                Console.WriteLine(player.Name + " has logged out!");

                player.LeaveGame();
            }
            else
            {
                ContentManager.Instance.PlayerManager.RemovePlayer(socketIndex);
            }
        }

        public void KickPlayer(int playerIndex)
        {
            var alertMessagePacket = new AlertMessagePacket();
            alertMessagePacket.WriteData("Alert!", "You have been kicked from the server!", 300, 300, Color.Red);
            ContentManager.Instance.PlayerManager.GetPlayer(playerIndex).SendPacket(alertMessagePacket);

            RemoveConnection(playerIndex);
        }

        public void RemoveConnection(int playerIndex)
        {
            _nettyServer.RemoveConnection(playerIndex);
        }
    }
}