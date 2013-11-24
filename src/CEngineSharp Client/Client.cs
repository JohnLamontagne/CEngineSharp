using CEngineSharp_Client.Audio;
using CEngineSharp_Client.Graphics;
using System;

namespace CEngineSharp_Client
{
    internal static class Client
    {
        public static CEngineSharp_Client.GameLoop.GameTimer GameTime;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Globals.ShuttingDown = false;
            Globals.KeyDirection = World.Entity.Directions.None;

            AudioManager.LoadSounds(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Sounds");

            RenderManager.Init();

            GameLoop.Start();
        }
    }
}