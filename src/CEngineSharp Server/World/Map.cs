using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Net.Packets;
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
                this.IsOccupied = false;
            }
        }

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

        private List<Player> players;

        private List<MapItem> items;

        public string Name { get; set; }

        private Tile[,] tiles;

        public int MapWidth
        {
            get { return this.tiles.GetLength(0); }
        }

        public int MapHeight
        {
            get { return this.tiles.GetLength(1); }
        }

        public int Version { get; set; }

        public Map()
        {
            this.tiles = new Tile[0, 0];

            this.players = new List<Player>();

            this.items = new List<MapItem>();
        }

        public Tile GetTile(Vector2i position)
        {
            return this.GetTile(position.X, position.Y);
        }

        public Tile GetTile(int x, int y)
        {
            return this.tiles[x, y];
        }

        public void SetTile(Vector2i position, Tile tile)
        {
            this.SetTile(position.X, position.Y, tile);
        }

        public void SetTile(int x, int y, Tile tile)
        {
            this.tiles[x, y] = tile;
        }

        public MapItem[] GetMapItems()
        {
            return this.items.ToArray();
        }

        public MapItem GetMapItem(Vector2i mapItemPos)
        {
            foreach (var mapItem in this.items)
            {
                if (mapItem.X == mapItemPos.X && mapItem.Y == mapItemPos.Y)
                    return mapItem;
            }

            return null;
        }

        public void RemoveMapItem(MapItem mapItem)
        {
            var despawnMapItemPacket = new DespawnMapItemPacket();
            despawnMapItemPacket.WriteData(mapItem);
            this.SendPacket(despawnMapItemPacket, true);

            this.items.Remove(mapItem);
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

        public void SendPacket(Packet packet, bool checkPlayerLoaded = false)
        {
            foreach (var player in this.players)
            {
                if (checkPlayerLoaded)
                {
                    if (!player.GetInMap())
                        continue;
                }

                player.SendPacket(packet);
            }
        }

        public void RemovePlayer(Player player, bool leftGame)
        {
            // Free up the tile.
            this.GetTile(player.Position).IsOccupied = false;

            // Remove the player from the map player list.
            this.players.Remove(player);

            LogoutPacket logoutPacket = new LogoutPacket();
            logoutPacket.WriteData(player.PlayerIndex);
            this.SendPacket(logoutPacket);

            if (leftGame)
            {
                foreach (var mPlayer in this.players)
                {
                    mPlayer.SendMessage(player.Name + " has left " + ServerConfiguration.GameName + ".");
                }
            }
        }

        public void SpawnItem(Item item, int x, int y, int spawnTime)
        {
            MapItem mapItem = new MapItem(item, x, y, spawnTime);

            this.items.Add(mapItem);

            var spawnMapItemPacket = new SpawnMapItemPacket();
            spawnMapItemPacket.WriteData(mapItem);
            this.SendPacket(spawnMapItemPacket, true);
        }

        public void AddPlayer(Player player)
        {
            this.players.Add(player);

            var playerDataPacket = new PlayerDataPacket();
            playerDataPacket.WriteData(player);

            foreach (var mPlayer in this.players)
            {
                if (mPlayer != player)
                    mPlayer.SendPacket(playerDataPacket);
            }
        }

        public Player GetPlayer(int playerIndex)
        {
            return this.players[playerIndex];
        }

        public Player[] GetPlayers()
        {
            return this.players.ToArray();
        }
    }
}