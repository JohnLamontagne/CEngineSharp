using CEngineSharp_Client.Graphics.TextureManager;

namespace CEngineSharp_Client.Graphics
{
    public class RenderManager
    {
        #region Singleton

        private static RenderManager _renderManager;

        public static RenderManager Instance { get { return _renderManager ?? (_renderManager = new RenderManager()); } }

        #endregion

        private Renderer _renderer;

        public Renderer CurrentRenderer { get { return _renderer; } private set { _renderer = value; } }

        public int CurrentResolutionWidth { get; set; }

        public int CurrentResolutionHeight { get; set; }

        public ITextureManager TextureManager { get; private set; }

        public bool FpsLock { get; set; }

        private RenderStates _renderstate;

        private bool _renderStateChanged = false;

        public RenderStates RenderState
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

        public long FrameTime { get; set; }

        private long _nextRenderTime;

        public void Initiate()
        {
            RenderState = RenderStates.RenderMenu;
        }

        public void Render(GameTime gameTime)
        {
            if (_renderStateChanged)
            {
                switch (_renderstate)
                {
                    case RenderStates.RenderGame:

                        if (_renderer != null)
                            _renderer.Unload();

                        TextureManager = new GameTextureManager();
                        TextureManager.LoadTextures();
                        CurrentRenderer = new GameRenderer(_renderer.GetWindow());
                        _renderStateChanged = false;
                        break;

                    case RenderStates.RenderMenu:

                        if (_renderer != null)
                        {
                            _renderer.Unload();
                            TextureManager = new MenuTextureManager();
                            TextureManager.LoadTextures();
                            CurrentRenderer = new MenuRenderer(_renderer.GetWindow());
                        }
                        else
                        {
                            TextureManager = new MenuTextureManager();
                            TextureManager.LoadTextures();
                            CurrentRenderer = new MenuRenderer();
                        }

                        _renderStateChanged = false;
                        break;
                }
            }

            if (_renderer == null || !_renderer.CanRender) return;

            var startTime = gameTime.TotalElapsedTime;

            if (!this.FpsLock || gameTime.TotalElapsedTime >= _nextRenderTime)
                _renderer.Render(gameTime);

            FrameTime = gameTime.TotalElapsedTime - startTime;
            _nextRenderTime = gameTime.TotalElapsedTime + ((1000 / Constants.MAX_FPS) - this.FrameTime);
        }


        public void ForceRenderState(RenderStates renderState)
        {
            switch (renderState)
            {
                case RenderStates.RenderGame:
                    if (_renderer != null)
                        _renderer.Unload();

                    TextureManager = new GameTextureManager();
                    TextureManager.LoadTextures();
                    CurrentRenderer = new GameRenderer(_renderer.GetWindow());
                    _renderStateChanged = false;
                    break;

                case RenderStates.RenderMenu:
                    if (_renderer != null)
                    {
                        _renderer.Unload();
                        TextureManager = new MenuTextureManager();
                        TextureManager.LoadTextures();
                        CurrentRenderer = new MenuRenderer(_renderer.GetWindow());
                    }
                    else
                    {
                        TextureManager = new MenuTextureManager();
                        TextureManager.LoadTextures();
                        CurrentRenderer = new MenuRenderer();
                    }

                    _renderStateChanged = false;
                    break;
            }
        }
    }
}