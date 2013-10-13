using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server
{
    public static class Constants
    {
        public static readonly string FilePath_Data = AppDomain.CurrentDomain.BaseDirectory + "Data/";
        public static readonly string FilePath_Maps = FilePath_Data + "Maps/";
        public static readonly string FilePath_Accounts = FilePath_Data + "Accounts/";
        public static readonly string FilePath_Items = FilePath_Data + "Items/";
        public static readonly string FilePath_Npcs = FilePath_Data + "Npcs/";

        public const int MIN_MAP_WIDTH = 25;
        public const int MIN_MAP_HEIGHT = 25;

        public const string SERVER_MESSAGE_HELLO = "Hello";
    }
}