using CEngineSharp_Client.Graphics;
using System;

namespace CEngineSharp_Client
{
    public static class GameLoop
    {
        public static void Start()
        {
            while (!Globals.ShuttingDown)
            {
                RenderManager.Render();
            }
        }
    }
}