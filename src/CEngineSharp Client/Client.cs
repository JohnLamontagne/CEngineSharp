using CEngineSharp_Client.Audio;
using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Utilities;
using CEngineSharp_Client.World.Content_Managers;
using SFML.Graphics;
using System;

namespace CEngineSharp_Client
{
    internal static class Client
    {
        public static bool ShuttingDown { get; set; }

        public static bool InGame { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var gameTime = new GameTime();

            ShuttingDown = false;

            ServiceLocator.NetManager = new NetManager();
            ServiceLocator.WorldManager = new WorldManager();
            ServiceLocator.ScreenManager = new ScreenManager();

            AudioManager.Instance.SfxManager.LoadSounds(Constants.FILEPATH_SFX);
            AudioManager.Instance.MusicManager.LoadMusic(Constants.FILEPATH_MUSIC);

            RenderWindow renderWindow = new RenderWindow(new SFML.Window.VideoMode(800, 600), "CEngineSharp", SFML.Window.Styles.Default);

            ServiceLocator.ScreenManager.AddScreen("mainMenu", new MenuScreen(renderWindow));
            ServiceLocator.ScreenManager.AddScreen("gameScreen", new GameScreen(renderWindow));
            ServiceLocator.ScreenManager.SetActiveScreen("mainMenu");
            GameLoop.Start(gameTime);
        }
    }
}