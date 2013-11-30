using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.World;
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

        public static void Start(GameTimer gameTime)
        {
            long playerUpdateTmr = 0;

            while (!Globals.ShuttingDown)
            {
                Networking.ExecuteQueue();

                if (Globals.MyIndex < GameWorld.PlayerCount && GameWorld.GetPlayer(Globals.MyIndex) != null)
                {
                    GameWorld.GetPlayer(Globals.MyIndex).TryMove();

                    if (playerUpdateTmr < gameTime.GetTotalTimeElapsed())
                    {
                        foreach (var player in GameWorld.GetPlayers())
                        {
                            player.Update();
                        }

                        playerUpdateTmr = gameTime.GetTotalTimeElapsed() + 9;
                    }
                }

                RenderManager.Render(gameTime);
            }
        }
    }
}