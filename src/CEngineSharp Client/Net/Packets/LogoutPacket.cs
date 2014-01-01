using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class LogoutPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int playerIndex = this.DataBuffer.ReadInteger();

            if (PlayerManager.MyIndex == playerIndex)
            {
                RenderManager.RenderState = RenderStates.Render_Menu;

                PlayerManager.ClearPlayers();

                return;
            }

            PlayerManager.RemovePlayer(playerIndex);
        }

        public override int PacketID
        {
            get { return 4; }
        }
    }
}