using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Networking;
using CEngineSharp_Server.Networking.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;

using System;
using System.IO;
using System.Windows.Forms;

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

            ServerConfiguration.LoadConfig();
            ContentManager.Instance.LoadContent();
            NetManager.Instance.Start();
            var serverLoop = new ServerLoop();
            serverLoop.Start();
        }

    }
}