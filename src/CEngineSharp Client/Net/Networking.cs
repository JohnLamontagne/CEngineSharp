using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net.Packets.PlayerUpdatePackets;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CEngineSharp_Client.Net
{
    public class Networking
    {
        private static Networking _networking;

        public static Networking Instance
        {
            get
            {
                if (_networking == null) _networking = new Networking();

                return _networking;
            }
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

                RenderManager.RenderState = RenderStates.RenderMenu;

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

                RenderManager.RenderState = RenderStates.RenderMenu;

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