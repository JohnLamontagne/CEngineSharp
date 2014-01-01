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
            GameTime gameTime = new GameTime();

            Client.ShuttingDown = false;

            AudioManager.LoadSounds(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Sounds");

            RenderManager.Initiate();

            GameLoop.Start(gameTime);
        }
    }
}