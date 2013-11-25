using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Entity;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class PlayerDataPacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            int playerIndex = this.DataBuffer.ReadInteger();
            string playerName = this.DataBuffer.ReadString();
            int level = this.DataBuffer.ReadInteger();
            int positionX = this.DataBuffer.ReadInteger();
            int positionY = this.DataBuffer.ReadInteger();
            byte direction = this.DataBuffer.ReadByte();

            GameRenderer gameRenderer = RenderManager.CurrentRenderer as GameRenderer;

            Player player = new Player(gameRenderer.TextureManager.GetCharacterTexture("Bob"));
            player.Warp(positionX, positionY, (Directions)direction);
            GameWorld.AddPlayer(playerIndex, player);

            if (playerIndex == Globals.MyIndex)
            {
                player.Camera.SnapToTarget();

                Globals.InGame = true;
            }
        }

        public override string PacketID
        {
            get { return "PlayerDataPacket"; }
        }
    }
}