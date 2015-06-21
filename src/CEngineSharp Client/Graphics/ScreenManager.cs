using CEngineSharp_Client.Graphics.TextureManager;
using CEngineSharp_Client.Utilities;
using SFML.System;
using System.Collections.Generic;

namespace CEngineSharp_Client.Graphics
{
    public class ScreenManager
    {
        private Dictionary<string, Screen> _screens;
        private Screen _activeScreen;
        private long _nextRenderTime;

        public long FrameTime { get; set; }

        public bool FpsLock { get; set; }

        public Vector2i Resolution { get; private set; }

        public Screen ActiveScreen { get { return _activeScreen; } }

        public ScreenManager()
        {
            _screens = new Dictionary<string, Screen>();
        }

        public void Render(GameTime gameTime)
        {
            if (_activeScreen == null || !_activeScreen.CanRender) return;

            var startTime = gameTime.TotalElapsedTime;

            if (!this.FpsLock || gameTime.TotalElapsedTime >= _nextRenderTime)
                _activeScreen.Render(gameTime);

            FrameTime = gameTime.TotalElapsedTime - startTime;
            _nextRenderTime = gameTime.TotalElapsedTime + ((1000 / Constants.MAX_FPS) - this.FrameTime);
        }

        public void AddScreen(string name, Screen screen)
        {
            if (!_screens.ContainsKey(name))
                _screens.Add(name, screen);
        }

        public void SetActiveScreen(string screenName)
        {
            if (_screens.ContainsKey(screenName))
            {
                if (_activeScreen != null)
                    _activeScreen.Unload();

                _activeScreen = _screens[screenName];

                this.Resolution = new Vector2i((int)_activeScreen.Window.Size.X, (int)_activeScreen.Window.Size.Y);
            }
        }
    }
}