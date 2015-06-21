using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.Utilities.ServiceLocators;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using System;
using System.Diagnostics;

namespace CEngineSharp_Server.GameLogic
{
    public class ServerLoop
    {
        public class GameTimer
        {
            private readonly Stopwatch _stopWatch;

            public GameTimer()
            {
                _stopWatch = new Stopwatch();
                _stopWatch.Start();
            }

            public long GetTotalTimeElapsed()
            {
                return _stopWatch.ElapsedMilliseconds;
            }
        }

        public static GameTimer GameTime;

        public ServerLoop()
        {
            GameTime = new GameTimer();
        }

        public void Start()
        {
            long lastConsoleTitleUpdateTime = 0;
            long lastCpsCheck = 0;
            int cps = 0;
            int cpsCount = 0;

            // Continue processing the server-logic until it's time to shut things down.
            while (!Server.ShuttingDown)
            {
                if (lastConsoleTitleUpdateTime <= GameTime.GetTotalTimeElapsed())
                {
                    var serverWindowTitle = ServerConfiguration.GameName + " - " + ServerConfiguration.ServerIP + ":" +
                                               ServerConfiguration.ServerPort + " - Player Count: " +
                                               ContentManager.Instance.PlayerManager.PlayerCount + " - Debug Mode: " +
                                               (ServerConfiguration.SupressionLevel == ErrorHandler.ErrorLevels.Debug
                                                   ? "On"
                                                   : "Off") + " - Cps: " + cps + "/sec";

                    Console.Title = serverWindowTitle;

                    lastConsoleTitleUpdateTime = GameTime.GetTotalTimeElapsed() + 500;
                }

                NetServiceLocator.Singleton.GetService().Update();

                if (lastCpsCheck <= GameTime.GetTotalTimeElapsed())
                {
                    cps = cpsCount;
                    cpsCount = 0;
                    lastCpsCheck = GameTime.GetTotalTimeElapsed() + 1000;
                }
                else
                    cpsCount++;
            }

            // Save the game world.
            ContentManager.Instance.PlayerManager.SavePlayers();
            // Terminate the server.
            Environment.Exit(0);
        }
    }
}