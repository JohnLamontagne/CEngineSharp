using CEngineSharp_Client.Graphics.TextureManager;
using SFML.Graphics;
using SFML.Window;
using System;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public abstract class Renderer
    {
        protected readonly string themeConfigurationPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Graphics\Gui\Black.conf";

        public Renderer()
        {
            _window = new RenderWindow(new VideoMode(800, 600), "CEngine# Client", Styles.Default);
            _window.Resized += _window_Resized;
            _gui = new Gui(_window);

            _gui.GlobalFont = new Font(AppDomain.CurrentDomain.BaseDirectory + @"Data\Graphics\Fonts\MainFont.ttf");

            LoadInterface();
        }

        private void _window_Resized(object sender, SizeEventArgs e)
        {
            _window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        public Renderer(RenderWindow window)
        {
            _window = window;
            _gui = new Gui(_window);

            _gui.GlobalFont = new Font(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\Fonts\MainFont.ttf");

            LoadInterface();
        }

        protected RenderWindow _window;

        protected Gui _gui;

        protected abstract void LoadInterface();

        public abstract void Render();

        public void Unload()
        {
            _gui.GetWidgets().Clear();

            _gui = null;
        }

        public RenderWindow GetWindow()
        {
            return _window;
        }
    }
}