using System.IO;

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