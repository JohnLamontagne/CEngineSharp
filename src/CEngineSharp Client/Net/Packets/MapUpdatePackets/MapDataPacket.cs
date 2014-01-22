using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using CEngineSharp_Utilities;
using SFML.Graphics;
using SFML.Window;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets.MapUpdatePackets
{
    public class MapDataPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var map = new Map
            {
                Name = this.DataBuffer.ReadString(),
                Version = this.DataBuffer.ReadInteger()
            };

            map.ResizeMap(this.DataBuffer.ReadInteger(), this.DataBuffer.ReadInteger());

            while (RenderManager.Instance.CurrentRenderer is MenuRenderer) ;

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map.SetTile(x, y, new Map.Tile());

                    map.GetTile(x, y).Blocked = this.DataBuffer.ReadBool();

                    foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                    {
                        if (!this.DataBuffer.ReadBool()) continue;

                        var textureNumber = this.DataBuffer.ReadInteger();
                        var left = this.DataBuffer.ReadInteger();
                        var top = this.DataBuffer.ReadInteger();
                        var width = this.DataBuffer.ReadInteger();
                        var height = this.DataBuffer.ReadInteger();

                        var textureRect = new IntRect(left, top, width, height);

                        var tileSprite = new Sprite(RenderManager.Instance.TextureManager.GetTexture("tileset" + textureNumber.ToString()))
                        {
                            TextureRect = textureRect
                        };

                        map.GetTile(x, y).SetLayer(layer, new Map.Tile.Layer(tileSprite, x, y));
                    }
                }
            }

            var mapNpcCount = this.DataBuffer.ReadInteger();

            for (int i = 0; i < mapNpcCount; i++)
            {
                var npc = new Npc()
                {
                    Name = this.DataBuffer.ReadString(),
                    Level = this.DataBuffer.ReadInteger(),
                    Sprite = new Sprite(RenderManager.Instance.TextureManager.GetTexture("npc" + this.DataBuffer.ReadInteger())),
                };
                var position = this.DataBuffer.ReadVector();
                npc.Position = new Vector2i(position.X, position.Y);
            }

            map.CacheMap();
            MapManager.Map = map;

            // Notify the server that we're now in the game.
            var mapCheckPacket = new MapCheckPacket();
            mapCheckPacket.WriteData(true);
            NetManager.Instance.SendPacket(mapCheckPacket);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.MapDataPacket; }
        }
    }
}