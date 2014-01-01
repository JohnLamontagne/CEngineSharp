using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using SharpNetty;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Client.Net
{
    public static class Networking
    {
        private static NettyClient nettyClient;

        private static List<Packet> PacketExecutionQueue;

        public static bool Connect()
        {
            if (nettyClient == null)
            {
                nettyClient = new NettyClient(true);

                nettyClient.Handle_ConnectionLost = Handle_ConnectionLost;
                nettyClient.Handle_Packet = Handle_Packet;

                Networking.PacketExecutionQueue = new List<Packet>();
            }

            return nettyClient.Connect(Constants.IP, Constants.PORT, 1);
        }

        public static void Disconnect()
        {
            if (Client.InGame)
            {
                Client.InGame = false;

                RenderManager.RenderState = RenderStates.Render_Menu;

                PlayerManager.ClearPlayers();
            }

            nettyClient.Disconnect();
        }

        private static void Handle_Packet(Packet packet)
        {
            Networking.PacketExecutionQueue.Add(packet);
        }

        private static void Handle_ConnectionLost()
        {
            if (Client.InGame)
            {
                Client.InGame = false;

                RenderManager.RenderState = RenderStates.Render_Menu;

                PlayerManager.ClearPlayers();
            }
        }

        public static void ExecuteQueue()
        {
            try
            {
                if (Networking.PacketExecutionQueue == null) return;

                for (int i = 0; i < Networking.PacketExecutionQueue.Count; i++)
                {
                    if (Networking.PacketExecutionQueue[i] != null)
                    {
                        Networking.PacketExecutionQueue[i].Execute(nettyClient);
                        Networking.PacketExecutionQueue.RemoveAt(i);
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        public static void SendPacket(Packet packet)
        {
            nettyClient.SendPacket(packet);
        }
    }
}