using SFML.Graphics;
using SFML.Window;
using System;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public abstract class Renderer
    {
        protected readonly string themeConfigurationPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Graphics\Gui\Black.conf";

        public bool CanRender { get; set; }

        public Renderer()
        {
            _window = new RenderWindow(new VideoMode(832, 640), "CEngine# Client", Styles.Default);
            _window.Closed += _window_Closed;

            this.Gui = new Gui(_window);

            this.Gui.GlobalFont = new Font(AppDomain.CurrentDomain.BaseDirectory + @"Data\Graphics\Fonts\MainFont.ttf");

            LoadInterface();
        }

        private void _window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public Renderer(RenderWindow window)
        {
            _window = window;
            this.Gui = new Gui(_window);

            this.Gui.GlobalFont = new Font(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\Fonts\MainFont.ttf");

            LoadInterface();
        }

        protected RenderWindow _window;

        public Gui Gui { get; set; }

        protected abstract void LoadInterface();

        public abstract void Render(GameLoop.GameTimer gameTime);

        public void Unload()
        {
            this.Gui.GetWidgets().Clear();

            this.Gui = null;
        }

        public RenderWindow GetWindow()
        {
            return _window;
        }
    }
}