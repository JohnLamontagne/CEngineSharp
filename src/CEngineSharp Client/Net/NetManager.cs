using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Client.Net
{
    public class NetManager
    {
        private NetClient _netClient;
        private Dictionary<PacketType, List<Action<PacketReceivedEventArgs>>> _packetHandlers;

        private List<Tuple<NetOutgoingMessage, NetDeliveryMethod, ChannelTypes>> _packetCache;

        public bool Connected { get { return _netClient.ConnectionStatus == NetConnectionStatus.Connected; } }

        public NetManager()
        {
            _packetHandlers = new Dictionary<PacketType, List<Action<PacketReceivedEventArgs>>>();
            _packetCache = new List<Tuple<NetOutgoingMessage, NetDeliveryMethod, ChannelTypes>>();

            NetPeerConfiguration config = new NetPeerConfiguration("CEngineSharp");
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _netClient = new NetClient(config);
            _netClient.Start();
        }

        public void Connect()
        {
            _netClient.DiscoverKnownPeer(Constants.IP, Constants.PORT);
        }

        public void Disconnect()
        {
            _netClient.Disconnect("Cya");
        }

        public void Update()
        {
            NetIncomingMessage message;
            while ((message = _netClient.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        PacketType packetType = (PacketType)message.ReadInt16();

                        if (_packetHandlers.ContainsKey(packetType))
                        {
                            PacketReceivedEventArgs args = new PacketReceivedEventArgs(message);

                            foreach (var handler in _packetHandlers[packetType])
                                handler.Invoke(args);
                        }
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        _netClient.Connect(message.SenderEndPoint);
                        break;
                }

                _netClient.Recycle(message);
            }

            if (_packetCache.Count > 0 && _netClient.ConnectionStatus == NetConnectionStatus.Connected)
            {
                this.SendPacketCache();
            }
        }

        private void SendPacketCache()
        {
            for (int i = 0; i < _packetCache.Count; i++)
            {
                var success = this.SendMessage(_packetCache[i].Item1, _packetCache[i].Item2, _packetCache[i].Item3);

                if (success)
                {
                    _packetCache.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool SendMessage(NetOutgoingMessage message, NetDeliveryMethod method, ChannelTypes channelType)
        {
            if (_netClient.ConnectionStatus == NetConnectionStatus.Connected)
            {
                _netClient.SendMessage(message, method, (int)channelType);
                return true;
            }
            else
            {
                _packetCache.Add(new Tuple<NetOutgoingMessage, NetDeliveryMethod, ChannelTypes>(message, method, channelType));
                return false;
            }
        }

        public void AddPacketHandler(PacketType packetType, Action<PacketReceivedEventArgs> handler)
        {
            if (!_packetHandlers.ContainsKey(packetType))
                _packetHandlers.Add(packetType, new List<Action<PacketReceivedEventArgs>>());

            _packetHandlers[packetType].Add(handler);
        }

        public NetOutgoingMessage ConstructMessage()
        {
            return _netClient.CreateMessage();
        }
    }
}