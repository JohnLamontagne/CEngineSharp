using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    internal class SpawnMapItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            string name = this.DataBuffer.ReadString();
            int textureNumber = this.DataBuffer.ReadInteger();
            int x = this.DataBuffer.ReadInteger();
            int y = this.DataBuffer.ReadInteger();
            int duration = this.DataBuffer.ReadInteger();

            Item item = new Item(name, textureNumber);
            item.Sprite.Position = new SFML.Window.Vector2f(x * 32, y * 32);

            MapManager.Map.SpawnMapItem(item, x, y, duration);
        }

        public override int PacketID
        {
            get { return 10; }
        }
    }
}