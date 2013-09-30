using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Server.World
{
    public static class GameWorld
    {
        /// <summary>
        /// What do you think of storing the game world in this manner, i.e., through static generic lists etc. in a static class.
        /// Yeah
        /// </summary>
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();

        public static List<NPC> Npcs = new List<NPC>();
        public static List<Map> Maps = new List<Map>();

        public static void SaveWorld()
        {
            Console.WriteLine("Saving players...");

            foreach (var player in Players.Values)
            {
                if (player.LoggedIn)
                    player.Save();
            }

            Console.WriteLine("Saving npcs...");

            foreach (var npc in Npcs)
            {
                npc.Save();
            }
        }

        public static void LoadWorld()
        {
            Console.WriteLine("Checking world integrity...");
            CheckWorldIntegrity();

            Console.WriteLine("Loading npcs...");
            LoadNpcs();

            Console.WriteLine("Loading maps...");
            LoadMaps();
        }

        private static void CheckWorldIntegrity()
        {
            // Check to make sure our files containing the world exist.
            if (!Directory.Exists(Constants.FilePath_Accounts)) Directory.CreateDirectory(Constants.FilePath_Accounts);
            if (!Directory.Exists(Constants.FilePath_Npcs)) Directory.CreateDirectory(Constants.FilePath_Npcs.TrimEnd('/'));
            if (!Directory.Exists(Constants.FilePath_Maps)) Directory.CreateDirectory(Constants.FilePath_Maps.TrimEnd('/'));
            if (!Directory.Exists(Constants.FilePath_Items)) Directory.CreateDirectory(Constants.FilePath_Items.TrimEnd('/'));

            // Check to make sure the file for storing players name is there.
            if (!File.Exists(Constants.FilePath_Data + "names.txt")) File.Create(Constants.FilePath_Data + "names.txt");
        }

        private static void LoadNpcs()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Constants.FilePath_Npcs);

            foreach (var file in dirInfo.GetFiles("*.dat"))
            {
                NPC npc = new NPC();
                npc.Load(file.Name);
                Npcs.Add(npc);
            }

            Console.WriteLine("Loaded {0} npcs!", Npcs.Count);
        }

        private static void LoadMaps()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Constants.FilePath_Maps);

            foreach (var file in dirInfo.GetFiles("*.map"))
            {
                Map map = new Map();
                map.Load(file.Name);
                GameWorld.Maps.Add(map);
            }

            Console.WriteLine("Loaded {0} maps!", Maps.Count);
        }
    }
}