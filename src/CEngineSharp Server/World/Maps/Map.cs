using CEngineSharp_Server;
using CEngineSharp_Server.Networking.Packets.AuthenticationPackets;
using CEngineSharp_Server.Networking.Packets.MapUpdatePackets;
using CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using SharpNetty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CEngineSharp_Server.World.Maps
{
    public class Map
    {

        public class Tile
        {
            private readonly Layer[] _layers;

            public bool Blocked { get; set; }

            public bool IsOccupied { get; set; }

            public class Layer
            {
                public Rect SpriteRect { get; set; }

                public int TextureNumber { get; set; }
            }

            public Tile()
            {
                this._layers = new Layer[(int)Layers.Fringe2 + 1];
                this.IsOccupied = false;
            }

            public Layer GetLayer(Layers layer)
            {
                return this._layers[(int)layer];
            }

            public void SetLayer(Layer layerValue, Layers layer)
            {
                this._layers[(int)layer] = layerValue;
            }
        }

        private List<Player> players;

        private List<MapItem> items;

        private List<MapNpc> mapNpcs;

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

            this.mapNpcs = new List<MapNpc>();
        }

        public Tile GetTile(Vector position)
        {
            return this.GetTile(position.X, position.Y);
        }

        public Tile GetTile(int x, int y)
        {
            return this.tiles[x, y];
        }

        public void SetTile(Vector position, Tile tile)
        {
            this.SetTile(position.X, position.Y, tile);
        }

        public void SetTile(int x, int y, Tile tile)
        {
            this.tiles[x, y] = tile;
        }

        public void AddPlayer(Player player)
        {
            this.players.Add(player);

            var playerDataPacket = new PlayerDataPacket();
            playerDataPacket.WriteData(player);

            foreach (var mPlayer in this.players.Where(mPlayer => mPlayer != player))
            {
                mPlayer.SendPacket(playerDataPacket);
            }
        }

        public void RemovePlayer(Player player, bool leftGame)
        {
            // Free up the tile.
            this.GetTile(player.Position).IsOccupied = false;

            // Remove the player from the map player list.
            this.players.Remove(player);

            var logoutPacket = new LogoutPacket();
            logoutPacket.WriteData(player.PlayerIndex);
            this.SendPacket(logoutPacket);

            if (!leftGame) return;

            foreach (var mapPlayer in this.players)
            {
                mapPlayer.SendMessage(player.Name + " has left " + ServerConfiguration.GameName + ".");
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

        public MapItem GetMapItem(Vector mapItemPos)
        {
            return this.items.FirstOrDefault(mapItem => mapItem.Position == mapItemPos);
        }

        public MapItem[] GetMapItems()
        {
            return this.items.ToArray();
        }

        public void RemoveMapItem(MapItem mapItem)
        {
            var despawnMapItemPacket = new DespawnMapItemPacket();
            despawnMapItemPacket.WriteData(mapItem);
            this.SendPacket(despawnMapItemPacket, true);

            this.items.Remove(mapItem);
        }

        public void SpawnItem(Item item, Vector position, int spawnTime)
        {
            var mapItem = new MapItem(item, position, spawnTime);

            this.items.Add(mapItem);

            var spawnMapItemPacket = new SpawnMapItemPacket();
            spawnMapItemPacket.WriteData(mapItem);
            this.SendPacket(spawnMapItemPacket, true);
        }

        public MapNpc GetMapNpc(int mapNpcIndex)
        {
            return this.mapNpcs[mapNpcIndex];
        }

        public MapNpc[] GetMapNpcs()
        {
            return this.mapNpcs.ToArray();
        }

        public void SpawnMapNpc(Npc npc, Vector position)
        {

            this.mapNpcs.Add(new MapNpc(npc, position));

            // TODO: send the spawn packet.
        }

        public void DespawnMapNpc(int mapNpcIndex)
        {
            this.DespawnMapNpc(this.mapNpcs[mapNpcIndex]);
        }

        public void DespawnMapNpc(MapNpc mapNpc)
        {
            this.mapNpcs.Remove(mapNpc);
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            var columnCount = this.tiles.GetLength(1);
            var columnCount2 = newHeight;
            var columns = this.tiles.GetUpperBound(0);

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
                    if (!player.InMap)
                        continue;
                }

                player.SendPacket(packet);
            }
        }
    }
}