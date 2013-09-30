using CEngineSharp_Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server
{
    public static class Globals
    {
        public static int CurrentConnections
        {
            get
            {
                return GameWorld.Players.Count;
            }
        }

        public static bool ShuttingDown = false;
    }
}