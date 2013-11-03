using SFML.Graphics;
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

        public Tile[,] Tiles;

        public string Name { get; set; }

        public Map()
        {
            this.Tiles = new Tile[0, 0];
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            int columnCount = Tiles.GetLength(1);
            int columnCount2 = newHeight;
            int columns = Tiles.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(Tiles, co * columnCount, newArray, co * columnCount2, columnCount);

            this.Tiles = newArray;

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    this.Tiles[x, y] = new Tile();
                }
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (var tile in Tiles)
            {
                if (tile == null) continue;

                foreach (var layer in tile.Layers)
                {
                    if (layer == null)
                        continue;

                    window.Draw(layer.Sprite);
                }
            }
        }

        public void Save(List<Texture> tileSetSprites)
        {
            using (FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Data/Maps/" + this.Name + ".map", FileMode.OpenOrCreate))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(this.Name);

                    binaryWriter.Write(this.Tiles.GetLength(0));
                    binaryWriter.Write(this.Tiles.GetLength(1));

                    for (int x = 0; x < this.Tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < this.Tiles.GetLength(1); y++)
                        {
                            foreach (Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (this.Tiles[x, y].Layers[(int)layer] == null)
                                {
                                    binaryWriter.Write(false);
                                    continue;
                                }

                                binaryWriter.Write(true);

                                binaryWriter.Write(tileSetSprites.IndexOf(this.Tiles[x, y].Layers[(int)layer].Sprite.Texture));

                                binaryWriter.Write(this.Tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Left);
                                binaryWriter.Write(this.Tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Top);
                                binaryWriter.Write(this.Tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Width);
                                binaryWriter.Write(this.Tiles[x, y].Layers[(int)layer].Sprite.TextureRect.Height);
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

                    int mapWidth = binaryReader.ReadInt32();
                    int mapHeight = binaryReader.ReadInt32();

                    map.ResizeMap(mapWidth, mapHeight);

                    for (int x = 0; x < mapWidth; x++)
                    {
                        for (int y = 0; y < mapHeight; y++)
                        {
                            map.Tiles[x, y] = new Tile();

                            foreach (Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (binaryReader.ReadBoolean() == false) continue;

                                int tileSetTextureIndex = binaryReader.ReadInt32();
                                int tileLeft = binaryReader.ReadInt32();
                                int tileTop = binaryReader.ReadInt32();
                                int tileWidth = binaryReader.ReadInt32();
                                int tileHeight = binaryReader.ReadInt32();

                                map.Tiles[x, y].Layers[(int)layer] = new Tile.Layer(new Sprite(tileSetTextures[tileSetTextureIndex], new IntRect(tileLeft, tileTop, tileWidth, tileHeight)), x, y);
                            }
                        }
                    }
                }
            }

            return map;
        }
    }
}