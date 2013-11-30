﻿using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
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
            nettyClient.Disconnect();
        }

        private static void Handle_Packet(Packet packet)
        {
            Networking.PacketExecutionQueue.Add(packet);
        }

        private static void Handle_ConnectionLost()
        {
            if (Globals.InGame)
            {
                Globals.InGame = false;

                RenderManager.SetRenderState(RenderStates.Render_Menu);

                GameWorld.ClearPlayers();
            }
        }

        public static void ExecuteQueue()
        {
            if (Networking.PacketExecutionQueue == null) return;

            for (int i = 0; i < Networking.PacketExecutionQueue.Count; i++)
            {
                Networking.PacketExecutionQueue[i].Execute(nettyClient);
                Networking.PacketExecutionQueue.RemoveAt(i);
            }
        }

        public static void SendPacket(Packet packet)
        {
            nettyClient.SendPacket(packet);
        }
    }
}