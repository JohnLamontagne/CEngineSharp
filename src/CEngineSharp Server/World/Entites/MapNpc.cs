using CEngineSharp_Server.Utilities;
using System;

namespace CEngineSharp_Server.World.Entities
{
    public class MapNpc : Npc
    {
        public MapNpc(Npc npc)
        {
            this.Name = npc.Name;
            this.Level = npc.Level;
            this.TextureNumber = npc.TextureNumber;
        }
    }
}