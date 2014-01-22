using CEngineSharp_Server;
using CEngineSharp_Utilities;

namespace CEngineSharp_Server.World.Entities
{
    public class MapNpc : Npc
    {
        public MapNpc(Npc npc, Vector position)
        {
            this.Name = npc.Name;
            this.Level = npc.Level;
            this.TextureNumber = npc.TextureNumber;
            this.Position = position;
        }
    }
}