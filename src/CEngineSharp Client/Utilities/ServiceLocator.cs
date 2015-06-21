using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.World.Content_Managers;

namespace CEngineSharp_Client.Utilities
{
    public static class ServiceLocator
    {
        public static NetManager NetManager { get; set; }

        public static WorldManager WorldManager { get; set; }

        public static ScreenManager ScreenManager { get; set; }
    }
}