using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Client
{
    public static class Globals
    {
        public static int MyIndex;
        public static bool ShuttingDown = false;

        public static int CurrentResolutionWidth { get; set; }

        public static int CurrentResolutionHeight { get; set; }
    }
}