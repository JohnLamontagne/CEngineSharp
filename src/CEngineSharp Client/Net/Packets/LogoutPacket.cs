﻿using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class LogoutPacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            int playerIndex = this.DataBuffer.ReadInteger();

            if (Globals.MyIndex == playerIndex)
            {
                if (Globals.InGame)
                {
                    Globals.InGame = false;
                    RenderManager.SetRenderState(RenderStates.Render_Menu);

                    GameWorld.ClearPlayers();

                    return;
                }
            }

            GameWorld.RemovePlayer(playerIndex);
        }

        public override string PacketID
        {
            get { return "LogoutPacket"; }
        }
    }
}