using CEngineSharp_Client.Graphics;
using SFML.Graphics;
using SFML.Window;
using System;
using System.IO;

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

        private Tile[,] tiles;

        public string Name { get; set; }

        public int Width { get { return this.tiles.GetLength(0); } }

        public int Height { get { return this.tiles.GetLength(1); } }

        public int Version { get; set; }

        public Map()
        {
            this.tiles = new Tile[0, 0];
        }

        public Tile GetTile(int x, int y)
        {
            return this.tiles[x, y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            this.tiles[x, y] = tile;
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            int columnCount = this.tiles.GetLength(1);
            int columnCount2 = newHeight;
            int columns = this.tiles.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(this.tiles, co * columnCount, newArray, co * columnCount2, columnCount);

            this.tiles = newArray;
        }

        public void CacheMap()
        {
            using (FileStream fileStream = new FileStream(Constants.FILEPATH_CACHE + "Maps/" + this.Name + ".map", FileMode.OpenOrCreate))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(this.Name);

                    binaryWriter.Write(this.Version);

                    binaryWriter.Write(this.tiles.GetLength(0));
                    binaryWriter.Write(this.tiles.GetLength(1));

                    for (int x = 0; x < this.tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < this.tiles.GetLength(1); y++)
                        {
                            foreach (Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (this.tiles[x, y].GetLayer(layer) == null)
                                {
                                    binaryWriter.Write(false);
                                    continue;
                                }

                                binaryWriter.Write(true);

                                binaryWriter.Write((RenderManager.CurrentRenderer as GameRenderer).TextureManager.GetTileSetTextureIndex(this.tiles[x, y].GetLayer(layer).Sprite.Texture));

                                binaryWriter.Write(this.tiles[x, y].GetLayer(layer).Sprite.TextureRect.Left);
                                binaryWriter.Write(this.tiles[x, y].GetLayer(layer).Sprite.TextureRect.Top);
                                binaryWriter.Write(this.tiles[x, y].GetLayer(layer).Sprite.TextureRect.Width);
                                binaryWriter.Write(this.tiles[x, y].GetLayer(layer).Sprite.TextureRect.Height);
                            }
                        }
                    }
                }
            }
        }

        public void LoadCache(string mapName)
        {
            using (FileStream fileStream = new FileStream(Constants.FILEPATH_CACHE + "Maps/" + mapName + ".map", FileMode.OpenOrCreate))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    this.Name = binaryReader.ReadString();

                    this.Version = binaryReader.ReadInt32();

                    int mapWidth = binaryReader.ReadInt32();
                    int mapHeight = binaryReader.ReadInt32();

                    this.ResizeMap(mapWidth, mapHeight);

                    while (RenderManager.CurrentRenderer is MenuRenderer) ;

                    for (int x = 0; x < mapWidth; x++)
                    {
                        for (int y = 0; y < mapHeight; y++)
                        {
                            this.SetTile(x, y, new Map.Tile());

                            foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (binaryReader.ReadBoolean() == false) continue;

                                int tileSetTextureIndex = binaryReader.ReadInt32();
                                int tileLeft = binaryReader.ReadInt32();
                                int tileTop = binaryReader.ReadInt32();
                                int tileWidth = binaryReader.ReadInt32();
                                int tileHeight = binaryReader.ReadInt32();

                                IntRect tileRect = new IntRect(tileLeft, tileTop, tileWidth, tileHeight);

                                this.GetTile(x, y).SetLayer(layer, new Tile.Layer(new Sprite((RenderManager.CurrentRenderer as GameRenderer).TextureManager.GetTileSetTexture(tileSetTextureIndex), tileRect), x, y));
                            }
                        }
                    }
                }
            }
        }

        public void Draw(RenderWindow window)
        {
            window.SetView(GameWorld.GetPlayer(Globals.MyIndex).Camera.GetView());

            this.DrawLowerTiles(window);

            foreach (var player in GameWorld.GetPlayers())
                player.Draw(window);

            this.DrawUpperTiles(window);

            window.SetView(window.DefaultView);
        }

        private void DrawLowerTiles(RenderWindow window)
        {
            Camera camera = GameWorld.GetPlayer(Globals.MyIndex).Camera;

            int left = camera.ViewLeft / 32;
            int top = camera.ViewTop / 32;
            int width = left + (Globals.CurrentResolutionWidth / 32) + 1;
            int height = top + (Globals.CurrentResolutionHeight / 32) + 1;

            if (width > this.Width)
                width = this.Width;

            if (height > this.Height)
                height = this.Height;

            for (int x = left; x < width; x++)
            {
                for (int y = top; y < height; y++)
                {
                    if (this.tiles[x, y].GetLayer(Layers.Ground) != null)
                        window.Draw(this.tiles[x, y].GetLayer(Layers.Ground).Sprite);

                    if (this.tiles[x, y].GetLayer(Layers.Mask) != null)
                        window.Draw(this.tiles[x, y].GetLayer(Layers.Mask).Sprite);

                    if (this.tiles[x, y].GetLayer(Layers.Mask2) != null)
                        window.Draw(this.tiles[x, y].GetLayer(Layers.Mask2).Sprite);
                }
            }
        }

        private void DrawUpperTiles(RenderWindow window)
        {
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    if (this.tiles[x, y].GetLayer(Layers.Fringe) != null)
                        window.Draw(this.tiles[x, y].GetLayer(Layers.Fringe).Sprite);

                    if (this.tiles[x, y].GetLayer(Layers.Fringe2) != null)
                        window.Draw(this.tiles[x, y].GetLayer(Layers.Fringe2).Sprite);
                }
            }
        }
    }
}