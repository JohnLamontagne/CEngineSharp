using CEngineSharp_Server.World.Entities;
using CEngineSharp_World.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEngineSharp_Server.World.Content_Managers
{
    public sealed class NpcManager
    {

        private readonly List<Npc> _npcs;

        public NpcManager()
        {
            _npcs = new List<Npc>();
        }

        public Npc GetNpc(int npcIndex)
        {
            return _npcs[npcIndex];
        }

        public Npc GetNpc(string npcName)
        {
            return _npcs.FirstOrDefault(npc => npc.Name == npcName);
        }

        public Npc[] GetNpcs()
        {
            return _npcs.ToArray();
        }

        public void LoadNpcs()
        {
            var directoryInfo = new DirectoryInfo(Constants.FILEPATH_NPCS);

            foreach (var file in directoryInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                _npcs.Add(BaseNpc.LoadNpc(file.FullName) as Npc);
            }
        }

        public void SaveNpcs()
        {
            foreach (var npc in _npcs)
                npc.Save(Constants.FILEPATH_NPCS + npc.Name + ".dat");
        }
    }
}
