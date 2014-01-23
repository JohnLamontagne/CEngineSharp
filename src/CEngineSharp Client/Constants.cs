using System;

namespace CEngineSharp_Client
{
    public static class Constants
    {
        public static readonly string FILEPATH_DATA = AppDomain.CurrentDomain.BaseDirectory + "/Data/";

        public static readonly string FILEPATH_CACHE = FILEPATH_DATA + "/Cache/";

        public static readonly string FILEPATH_GRAPHICS = FILEPATH_DATA + "/Graphics/";

        public static readonly string FILEPATH_MUSIC = FILEPATH_DATA + "/Music/";

        public static readonly string FILEPATH_SFX = FILEPATH_DATA + "/Sfx/";

        public const int PORT = 4500;

        public const string IP = "127.0.0.1";

        public const int MAX_CHAT_LINES = 10;

        public const int MAX_FPS = 64;

        public const int MAX_VIEW_WIDTH = 800;

        public const int MAX_VIEW_HEIGHT = 600;
    }
}