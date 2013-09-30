using CEngineSharp_Client.Net;
using System;

namespace CEngineSharp_Client
{
    internal static class Program
    {
        // I feel like that's not a good practice.
        // Yeah, I couldn't think of any other way to allow the rest of the program to interact with the graphics... unless I pass it around a ton.
        // Problem #2
        public static Graphics GameGraphics;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Program.GameGraphics = new Graphics();

            Program.GameGraphics.Render();
        }
    }
}