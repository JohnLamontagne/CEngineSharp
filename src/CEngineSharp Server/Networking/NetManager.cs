using CEngineSharp_Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Server.Networking
{
    public class NetManager
    {
        private readonly NetServer _netServer;

        private Dictionary<PacketType, List<Action<PacketReceivedEventArgs>>> _packetHandlers;

        public event EventHandler<ConnectionEventArgs> Connection_Received;

        public event EventHandler<ConnectionEventArgs> Connection_Lost;

        public NetManager()
        {
            _packetHandlers = new Dictionary<PacketType, List<Action<PacketReceivedEventArgs>>>();

            NetPeerConfiguration config = new NetPeerConfiguration("CEngineSharp");
            config.Port = Constants.SERVER_PORT;
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            _netServer = new NetServer(config);
        }

        public void Start()
        {
            _netServer.Start();
        }

        public void Update()
        {
            NetIncomingMessage message;

            while ((message = _netServer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        PacketType packetType = (PacketType)message.ReadInt16();

                        if (_packetHandlers.ContainsKey(packetType))
                        {
                            var eventArgs = new PacketReceivedEventArgs(message, message.SenderConnection);

                            foreach (var handler in _packetHandlers[packetType])
                                handler.Invoke(eventArgs);
                        }
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        message.SenderConnection.Approve();
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine("Established connection with {0}.", message.SenderConnection.ToString());

                            if (this.Connection_Received != null)
                                this.Connection_Received.Invoke(this, new ConnectionEventArgs(message.SenderConnection));
                        }
                        else if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            Console.WriteLine("Lost connection with {0}.", message.SenderConnection.ToString());

                            if (this.Connection_Lost != null)
                                this.Connection_Lost.Invoke(this, new ConnectionEventArgs(message.SenderConnection));
                        }
                        break;

                    case NetIncomingMessageType.DiscoveryRequest:
                        NetOutgoingMessage response = _netServer.CreateMessage();
                        _netServer.SendDiscoveryResponse(response, message.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(message.ReadString());
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine(message.ReadString());
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                }
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
            return _netServer.CreateMessage();
        }
    }
}