using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.World;
using System;
using System.Diagnostics;

namespace CEngineSharp_Client
{
    public static class GameLoop
    {
        public static void Start(GameTime gameTime)
        {

            while (!Globals.ShuttingDown)
            {
                gameTime.Update();

                Networking.ExecuteQueue();

                if (Globals.InGame && GameWorld.GetPlayer(Globals.MyIndex) != null)
                {
                    GameWorld.GetPlayer(Globals.MyIndex).TryMove();

                    foreach (var player in GameWorld.GetPlayers())
                    {
                        player.Update(gameTime);
                    }
                }


                RenderManager.Render(gameTime);

            }
        }
    }

    public class GameTime
    {
        private Stopwatch stopWatch;

        private long startUpdateTime;

        public long TotalElapsedTime
        {
            get { return stopWatch.ElapsedMilliseconds; }
        }

        public long UpdateTime { get; private set; }

        public GameTime()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        public GameTime Start()
        {
            stopWatch.Start();
            return this;
        }

        public GameTime Restart()
        {
            stopWatch.Restart();
            return this;
        }

        public void Update()
        {
            this.UpdateTime = this.TotalElapsedTime - this.startUpdateTime;

            startUpdateTime = this.TotalElapsedTime;
        }
    }
}