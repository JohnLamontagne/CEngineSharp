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
        public override void Execute(Netty netty)
        {
            Map map = new Map();

            map.Name = this.DataBuffer.ReadString();

            map.Version = this.DataBuffer.ReadInteger();

            map.ResizeMap(DataBuffer.ReadInteger(), DataBuffer.ReadInteger());

            while (RenderManager.CurrentRenderer is MenuRenderer) ;

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map.SetTile(x, y, new Map.Tile());

                    map.GetTile(x, y).Blocked = this.DataBuffer.ReadBool();

                    foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                    {
                        if (this.DataBuffer.ReadBool())
                        {
                            int textureNumber = this.DataBuffer.ReadInteger();
                            int left = this.DataBuffer.ReadInteger();
                            int top = this.DataBuffer.ReadInteger();
                            int width = this.DataBuffer.ReadInteger();
                            int height = this.DataBuffer.ReadInteger();

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

            // Notify the server that we're now in the game.
            MapCheckPacket mapCheckPacket = new MapCheckPacket();
            mapCheckPacket.WriteData(true);
            Networking.SendPacket(mapCheckPacket);
        }

        public override int PacketID
        {
            get { return 6; }
        }
    }
}