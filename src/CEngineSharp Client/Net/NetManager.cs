using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Client.Net
{
    public class NetManager
    {
        private static NetManager _networking;

        public static NetManager Instance
        {
            get { return _networking ?? (_networking = new NetManager()); }
        }

        private NettyClient _nettyClient;

        private List<Packet> _packetExecutionQueue;

        public bool Connect()
        {
            if (_nettyClient == null)
            {
                _nettyClient = new NettyClient(true)
                {
                    Handle_ConnectionLost = Handle_ConnectionLost,
                    Handle_Packet = Handle_Packet
                };

                _packetExecutionQueue = new List<Packet>();
            }

            return _nettyClient.Connect(Constants.IP, Constants.PORT, 1);
        }

        public void Disconnect()
        {
            if (Client.InGame)
            {
                Client.InGame = false;

                RenderManager.Instance.RenderState = RenderStates.RenderMenu;

                PlayerManager.ClearPlayers();
            }

            _nettyClient.Disconnect();
        }

        private void Handle_Packet(Packet packet)
        {
            _packetExecutionQueue.Add(packet);
        }

        private void Handle_ConnectionLost()
        {
            if (Client.InGame)
            {
                Client.InGame = false;

                RenderManager.Instance.RenderState = RenderStates.RenderMenu;

                PlayerManager.ClearPlayers();
            }
        }

        public void ExecuteQueue()
        {
            if (_packetExecutionQueue == null) return;

            int packetCount = _packetExecutionQueue.Count;

            for (int i = 0; i < packetCount; i++)
            {
                _packetExecutionQueue[i].Execute(_nettyClient);
            }

            for (int i = packetCount - 1; i >= 0; i--)
            {
                _packetExecutionQueue.RemoveAt(i);
            }

        }

        public void SendPacket(Packet packet)
        {
            _nettyClient.SendPacket(packet);
        }
    }
}