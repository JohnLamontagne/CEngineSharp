using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using CEngineSharp_Utilities;
using SFML.Window;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets.PlayerUpdatePackets
{
    public class PlayerDataPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var playerIndex = this.DataBuffer.ReadInteger();
            var playerName = this.DataBuffer.ReadString();
            var level = this.DataBuffer.ReadInteger();
            var positionX = this.DataBuffer.ReadInteger();
            var positionY = this.DataBuffer.ReadInteger();
            var direction = this.DataBuffer.ReadByte();
            var textureNumber = this.DataBuffer.ReadInteger();

            var gameRenderer = RenderManager.CurrentRenderer as GameRenderer;

            var player = new Player(RenderManager.TextureManager.GetTexture("character" + textureNumber), new Vector2i(positionX, positionY));
            PlayerManager.AddPlayer(playerIndex, player);

            if (playerIndex == PlayerManager.MyIndex)
            {
                Client.InGame = true;
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.PlayerDataPacket; }
        }
    }
}