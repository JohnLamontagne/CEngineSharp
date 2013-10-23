using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Client.World
{
    public static class GameWorld
    {
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();

        public static Map CurrentMap { get; set; }
    }
}