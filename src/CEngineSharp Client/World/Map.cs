using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets.PlayerUpdatePackets;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenderStates = CEngineSharp_Client.Graphics.RenderStates;

namespace CEngineSharp_Client.World
{
    public class Map
    {
        public enum Layers
        {
            Ground,
            Mask,
            Mask2,
            Fringe,
            Fringe2
        }

        public class Tile
        {
            public class Layer
            {
                private Sprite _sprite;

                public Sprite Sprite { get { return this._sprite; } }

                public Layer(Sprite sprite, int posX, int posY)
                {
                    _sprite = sprite;
                    this.Sprite.Position = new Vector2f(posX * 32, posY * 32);
                }
            }

            private Layer[] _layers;

            public bool Blocked { get; set; }


            public bool IsOccupied { get; set; }

            public Tile()
            {
                _layers = new Layer[(int)Layers.Fringe2 + 1];
            }

            public Layer[] GetLayers()
            {
                return _layers;
            }

            public Layer GetLayer(Layers layer)
            {
                return _layers[(int)layer];
            }

            public void SetLayer(Layers layer, Layer newLayer)
            {
                _layers[(int)layer] = newLayer;
            }
        }

        public class MapItem
        {
            public Item Item { get; set; }

            public long SpawnDuration { get; set; }

            public MapItem(Item item, int spawnDuration = -1)
            {
                this.Item = item;

                this.SpawnDuration = spawnDuration;
            }
        }

        private readonly List<MapItem> _items;

        private readonly List<Npc> _mapNpcs;

        private Tile[,] _tiles;

        public string Name { get; set; }

        public int Width { get { return this._tiles.GetLength(0); } }

        public int Height { get { return this._tiles.GetLength(1); } }

        public int Version { get; set; }

        public Map()
        {
            this._tiles = new Tile[0, 0];
            this._items = new List<MapItem>();
            this._mapNpcs = new List<Npc>();
        }

        public Tile GetTile(int x, int y)
        {
            return this._tiles[x, y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            this._tiles[x, y] = tile;
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            int columnCount = this._tiles.GetLength(1);
            int columnCount2 = newHeight;
            int columns = this._tiles.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(this._tiles, co * columnCount, newArray, co * columnCount2, columnCount);

            this._tiles = newArray;
        }

        public void SpawnMapItem(Item item, int x, int y, int spawnDuration)
        {
            this._items.Add(new MapItem(item, spawnDuration));
        }

        public void CacheMap()
        {
            using (var fileStream = new FileStream(Constants.FILEPATH_CACHE + "Maps/" + this.Name + ".map", FileMode.OpenOrCreate))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(this.Name);

                    binaryWriter.Write(this.Version);

                    binaryWriter.Write(this._tiles.GetLength(0));
                    binaryWriter.Write(this._tiles.GetLength(1));

                    for (int x = 0; x < this._tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < this._tiles.GetLength(1); y++)
                        {
                            binaryWriter.Write(this.GetTile(x, y).Blocked);

                            foreach (Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (this._tiles[x, y].GetLayer(layer) == null)
                                {
                                    binaryWriter.Write(false);
                                    continue;
                                }

                                binaryWriter.Write(true);
                                binaryWriter.Write(RenderManager.Instance.TextureManager.GetTextureName(this._tiles[x, y].GetLayer(layer).Sprite.Texture));
                                binaryWriter.Write(this._tiles[x, y].GetLayer(layer).Sprite.TextureRect.Left);
                                binaryWriter.Write(this._tiles[x, y].GetLayer(layer).Sprite.TextureRect.Top);
                                binaryWriter.Write(this._tiles[x, y].GetLayer(layer).Sprite.TextureRect.Width);
                                binaryWriter.Write(this._tiles[x, y].GetLayer(layer).Sprite.TextureRect.Height);
                            }
                        }
                    }
                }
            }
        }

        public void LoadCache(string mapName)
        {
            if (RenderManager.Instance.CurrentRenderer is MenuRenderer)
            {
                RenderManager.Instance.ForceRenderState(RenderStates.RenderGame);
            }

            using (var fileStream = new FileStream(Constants.FILEPATH_CACHE + "Maps/" + mapName + ".map", FileMode.OpenOrCreate))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    this.Name = binaryReader.ReadString();

                    this.Version = binaryReader.ReadInt32();

                    var mapWidth = binaryReader.ReadInt32();
                    var mapHeight = binaryReader.ReadInt32();

                    this.ResizeMap(mapWidth, mapHeight);

                    for (int x = 0; x < mapWidth; x++)
                    {
                        for (int y = 0; y < mapHeight; y++)
                        {
                            this.SetTile(x, y, new Map.Tile());

                            this.GetTile(x, y).Blocked = binaryReader.ReadBoolean();

                            foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (binaryReader.ReadBoolean() == false) continue;

                                var tileSetTextureName = binaryReader.ReadString();
                                var tileLeft = binaryReader.ReadInt32();
                                var tileTop = binaryReader.ReadInt32();
                                var tileWidth = binaryReader.ReadInt32();
                                var tileHeight = binaryReader.ReadInt32();

                                var tileRect = new IntRect(tileLeft, tileTop, tileWidth, tileHeight);

                                this.GetTile(x, y).SetLayer(layer, new Tile.Layer(new Sprite(RenderManager.Instance.TextureManager.GetTexture(tileSetTextureName), tileRect), x, y));
                            }
                        }
                    }
                }
            }
        }

        public Npc GetMapNpc(int index)
        {
            return this._mapNpcs[index];
        }



        public void RemoveMapItem(int mapItemX, int mapItemY)
        {
            MapItem mapItem = this.FindMapItem(new Vector2f(mapItemX * 32, mapItemY * 32));

            if (mapItem != null)
            {
                this._items.Remove(mapItem);
            }
        }

        public void TryPickupItem()
        {
            var x = PlayerManager.GetPlayer(PlayerManager.MyIndex).Position.X * 32;
            var y = PlayerManager.GetPlayer(PlayerManager.MyIndex).Position.Y * 32;

            var mapItem = this.FindMapItem(new Vector2f(x, y));

            if (mapItem == null) return;

            var pickupItemPacket = new PickupItemPacket();
            pickupItemPacket.WriteData(mapItem);
            NetManager.Instance.SendPacket(pickupItemPacket);
        }

        public MapItem FindMapItem(Vector2f position)
        {
            return this._items.FirstOrDefault(mapItem => mapItem.Item.Sprite.Position.X == position.X && mapItem.Item.Sprite.Position.Y == position.Y);
        }

        public void Draw(RenderWindow window)
        {
            var camera = PlayerManager.GetPlayer(PlayerManager.MyIndex).Camera;

            var left = (int)(camera.ViewRect.Left / 32);
            var top = (int)(camera.ViewRect.Top / 32);

            var width = left + (int)(camera.ViewRect.Width / 32) + 2;
            var height = top + (int)(camera.ViewRect.Height / 32) + 1;

            if (width > this.Width)
                width = this.Width;

            if (height > this.Height)
                height = this.Height;

            this.DrawLowerTiles(window, left, top, width, height);

            this.DrawMapItems(window, left, top, width, height);

            this.DrawPlayers(window, left, top, width, height);

            this.DrawNpcs(window, left, top, width, height);

            this.DrawUpperTiles(window, left, top, width, height);

        }

        private void DrawNpcs(RenderTarget window, int left, int top, int width, int height)
        {
            foreach (var npc in this._mapNpcs)
            {
                if (npc.X < left || npc.X > (left + width)) continue;
                if (npc.Y < top || npc.Y > (top + height)) continue;

                npc.Draw(window);
            }
        }

        private void DrawPlayers(RenderTarget window, int left, int top, int width, int height)
        {
            foreach (var player in PlayerManager.GetPlayers())
            {
                if (player.Position.X < left || player.Position.X > (left + width)) continue;
                if (player.Position.Y < top || player.Position.Y > (top + height)) continue;

                player.Draw(window);
            }
        }

        private void DrawMapItems(RenderTarget window, int left, int top, int width, int height)
        {
            // Multiply the width, height, left, and top by 32 to bring it up to scale with the actual screen size (32x32 tiles).
            width *= 32;
            height *= 32;
            left *= 32;
            top *= 32;

            foreach (var mapItem in this._items)
            {
                if (mapItem.Item.Sprite.Position.X >= left && mapItem.Item.Sprite.Position.X <= (left + width))
                {
                    if (mapItem.Item.Sprite.Position.Y >= top && mapItem.Item.Sprite.Position.Y <= (top + height))
                    {
                        window.Draw(mapItem.Item.Sprite);
                    }
                }
            }
        }

        private void DrawLowerTiles(RenderTarget window, int left, int top, int width, int height)
        {
            for (int x = left; x < width; x++)
            {
                for (int y = top; y < height; y++)
                {
                    if (this._tiles[x, y].GetLayer(Layers.Ground) != null)
                        window.Draw(this._tiles[x, y].GetLayer(Layers.Ground).Sprite);

                    if (this._tiles[x, y].GetLayer(Layers.Mask) != null)
                        window.Draw(this._tiles[x, y].GetLayer(Layers.Mask).Sprite);

                    if (this._tiles[x, y].GetLayer(Layers.Mask2) != null)
                        window.Draw(this._tiles[x, y].GetLayer(Layers.Mask2).Sprite);
                }
            }
        }

        private void DrawUpperTiles(RenderTarget window, int left, int top, int width, int height)
        {
            for (int x = left; x < width; x++)
            {
                for (int y = top; y < height; y++)
                {
                    if (this._tiles[x, y].GetLayer(Layers.Fringe) != null)
                        window.Draw(this._tiles[x, y].GetLayer(Layers.Fringe).Sprite);

                    if (this._tiles[x, y].GetLayer(Layers.Fringe2) != null)
                        window.Draw(this._tiles[x, y].GetLayer(Layers.Fringe2).Sprite);
                }
            }
        }

        public void SpawnMapNpc(Npc mapNpc)
        {
            this._mapNpcs.Add(mapNpc);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var npc in this._mapNpcs)
            {
                npc.Update(gameTime);
            }
        }
    }
}