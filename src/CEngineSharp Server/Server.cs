using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.Utilities.ServiceLocators;
using CEngineSharp_Server.World;
using System;

namespace CEngineSharp_Server
{
    public static class Server
    {
        public static bool ShuttingDown { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            NetServiceLocator.Singleton.SetService(new Networking.NetManager());

            ServerConfiguration.LoadConfig();
            ContentManager.Instance.LoadContent();
            NetServiceLocator.Singleton.GetService().Start();
            var serverLoop = new ServerLoop();
            serverLoop.Start();
        }
    }
}