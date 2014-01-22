using CEngineSharp_Client.Net;
using SFML.Graphics;
using SFML.Window;
using System;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public abstract class Renderer
    {
        protected readonly string ThemeConfigurationPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Graphics\Gui\Black.conf";

        public bool CanRender { get; set; }

        protected Renderer()
        {
            Window = new RenderWindow(new VideoMode(832, 640), "CEngine# Client", Styles.Default);
            Window.Closed += _window_Closed;

            this.Gui = new Gui(Window)
            {
                GlobalFont = new Font(AppDomain.CurrentDomain.BaseDirectory + @"Data\Graphics\Fonts\MainFont.ttf")
            };

            LoadInterface();
        }

        private void _window_Closed(object sender, EventArgs e)
        {
            NetManager.Instance.Disconnect();
            Environment.Exit(0);
        }

        protected Renderer(RenderWindow window)
        {
            Window = window;
            this.Gui = new Gui(Window)
            {
                GlobalFont = new Font(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\Fonts\MainFont.ttf")
            };

            this.LoadInterface();
        }

        protected RenderWindow Window;

        public Gui Gui { get; set; }

        protected abstract void LoadInterface();

        public abstract void Render(GameTime gameTime);

        public void Unload()
        {
            this.Gui.GetWidgets().Clear();

            this.Gui = null;
        }

        public RenderWindow GetWindow()
        {
            return Window;
        }
    }
}