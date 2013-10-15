using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Client
{
    public static class GameLoop
    {
        public static void Start()
        {
            while (!Globals.ShuttingDown)
            {
                Program.CurrentRenderer.Render();
            }
        }
    }
}