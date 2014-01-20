using CEngineSharp_Server.Networking.Packets.MapUpdatePackets;
using CEngineSharp_Server.Networking.Packets.PlayerUpdatePackets;
using CEngineSharp_Server.Networking.Packets.SocialPackets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Maps;
using CEngineSharp_World;
using CEngineSharp_World.Entities;
using SharpNetty;
using System;
using System.Collections.Generic;
using Map = CEngineSharp_Server.World.Maps.Map;

namespace CEngineSharp_Server.World.Entities
{
    public class Player : BasePlayer, IEntity
    {
        public enum AccessLevels
        {
            Player = 0,
            PlayerModerator,
            Moderator,
            Admin
        }

        private readonly List<Item> _inventory = new List<Item>();

        public bool LoggedIn { get; set; }

        public AccessLevels AccessLevel { get; set; }

        public Map Map { get; set; }

        public int PlayerIndex { get; private set; }

        public byte Direction { get; set; }

        public bool InMap { get; set; }

        public NettyServer.Connection Connection { get; set; }

        public int InventoryCount { get { return _inventory.Count; } }

        public Player(int index)
        {
            this.Position = new Vector(0, 0);
            this.PlayerIndex = index;
        }

        public Item[] GetInventory()
        {
            return _inventory.ToArray();
        }

        public void Attack(IEntity attacker)
        {
            var damage = attacker.GetDamage();

            if (this.GetStat(Stats.Health) - damage <= 0)
                this.Die(attacker);
        }

        public void Die(IEntity murderer)
        {
            this.SendMessage("Oh dear, you are dead!");

            // Respawn the player.
            this.Respawn();
        }

        public void Interact(IEntity interactor)
        {
            throw new NotImplementedException();
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
        }

        public void MoveTo(Vector vector, byte direction)
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

            var movementPacket = new PlayerMovementPacket();
            movementPacket.WriteData(this.PlayerIndex, this.Position, direction);
            this.Map.SendPacket(movementPacket);
        }

        public void GiveItem(Item item)
        {
            if (_inventory.Count < Constants.MAX_INVENTORY_ITEMS)
            {
                _inventory.Add(item);

                if (this.InMap)
                    this.SendInventory();

                this.SendMessage("You have received " + item.Name);
            }
            else
            {
                this.SendMessage("Your inventory is full!");
            }
        }

        public void RemoveItem(Item item)
        {
            if (_inventory.Remove(item))
                this.SendInventory();
        }

        public void RemoveItem(int slotNum)
        {
            if (slotNum < _inventory.Count && slotNum >= 0)
            {
                _inventory.RemoveAt(slotNum);
                this.SendInventory();
            }
        }

        public void DropItem(int slotNum)
        {
            if (slotNum < 0 && slotNum >= _inventory.Count) return;

            var item = _inventory[slotNum];

            if (item != null)
            {
                this.Map.SpawnItem(item, new Vector(this.Position), 50000);
                this.RemoveItem(item);
            }
        }

        public void TryPickupItem(Vector mapItemPos)
        {
            MapItem mapItem = this.Map.GetMapItem(mapItemPos);

            if (mapItem == null) return;

            this.Map.RemoveMapItem(mapItem);

            this.GiveItem(mapItem.Item);
        }

        public void EnterGame()
        {
            this.LoggedIn = true;

            this.SetStat(Stats.Health, 100);

            if (this.MapNum < 0)
                this.MapNum = 0;

            this.JoinMap(ContentManager.Instance.MapManager.GetMap(this.MapNum));

            Console.WriteLine(this.Name + " has logged in!");

            //            var chatMessagePacket = new ChatMessagePacket();
            //            chatMessagePacket.WriteData(this.Name + " has logged in!");
            //            PlayerManager.Instance.BroadcastPacket(chatMessagePacket);

            var messagePacket = new ChatMessagePacket();
            messagePacket.WriteData(string.Format("Welcome to {0}, {1}", ServerConfiguration.GameName, this.Name));
            this.SendPacket(messagePacket);
        }

        public void JoinMap(Map map)
        {
            if (this.Map != null)
                this.LeaveMap(false);

            map.AddPlayer(this);
            this.Map = map;
            this.MapNum = ContentManager.Instance.MapManager.GetMapIndex(map);

            this.InMap = false;

            var mapCheckPacket = new MapCheckPacket();
            mapCheckPacket.WriteData(this.Map.Name, this.Map.Version);
            this.SendPacket(mapCheckPacket);
        }

        public void SendMessage(string message)
        {
            var chatMessagePacket = new ChatMessagePacket();
            chatMessagePacket.WriteData(message);
            this.SendPacket(chatMessagePacket);
        }

        public void LeaveGame()
        {
            this.LeaveMap(true);

            this.Save(Constants.FILEPATH_PLAYERS);
            ContentManager.Instance.PlayerManager.RemovePlayer(this.PlayerIndex);
        }

        public void LeaveMap(bool leftGame)
        {
            this.Map.RemovePlayer(this, leftGame);
        }

        private void Respawn()
        {
        }

        public void SendPlayerData()
        {
            var playerDataPacket = new PlayerDataPacket();
            playerDataPacket.WriteData(this);
            this.SendPacket(playerDataPacket);

            this.SendInventory();
            this.SendVitals();
        }

        public void SendInventory()
        {
            var invenUpdatePacket = new UpdateInventoryPacket();
            invenUpdatePacket.WriteData(this);
            this.SendPacket(invenUpdatePacket);
        }

        public void SendVitals()
        {
            var vitalUpdatePacket = new UpdatePlayerStatsPacket();
            vitalUpdatePacket.WriteData(this);
            this.SendPacket(vitalUpdatePacket);
        }

        public void SendPacket(Packet packet)
        {
            this.Connection.SendPacket(packet);
        }
    }
}