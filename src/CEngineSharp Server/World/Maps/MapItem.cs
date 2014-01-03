using System;

namespace CEngineSharp_Server.World.Maps
{
    public class MapItem
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Item Item { get; set; }

        public int SpawnDuration { get; set; }

        public MapItem(Item item, int x, int y, int spawnDuration = -1)
        {
            this.Item = item;
            this.X = x;
            this.Y = y;

            this.SpawnDuration = spawnDuration;
        }
    }

}
