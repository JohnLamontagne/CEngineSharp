using CEngineSharp_Server.World.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server.World.Content_Managers
{
    class NpcManager
    {
        private static List<Npc> _npcs = new List<Npc>();

        public Npc GetNpc(int npcIndex)
        {
            return _npcs[npcIndex];
        }

        public Npc GetNpc(string npcName)
        {
            foreach (var npc in _npcs)
            {
                if (npc.Name == npcName)
                    return npc;
            }

            return null;
        }

        public Npc[] GetNpcs()
        {
            return _npcs.ToArray();
        }

        public static void LoadNpcs()
        {
            DirectoryInfo dI = new DirectoryInfo(Constants.FILEPATH_NPCS);
            FileInfo[] fileInfo = dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly);

            Console.WriteLine("Loading npcs...");

            foreach (var npcFile in fileInfo)
            {
                _npcs.Add(NpcManager.LoadNpc(npcFile.FullName));
            }

            Console.WriteLine("Loaded {0} npcs", fileInfo.Count());
        }

        private static Npc LoadNpc(string filePath)
        {
            Npc npc = new Npc();

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    npc.Name = binaryReader.ReadString();
                    npc.TextureNumber = binaryReader.ReadInt32();
                }
            }

            return npc;
        }
    }
}
