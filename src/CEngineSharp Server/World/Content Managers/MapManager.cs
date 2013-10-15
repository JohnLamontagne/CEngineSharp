using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            FileInfo[] fileInfo = dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly);

            // for testing purposed only
            _maps.Add(new Map());
            //

            Console.WriteLine("Loading maps...");

            foreach (var mapFile in fileInfo)
            {
                MapManager.LoadMap(mapFile.Name);
            }

            Console.WriteLine("Loaded {0} maps", fileInfo.Count());
        }

        private static Map LoadMap(string fileName)
        {
            Map map = new Map();
            int mapHeight;
            int mapWidth;

            using (FileStream fileStream = new FileStream(Constants.FILEPATH_MAPS + fileName, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    mapWidth = binaryReader.ReadInt32();
                    mapHeight = binaryReader.ReadInt32();
                    map.ResizeMap(mapWidth, mapHeight);
                }
            }

            return map;
        }

        public static void SaveMaps()
        {
        }
    }
}