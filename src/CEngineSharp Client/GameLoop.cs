using CEngineSharp_Client.Utilities;
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

                ServiceLocator.NetManager.Update();

                if (Client.InGame && ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID) != null)
                {
                    ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID).TryMove();

                    foreach (var player in ServiceLocator.WorldManager.PlayerManager.GetPlayers())
                    {
                        player.Update(gameTime);
                    }

                    ServiceLocator.WorldManager.MapManager.Map.Update(gameTime);
                }

                // Render
                ServiceLocator.ScreenManager.Render(gameTime);
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