using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEngineSharp_Server.World.Content_Managers
{
    public static class MapManager
    {
        private static List<Map> _maps = new List<Map>();

        public static Map GetMap(int mapIndex)
        {
            return _maps[mapIndex];
        }

        public static void AddMap(Map map)
        {
            _maps.Add(map);
        }

        public static int GetMapIndex(Map map)
        {
            return _maps.IndexOf(map);
        }

        public static void LoadMaps()
        {
            DirectoryInfo dI = new DirectoryInfo(Constants.FILEPATH_MAPS);
            FileInfo[] fileInfo = dI.GetFiles("*.map", SearchOption.TopDirectoryOnly);

            Console.WriteLine("Loading maps...");

            foreach (var mapFile in fileInfo)
            {
                _maps.Add(MapManager.LoadMap(mapFile.FullName));
            }

            Console.WriteLine("Loaded {0} maps", fileInfo.Count());
        }

        private static Map LoadMap(string fileName)
        {
            Map map = new Map();

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
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
                            map.SetTile(x, y, new Map.Tile());

                            map.GetTile(x, y).Blocked = binaryReader.ReadBoolean();

                            foreach (Layers layer in Enum.GetValues(typeof(Layers)))
                            {
                                if (binaryReader.ReadBoolean() == false) continue;

                                int tileSetTextureIndex = binaryReader.ReadInt32();
                                int tileLeft = binaryReader.ReadInt32();
                                int tileTop = binaryReader.ReadInt32();
                                int tileWidth = binaryReader.ReadInt32();
                                int tileHeight = binaryReader.ReadInt32();

                                var tileLayer = new Map.Tile.Layer();
                                tileLayer.SpriteRect = new Rect(tileLeft, tileTop, tileHeight, tileWidth);
                                tileLayer.TextureNumber = tileSetTextureIndex;
                                map.GetTile(new Vector2i(x, y)).SetLayer(tileLayer, layer);
                            }
                        }
                    }
                }
            }
            return map;
        }
    }
}