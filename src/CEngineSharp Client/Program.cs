using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using System;

namespace CEngineSharp_Client
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AudioManager.LoadSounds(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Sounds");

            RenderManager.Init();

            GameLoop.Start();
        }
    }
}