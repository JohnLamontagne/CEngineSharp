using CEngineSharp_Client.Graphics.TextureManager;

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

        private static long _nextRenderTime;

        public static void Initiate()
        {
            RenderManager.RenderState = RenderStates.RenderMenu;
        }

        public static void Render(GameTime gameTime)
        {
            if (_renderStateChanged)
            {
                switch (_renderstate)
                {
                    case RenderStates.RenderGame:

                        if (_renderer != null)
                            _renderer.Unload();

                        RenderManager.TextureManager = new GameTextureManager();
                        RenderManager.TextureManager.LoadTextures();
                        RenderManager.CurrentRenderer = new GameRenderer(_renderer.GetWindow());
                        _renderStateChanged = false;
                        break;

                    case RenderStates.RenderMenu:

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

            if (_renderer == null || !_renderer.CanRender) return;

            var startTime = gameTime.TotalElapsedTime;
            _renderer.Render(gameTime);
            RenderManager.FrameTime = gameTime.TotalElapsedTime - startTime;
            RenderManager._nextRenderTime = gameTime.TotalElapsedTime + ((1000 / Constants.MAX_FPS) - RenderManager.FrameTime);
        }
    }
}