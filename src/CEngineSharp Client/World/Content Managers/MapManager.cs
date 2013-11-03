using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Client.World.Content_Managers
{
    public class MapManager
    {
        public static Map Map { get; set; }

        public static bool CheckMapExistence(string mapName)
        {
            return (File.Exists(Constants.FILEPATH_CACHE + "Maps/" + mapName + ".map"));
        }
    }
}