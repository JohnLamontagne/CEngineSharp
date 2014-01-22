using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.World.Content_Managers;
using System.Diagnostics;

namespace CEngineSharp_Client
{
    public static class GameLoop
    {
        public static void Start(GameTime gameTime)
        {
            while (!Client.ShuttingDown)
            {
                gameTime.Update();

                NetManager.Instance.ExecuteQueue();

                if (Client.InGame && PlayerManager.GetPlayer(PlayerManager.MyIndex) != null)
                {
                    PlayerManager.GetPlayer(PlayerManager.MyIndex).TryMove();

                    foreach (var player in PlayerManager.GetPlayers())
                    {
                        player.Update(gameTime);
                    }

                    MapManager.Map.Update(gameTime);
                }

                RenderManager.Instance.Render(gameTime);

            }
        }
    }

    public class GameTime
    {
        private readonly Stopwatch _stopWatch;

        private long _startUpdateTime;

        public long TotalElapsedTime
        {
            get { return _stopWatch.ElapsedMilliseconds; }
        }

        public long UpdateTime { get; private set; }

        public GameTime()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public GameTime Start()
        {
            _stopWatch.Start();
            return this;
        }

        public GameTime Restart()
        {
            _stopWatch.Restart();
            return this;
        }

        public void Update()
        {
            this.UpdateTime = this.TotalElapsedTime - this._startUpdateTime;

            _startUpdateTime = this.TotalElapsedTime;
        }
    }
}