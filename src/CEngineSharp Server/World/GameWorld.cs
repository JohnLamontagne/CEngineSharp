using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server.World
{
    public static class GameWorld
    {
        public static Player[] Players = new Player[1];

        public static void SaveGameWorld()
        {
            Console.WriteLine("Saving players...");

            foreach (var player in Players)
            {
                if (player == null) continue;

                if (player.LoggedIn)
                    player.Save();
            }
        }
    }
}