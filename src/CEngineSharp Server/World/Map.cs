using CEngineSharp_Server.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Server.World
{
    public class Map
    {
        public class Tile
        {
            public Tile(Layers layer, int textureNum, Rect textureRect, bool isWall)
            {
                this.Layer = layer;
                this.TextureNumber = textureNum;
                this.TextureRect = textureRect;
                _isWall = isWall;
                _isOccupied = false;
            }

            public enum Layers
            {
                Ground,
                Mask
            }

            public readonly Layers Layer;

            public readonly int TextureNumber;

            public readonly Rect TextureRect;

            private bool _isWall;

            private bool _isOccupied;

            public bool IsOccupied
            {
                get { return _isOccupied; }
                set { _isOccupied = value; }
            }

            public bool IsWall
            {
                get { return _isWall; }
            }

            public bool IsBlocked()
            {
                return (_isOccupied || _isWall);
            }
        }

        private string _name;
        private Tile[,] _tiles;

        public List<int> Players;

        public Map()
        {
            Players = new List<int>();
        }

        public string Name
        {
            get { return _name; }
        }

        public Tile GetTile(int x, int y)
        {
            return (_tiles[x, y] != null ? _tiles[x, y] : null);
        }

        public void Load(string fileName)
        {
            int mapWidth;
            int mapHeight;

            using (FileStream fs = new FileStream(Constants.FilePath_Maps + fileName, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    _name = binaryReader.ReadString();
                    mapWidth = binaryReader.ReadInt32();
                    mapHeight = binaryReader.ReadInt32();
                    _tiles = new Tile[mapWidth, mapHeight];

                    for (int x = 0; x < _tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < _tiles.GetLength(1); y++)
                        {
                            if (binaryReader.ReadBoolean())
                            {
                                Tile.Layers layer;
                                Rect textureRect;
                                int textureNum;
                                bool isWall;
                                Tile tile;

                                layer = (Tile.Layers)binaryReader.ReadByte();
                                textureNum = binaryReader.ReadInt32();
                                textureRect = new Rect(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
                                isWall = binaryReader.ReadBoolean();
                                tile = new Tile(layer, textureNum, textureRect, isWall);
                            }
                        }
                    }
                }
            }
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(Constants.FilePath_Maps + this.Name + ".map", FileMode.OpenOrCreate))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fs))
                {
                    binaryWriter.Write(_name);

                    binaryWriter.Write(_tiles.GetLength(0));
                    binaryWriter.Write(_tiles.GetLength(1));

                    for (int x = 0; x < _tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < _tiles.GetLength(1); y++)
                        {
                            var tile = this.GetTile(x, y);

                            if (tile == null)
                            {
                                binaryWriter.Write(false);
                                continue;
                            }

                            binaryWriter.Write(true);
                            binaryWriter.Write((byte)tile.Layer);
                            binaryWriter.Write(tile.TextureNumber);
                            binaryWriter.Write(tile.TextureRect.Left);
                            binaryWriter.Write(tile.TextureRect.Top);
                            binaryWriter.Write(tile.TextureRect.Height);
                            binaryWriter.Write(tile.TextureRect.Width);
                            binaryWriter.Write(tile.IsWall);
                            binaryWriter.Write(tile.IsOccupied);
                        }
                    }
                }
            }
        }
    }
}