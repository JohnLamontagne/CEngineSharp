using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Net;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CEngineSharp_Server
{
    public static class Server
    {
        public static Networking Networking;

        private static ServerLoop _serverLoop;

        private static Thread _serverLoopThread;

        // Keeps it safe from the GC.
        private static ConsoleEventDelegate handler;

        // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        private static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            ServerConfiguration.LoadConfig();

            Console.Title = ServerConfiguration.GameName + " - " + ServerConfiguration.ServerIP + ":" + ServerConfiguration.ServerPort + " - Player Count: " + Globals.CurrentConnections +
                " - Debug Mode: " + (ServerConfiguration.SupressionLevel == ErrorHandler.ErrorLevels.Low ? "On" : "Off");

            Server.Networking = new Networking();

            _serverLoop = new ServerLoop();
            _serverLoopThread = new Thread(_serverLoop.Start);
        }

        private static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Globals.ShuttingDown = true;

                // Save the game world.
                GameWorld.SaveGameWorld();
            }

            return false;
        }
    }
}