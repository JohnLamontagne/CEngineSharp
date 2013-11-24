using CEngineSharp_Server.Utilities;
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
                _maps.Add(MapManager.LoadMap(mapFile.Name));
            }

            Console.WriteLine("Loaded {0} maps", fileInfo.Count());
        }

        private static Map LoadMap(string fileName)
        {
            Map map = new Map();

            using (FileStream fileStream = new FileStream(Constants.FILEPATH_MAPS + fileName, FileMode.Open))
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

                            foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                            {
                                if (binaryReader.ReadBoolean() == false) continue;

                                int tileSetTextureIndex = binaryReader.ReadInt32();
                                int tileLeft = binaryReader.ReadInt32();
                                int tileTop = binaryReader.ReadInt32();
                                int tileWidth = binaryReader.ReadInt32();
                                int tileHeight = binaryReader.ReadInt32();

                                map.GetTile(x, y).Layers[(int)layer] = new Map.Tile.Layer();
                                map.GetTile(x, y).Layers[(int)layer].SpriteRect = new Rect(tileLeft, tileTop, tileHeight, tileWidth);
                                map.GetTile(x, y).Layers[(int)layer].TextureNumber = tileSetTextureIndex;
                            }
                        }
                    }
                }
            }
            return map;
        }
    }
}