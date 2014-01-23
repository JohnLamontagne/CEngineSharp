using CEngineSharp_Client.Audio;
using CEngineSharp_Client.Graphics;
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

            AudioManager.Instance.SfxManager.LoadSounds(Constants.FILEPATH_SFX);
            AudioManager.Instance.MusicManager.LoadMusic(Constants.FILEPATH_MUSIC);

            RenderManager.Instance.Initiate();

            GameLoop.Start(gameTime);
        }
    }
}