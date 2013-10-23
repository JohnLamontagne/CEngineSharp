using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Client.World
{
    public class Map
    {
        public class Tile
        {
            private Layer[] _layers;

            public bool Blocked { get; set; }

            public bool IsOccupied { get; set; }

            public class Layer
            {
            }
        }

        public Tile[,] Tiles
        {
            get;
            protected set;
        }

        public int Width { get { return this.Tiles.GetLength(0); } }

        public int Height { get { return this.Tiles.GetLength(1); } }
    }
}