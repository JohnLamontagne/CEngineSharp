using CEngineSharp_Client.Audio;
using CEngineSharp_Client.Graphics.TextureManager;
using System;

namespace CEngineSharp_Client.Graphics
{
    public static class RenderManager
    {
        private static Renderer _renderer;

        public static Renderer CurrentRenderer { get { return _renderer; } private set { _renderer = value; } }

        public static int CurrentResolutionWidth { get; set; }

        public static int CurrentResolutionHeight { get; set; }

        public static ITextureManager TextureManager { get; private set; }

        private static RenderStates _renderstate;

        private static bool _renderStateChanged = false;

        public static RenderStates RenderState
        {
            get
            {
                return _renderstate;
            }
            set
            {
                _renderstate = value;
                _renderStateChanged = true;
            }
        }

        public static long FrameTime { get; set; }

        private static long nextRenderTime;

        public static void Initiate()
        {
            RenderManager.RenderState = RenderStates.Render_Menu;
        }

        public static void Render(GameTime gameTime)
        {
            if (_renderStateChanged)
            {
                switch (_renderstate)
                {
                    case RenderStates.Render_Game:

                        if (_renderer != null)
                            _renderer.Unload();

                        RenderManager.TextureManager = new GameTextureManager();
                        RenderManager.TextureManager.LoadTextures();
                        RenderManager.CurrentRenderer = new GameRenderer(_renderer.GetWindow());
                        _renderStateChanged = false;
                        break;

                    case RenderStates.Render_Menu:

                        if (_renderer != null)
                        {
                            _renderer.Unload();
                            RenderManager.TextureManager = new MenuTextureManager();
                            RenderManager.TextureManager.LoadTextures();
                            RenderManager.CurrentRenderer = new MenuRenderer(_renderer.GetWindow());
                        }
                        else
                        {
                            RenderManager.TextureManager = new MenuTextureManager();
                            RenderManager.TextureManager.LoadTextures();
                            RenderManager.CurrentRenderer = new MenuRenderer();
                        }

                        _renderStateChanged = false;
                        break;
                }
            }

            if (_renderer.CanRender)// && gameTime.TotalElapsedTime >= RenderManager.nextRenderTime)
            {
                long startTime = gameTime.TotalElapsedTime;
                _renderer.Render(gameTime);
                RenderManager.FrameTime = gameTime.TotalElapsedTime - startTime;
                RenderManager.nextRenderTime = gameTime.TotalElapsedTime + ((1000 / 61) - RenderManager.FrameTime);
            }
        }
    }
}