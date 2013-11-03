using CEngineSharp_Server.Utilities;
using SharpNetty;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Server.World
{
    public class Map
    {
        public enum Layers
        {
            Ground,
            Mask,
            Mask2,
            Fringe,
            Fringe2
        }

        public class Tile
        {
            public Layer[] Layers;

            public bool Blocked { get; set; }

            public bool IsOccupied { get; set; }

            public class Layer
            {
                public Rect SpriteRect { get; set; }

                public int TextureNumber { get; set; }
            }

            public Tile()
            {
                this.Layers = new Layer[(int)Map.Layers.Fringe2 + 1];
            }
        }

        public string Name { get; set; }

        private Tile[,] tiles;

        private List<Player> players;

        public int MapWidth
        {
            get { return this.tiles.GetLength(0); }
        }

        public int MapHeight
        {
            get { return this.tiles.GetLength(1); }
        }

        public Map()
        {
            this.players = new List<Player>();
            this.tiles = new Tile[0, 0];
        }

        public Tile GetTile(int x, int y)
        {
            return this.tiles[x, y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            this.tiles[x, y] = tile;
        }

        public void RemovePlayer(Player player)
        {
            this.players.Remove(player);
        }

        public void AddPlayer(Player player)
        {
            this.players.Add(player);
        }

        public Player GetPlayer(int playerIndex)
        {
            return this.players[playerIndex];
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            int columnCount = this.tiles.GetLength(1);
            int columnCount2 = newHeight;
            int columns = this.tiles.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(this.tiles, co * columnCount, newArray, co * columnCount2, columnCount);

            this.tiles = newArray;
        }

        public void SendPacket(Packet packet)
        {
            foreach (var player in this.players)
                player.SendPacket(packet);
        }
    }
}