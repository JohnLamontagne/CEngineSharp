using CEngineSharp_Client.World.Entity;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Client.World
{
    public static class GameWorld
    {
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();

        public static Map CurrentMap { get; set; }
    }
}