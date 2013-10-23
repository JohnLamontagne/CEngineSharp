using CEngineSharp_Client.Graphics;
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

            Client.GameTime = new GameTimer();

            while (!Globals.ShuttingDown)
            {
                if (renderTmr < Client.GameTime.GetTotalTimeElapsed())
                {
                    RenderManager.Render();
                    renderTmr = (int)Client.GameTime.GetTotalTimeElapsed() + 10;
                }
            }
        }
    }
}