using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server
{
    public static class Constants
    {
        public static readonly string FILEPATH_DATA = AppDomain.CurrentDomain.BaseDirectory + "Data/";
        public static readonly string FILEPATH_MAPS = FILEPATH_DATA + "Maps/";
        public static readonly string FILEPATH_ACCOUNTS = FILEPATH_DATA + "Accounts/";
        public static readonly string FILEPATH_ITEMS = FILEPATH_DATA + "Items/";
        public static readonly string FILEPATH_NPCS = FILEPATH_DATA + "Npcs/";

        public const int MIN_MAP_WIDTH = 25;
        public const int MIN_MAP_HEIGHT = 25;

        public const string SERVER_MESSAGE_HELLO = "Hello";

        public const int SERVER_STARTER_MAP = 0;
    }
}