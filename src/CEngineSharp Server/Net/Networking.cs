using CEngineSharp_Server.Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net
{
    public class Networking
    {
        private NettyServer _nettyServer;

        public Networking()
        {
            _nettyServer = new NettyServer();
            _nettyServer.BindSocket(ServerConfiguration.ServerIP, ServerConfiguration.ServerPort);
            _nettyServer.Listen();
        }
    }
}