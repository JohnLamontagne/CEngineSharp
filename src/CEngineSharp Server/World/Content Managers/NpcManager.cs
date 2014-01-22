using CEngineSharp_Server;
using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
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
                _npcs.Add(this.LoadNpc(file.FullName));
            }
        }

        public void SaveNpcs()
        {
            foreach (var npc in _npcs)
                npc.Save(Constants.FILEPATH_NPCS + npc.Name + ".dat");
        }

        public Npc LoadNpc(string filePath)
        {
            var npc = new Npc() { Position = new Vector() };

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    npc.Name = binaryReader.ReadString();
                    npc.TextureNumber = binaryReader.ReadInt32();
                    npc.Position.X = binaryReader.ReadInt32();
                    npc.Position.Y = binaryReader.ReadInt32();

                    var statCount = binaryReader.ReadInt32();
                    for (int i = 0; i < statCount; i++)
                        npc.SetStat((Stats)i, binaryReader.ReadInt32());
                }
            }

            return npc;
        }
    }
}
