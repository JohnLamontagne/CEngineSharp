using CEngineSharp_World;

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