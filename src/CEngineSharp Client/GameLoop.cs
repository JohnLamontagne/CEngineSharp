using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using System;
using System.Diagnostics;

namespace CEngineSharp_Client
{
    public static class GameLoop
    {
        public class GameTimer
        {
            private Stopwatch stopWatch;

            public GameTimer()
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }

            public GameTimer Start()
            {
                stopWatch.Start();
                return this;
            }

            public GameTimer Restart()
            {
                stopWatch.Restart();
                return this;
            }

            public long GetTotalTimeElapsed()
            {
                return stopWatch.ElapsedMilliseconds;
            }
        }

        public static void Start()
        {
            int renderTmr = 0;
            int playerUpdateTmr = 0;

            Client.GameTime = new GameTimer();

            while (!Globals.ShuttingDown)
            {
                if (playerUpdateTmr < Client.GameTime.GetTotalTimeElapsed())
                {
                    foreach (var player in GameWorld.Players.Values)
                    {
                        player.Update();

                        playerUpdateTmr = (int)Client.GameTime.GetTotalTimeElapsed() + 10;
                    }
                }

                if (renderTmr < Client.GameTime.GetTotalTimeElapsed())
                {
                    RenderManager.Render();
                    renderTmr = (int)Client.GameTime.GetTotalTimeElapsed() + 10;
                }
            }
        }
    }
}