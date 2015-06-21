using CEngineSharp_Client.Graphics.TextureManager;
using CEngineSharp_Client.Utilities;
using SFML.Graphics;
using SFML.Window;
using System;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public abstract class Screen
    {
        private RenderWindow _window;

        public bool CanRender { get; set; }

        public RenderWindow Window { get { return _window; } }

        public Gui GUI { get; protected set; }

        public ITextureManager TextureManager { get; protected set; }

        public Screen()
        {
            _window = new RenderWindow(new VideoMode(832, 640), "CEngine# Client", Styles.Default);
            _window.Closed += _window_Closed;

            this.GUI = new Gui(Window)
            {
                GlobalFont = new Font(Constants.FILEPATH_GRAPHICS + @"\Fonts\MainFont.ttf")
            };
        }

        public Screen(RenderWindow window)
        {
            _window = window;
            this.GUI = new Gui(Window)
              {
                  GlobalFont = new Font(Constants.FILEPATH_GRAPHICS + @"\Fonts\MainFont.ttf")
              };
        }

        public void Unload()
        {
            this.GUI.GetWidgets().Clear();
        }

        protected abstract void LoadInterface();

        public abstract void Render(GameTime gameTime);

        private void _window_Closed(object sender, EventArgs e)
        {
            ServiceLocator.NetManager.Disconnect();
            Environment.Exit(0);
        }
    }
}