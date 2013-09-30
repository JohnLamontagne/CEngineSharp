using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using System;
using System.Diagnostics;
using System.Threading;

namespace CEngineSharp_Server.GameLogic
{
    public class ServerLoop
    {
        public class GameTime
        {
            private Stopwatch stopWatch;

            public GameTime()
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }

            public long GetTotalTimeElapsed()
            {
                return stopWatch.ElapsedMilliseconds;
            }
        }

        public ServerLoop()
        {
            gameTime = new GameTime();
        }

        private GameTime gameTime;

        public void Start()
        {
            long lastConsoleTitleUpdateTime = 0;
            long lastCpsCheck = 0;
            int cps = 0;
            int cpsCount = 0;

            while (!Globals.ShuttingDown)
            {
                if (lastConsoleTitleUpdateTime <= gameTime.GetTotalTimeElapsed())
                {
                    Server.ServerWindow.SetTitle(ServerConfiguration.GameName + " - " + ServerConfiguration.ServerIP + ":" + ServerConfiguration.ServerPort + " - Player Count: " + Globals.CurrentConnections +
                        " - Debug Mode: " + (ServerConfiguration.SupressionLevel == ErrorHandler.ErrorLevels.Low ? "On" : "Off") + " - Cps: " + cps + "/sec");

                    lastConsoleTitleUpdateTime = gameTime.GetTotalTimeElapsed() + 500;
                }

                if (lastCpsCheck <= gameTime.GetTotalTimeElapsed())
                {
                    cps = cpsCount;
                    cpsCount = 0;
                    lastCpsCheck = gameTime.GetTotalTimeElapsed() + 1000;
                }
                else
                    cpsCount++;
            }

            GameWorld.SaveWorld();

            Environment.Exit(0);
        }
    }
}