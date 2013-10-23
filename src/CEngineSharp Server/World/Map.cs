using CEngineSharp_Server.Utilities;
using SharpNetty;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Server.World
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
                public Rect SpriteRect { get; set; }
            }
        }

        public Tile[,] Tiles
        {
            get;
            protected set;
        }

        public List<Player> Players
        {
            get;
            set;
        }

        public Map()
        {
            this.Players = new List<Player>();
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            int columnCount = Tiles.GetLength(1);
            int columnCount2 = newHeight;
            int columns = Tiles.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(Tiles, co * columnCount, newArray, co * columnCount2, columnCount);

            Tiles = newArray;
        }

        public void SendPacket(Packet packet)
        {
            foreach (var player in this.Players)
                player.SendPacket(packet);
        }
    }
}