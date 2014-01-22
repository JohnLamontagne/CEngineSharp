using CEngineSharp_Server;
using CEngineSharp_Utilities;

namespace CEngineSharp_Server.World.Maps
{
    public class MapItem
    {
        public Vector Position;

        public Item Item { get; set; }

        public int SpawnDuration { get; set; }

        public MapItem(Item item, Vector position, int spawnDuration = -1)
        {
            this.Item = item;
            this.Position = position;
            this.SpawnDuration = spawnDuration;
        }
    }

}
