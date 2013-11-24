using CEngineSharp_Client.World.Entity;

namespace CEngineSharp_Client
{
    public static class Globals
    {
        public static int MyIndex { get; set; }

        public static bool ShuttingDown { get; set; }

        public static bool InGame { get; set; }

        public static int CurrentResolutionWidth { get; set; }

        public static int CurrentResolutionHeight { get; set; }

        public static Directions KeyDirection { get; set; }
    }
}