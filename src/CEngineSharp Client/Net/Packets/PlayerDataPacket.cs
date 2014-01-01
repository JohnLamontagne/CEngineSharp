using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class PlayerDataPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int playerIndex = this.DataBuffer.ReadInteger();
            string playerName = this.DataBuffer.ReadString();
            int level = this.DataBuffer.ReadInteger();
            int positionX = this.DataBuffer.ReadInteger();
            int positionY = this.DataBuffer.ReadInteger();
            byte direction = this.DataBuffer.ReadByte();

            GameRenderer gameRenderer = RenderManager.CurrentRenderer as GameRenderer;

            Player player = new Player(RenderManager.TextureManager.GetTexture("character0"));
            player.Warp(positionX, positionY, (Directions)direction);
            PlayerManager.AddPlayer(playerIndex, player);

            if (playerIndex == PlayerManager.MyIndex)
            {
                player.Camera.SnapToTarget();

                Client.InGame = true;
            }
        }

        public override int PacketID
        {
            get { return 8; }
        }
    }
}