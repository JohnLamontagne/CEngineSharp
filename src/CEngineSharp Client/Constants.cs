using System;

namespace CEngineSharp_Client
{
    public static class Constants
    {
        public static readonly string FILEPATH_CACHE = AppDomain.CurrentDomain.BaseDirectory + "Data/Cache/";

        public static readonly string FILEPATH_GRAPHICS = AppDomain.CurrentDomain.BaseDirectory + "Data/Graphics/";

        public const int PORT = 4500;

        public const string IP = "127.0.0.1";

        public const int MAX_CHAT_LINES = 10;

        public const int MAX_FPS = 64;

        public const int MAX_VIEW_WIDTH = 800;

        public const int MAX_VIEW_HEIGHT = 600;
    }
}