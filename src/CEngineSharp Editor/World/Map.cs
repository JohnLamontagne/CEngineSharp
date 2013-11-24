using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Editor.World
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
            public RectangleShape BlockedCover { get; set; }

            public bool Blocked { get; set; }

            public bool IsOccupied { get; set; }

            public Layer[] Layers;

            public class Layer
            {
                public Sprite Sprite { get; set; }

                public Layer(Sprite sprite, int posX, int posY)
                {
                    this.Sprite = sprite;
                    this.Sprite.Position = new SFML.Window.Vector2f(posX * 32, posY * 32);
                }
            }

            public Tile()
            {
                this.Layers = new Layer[(int)Map.Layers.Fringe2 + 1];
            }
        }

        private Tile[,] tiles;

        public string Name { get; set; }

        public int Version { get; private set; }

        public int MapWidth { get { return this.tiles.GetLength(0); } }

        public int MapHeight { get { return this.tiles.GetLength(1); } }

        public Map()
        {
            this.tiles = new Tile[0, 0];
        }

        public Tile GetTile(int x, int y)
        {
            if (x > this.MapWidth || y > this.MapHeight || y < 0 || x < 0) return null;

            return this.tiles[x, y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            this.tiles[x, y] = tile;
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            Tile[,] newArray = new Tile[newWidth, newHeight];
            int minX = Math.Min(this.tiles.GetLength(0), newArray.GetLength(0));
            int minY = Math.Min(this.tiles.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minX; ++i)
                Array.Copy(this.tiles, i * this.tiles.GetLength(1), newArray, i * newArray.GetLength(1), minY);

            this.tiles = newArray;
        }

        public void Draw(RenderWindow window, int left, int top, int width, int height)
        {
            width += left;
            height += top;

            for (int x = left; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (this.tiles[x, y] != null)
                    {
                        foreach (var layer in this.tiles[x, y].Layers)
                        {
                            if (layer != null)

                                window.Draw(layer.Sprite);
                        }

                        if (this.tiles[x, y].BlockedCover != null)
                            window.Draw(this.tiles[x, y].BlockedCover);
                    }
                }
            }
        }

        public void Save(string filePath, List<Texture> tileSetSprites)
        {
            this.Version++;

            using (FileStream fileStream = new FileStream(filePath + this.Name + ".map", FileMode.OpenOrCreate))
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
                            binaryWriter.Write(this.GetTile(x, y).Blocked);

                            foreach (Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (this.tiles[x, y].Layers[(int)layer] == null)
                                {
                                    binaryWriter.Write(false);
                                    continue;
                                }

                                binaryWriter.Write(true);

                                binaryWriter.Write(tileSetSprites.IndexOf(this.GetTile(x, y).Layers[(int)layer].Sprite.Texture));

                                binaryWriter.Write(this.tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Left);
                                binaryWriter.Write(this.tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Top);
                                binaryWriter.Write(this.tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Width);
                                binaryWriter.Write(this.tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Height);
                            }
                        }
                    }
                }
            }
        }

        public static Map LoadMap(string filePath, List<Texture> tileSetTextures)
        {
            Map map = new Map();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    map.Name = binaryReader.ReadString();

                    map.Version = binaryReader.ReadInt32();

                    int mapWidth = binaryReader.ReadInt32();
                    int mapHeight = binaryReader.ReadInt32();

                    map.ResizeMap(mapWidth, mapHeight);

                    for (int x = 0; x < mapWidth; x++)
                    {
                        for (int y = 0; y < mapHeight; y++)
                        {
                            map.tiles[x, y] = new Tile();

                            map.tiles[x, y].Blocked = binaryReader.ReadBoolean();

                            if (map.tiles[x, y].Blocked)
                            {
                                map.tiles[x, y].BlockedCover = new RectangleShape(new SFML.Window.Vector2f(32, 32));
                                map.tiles[x, y].BlockedCover.FillColor = new Color(255, 0, 0, 100);
                                map.tiles[x, y].BlockedCover.Position = new Vector2f(x * 32, y * 32);
                            }

                            foreach (Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (binaryReader.ReadBoolean() == false) continue;

                                int tileSetTextureIndex = binaryReader.ReadInt32();
                                int tileLeft = binaryReader.ReadInt32();
                                int tileTop = binaryReader.ReadInt32();
                                int tileWidth = binaryReader.ReadInt32();
                                int tileHeight = binaryReader.ReadInt32();

                                map.tiles[x, y].Layers[(int)layer] = new Tile.Layer(new Sprite(tileSetTextures[tileSetTextureIndex], new IntRect(tileLeft, tileTop, tileWidth, tileHeight)), x, y);
                            }
                        }
                    }
                }
            }

            return map;
        }
    }
}