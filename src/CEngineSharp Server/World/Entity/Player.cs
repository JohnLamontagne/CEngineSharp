﻿using CEngineSharp_Server.Net;
using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharedGameData;
using SharedGameData.World;
using SharedGameData.World.Entities;
using SharpNetty;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Server.World
{
    public class Player : IEntity
    {
        public enum AccessLevels
        {
            Player = 0,
            Player_Moderator,
            Moderator,
            Admin
        }

        private SharpNetty.NettyServer.Connection _connection;
        private ushort[] _vitals = new ushort[(int)Vitals.Energy + 1];
        private ushort[] _stats = new ushort[(int)Stats.Strength + 1];
        private readonly int _playerIndex;
        private List<Item> _inventory = new List<Item>();

        public string Name { get; set; }

        public ushort Level { get; set; }

        public bool LoggedIn { get; set; }

        public string IP { get; protected set; }

        public string Password { get; set; }

        public AccessLevels AccessLevel { get; set; }

        public Map Map { get; set; }

        public Vector2i Position { get; set; }

        public int PlayerIndex { get { return _playerIndex; } }

        public byte Direction { get; set; }

        private bool inMap;

        public SharpNetty.NettyServer.Connection Connection { get { return _connection; } }

        public int MapNum
        {
            get
            {
                return MapManager.GetMapIndex(this.Map);
            }
        }

        public Player(NettyServer.Connection connection, int index)
        {
            string ip = connection.Socket.RemoteEndPoint.ToString();
            this.IP = connection.Socket.RemoteEndPoint.ToString().Remove(ip.IndexOf(':'), ip.Length - ip.IndexOf(':'));

            this.Position = new Vector2i(0, 0);

            _connection = connection;
            _playerIndex = index;
        }

        public Item[] GetInventory()
        {
            return _inventory.ToArray();
        }

        public void SetInMap(bool value)
        {
            this.inMap = value;
        }

        public bool GetInMap()
        {
            return this.inMap;
        }

        public ushort GetVital(Vitals vital)
        {
            return _vitals[(int)vital];
        }

        public void SetVital(Vitals vital, ushort value)
        {
            _vitals[(int)vital] = value;
        }

        public ushort GetStat(Stats stat)
        {
            return _stats[(int)stat];
        }

        public void SetStat(Stats stat, ushort value)
        {
            _stats[(int)stat] = value;
        }

        public void Attack(IEntity attacker)
        {
            int damage = attacker.GetDamage();

            if (this.GetVital(Vitals.HitPoints) - damage <= 0)
                this.Die(attacker);
        }

        public void Die(IEntity murderer)
        {
            this.SendMessage("Oh dear, you are dead!");

            // Respawn the player.
            this.Respawn();
        }

        public void MoveTo(Vector2i vector, byte direction)
        {
            this.Direction = direction;

            //// If the tile is blocked, we obviously can't move to it.
            if (vector.X >= 0 && vector.Y >= 0 && vector.X < this.Map.MapWidth && vector.Y < this.Map.MapHeight && !this.Map.GetTile(vector).Blocked && !this.Map.GetTile(vector).IsOccupied)
            {
                // Unblock the previous tile so that another entity may occupy it.
                this.Map.GetTile(this.Position).IsOccupied = false;

                // Change our character's position to the new position.
                this.Position = vector;

                // Block the tile that we're moving to.
                this.Map.GetTile(vector).IsOccupied = true;
            }

            var movementPacket = new MovementPacket();
            movementPacket.WriteData(this.PlayerIndex, this.Position, direction);
            this.Map.SendPacket(movementPacket);
        }

        public void GiveItem(Item item)
        {
            if (_inventory.Count < Constants.MAX_INVENTORY_ITEMS)
            {
                _inventory.Add(item);

                if (this.inMap)
                {
                    InventoryUpdatePacket invenUpdatePacket = new InventoryUpdatePacket();
                    invenUpdatePacket.WriteData(this);
                    this.SendPacket(invenUpdatePacket);
                }

                this.SendMessage("You have received " + item.Name);
            }
            else
            {
                this.SendMessage("Your inventory is full!");
            }
        }

        public void TryPickupItem(Vector2i mapItemPos)
        {
            Map.MapItem mapItem = this.Map.GetMapItem(mapItemPos);

            if (mapItem == null) return;

            this.Map.RemoveMapItem(mapItem);

            this.GiveItem(mapItem.Item);
        }

        public void EnterGame()
        {
            this.JoinMap(MapManager.GetMap(0));

            Console.WriteLine(this.Name + " has logged in!");

            var chatMessagePacket = new ChatMessagePacket();
            chatMessagePacket.WriteData(this.Name + " has logged in!");
            PlayerManager.BroadcastPacket(chatMessagePacket);

            var messagePacket = new ChatMessagePacket();
            messagePacket.WriteData(string.Format("Welcome to {0}, {1}", ServerConfiguration.GameName, this.Name));
            this.SendPacket(messagePacket);

            this.GiveItem(ItemManager.GetItem(0));

            this.Map.SpawnItem(ItemManager.GetItem(0), 4, 4, 5000);
        }

        public void SendPlayerData()
        {
            var playerDataPacket = new PlayerDataPacket();
            playerDataPacket.WriteData(this);
            this.SendPacket(playerDataPacket);

            var inventoryUpdatePacket = new InventoryUpdatePacket();
            inventoryUpdatePacket.WriteData(this);
            this.SendPacket(inventoryUpdatePacket);
        }

        public void LeaveGame()
        {
            this.LeaveMap(true);

            PlayerManager.SavePlayer(this);
            PlayerManager.RemovePlayer(this.PlayerIndex);
        }

        public void LeaveMap(bool leftGame)
        {
            this.Map.RemovePlayer(this, leftGame);
        }

        private void Respawn()
        {
        }

        public int GetDamage()
        {
            Random random = new Random();

            return (int)((this.GetStat(Stats.Strength) * random.NextDouble()) + random.Next(1, 10));
        }

        public void Interact(IEntity interactor)
        {
            throw new NotImplementedException();
        }

        public void SendPacket(Packet packet)
        {
            _connection.SendPacket(packet);
        }

        public void JoinMap(Map map)
        {
            if (this.Map != null)
                this.LeaveMap(false);

            map.AddPlayer(this);
            this.Map = map;

            this.inMap = false;

            MapCheckPacket mapCheckPacket = new MapCheckPacket();
            mapCheckPacket.WriteData(this.Map.Name, this.Map.Version);
            this.SendPacket(mapCheckPacket);
        }

        public void SendMessage(string message)
        {
            var chatMessagePacket = new ChatMessagePacket();
            chatMessagePacket.WriteData(message);
            this.SendPacket(chatMessagePacket);
        }
    }
}