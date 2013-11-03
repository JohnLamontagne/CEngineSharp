using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using SFML.Graphics;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class MapDataPacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            Map map = new Map();

            map.Name = this.PacketBuffer.ReadString();

            map.ResizeMap(PacketBuffer.ReadInteger(), PacketBuffer.ReadInteger());

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map.SetTile(x, y, new Map.Tile());

                    foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                    {
                        if (this.PacketBuffer.ReadBool())
                        {
                            int textureNumber = this.PacketBuffer.ReadInteger();
                            int left = this.PacketBuffer.ReadInteger();
                            int top = this.PacketBuffer.ReadInteger();
                            int width = this.PacketBuffer.ReadInteger();
                            int height = this.PacketBuffer.ReadInteger();

                            IntRect textureRect = new IntRect(left, top, width, height);

                            Sprite tileSprite = new Sprite((RenderManager.CurrentRenderer as GameRenderer).TextureManager.GetTileSetTexture(textureNumber));
                            tileSprite.TextureRect = textureRect;

                            map.GetTile(x, y).SetLayer(layer, new Map.Tile.Layer(tileSprite, x, y));
                        }
                    }
                }
            }

            map.CacheMap();
            MapManager.Map = map;
        }

        public override string PacketID
        {
            get { return "MapDataPacket"; }
        }
    }
}